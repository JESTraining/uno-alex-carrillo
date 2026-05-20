import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import type { Issue } from "../types/issue";
import { issueService } from "../services/issueService";
import { attachmentService } from "../services/attachmentService";
import { IssueForm } from "../components/issues/IssueForm";
import { FileUpload } from "../components/attachments/FileUpload";

export const IssueDetailPage = () => {
  const { id } = useParams();

  const [issue, setIssue] = useState<Issue | null>(null);

  const [loading, setLoading] = useState(true);

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
    values: Partial<Issue>
  ) => {
    if (!id) return;

    await issueService.update(id, values);

    await loadIssue(id);
  };

  const handleUpload = async (
    file: File
  ) => {
    if (!id) return;

    await attachmentService.upload(
      id,
      file
    );

    await loadIssue(id);
  };

  const handleRemoveAttachment = (
    attachmentId: string
  ) => {
    if (!issue) return;

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
      <h1>Issue Detail</h1>

      <IssueForm
        issue={issue}
        onSubmit={handleUpdate}
      />

      <hr />

      <h2>Attachments</h2>

      <FileUpload
        onUpload={handleUpload}
      />

      <div
        style={{
          display: "grid",
          gridTemplateColumns:
            "repeat(auto-fit, minmax(200px, 1fr))",
          gap: "16px",
          marginTop: "16px",
        }}
      >
        {issue.attachments.map(
          (attachment) => {
            const isImage =
              attachment.fileUrl.match(
                /\.(jpg|jpeg|png|gif|webp)$/i
              );

            return (
              <div
                key={attachment.id}
                style={{
                  border:
                    "1px solid #ddd",
                  padding: "12px",
                  borderRadius: "8px",
                }}
              >
                {isImage ? (
                  <img
                    src={attachment.fileUrl}
                    alt={
                      attachment.fileName
                    }
                    style={{
                      width: "100%",
                      height: "160px",
                      objectFit: "cover",
                      borderRadius: "6px",
                    }}
                  />
                ) : (
                  <p>
                    {
                      attachment.fileName
                    }
                  </p>
                )}

                <button
                  style={{
                    marginTop: "12px",
                  }}
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
  );
};