import React from "react";
import { render, screen, waitFor, fireEvent } from "@testing-library/react";
import EditStatusPage from "./EditStatusPage";

// Mock react-router-dom hooks
jest.mock("react-router-dom", () => ({
  useParams: jest.fn(),
  useNavigate: jest.fn(),
}));

// Mock API module
jest.mock("../../api/dailyStatusApi", () => ({
  getDailyStatusById: jest.fn(),
  updateDailyStatus: jest.fn(),
}));

import { useParams, useNavigate } from "react-router-dom";
import { getDailyStatusById, updateDailyStatus } from "../../api/dailyStatusApi";

describe("EditStatusPage", () => {
  const mockNavigate = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();

    // Setup default mock implementations
    useParams.mockReturnValue({ id: "123" });
    useNavigate.mockReturnValue(mockNavigate);
  });

  test("shows loading initially and then displays fetched data", async () => {
    const fakeStatus = {
      developerId: "12345",
      developerName: "John Doe",
      taskDetails: "Fix bugs",
      didYesterday: "Reviewed PRs",
      doingToday: "Write tests",
      blockers: "None",
      submissionDate: "2025-08-02T00:00:00.000Z",
    };

    getDailyStatusById.mockResolvedValueOnce(fakeStatus);

    render(<EditStatusPage />);

    // Loading shown initially
    expect(screen.getByText(/loading/i)).toBeInTheDocument();

    // Wait for data to load and inputs to be populated
    await waitFor(() => {
      expect(screen.getByPlaceholderText(/Developer Name/i).value).toBe("John Doe");
    });

    expect(screen.getByPlaceholderText(/Task Details/i).value).toBe("Fix bugs");
    expect(screen.getByPlaceholderText(/Did Yesterday/i).value).toBe("Reviewed PRs");
    expect(screen.getByPlaceholderText(/Doing Today/i).value).toBe("Write tests");
    expect(screen.getByPlaceholderText(/Blockers/i).value).toBe("None");

    // Date input should show only YYYY-MM-DD
    expect(screen.getByDisplayValue("2025-08-02")).toBeInTheDocument();
  });

  test("handles input changes", async () => {
    const fakeStatus = {
      developerId: "12345",
      developerName: "John Doe",
      taskDetails: "",
      didYesterday: "",
      doingToday: "",
      blockers: "",
      submissionDate: "2025-08-02T00:00:00.000Z",
    };
    getDailyStatusById.mockResolvedValueOnce(fakeStatus);

    render(<EditStatusPage />);

    // Wait for data load
    await waitFor(() => {
      expect(screen.getByPlaceholderText(/Developer Name/i).value).toBe("John Doe");
    });

    const devNameInput = screen.getByPlaceholderText(/Developer Name/i);
    fireEvent.change(devNameInput, { target: { value: "Jane Smith" } });
    expect(devNameInput.value).toBe("Jane Smith");

    const taskInput = screen.getByPlaceholderText(/Task Details/i);
    fireEvent.change(taskInput, { target: { value: "New task details" } });
    expect(taskInput.value).toBe("New task details");
  });

  test("calls updateDailyStatus and navigates on save", async () => {
    const fakeStatus = {
      developerId: "12345",
      developerName: "John Doe",
      taskDetails: "Fix bugs",
      didYesterday: "Reviewed PRs",
      doingToday: "Write tests",
      blockers: "None",
      submissionDate: "2025-08-02T00:00:00.000Z",
    };
    getDailyStatusById.mockResolvedValueOnce(fakeStatus);
    updateDailyStatus.mockResolvedValueOnce({}); // mock update success

    render(<EditStatusPage />);

    // Wait for data load
    await waitFor(() => {
      expect(screen.getByPlaceholderText(/Developer Name/i).value).toBe("John Doe");
    });

    // Change some value
    const taskInput = screen.getByPlaceholderText(/Task Details/i);
    fireEvent.change(taskInput, { target: { value: "Updated task" } });

    // Click Save button
    const saveButton = screen.getByRole("button", { name: /save/i });
    fireEvent.click(saveButton);

    // Wait for updateDailyStatus to be called with updated data
    await waitFor(() => {
      expect(updateDailyStatus).toHaveBeenCalledWith("123", expect.objectContaining({
        taskDetails: "Updated task",
      }));
    });

    // Verify navigate was called to '/status'
    expect(mockNavigate).toHaveBeenCalledWith("/status");
  });

  test("navigates to /status on cancel click", async () => {
    getDailyStatusById.mockResolvedValueOnce({
      developerId: "12345",
      developerName: "",
      taskDetails: "",
      didYesterday: "",
      doingToday: "",
      blockers: "",
      submissionDate: "",
    });

    render(<EditStatusPage />);

    // Wait for loading to be done
    await waitFor(() => {
      expect(screen.queryByText(/loading/i)).not.toBeInTheDocument();
    });

    // Click Cancel button
    const cancelButton = screen.getByRole("button", { name: /cancel/i });
    fireEvent.click(cancelButton);

    // Ensure navigate called to '/status'
    expect(mockNavigate).toHaveBeenCalledWith("/status");
  });

  test("shows alert on fetch error and stops loading", async () => {
    getDailyStatusById.mockRejectedValueOnce(new Error("API failed"));

    // Mock window.alert
    window.alert = jest.fn();

    render(<EditStatusPage />);

    // Wait for the effect to finish
    await waitFor(() => {
      expect(window.alert).toHaveBeenCalledWith("Failed to load status");
    });

    // Loading should be false and not shown
    expect(screen.queryByText(/loading/i)).not.toBeInTheDocument();
  });
});