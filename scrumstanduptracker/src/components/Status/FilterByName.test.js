import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import FilterByName from "./FilterByName";

describe("FilterByName", () => {
  const mockOnFilter = jest.fn();

  beforeEach(() => {
    mockOnFilter.mockClear();
    // Mock window.alert globally
    jest.spyOn(window, "alert").mockImplementation(() => {});
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  test("renders input and filter button", () => {
    render(<FilterByName onFilter={mockOnFilter} />);

    expect(screen.getByPlaceholderText(/Enter Developer Name/i)).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /Filter By Name/i })).toBeInTheDocument();
  });

  test("typing in input updates value", () => {
    render(<FilterByName onFilter={mockOnFilter} />);
    const input = screen.getByPlaceholderText(/Enter Developer Name/i);

    fireEvent.change(input, { target: { value: "John" } });
    expect(input.value).toBe("John");

    fireEvent.change(input, { target: { value: "  Jane Doe  " } });
    expect(input.value).toBe("  Jane Doe  ");
  });

  test("clicking filter button calls onFilter with trimmed input", () => {
    render(<FilterByName onFilter={mockOnFilter} />);
    const input = screen.getByPlaceholderText(/Enter Developer Name/i);
    const button = screen.getByRole("button", { name: /Filter By Name/i });

    fireEvent.change(input, { target: { value: "  JohnDoe  " } });
    fireEvent.click(button);

    expect(mockOnFilter).toHaveBeenCalledTimes(1);
    expect(mockOnFilter).toHaveBeenCalledWith("JohnDoe");
  });

  test("clicking filter button with empty input shows alert and does not call onFilter", () => {
    render(<FilterByName onFilter={mockOnFilter} />);
    const input = screen.getByPlaceholderText(/Enter Developer Name/i);
    const button = screen.getByRole("button", { name: /Filter By Name/i });

    fireEvent.change(input, { target: { value: "    " } });
    fireEvent.click(button);

    expect(window.alert).toHaveBeenCalledWith("Please enter a developer name to filter.");
    expect(mockOnFilter).not.toHaveBeenCalled();
  });
});
