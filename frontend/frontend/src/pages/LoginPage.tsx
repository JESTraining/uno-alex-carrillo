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
      <h1>Login</h1>

      <form
        onSubmit={handleSubmit(onSubmit)}
      >
        <div>
          <label>Email</label>

          <input
            type="email"
            {...register("email", {
              required: true,
            })}
          />

          {errors.email && (
            <p>Email is required</p>
          )}
        </div>

        <div>
          <label>Password</label>

          <input
            type="password"
            {...register("password", {
              required: true,
            })}
          />

          {errors.password && (
            <p>Password is required</p>
          )}
        </div>

        <button type="submit">
          Login
        </button>
      </form>
    </div>
  );
};