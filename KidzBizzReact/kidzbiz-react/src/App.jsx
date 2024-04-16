import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./components/Home";
import Login from "./components/Login";
import Register from "./components/Register";


function App() {
  return (
    <>
    {/*<Lobi/>*/}
   
      <div>
        <Routes>
          <Route path="/" element={<Home />}></Route>
          <Route path="/Register" element={<Register />}></Route>
          <Route path="/Login" element={<Login />}></Route>
        </Routes>
      </div>
    
    </>
  );
}

export default App;
