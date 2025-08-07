import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import CreateStatusForm from "./CreateStatusForm";

describe("CreateStatusForm", () => {
  const mockOnCreate = jest.fn(); //mock func
  const mockOnCancel = jest.fn(); //mock func

  beforeEach(() => { 
    mockOnCreate.mockClear(); //Clears previous calls
    mockOnCancel.mockClear(); //Clears previous calls
    localStorage.setItem("id", "12345"); // developerId
    localStorage.setItem("userName", "Alice"); // developerName to show in input
  });

  afterEach(() => {
    localStorage.clear(); // Clears localStorage after each test
  });

  test("renders all input fields and buttons", () => {
    render(<CreateStatusForm onCreate={mockOnCreate} onCancel={mockOnCancel} />);

    // Developer Name input shows localStorage userName value
    const devNameInput = screen.getByPlaceholderText(/Developer Name/i);
    expect(devNameInput).toBeInTheDocument();
    expect(devNameInput.value).toBe("Alice"); // readonly value from localStorage

    expect(screen.getByPlaceholderText(/Task Details/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Did Yesterday/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Doing Today/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Blockers/i)).toBeInTheDocument();

    expect(screen.getByRole("button", { name: /Add/i })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /Cancel/i })).toBeInTheDocument();
  });

  test("typing in input fields updates their values", () => {
    render(<CreateStatusForm onCreate={mockOnCreate} onCancel={mockOnCancel} />);

    // Developer Name is read-only, do NOT fire change events on it
    const devNameInput = screen.getByPlaceholderText(/Developer Name/i);
    expect(devNameInput.value).toBe("Alice");

    // Other fields are editable
    const taskInput = screen.getByPlaceholderText(/Task Details/i);
    fireEvent.change(taskInput, { target: { value: "Fix bugs" } });
    expect(taskInput.value).toBe("Fix bugs");

    const didYesterdayInput = screen.getByPlaceholderText(/Did Yesterday/i);
    fireEvent.change(didYesterdayInput, { target: { value: "Reviewed PRs" } });
    expect(didYesterdayInput.value).toBe("Reviewed PRs");

    const doingTodayInput = screen.getByPlaceholderText(/Doing Today/i);
    fireEvent.change(doingTodayInput, { target: { value: "Write tests" } });
    expect(doingTodayInput.value).toBe("Write tests");

    const blockersInput = screen.getByPlaceholderText(/Blockers/i);
    fireEvent.change(blockersInput, { target: { value: "None" } });
    expect(blockersInput.value).toBe("None");
  });

  test("submitting form calls onCreate with formData", () => {
    render(<CreateStatusForm onCreate={mockOnCreate} onCancel={mockOnCancel} />);

    // Developer Name input is readonly and populated from localStorage (no typing)
    const devNameInput = screen.getByPlaceholderText(/Developer Name/i);
    expect(devNameInput.value).toBe("Alice");

    // Fill in other required fields
    fireEvent.change(screen.getByLabelText(/Submission Date/i), {
      target: { value: "2025-08-02" },
    });

    fireEvent.change(screen.getByPlaceholderText(/Task Details/i), {
      target: { value: "Implement feature" },
    });

    // Optional fields left empty

    // Submit form
    fireEvent.click(screen.getByRole("button", { name: /Add/i }));

    // Check callback called once with correct form data including localStorage values
    expect(mockOnCreate).toHaveBeenCalledTimes(1);
    const calledWith = mockOnCreate.mock.calls[0][0];
    expect(calledWith.developerName).toBe("Alice"); // from localStorage
    expect(calledWith.submissionDate).toBe("2025-08-02");
    expect(calledWith.taskDetails).toBe("Implement feature");
    expect(calledWith.developerId).toBe("12345"); // from localStorage
  });

  test("clicking cancel calls onCancel", () => {
    render(<CreateStatusForm onCreate={mockOnCreate} onCancel={mockOnCancel} />);
    const cancelButton = screen.getByRole("button", { name: /Cancel/i });
    fireEvent.click(cancelButton);
    expect(mockOnCancel).toHaveBeenCalledTimes(1);
  });
});