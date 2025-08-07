import React, { useEffect, useState } from "react";
import { getAllStatus, getMyStatusByName, getStatusByDate } from "../../api/dailyStatusApi";
import { useNavigate } from "react-router-dom";
import FilterByName from "./FilterByName";
import FilterByDate from "./FilterByDate";
import "../../css/GetAllStatus.css";

const GetAllPage = () => {
  const [statuses, setStatuses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isFiltered, setIsFiltered] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    fetchAllStatuses();
  }, []);

  const fetchAllStatuses = async () => {
    setLoading(true);
    setError(null);
    setIsFiltered(false);
    try {
      const data = await getAllStatus();
      setStatuses(data || []);
    } catch (err) {
      setError("Failed to load statuses");
      setStatuses([]);
    } finally {
      setLoading(false);
    }
  };

  const handleFilterByName = async (name) => {
    setLoading(true);
    setError(null);
    setIsFiltered(true);
    try {
      const result = await getMyStatusByName(name);
      console.log("Filter by name result:", result); 
      if (!result || (Array.isArray(result) && result.length === 0)) {
        setError("No statuses found for developer: " + name);
        setStatuses([]);
      } else {
        setStatuses(result);
      }
    } catch (err) {
      setError("Error fetching statuses for developer: " + name);
      setStatuses([]);
    } finally {
      setLoading(false);
    }
  };

  const handleFilterByDate = async (dateString) => {
    setLoading(true);
    setError(null);
    setIsFiltered(true);
    try {
      const result = await getStatusByDate(dateString);
      console.log("Filter by date result:", result);
      if (!result || (Array.isArray(result) && result.length === 0)) {
        setError(`No statuses found for date: ${dateString}`);
        setStatuses([]);
      } else {
        setStatuses(result);
      }
    } catch (err) {
      setError(`Error fetching statuses for date: ${dateString}`);
      setStatuses([]);
    } finally {
      setLoading(false);
    }
  };

  const handleBackClick = () => {
    navigate("/status");
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("id");
    localStorage.removeItem("userName");
    navigate("/login");
  };

  if (loading) return <div>Loading statuses...</div>;
  if (error) return <div style={{ color: "red", marginBottom: '10px' }}>{error}</div>;

  return (
    <div className="status-container">
      <div className="status-header" style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h2>All Developer Statuses</h2>
        <div>
          <button type="button" className="back-btn" onClick={handleBackClick} >
            Back
          </button>
          <button type="button" className="logout-btn" onClick={handleLogout} >
            Logout
          </button>
        </div>
      </div>

      {/* Filters Container */}
      <div className="filters-container">
        <FilterByName onFilter={handleFilterByName} />
        <FilterByDate onFilter={handleFilterByDate} />
        <button type="button" className="clear-filters-btn" onClick={fetchAllStatuses}>
          Clear Filters
        </button>
      </div>

      {statuses.length === 0 ? (
        !loading && <p>{isFiltered ? "No matching statuses found." : "No statuses available."}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Developer</th>
              <th>Date</th>
              <th>Task Details</th>
              <th>Did Yesterday</th>
              <th>Doing Today</th>
              <th>Blockers</th>
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
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default GetAllPage;