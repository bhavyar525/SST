import React, { useState } from 'react';
import { login } from '../../api/authApi';
import { useNavigate, Link } from 'react-router-dom';

export default function LoginForm() {
  const [formData, setFormData] = useState({
    userName: '',
    password: ''
  });

  const [message, setMessage] = useState('');
  const [token, setToken] = useState(null);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');
    try {
      const res = await login(formData);

      localStorage.setItem("token", res.data.token);
      localStorage.setItem("id", res.data.id);
      localStorage.setItem("userName", res.data.userName);

      setToken(res.data.token);
      setMessage(`Welcome, ${res.data.userName}!`);
      setFormData({ userName: '', password: '' });

      navigate("/status");
    } catch (error) {
      setMessage(error.response?.data?.message || 'Login failed');
      setFormData({ userName: '', password: '' });
    }
  };

  return (
    <form onSubmit={handleSubmit} autoComplete="off" >
      <h1>Scrum StandUp Tracker</h1>
      <h2>Login</h2>
      
      <input
        type="text"
        name="userName"
        autoComplete="user"
        placeholder="Username"
        value={formData.userName}
        onChange={(e) => setFormData({ ...formData, userName: e.target.value })}
        required
      />
      <br />
      <input
        type="password"
        name="password"
        autoComplete="cur-pass"
        placeholder="Password"
        value={formData.password}
        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
        required
      />
      <br />
      <button type="submit">Login</button>

      <p>
        Don't have an account? <Link to="/register">Register here</Link>
      </p>

      {message && <p>{message}</p>}
      {token && (
        <div>
          <h4>Your Token:</h4>
          <textarea
            value={token}
            readOnly
            style={{ width: '100%', height: 80 }}
          />
        </div>
      )}
    </form>
  );
}