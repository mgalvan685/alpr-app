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
    return <div className="text-gray-600">Loading videos...</div>;
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
      <div className="mb-4">
        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="border rounded px-3 py-2"
        >
          <option value="all">All Statuses</option>
          <option value="Pending">Pending</option>
          <option value="Processing">Processing</option>
          <option value="Completed">Completed</option>
          <option value="Failed">Failed</option>
        </select>
      </div>

      {/* Videos Table */}
      <div className="bg-white shadow rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-gray-100 text-gray-700">
            <tr>
              <th className="px-4 py-3">File Name</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3">Uploaded</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>

          <tbody>
            {filteredVideos.map((v) => (
              <tr key={v.id} className="border-b hover:bg-gray-50">
                <td className="px-4 py-3">{v.fileName}</td>
                <td className="px-4 py-3">{v.processingStatus}</td>
                <td className="px-4 py-3">
                  {new Date(v.uploadTime).toLocaleString()}
                </td>
                <td className="px-4 py-3 text-right">
                  <a
                    href={`/videos/${v.id}`}
                    className="text-blue-600 hover:underline"
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
                  className="px-4 py-6 text-center text-gray-600"
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