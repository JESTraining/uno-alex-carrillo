import { Link } from "react-router-dom";
import type { Issue } from "../../types/issue";

type Props = {
  issues: Issue[];
};

export const IssueTable = ({
  issues,
}: Props) => {
  return (
    <table>
      <thead>
        <tr>
          <th>Title</th>
          <th>Priority</th>
          <th>Status</th>
          <th>Assignee</th>
        </tr>
      </thead>

      <tbody>
        {issues.map((issue) => (
          <tr key={issue.id}>
            <td>
              <Link to={`/issues/${issue.id}`}>
                {issue.title}
              </Link>
            </td>

            <td>{issue.priority}</td>

            <td>{issue.status}</td>

            <td>{issue.assignee}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};