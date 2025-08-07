import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import GetAllPage from "./GetAllPage";
import * as api from "../../api/dailyStatusApi";
import { MemoryRouter } from "react-router-dom";

// Mock useNavigate once at the top-level for navigation tracking
const mockNavigate = jest.fn();
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: () => mockNavigate,
}));

// Mock Filter components to isolate GetAllPage tests
jest.mock("./FilterByName", () => ({ onFilter }) => (
  <button onClick={() => onFilter("Janhvi")}>Mock Filter By Name</button>
));
jest.mock("./FilterByDate", () => ({ onFilter }) => (
  <button onClick={() => onFilter("2025-08-02")}>Mock Filter By Date</button>
));

describe("GetAllPage", () => {
  beforeEach(() => {
    jest.clearAllMocks();
    localStorage.clear();
  });

  test("shows loading initially then displays fetched statuses", async () => {
    const mockData = [
      {
        id: 1,
        developerName: "Jahnvi",
        submissionDate: "2025-08-01T00:00:00Z",
        taskDetails: "Task A",
        didYesterday: "Did A",
        doingToday: "Doing A",
        blockers: "None",
      },
    ];
    jest.spyOn(api, "getAllStatus").mockResolvedValue(mockData);

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    expect(screen.getByText(/Loading statuses/i)).toBeInTheDocument();

    // Wait for data to be loaded and rendered
    expect(await screen.findByText("Jahnvi")).toBeInTheDocument();
    expect(screen.getByText("Task A")).toBeInTheDocument();
  });

  test("handles error during fetchAllStatuses", async () => {
    jest.spyOn(api, "getAllStatus").mockRejectedValue(new Error("API Failure"));

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    expect(screen.getByText(/Loading statuses/i)).toBeInTheDocument();

    expect(await screen.findByText(/Failed to load statuses/i)).toBeInTheDocument();
  });

  test("filters by developer name using FilterByName", async () => {
    const filteredData = [
      {
        id: 2,
        developerName: "Jahnvi",
        submissionDate: "2025-08-02T00:00:00Z",
        taskDetails: "Filtered Task",
        didYesterday: "Did B",
        doingToday: "Doing B",
        blockers: "Blocker B",
      },
    ];

    jest.spyOn(api, "getAllStatus").mockResolvedValue([]);
    // Return array directly here, not wrapped in an object
    jest.spyOn(api, "getMyStatusByName").mockResolvedValue(filteredData);

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    // Click on mocked filter by name button
    fireEvent.click(await screen.findByText("Mock Filter By Name"));

    expect(await screen.findByText("Filtered Task")).toBeInTheDocument();
  });

  test("filters by date using FilterByDate", async () => {
    const dateFilteredData = [
      {
        id: 3,
        developerName: "Jahnvi",
        submissionDate: "2025-08-02T00:00:00Z",
        taskDetails: "Date Filtered Task",
        didYesterday: "Did C",
        doingToday: "Doing C",
        blockers: "Blocker C",
      },
    ];

    jest.spyOn(api, "getAllStatus").mockResolvedValue([]);
    // Return array directly here as well
    jest.spyOn(api, "getStatusByDate").mockResolvedValue(dateFilteredData);

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    // Click on mocked filter by date button
    fireEvent.click(await screen.findByText("Mock Filter By Date"));

    expect(await screen.findByText("Date Filtered Task")).toBeInTheDocument();
  });

  test("clear filters button fetches all statuses", async () => {
    const allStatuses = [
      {
        id: 4,
        developerName: "ClearFilterUser",
        submissionDate: "2025-08-03T00:00:00Z",
        taskDetails: "Clear Filter Task",
        didYesterday: "Did D",
        doingToday: "Doing D",
        blockers: "None",
      },
    ];

    const getAllSpy = jest.spyOn(api, "getAllStatus").mockResolvedValue(allStatuses);

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    // Click Clear Filters button
    fireEvent.click(await screen.findByText(/Clear Filters/i));

    expect(await screen.findByText("Clear Filter Task")).toBeInTheDocument();
    expect(getAllSpy).toHaveBeenCalledTimes(2); // on mount and on button click
  });

  test("back button navigates to /status", async () => {
    jest.spyOn(api, "getAllStatus").mockResolvedValue([]);

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    fireEvent.click(await screen.findByText("Back"));
    expect(mockNavigate).toHaveBeenCalledWith("/status");
  });

  test("logout clears localStorage and navigates to /login", async () => {
    localStorage.setItem("token", "tokenValue");
    localStorage.setItem("id", "userId");
    localStorage.setItem("userName", "user");

    jest.spyOn(api, "getAllStatus").mockResolvedValue([]);

    render(
      <MemoryRouter>
        <GetAllPage />
      </MemoryRouter>
    );

    fireEvent.click(await screen.findByText("Logout"));

    expect(localStorage.getItem("token")).toBeNull();
    expect(localStorage.getItem("id")).toBeNull();
    expect(localStorage.getItem("userName")).toBeNull();
    expect(mockNavigate).toHaveBeenCalledWith("/login");
  });
});
