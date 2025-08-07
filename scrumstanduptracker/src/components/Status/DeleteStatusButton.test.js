import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import DeleteStatusButton from "./DeleteStatusButton";

//Defining test suite - DeleteStatusButton
describe("DeleteStatusButton", () => {
//Create mock func named mockOnDel using Jest
    const mockOnDelete = jest.fn();

//Runs this func bef each individual test inside the suite

  beforeEach(() => {
    jest.clearAllMocks();
  });

//test() - single test case
  test("renders Delete button", () => {
    render(<DeleteStatusButton statusId={42} onDelete={mockOnDelete} />);
    const button = screen.getByRole("button", { name: /delete/i });
    expect(button).toBeInTheDocument();
  });

  test("calls onDelete with statusId when confirmed", () => {
    // Mock window.confirm to simulate user clicking "OK"
    window.confirm = jest.fn(() => true);

    render(<DeleteStatusButton statusId={42} onDelete={mockOnDelete} />);
    const button = screen.getByRole("button", { name: /delete/i });

    fireEvent.click(button);

    expect(window.confirm).toHaveBeenCalledWith(
      "Are you sure you want to delete this status?"
    );
    expect(mockOnDelete).toHaveBeenCalledWith(42);
    expect(mockOnDelete).toHaveBeenCalledTimes(1);
  });

  test("does not call onDelete when cancel is clicked in confirm", () => {
    // Mock window.confirm to simulate user clicking "Cancel"
    window.confirm = jest.fn(() => false);

    render(<DeleteStatusButton statusId={42} onDelete={mockOnDelete} />);
    const button = screen.getByRole("button", { name: /delete/i });

    fireEvent.click(button);

    expect(window.confirm).toHaveBeenCalledWith(
      "Are you sure you want to delete this status?"
    );
    expect(mockOnDelete).not.toHaveBeenCalled();
  });
});
