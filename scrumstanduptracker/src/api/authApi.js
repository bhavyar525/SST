import axios from "axios";
const API_BASE = "https://localhost:7079/api/Auth";

//These are func's which talk to backend API over HTTP.
export const register = (data) => axios.post(`${API_BASE}/register`,data);
export const login = (data) => axios.post(`${API_BASE}/login`, data);