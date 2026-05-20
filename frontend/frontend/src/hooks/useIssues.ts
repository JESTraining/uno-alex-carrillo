import { useEffect, useState } from "react";
import { issueService } from "../services/issueService";
import type { Issue } from "../types/issue";
import type { PaginatedResponse } from "../types/pagination";

export const useIssues = () => {
  const [data, setData] =
    useState<
      PaginatedResponse<Issue>
    >();

  const [loading, setLoading] =
    useState(true);

  const [page, setPage] =
    useState(1);

  useEffect(() => {
    loadIssues();
  }, [page]);

  const loadIssues = async () => {
    try {
      setLoading(true);

      const response =
        await issueService.getAll(
          page,
          10 // or whatever your default page size is
        );

      setData(response);
    } finally {
      setLoading(false);
    }
  };

  return {
    data,
    loading,
    page,
    setPage,
  };
};