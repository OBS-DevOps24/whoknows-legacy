import "./index.css";
import Layout from "./Layout";
import { Routes, Route } from "react-router-dom";
import About from "./pages/about/About";

function App() {
  return (

    <Layout>
      <Routes>
        <Route path="/about" element={<About />} />
      </Routes>
    </Layout>
  );
}

export default App;
