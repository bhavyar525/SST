import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import RegisterForm from "./components/Auth/RegisterForm";
import LoginForm from "./components/Auth/LoginForm";
import MyStatusPage from './components/Status/MyStatusPage';
import GetAllPage from './components/Status/GetAllPage';
import EditStatusPage from './components/Status/EditStatusPage';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/register" element={<RegisterForm />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/all-statuses" element={<GetAllPage />} />
        <Route path="/status" element={<MyStatusPage/>} />
         <Route path="/edit-status/:id" element={<EditStatusPage />} />
      </Routes>
    </Router>
  );
}

export default App;