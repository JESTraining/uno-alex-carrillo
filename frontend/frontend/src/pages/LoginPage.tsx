import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { authService } from "../services/authService";
import type { LoginRequest } from "../types/auth";

export const LoginPage = () => {
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginRequest>();

  const onSubmit = async (
    data: LoginRequest
  ) => {
    const response =
      await authService.login(data);

    localStorage.setItem(
      "token",
      response.token
    );

    localStorage.setItem(
      "user",
      JSON.stringify(response.user)
    );

    navigate("/issues");
  };

  return (
    <div className="container">
      <div className="page-card">
        <div className="page-header">
          <div>
            <h1>Login</h1>
            <p className="page-subtitle">
              Access the issue tracker dashboard.
            </p>
          </div>
        </div>

        <form
          className="form form--narrow"
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className="form-field">
            <label>Email</label>

            <input
              type="email"
              {...register("email", {
                required: true,
              })}
            />

            {errors.email && (
              <p className="form-error">
                Email is required
              </p>
            )}
          </div>

          <div className="form-field">
            <label>Password</label>

            <input
              type="password"
              {...register("password", {
                required: true,
              })}
            />

            {errors.password && (
              <p className="form-error">
                Password is required
              </p>
            )}
          </div>

          <button type="submit">
            Login
          </button>
        </form>
      </div>
    </div>
  );
};
