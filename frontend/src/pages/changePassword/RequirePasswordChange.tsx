import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function RequirePasswordChange({
  children,
}: {
  children: JSX.Element;
}) {
  const auth = useAuth();

  const location = useLocation();

  if (auth.isLoggedIn && auth.isPasswordExpired) {
    return (
      <Navigate to="/change-password" state={{ from: location }} replace />
    );
  }

  return children;
}
