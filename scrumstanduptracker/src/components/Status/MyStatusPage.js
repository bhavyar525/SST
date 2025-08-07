import React, { useEffect, useState } from "react";
import {
  getMyStatusByName,
  postDailyStatus,
  deleteDailyStatus,
} from "../../api/dailyStatusApi";
import "../../css/StatusForm.css";
import { useNavigate } from "react-router-dom";
import CreateStatusForm from "./CreateStatusForm";
import StatusList from "./StatusList";

const MyStatusPage = () => {
  const [statuses, setStatuses] = useState([]);
  const [showAddForm, setShowAddForm] = useState(false);

  const developerName = localStorage.getItem("userName") || "";
  const navigate = useNavigate();

  useEffect(() => {
    if (developerName) {
      fetchStatuses();
    }
  }, [developerName]);

  const fetchStatuses = async () => {
    try {
      const statuses = await getMyStatusByName(developerName);
      setStatuses(statuses || []);
    } catch (error) {
      console.error("Failed to fetch statuses:", error);
      setStatuses([]);
    }
  };

  const handleCreate = async (newStatus) => {
    try {
      await postDailyStatus(newStatus);
      setShowAddForm(false);
      fetchStatuses();
    } catch (error) {
      console.error("Failed to add status:", error);
      alert("Failed to add status.");
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteDailyStatus(id);
      fetchStatuses();
    } catch {
      alert("Delete failed.");
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("id");
    localStorage.removeItem("userName");
    navigate("/login");
  };

  const handleGetAllStatusClick = () => {
    navigate("/all-statuses");
  };

  const handleEditClick = (id) => {
    navigate(`/edit-status/${id}`);
  };

  return (
    <div className="status-container">
      <div className="status-header">
        <h2>
          My Daily Status
          {developerName && (
            <span style={{ fontWeight: "normal", fontSize: "1.5rem", marginLeft: 10 }}>
              - Developer: {developerName}
            </span>
          )}
        </h2>
        <button className="logout-btn" onClick={handleLogout}>Logout</button>
      </div>

      <button
        className="get-all-status-btn"
        onClick={handleGetAllStatusClick}
        style={{ marginLeft: "10px", marginRight: "15px" }}
      >
        Get All Statuses
      </button>

      {showAddForm ? (
        <CreateStatusForm onCreate={handleCreate} onCancel={() => setShowAddForm(false)} />
      ) : (
        <>
          <button onClick={() => setShowAddForm(true)} style={{ marginBottom: "15px" }}>
            Add New Status
          </button>
          <StatusList statuses={statuses} onEdit={handleEditClick} onDelete={handleDelete} />
        </>
      )}
    </div>
  );
};

export default MyStatusPage;