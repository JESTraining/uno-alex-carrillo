import { IssueTable } from "../components/issues/IssueTable";
import { useIssues } from "../hooks/useIssues";

export const IssueListPage = () => {
  const {
    data,
    loading,
    page,
    setPage,
  } = useIssues();

  const currentPage =
    data?.page && data.page > 0
      ? data.page
      : 1;

  const totalPages =
    data?.totalPages && data.totalPages > 0
      ? data.totalPages
      : 1;

  if (loading) {
    return (
      <div className="container">
        <p className="loading-state">
          Loading issues...
        </p>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="page-card">
        <div className="page-header">
          <div>
            <h1>Issues</h1>
            <p className="page-subtitle">
              Review, prioritize, and open issue details.
            </p>
          </div>
        </div>

        <IssueTable
          issues={data?.items || []}
        />

        <div className="pagination">
          <button
            disabled={
              !data?.hasPreviousPage
            }
            onClick={() =>
              setPage(page - 1)
            }
          >
            Previous
          </button>

          <span>
            Page {currentPage} of{" "}
            {totalPages}
          </span>

          <button
            disabled={
              !data?.hasNextPage
            }
            onClick={() =>
              setPage(page + 1)
            }
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
};
