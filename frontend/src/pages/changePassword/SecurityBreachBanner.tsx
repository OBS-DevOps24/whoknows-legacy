import { Link } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function SecurityBreachBanner() {
  const auth = useAuth();
  const showBanner = !auth?.isLoggedIn || auth?.isPasswordExpired;

  if (!showBanner) return null;

  return (
    <div className="relative px-4 py-3 text-center text-red-700 bg-yellow-100 border border-yellow-400 rounded">
      ⚠️ <strong>Security Alert:</strong> We have detected a potential security
      breach. Please update your password immediately to secure your account.
      Click{" "}
      <Link className="font-bold underline" to={"/change-password"}>
        here
      </Link>{" "}
      to change your password.
    </div>
  );
}
