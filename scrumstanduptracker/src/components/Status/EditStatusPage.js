import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getDailyStatusById, updateDailyStatus } from "../../api/dailyStatusApi";

const EditStatusPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [status, setStatus] = useState({
    developerId: localStorage.getItem("id"),
    developerName: localStorage.getItem("userName") || "",
    taskDetails: "",
    didYesterday: "",
    doingToday: "",
    blockers: "",
    submissionDate: "",
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStatus = async () => {
      try {
        const data = await getDailyStatusById(id);
        setStatus(data);
      } catch (error) {
        alert("Failed to load status");
      } finally {
        setLoading(false);
      }
    };
    fetchStatus();
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setStatus((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSave = async () => {
    try {
      await updateDailyStatus(id, status);
      navigate("/status");
    } catch (error) {
      navigate("/status");
    }
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div className="edit-status-page-wrapper">
      <div className="form-section add-status-form">
        <h4>Edit Status</h4>
        <input
          name="developerName"
          placeholder="Developer Name"
          value={status.developerName || ""}
          onChange={handleChange}
          readOnly
        />
        <input
          name="submissionDate"
          type="date"
          value={status.submissionDate?.slice(0, 10) || ""}
          onChange={handleChange}
        />
        <input
          name="taskDetails"
          placeholder="Task Details"
          value={status.taskDetails || ""}
          onChange={handleChange}
        />
        <input
          name="didYesterday"
          placeholder="Did Yesterday"
          value={status.didYesterday || ""}
          onChange={handleChange}
        />
        <input
          name="doingToday"
          placeholder="Doing Today"
          value={status.doingToday || ""}
          onChange={handleChange}
        />
        <input
          name="blockers"
          placeholder="Blockers"
          value={status.blockers || ""}
          onChange={handleChange}
        />
        <button onClick={handleSave} className="add-status-btn"> Save </button>
        <button onClick={() => navigate("/status")} className="cancel-status-btn"> Cancel</button>
      </div>
    </div>
  );
};

export default EditStatusPage;
