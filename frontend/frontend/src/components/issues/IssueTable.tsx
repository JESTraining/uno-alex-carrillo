import { Link } from "react-router-dom";
import type { Issue } from "../../types/issue";

type Props = {
  issues: Issue[];
};

export const IssueTable = ({
  issues,
}: Props) => {
  return (
    <div className="table-shell">
      <table className="issue-table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Priority</th>
            <th>Status</th>
            <th>Assignee</th>
          </tr>
        </thead>

        <tbody>
          {issues.length === 0 && (
            <tr>
              <td
                className="issue-table__empty"
                colSpan={4}
              >
                No issues found.
              </td>
            </tr>
          )}

          {issues.map((issue) => (
            <tr key={issue.id}>
              <td
                className="issue-table__title"
                data-label="Title"
              >
                <Link to={`/issues/${issue.id}`}>
                  {issue.title}
                </Link>
              </td>

              <td data-label="Priority">
                {issue.priority}
              </td>

              <td data-label="Status">
                {issue.status}
              </td>

              <td data-label="Assignee">
                {issue.assignee || "Unassigned"}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
