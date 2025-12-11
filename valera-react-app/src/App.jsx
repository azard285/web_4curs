import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import ValeraList from './components/ValeraList';
import ValeraStats from './components/ValeraStats';
import Navbar from './components/Navbar';
import { authApi } from './services/authApi';
import './App.css';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);

  // При загрузке проверяем аутентификацию
  useEffect(() => {
    const checkAuth = () => {
      const authenticated = authApi.isAuthenticated();
      setIsAuthenticated(authenticated);
      if (authenticated) {
        setCurrentUser(authApi.getCurrentUser());
      }
      console.log('Auth check:', authenticated, 'User:', authApi.getCurrentUser());
    };
    
    checkAuth();
  }, []);

  const handleLogin = (userData) => {
    console.log('Login successful:', userData);
    setIsAuthenticated(true);
    setCurrentUser(userData);
  };

  const handleRegister = (userData) => {
    console.log('Register successful:', userData);
    setIsAuthenticated(true);
    setCurrentUser(userData);
  };

  const handleLogout = () => {
    console.log('Logging out');
    authApi.logout();
    setIsAuthenticated(false);
    setCurrentUser(null);
  };

  return (
    <Router>
      <div className="App">
        <Navbar 
          isAuthenticated={isAuthenticated} 
          currentUser={currentUser}
          onLogout={handleLogout}
        />
        
        <Routes>
          {/* Public routes */}
          <Route 
            path="/login" 
            element={
              !isAuthenticated ? 
                <Login onLogin={handleLogin} /> : 
                <Navigate to="/valeras" />
            } 
          />
          <Route 
            path="/register" 
            element={
              !isAuthenticated ? 
                <Register onRegister={handleRegister} /> : 
                <Navigate to="/valeras" />
            } 
          />
          
          {/* Protected routes */}
          <Route 
            path="/valeras" 
            element={
              isAuthenticated ? 
                <ValeraList user={currentUser} /> : 
                <Navigate to="/login" />
            } 
          />
          <Route 
            path="/valeras/:id" 
            element={
              isAuthenticated ? 
                <ValeraStats user={currentUser} /> : 
                <Navigate to="/login" />
            } 
          />
          
          {/* Default route */}
          <Route 
            path="/" 
            element={
              <Navigate to={isAuthenticated ? "/valeras" : "/login"} />
            } 
          />
        </Routes>
      </div>
    </Router>
  );
}

export default App;