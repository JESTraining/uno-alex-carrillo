import { useEffect, useState } from "react";
import {
  Link,
  useNavigate,
  useParams,
} from "react-router-dom";
import { toast } from "react-toastify";
import type {
  Issue,
  UpdateIssueRequest,
} from "../types/issue";
import { issueService } from "../services/issueService";
import { attachmentService } from "../services/attachmentService";
import { IssueForm } from "../components/issues/IssueForm";
import { FileUpload } from "../components/attachments/FileUpload";
import { useAssignees } from "../hooks/useAssignees";
import { resolveApiAssetUrl } from "../services/api/url";

const imageExtensions =
  /\.(jpg|jpeg|png|gif|webp|bmp|svg)$/i;

export const IssueDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [issue, setIssue] = useState<Issue | null>(null);

  const [loading, setLoading] = useState(true);

  const {
    assignees,
    loading: assigneesLoading,
  } = useAssignees();

  useEffect(() => {
    if (!id) return;

    loadIssue(id);
  }, [id]);

  const loadIssue = async (
    issueId: string
  ) => {
    try {
      const data =
        await issueService.getById(issueId);

      setIssue(data);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async (
    values: UpdateIssueRequest
  ) => {
    if (!id) return;

    await issueService.update(id, values);

    toast.success("Issue updated successfully");
    navigate("/issues");
  };

  const handleUpload = async (
    file: File
  ) => {
    if (!id) return;

    await attachmentService.upload(
      id,
      file
    );

    toast.success("Attachment uploaded successfully");
    await loadIssue(id);
  };

  const handleRemoveAttachment = async (
    attachmentId: string
  ) => {
    if (!issue || !id) return;

    await attachmentService.delete(
      id,
      attachmentId
    );

    toast.success("Attachment removed successfully");
    setIssue({
      ...issue,
      attachments:
        issue.attachments.filter(
          (attachment) =>
            attachment.id !== attachmentId
        ),
    });
  };

  if (loading) {
    return (
      <div className="container">
        <p>Loading issue...</p>
      </div>
    );
  }

  if (!issue) {
    return (
      <div className="container">
        <p>Issue not found</p>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="page-card">
        <div className="page-header">
          <div>
            <h1>Issue Detail</h1>
            <p className="page-subtitle">
              Update the issue and manage its attachments.
            </p>
          </div>

          <Link
            className="button button--secondary"
            to="/issues"
          >
            Back to issues
          </Link>
        </div>

        <IssueForm
          issue={issue}
          assignees={assignees}
          assigneesLoading={assigneesLoading}
          onSubmit={handleUpdate}
        />

        <hr className="divider" />

        <div className="section-header">
          <div>
            <h2>Attachments</h2>
            <p>
              Upload images related to this issue.
            </p>
          </div>
        </div>

        <FileUpload
          onUpload={handleUpload}
        />

        {issue.attachments.length === 0 && (
          <p className="empty-state">
            No attachments yet.
          </p>
        )}

        <div className="attachment-grid">
          {issue.attachments.map(
            (attachment) => {
              const isImage =
                imageExtensions.test(
                  attachment.fileName
                ) ||
                imageExtensions.test(
                  attachment.fileUrl
                );

              const fileUrl =
                resolveApiAssetUrl(
                  attachment.fileUrl
                );

              return (
                <div
                  className="attachment-card"
                  key={attachment.id}
                >
                  {isImage ? (
                    <a
                      className="attachment-preview"
                      href={fileUrl}
                      target="_blank"
                      rel="noreferrer"
                    >
                      <img
                        src={fileUrl}
                        alt={
                          attachment.fileName
                        }
                      />
                    </a>
                  ) : (
                    <p className="attachment-file">
                      Preview unavailable
                    </p>
                  )}

                  <p className="attachment-name">
                    {attachment.fileName}
                  </p>

                  <button
                    onClick={() =>
                      handleRemoveAttachment(
                        attachment.id
                      )
                    }
                  >
                    Remove
                  </button>
                </div>
              );
            }
          )}
        </div>
      </div>
    </div>
  );
};
