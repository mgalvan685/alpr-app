import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getVideoById, VideoDto } from "../api/videos";
import { getPlateSightingsForVideo, PlateSightingDto } from "../api/plates";

export default function VideoDetailPage() {
  const { id } = useParams();
  const [video, setVideo] = useState<VideoDto | null>(null);
  const [sightings, setSightings] = useState<PlateSightingDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function load() {
      const v = await getVideoById(Number(id));
      const s = await getPlateSightingsForVideo(Number(id));

      setVideo(v);
      setSightings(s);
      setLoading(false);
    }

    load();
  }, [id]);

  if (loading) {
    return <div className="text-muted-foreground">Loading video...</div>;
  }

  if (!video) {
    return (
      <div className="text-muted-foreground">
        Video not found or failed to load.
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6 text-foreground">
        Video Details
      </h1>

      {/* Video Metadata */}
      <div className="bg-card border border-border rounded-lg p-6 mb-10">
        <div className="text-lg font-medium text-foreground mb-2">
          {video.fileName}
        </div>

        <div className="text-muted-foreground space-y-1">
          <div>
            <span className="font-medium text-foreground">Status:</span>{" "}
            {video.processingStatus}
          </div>
          <div>
            <span className="font-medium text-foreground">Uploaded:</span>{" "}
            {new Date(video.uploadTime).toLocaleString()}
          </div>
        </div>
      </div>

      {/* Sightings Table */}
      <h2 className="text-xl font-semibold mb-4 text-foreground">
        Sightings in This Video
      </h2>

      <div className="bg-card border border-border rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-muted text-muted-foreground">
            <tr>
              <th className="px-4 py-3">Plate</th>
              <th className="px-4 py-3">State</th>
              <th className="px-4 py-3">Timestamp</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>

          <tbody>
            {sightings.map((s) => (
              <tr
                key={s.id}
                className="border-b border-border hover:bg-muted/50 transition-colors"
              >
                <td className="px-4 py-3 font-mono text-foreground">
                  {s.plate}
                </td>

                <td className="px-4 py-3 text-muted-foreground">
                  {s.issueState}
                </td>

                <td className="px-4 py-3 text-muted-foreground">
                  {new Date(s.timestamp).toLocaleString()}
                </td>

                <td className="px-4 py-3 text-right">
                  <a
                    href={`/plates/${s.plate}`}
                    className="text-primary hover:text-primary/80 hover:underline"
                  >
                    View Plate
                  </a>
                </td>
              </tr>
            ))}

            {sightings.length === 0 && (
              <tr>
                <td
                  colSpan={4}
                  className="px-4 py-6 text-center text-muted-foreground"
                >
                  No sightings found for this video.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}