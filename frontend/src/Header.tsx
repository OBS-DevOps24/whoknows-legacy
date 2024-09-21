import { Link, useNavigate } from 'react-router-dom';
import { logoutUser } from './services/apiFacade';
import { useState } from 'react';

const Header = () => {
  const navigate = useNavigate();
  //const [user, setUser] = useState<string | null>(null);
  const [isLoggedIn, setIsLoggedIn] = useState(false);


  const handleLogout = () => {
    logoutUser().then(() => {
      setIsLoggedIn(false);
      navigate('/');
    });

  };

  return (
    <div className="navigation">
      <nav className="flex justify-between items-center p-4">
        <h1 className="text-2xl font-bold underline">
          <Link id="nav-logo" to="/">¿Who Knows?</Link>
        </h1>
        <div>
          {isLoggedIn ? (
            <button id="nav-logout" onClick={handleLogout} className="text-blue-500 hover:text-blue-700">
              Log out
            </button>
          ) : (
            <>
              <Link id="nav-register" to="/register" className="text-blue-500 hover:text-blue-700 mr-4">Register</Link>
              <Link id="nav-login" to="/login" className="text-blue-500 hover:text-blue-700">Log in</Link>
            </>
          )}
        </div>
      </nav>
    </div>
  );
};

export default Header;