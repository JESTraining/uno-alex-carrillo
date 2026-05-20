import { useForm } from "react-hook-form";
import type { Issue } from "../../types/issue";

type Props = {
  issue: Issue;
  onSubmit: (data: Partial<Issue>) => void;
};

export const IssueForm = ({
  issue,
  onSubmit,
}: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Issue>({
    defaultValues: issue,
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div>
        <label>Title</label>

        <input
          {...register("title", {
            required: true,
            minLength: 3,
          })}
        />

        {errors.title && (
          <p>
            Title must have at least 3 characters
          </p>
        )}
      </div>

      <button type="submit">
        Save
      </button>
    </form>
  );
};