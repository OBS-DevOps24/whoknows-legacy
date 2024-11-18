import { Link } from 'react-router-dom';

const Footer = () => {
  return (
    <footer className="footer bg-gray-100 text-gray-600 text-sm p-2">
      <div className="flex justify-between items-center">
        <div className="flex-1">OBS © 2024</div>
        <div className="flex-1 text-center">
          <Link to="/about" className="text-blue-500 hover:text-blue-700">About</Link>
        </div>
        <div className="flex-1"></div>
      </div>
    </footer>
  );
};

export default Footer;