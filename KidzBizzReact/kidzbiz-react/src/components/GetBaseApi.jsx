// src/config/apiConfig.js

const getBaseApiUrl = () => {
    const hostname = window.location.hostname;
    if (hostname === "localhost" || hostname === "127.0.0.1") {
      return "https://localhost:7034/api/";
    } else {
      return "https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/";
    }
  };
  
  export default getBaseApiUrl;
  