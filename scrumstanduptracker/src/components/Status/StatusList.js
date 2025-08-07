import React from "react";
import DeleteStatusButton from "./DeleteStatusButton";

const StatusList = ({ statuses, onEdit, onDelete }) => {
  return (
    <div className="table-section">
      <h4>Your Status Updates</h4>
      {statuses.length === 0 ? (
        <p>No statuses found.</p>
      ) : (
        <table id="dailyStatusTable">
          <thead>
            <tr>
              <th>Developer</th>
              <th>Date</th>
              <th>Task</th>
              <th>Did Yesterday</th>
              <th>Doing Today</th>
              <th>Blockers</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {statuses.map((status) => (
              <tr key={status.id}>
                <td>{status.developerName}</td>
                <td>{status.submissionDate?.slice(0, 10)}</td>
                <td>{status.taskDetails}</td>
                <td>{status.didYesterday}</td>
                <td>{status.doingToday}</td>
                <td>{status.blockers}</td>
                <td>
                  <div className="action-btn-container">
                    <button className="action-button" id="editBtn" onClick={() => onEdit(status.id)}>Edit</button>
                    <DeleteStatusButton statusId={status.id} onDelete={onDelete} />
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default StatusList;