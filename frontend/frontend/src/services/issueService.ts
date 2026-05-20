import { axiosClient } from "./api/axiosClient";
import type { Issue } from "../types/issue";
import type { PaginatedResponse } from "../types/pagination";

export const issueService = {
  getAll: async (page: number, pageSize: number) => {
    const response = await axiosClient.get<PaginatedResponse<Issue>>(`/issues?page=${page}&pageSize=${pageSize}`);
    return response.data;
  },

  getById: async (id: string) => {
    const response = await axiosClient.get<Issue>(`/issues/${id}`);
    return response.data;
  },

  update: async (id: string, data: Partial<Issue>) => {
    const response = await axiosClient.put(
      `/issues/${id}`,
      data
    );

    return response.data;
  },
};