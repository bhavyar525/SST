import React from "react";

const DeleteStatusButton = ({ statusId, onDelete }) => {
  const handleDeleteClick = () => {
    if (window.confirm("Are you sure you want to delete this status?")) {
      onDelete(statusId);
    }
  };

  return (
    <button className="action-button delete-btn" onClick={handleDeleteClick}>
      Delete
    </button>
  );
};

export default DeleteStatusButton;
