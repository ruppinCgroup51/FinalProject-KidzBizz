import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./components/Home";
import Login from "./components/Login";
import Register from "./components/Register";
import ChooseAvatar from "./components/ChooseAvatar";
import Lobi from "./components/Lobi";
import GameGuide from "./components/GameGuide";

function App() {
  return (
    <> 

    <Routes>
    <Route path="/Lobi" element={<Lobi />}/>
    <Route path="/game-guide" element={<GameGuide />} />
    </Routes>
  
      <div>
      {/* צריך להחזיר את זה שהלוגין יעבוד ולבדוק שאחרי הלוגין זה מנווט ללובי*/}
       {/* <Routes>
          <Route path="/" element={<Home />}></Route>
          <Route path="/ChooseAvatar" element={<ChooseAvatar />}></Route>
          <Route path="/Register" element={<Register />}></Route>
          <Route path="/Login" element={<Login />}></Route>
        </Routes>*/}

      </div>
    </>
  );
}

export default App;
