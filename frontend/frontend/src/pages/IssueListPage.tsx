import { IssueTable } from "../components/issues/IssueTable";
import { useIssues } from "../hooks/useIssues";

export const IssueListPage = () => {
  const {
    data,
    loading,
    page,
    setPage,
  } = useIssues();

  if (loading) {
    return <p>Loading...</p>;
  }

  return (
    <div className="container">
      <h1>Issues</h1>

      <IssueTable
        issues={data?.items || []}
      />

      <div>
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
          Page {data?.page} of{" "}
          {data?.totalPages}
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
  );
};