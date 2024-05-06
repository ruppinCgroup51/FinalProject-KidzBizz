import React, { useState } from "react";
import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./components/Home";
import Login from "./components/Login";
import Register from "./components/Register";
import ChooseAvatar from "./components/ChooseAvatar";
import Lobi from "./components/Lobi";
import GameGuide from "./components/GameGuide";
import GameBoard from "./components/GameBoard";
import UserContext from "./components/UserContext";
import PlayersRating from "./components/PlayersRating";

function App() {
  const [user, setUser] = useState(null);

  const handleLogin = (userData) => {
    setUser(userData);
  };

  return (
    <UserContext.Provider value={user}>
      <div>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/ChooseAvatar" element={<ChooseAvatar />} />
          <Route path="/Register" element={<Register />} />
          <Route path="/Login" element={<Login onLogin={handleLogin} />} />
          <Route path="/Lobi" element={<Lobi />} />
          <Route path="/game-guide" element={<GameGuide />} />
          <Route path="/GameBoard" element={<GameBoard />} />
          <Route path="/PlayersRating" element={<PlayersRating />} />
        </Routes>
      </div>
    </UserContext.Provider>
  );
}

export default App;
