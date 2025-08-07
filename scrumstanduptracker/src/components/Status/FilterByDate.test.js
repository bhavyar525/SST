import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import FilterByDate from "./FilterByDate";

describe("FilterByDate", () => {
  const mockOnFilter = jest.fn();

  beforeEach(() => {
    mockOnFilter.mockClear();
    jest.spyOn(window, "alert").mockImplementation(() => {});
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  test("renders date input and filter button", () => {
    render(<FilterByDate onFilter={mockOnFilter} />);

    // Try query by label text or placeholder instead of role
    // Use aria-label in component for this if missing
    const input = screen.getByLabelText("Filter date input");
    expect(input).toBeInTheDocument();

    expect(screen.getByRole("button", { name: /Filter by Date/i })).toBeInTheDocument();
  });

  test("changing date input updates value", () => {
    render(<FilterByDate onFilter={mockOnFilter} />);
    const input = screen.getByLabelText("Filter date input");

    fireEvent.change(input, { target: { value: "2025-08-15" } });
    expect(input.value).toBe("2025-08-15");
  });

  test("clicking filter button with valid date calls onFilter", () => {
    render(<FilterByDate onFilter={mockOnFilter} />);
    const input = screen.getByLabelText("Filter date input");
    const button = screen.getByRole("button", { name: /Filter by Date/i });

    fireEvent.change(input, { target: { value: "2025-08-15" } });
    fireEvent.click(button);

    expect(mockOnFilter).toHaveBeenCalledTimes(1);
    expect(mockOnFilter).toHaveBeenCalledWith("2025-08-15");
  });

  test("clicking filter button without date shows alert and does not call onFilter", () => {
    render(<FilterByDate onFilter={mockOnFilter} />);
    const button = screen.getByRole("button", { name: /Filter by Date/i });

    fireEvent.click(button);

    expect(window.alert).toHaveBeenCalledWith("Please select a date");
    expect(mockOnFilter).not.toHaveBeenCalled();
  });
});
