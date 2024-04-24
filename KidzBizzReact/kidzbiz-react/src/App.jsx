import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./components/Home";
import Login from "./components/Login";
import Register from "./components/Register";
import ChooseAvatar from "./components/ChooseAvatar";
import Lobi from "./components/Lobi";
import GameGuide from "./components/GameGuide";
import GameBoard from "./components/GameBoard";

function App() {
  return (
    <>
      <div>
        <Routes>
          <Route path="/" element={<Home />}></Route>
          <Route path="/ChooseAvatar" element={<ChooseAvatar />}></Route>
          <Route path="/Register" element={<Register />}></Route>
          <Route path="/Login" element={<Login />}></Route>
          <Route path="/Lobi" element={<Lobi />} />
          <Route path="/game-guide" element={<GameGuide />} />
          <Route path="/GameBoard" element={<GameBoard />} />
        </Routes>
      </div>
    </>
  );
}

export default App;
