import "./index.css";
import Layout from "./Layout";
import { Routes, Route } from "react-router-dom";
import About from "./pages/about/About";
import Register from "./pages/register/Register";

function App() {
  return (

    <Layout>
      <Routes>
        <Route path="/about" element={<About />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </Layout>
  );
}

export default App;
