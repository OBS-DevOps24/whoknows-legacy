import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from './context/AuthContext';

const Header = () => {
  const navigate = useNavigate();
  const { isLoggedIn, logout } = useAuth();

  const handleLogout = async () => {
    try {
      await logout();
      navigate('/');
    } catch (error) {
      console.error('Error logging out:', error);
    }
  };

  return (
    <div className="navigation">
      <nav className="flex items-center p-4">
        <div className="flex-1 flex justify-start">
          {/* This empty div maintains layout balance */}
        </div>
        <div className="flex-1 flex justify-center">
          <h1 className="text-2xl font-bold underline">
            <Link id="nav-logo" to="/">¿Who Knows?</Link>
          </h1>
        </div>
        <div className="flex-1 flex justify-end">
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