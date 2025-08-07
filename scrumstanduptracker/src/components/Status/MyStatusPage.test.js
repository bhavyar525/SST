import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import MyStatusPage from "./MyStatusPage";
import { MemoryRouter } from "react-router-dom";
import * as api from "../../api/dailyStatusApi";

// Mock react-router-dom useNavigate once at top-level for Jest module hoisting
const mockNavigate = jest.fn();
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: () => mockNavigate,
}));

// Mock CreateStatusForm child component
jest.mock("./CreateStatusForm", () => ({ onCreate, onCancel }) => (
  <div>
    <button onClick={() => onCreate({ task: "Mocked Task" })}>Mock Create</button>
    <button onClick={onCancel}>Cancel</button>
  </div>
));

// Mock StatusList child component
jest.mock("./StatusList", () => ({ statuses, onEdit, onDelete }) => (
  <div>
    {statuses.map((s, idx) => (
      <div key={idx}>
        <span>{s.task}</span>
        <button onClick={() => onEdit(s.id)}>Edit</button>
        <button onClick={() => onDelete(s.id)}>Delete</button>
      </div>
    ))}
  </div>
));

describe("MyStatusPage", () => {
  beforeEach(() => {
    localStorage.setItem("userName", "JohnDoe");
    localStorage.setItem("token", "fake-token");
    jest.clearAllMocks();
  });

  afterEach(() => {
    localStorage.clear();
  });

  test("renders with username and loads statuses", async () => {
    const mockStatuses = [{ id: 1, task: "Task 1" }];
    jest.spyOn(api, "getMyStatusByName").mockResolvedValue(mockStatuses);

    render(
      <MemoryRouter>
        <MyStatusPage />
      </MemoryRouter>
    );

    expect(await screen.findByText("Task 1")).toBeInTheDocument();
    expect(screen.getByText(/Developer: JohnDoe/i)).toBeInTheDocument();
  });

  test("clicking 'Add New Status' shows create form and calls postDailyStatus", async () => {
    jest.spyOn(api, "getMyStatusByName").mockResolvedValue([]);
    jest.spyOn(api, "postDailyStatus").mockResolvedValue();

    render(
      <MemoryRouter>
        <MyStatusPage />
      </MemoryRouter>
    );

    fireEvent.click(screen.getByText("Add New Status"));

    expect(screen.getByText("Mock Create")).toBeInTheDocument();

    fireEvent.click(screen.getByText("Mock Create"));

    await waitFor(() =>
      expect(api.postDailyStatus).toHaveBeenCalledWith({ task: "Mocked Task" })
    );
  });

  test("delete button calls deleteDailyStatus", async () => {
    const mockStatuses = [{ id: 2, task: "To Delete" }];
    jest.spyOn(api, "getMyStatusByName").mockResolvedValue(mockStatuses);
    jest.spyOn(api, "deleteDailyStatus").mockResolvedValue();

    render(
      <MemoryRouter>
        <MyStatusPage />
      </MemoryRouter>
    );

    expect(await screen.findByText("To Delete")).toBeInTheDocument();

    fireEvent.click(screen.getByText("Delete"));

    await waitFor(() =>
      expect(api.deleteDailyStatus).toHaveBeenCalledWith(2)
    );
  });

  test("logout clears localStorage and navigates to /login", async () => {
    render(
      <MemoryRouter>
        <MyStatusPage />
      </MemoryRouter>
    );

    fireEvent.click(screen.getByText("Logout"));

    expect(localStorage.getItem("token")).toBeNull();
    expect(localStorage.getItem("userName")).toBeNull();
    expect(mockNavigate).toHaveBeenCalledWith("/login");
  });
});