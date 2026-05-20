import { useNavigate } from "react-router-dom";

export const Navbar = () => {
  const navigate = useNavigate();

  const user = JSON.parse(
    localStorage.getItem("user") || "{}"
  );

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    navigate("/login");
  };

  return (
    <nav
      style={{
        display: "flex",
        justifyContent:
          "space-between",
        alignItems: "center",
        padding: "16px 24px",
        borderBottom:
          "1px solid #ddd",
      }}
    >
      <h2>Issue Tracker</h2>

      <div
        style={{
          display: "flex",
          alignItems: "center",
          gap: "12px",
        }}
      >
        <span>
          Welcome {user.name}
        </span>

        <button
          onClick={handleLogout}
        >
          Logout
        </button>
      </div>
    </nav>
  );
};