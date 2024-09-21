import "./index.css";
import Layout from "./Layout";
import { Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import About from "./pages/about/About";
import Register from "./pages/register/Register";
import Login from "./pages/login/Login";
import SearchPage from "./pages/search/SearchPage";

function App() {
  return (
    <AuthProvider>
      <Layout>
        <Routes>
          <Route path="/" element={<SearchPage />} />
          <Route path="/about" element={<About />} />
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
        </Routes>
      </Layout>
    </AuthProvider>
  );
}


export default App;
