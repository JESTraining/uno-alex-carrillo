import { ToastContainer } from "react-toastify";
import { AppRouter } from "./router/AppRouter";

import "react-toastify/dist/ReactToastify.css";
import "./styles/globals.css";

function App() {
  return (
    <>
      <AppRouter />

      <ToastContainer
        position="top-right"
      />
    </>
  );
}

export default App;
