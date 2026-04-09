import { useEffect, useState } from "react";
import { getVideos, VideoDto } from "../api/videos";

export default function VideosPage() {
  const [videos, setVideos] = useState<VideoDto[]>([]);
  const [loading, setLoading] = useState(true);

  // Step I: Status filter
  const [statusFilter, setStatusFilter] = useState("all");

  useEffect(() => {
    getVideos().then((data) => {
      setVideos(data);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div className="text-muted-foreground">Loading videos...</div>;
  }

  // Apply filter
  const filteredVideos =
    statusFilter === "all"
      ? videos
      : videos.filter((v) => v.processingStatus === statusFilter);

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6">Videos</h1>

      {/* Status Filter */}
      <div className="mb-4 flex items-center gap-3">
        <label className="text-sm font-medium text-muted-foreground">
            Filter by status:
         </label>

        <select
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
            className="
                bg-card
                border border-border
                text-foreground
                rounded-md
                px-3 py-2
                focus:outline-none
                focus:ring-2 focus:ring-ring
            "
        >
            <option value="">All</option>
            <option value="Pending">Pending</option>
            <option value="Processing">Processing</option>
            <option value="Completed">Completed</option>
            <option value="Failed">Failed</option>
        </select>
      </div>

      {/* Videos Table */}
      <div className="bg-card border border-border rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-muted text-muted-foreground">
            <tr>
              <th className="px-4 py-3">File Name</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3">Uploaded</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>

          <tbody>
            {filteredVideos.map((v) => (
              <tr key={v.id} className="border-b hover:bg-muted/50">
                <td className="px-4 py-3">{v.fileName}</td>
                <td className="px-4 py-3">{v.processingStatus}</td>
                <td className="px-4 py-3">
                  {new Date(v.uploadTime).toLocaleString()}
                </td>
                <td className="px-4 py-3 text-right">
                  <a
                    href={`/videos/${v.id}`}
                    className="text-primary hover:underline"
                  >
                    View
                  </a>
                </td>
              </tr>
            ))}

            {filteredVideos.length === 0 && (
              <tr>
                <td
                  colSpan={4}
                  className="px-4 py-6 text-center text-muted-foreground"
                >
                  No videos match this filter.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}