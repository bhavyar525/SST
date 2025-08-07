import React, { useState } from "react";

const CreateStatusForm = ({ onCreate, onCancel }) => {
  const [formData, setFormData] = useState({
    developerId: localStorage.getItem("id"),
    developerName: localStorage.getItem("userName") || "",
    taskDetails: "",
    didYesterday: "",
    doingToday: "",
    blockers: "", 
    submissionDate: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onCreate(formData);
  };

  return (
    <form className="form-section add-status-form" onSubmit={handleSubmit}>
      <h4>Add New Status</h4>
      <input
        name="developerName"
        placeholder="Developer Name"
        value={formData.developerName}
        readOnly
        required
      />
      <input
        name="submissionDate"
        type="date"
        aria-label="Submission Date"
        value={formData.submissionDate}
        onChange={handleChange}
        required
      />
      <input
        name="taskDetails"
        placeholder="Task Details"
        value={formData.taskDetails}
        onChange={handleChange}
        required
      />
      <input
        name="didYesterday"
        placeholder="Did Yesterday"
        value={formData.didYesterday}
        onChange={handleChange}
      />
      <input
        name="doingToday"
        placeholder="Doing Today"
        value={formData.doingToday}
        onChange={handleChange}
      />
      <input
        name="blockers"
        placeholder="Blockers"
        value={formData.blockers}
        onChange={handleChange}
      />
      <button type="submit" className="add-status-btn">
        Add
      </button>
      <button type="button" onClick={onCancel} className="cancel-status-btn">
        Cancel
      </button>
    </form>
  );
};

export default CreateStatusForm;