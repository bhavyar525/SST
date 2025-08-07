import axios from "axios";

const API_BASE = "https://localhost:7079/api/DailyStatus";

// Helper to get Authorization headers
const getAuthHeaders = () => {
  const token = localStorage.getItem("token");
  return token
    ? {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    : {};
};

// Get all statuses
export const getAllStatus = async () => {
  try {
    const response = await axios.get(API_BASE, getAuthHeaders());
    return response.data;
  } catch (error) {
    console.error("Error fetching all statuses:", error);
    throw error;
  }
};

// Get status by ID
export const getDailyStatusById = async (id) => {
  try {
    const response = await axios.get(`${API_BASE}/${id}`, getAuthHeaders());
    return response.data;
  } catch (error) {
    console.error("Error fetching status by ID:", error);
    throw error;
  }
};

// Get statuses by developer name (URL-encoded)
export const getMyStatusByName = async (name) => {
  try {
    const encodedName = encodeURIComponent(name);
    const response = await axios.get(`${API_BASE}/developer/${encodedName}`, getAuthHeaders());
    return response.data;
  } catch (error) {
    console.error("Error fetching status by name:", error);
    throw error;
  }
};

// Get statuses by date
export const getStatusByDate = async (dateString) => {
  try {
    const response = await axios.get(`${API_BASE}/date/${dateString}`, getAuthHeaders());
    return response.data;
  } catch (error) {
    console.error("Error fetching status by date:", error);
    throw error;
  }
};

// Add a new daily status
export const postDailyStatus = async (data) => {
  try {
    const response = await axios.post(API_BASE, data, getAuthHeaders());
    
    return response.data;
  } catch (error) {
    console.error("Failed posting daily status:", error);
    throw error;
  }
};

// Update status by ID
export const updateDailyStatus = async (id, data) => {
  try {
    const response = await axios.put(`${API_BASE}/${id}`, data, getAuthHeaders());
    console.log("update",response.data);
    
    return response.data;
  } catch (error) {
    console.error("Update failed:", error);
    throw error;
  }
};

// Delete status by ID
export const deleteDailyStatus = async (id) => {
  try {
    const response = await axios.delete(`${API_BASE}/${id}`, getAuthHeaders());
    return response.data;
  } catch (error) {
    console.error("Failed deleting daily status:", error);
    throw error;
  }
};