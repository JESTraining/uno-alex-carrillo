import { Navigate } from "react-router-dom";

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

  return children;
};