import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function ChangePassword() {
  const [oldPassword, setOldPassword] = useState("");
  const [newPassword1, setNewPassword1] = useState("");
  const [newPassword2, setNewPassword2] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const [isChanged, setIsChanged] = useState(false);

  const { changePassword } = useAuth();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError("");

    if (newPassword1 !== newPassword2) {
      setError("New passwords do not match");
      return;
    }

    if (oldPassword === newPassword1) {
      setError("New password must be different from the old password");
      return;
    }

    try {
      await changePassword({ oldPassword, newPassword: newPassword1 });
      setIsChanged(true);
      // navigate("/");
    } catch (error) {
      if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("An unexpected error occurred");
      }
    }
  };

  const inputClassName =
    "peer block w-full rounded-md border border-gray-200 py-[9px] pl-3 text-sm outline-2 placeholder:text-gray-500";

  return (
    <div>
      <h2 className="mb-4 text-2xl font-bold">Change password</h2>
      {error && (
        <div
          className="relative px-4 py-3 mb-4 text-red-700 bg-red-100 border border-red-400 rounded"
          role="alert"
        >
          <strong>Error:</strong> {error}
        </div>
      )}
      {isChanged ? (
        <div>
          <div
            className="relative px-4 py-3 mb-4 text-green-700 bg-green-100 border border-green-400 rounded"
            role="alert"
          >
            Password changed successfully
          </div>
        </div>
      ) : (
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="password" className="sr-only">
              Password
            </label>
            <input
              type="password"
              id="password"
              className={inputClassName}
              placeholder="Old password"
              value={oldPassword}
              onChange={e => setOldPassword(e.target.value)}
              required
            />
          </div>
          <div>
            <label htmlFor="password" className="sr-only">
              Password
            </label>
            <input
              type="password"
              id="password"
              className={inputClassName}
              placeholder="New password"
              value={newPassword1}
              onChange={e => setNewPassword1(e.target.value)}
              required
            />
          </div>
          <div>
            <label htmlFor="password" className="sr-only">
              Password
            </label>
            <input
              type="password"
              id="password"
              className={inputClassName}
              placeholder="New password confirm"
              value={newPassword2}
              onChange={e => setNewPassword2(e.target.value)}
              required
            />
          </div>
          <div>
            <button
              type="submit"
              className="w-full px-4 py-2 text-white bg-blue-500 rounded-md hover:bg-blue-600"
            >
              Change password
            </button>
          </div>
        </form>
      )}
    </div>
  );
}
