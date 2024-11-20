import "./index.css";
import Layout from "./Layout";
import { Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import About from "./pages/about/About";
import Register from "./pages/register/Register";
import Login from "./pages/login/Login";
import SearchPage from "./pages/search/SearchPage";
import Weather from "./pages/weather/Weather";
import ChangePassword from "./pages/changePassword/ChangePassword";
import RequireAuth from "./pages/changePassword/RequireAuth";
import SecurityBreachBanner from "./pages/changePassword/SecurityBreachBanner";
import RequirePasswordChange from "./pages/changePassword/RequirePasswordChange";

function App() {
  return (
    <AuthProvider>
      <SecurityBreachBanner />
      <Layout>
        <Routes>
          <Route
            path="/"
            element={
              <RequirePasswordChange>
                <SearchPage />
              </RequirePasswordChange>
            }
          />
          <Route
            path="/about"
            element={
              <RequirePasswordChange>
                <About />
              </RequirePasswordChange>
            }
          />
          <Route
            path="/register"
            element={
              <RequirePasswordChange>
                <Register />
              </RequirePasswordChange>
            }
          />
          <Route
            path="/login"
            element={
              <RequirePasswordChange>
                <Login />
              </RequirePasswordChange>
            }
          />
          <Route
            path="/change-password"
            element={
              <RequireAuth>
                <ChangePassword />
              </RequireAuth>
            }
          />
          <Route
            path="/weather"
            element={
              <RequirePasswordChange>
                <Weather />
              </RequirePasswordChange>
            }
          />
        </Routes>
      </Layout>
    </AuthProvider>
  );
}

export default App;
