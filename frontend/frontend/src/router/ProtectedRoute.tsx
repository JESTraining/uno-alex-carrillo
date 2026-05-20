import { Navigate } from "react-router-dom";
import { Navbar } from "../components/layout/Navbar";

type Props = {
  children: React.ReactNode;
};

export const ProtectedRoute = ({
  children,
}: Props) => {
  const token =
    localStorage.getItem("token");

  if (!token) {
    return (
      <Navigate to="/login" />
    );
  }

  return (
    <>
      <Navbar />
      {children}
    </>
  );
};
