import React from 'react';
import { Link } from 'react-router-dom';

const Navbar = ({ isAuthenticated, currentUser, onLogout }) => {
  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/">Valera Game</Link>
      </div>
      
      <div className="navbar-menu">
        {isAuthenticated ? (
          <>
            <span className="user-info">
              Welcome, {currentUser?.username} ({currentUser?.role})
            </span>
            <Link to="/valeras">My Valeras</Link>
            {currentUser?.role === 'Admin' && (
              <Link to="/valeras/all">All Valeras</Link>
            )}
            <button onClick={onLogout} className="logout-btn">
              Logout
            </button>
          </>
        ) : (
          <>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;