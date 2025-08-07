import React, { useState } from 'react';
import { register } from '../../api/authApi';
import { Link } from 'react-router-dom';
import '../../css/Register.css' 

export default function RegisterForm() {
  const [formData, setFormData] = useState({
    userName: '',
    email: '',
    password: ''
  });

  const [message, setMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage('');
    try {
      await register(formData);
      setMessage('Registration successful! You can now log in.');
      setFormData({ userName: '', email: '', password: '' });
    } catch (error) {
      setMessage(error.response?.data?.message || 'Registration failed');
       setFormData({ userName: '', email: '', password: '' });
    }
  };

  return (
    <form onSubmit={handleSubmit} autoComplete="off">
      <h1>Scrum StandUp Tracker</h1>
      <h2>Register</h2>
      <input
        type="text"
        placeholder="Username"  autoComplete="user-name"
        value={formData.userName}
        onChange={(e) => setFormData({ ...formData, userName: e.target.value })}
        required
      />
      <br />
      <input
        type="email"
        placeholder="Email"  autoComplete="new-email"
        value={formData.email}
        onChange={(e) => setFormData({ ...formData, email: e.target.value })}
        required
      />
      <br />
      <input
        type="password"
        placeholder="Password"  autoComplete="new-password"
        value={formData.password}
        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
        required
      />
      <br />
      <button type="submit">Register</button>
        <p>
        Already have an account? <Link to="/login">Login here</Link>
      </p>

      {message && <p id="register-message">{message}</p>}
    </form>
  );
}