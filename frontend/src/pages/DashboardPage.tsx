import { useEffect, useState } from "react";
import { getVideos, VideoDto } from "../api/videos";
import {
  getAllPlates,
  getAllSightings,
  PlateDto,
  PlateSightingDto,
} from "../api/plates";

export default function DashboardPage() {
  const [videos, setVideos] = useState<VideoDto[]>([]);
  const [plates, setPlates] = useState<PlateDto[]>([]);
  const [sightings, setSightings] = useState<PlateSightingDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function load() {
      const v = await getVideos();
      const p = await getAllPlates();
      const s = await getAllSightings();

      setVideos(v);
      setPlates(p);
      setSightings(s);
      setLoading(false);
    }

    load();
  }, []);

  if (loading) {
    return <div className="text-muted-foreground">Loading dashboard...</div>;
  }

  const recentVideos = videos.slice(0, 5);
  const recentSightings = sightings.slice(0, 5);

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6 text-foreground">Dashboard</h1>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
        <StatCard label="Total Videos" value={videos.length} />
        <StatCard label="Total Plates" value={plates.length} />
        <StatCard label="Total Sightings" value={sightings.length} />
      </div>

      {/* Recent Videos */}
      <div className="mb-10">
        <h2 className="text-xl font-semibold mb-4 text-foreground">
          Recent Videos
        </h2>
        <RecentVideos videos={recentVideos} />
      </div>

      {/* Recent Sightings */}
      <div>
        <h2 className="text-xl font-semibold mb-4 text-foreground">
          Recent Sightings
        </h2>
        <RecentSightings sightings={recentSightings} />
      </div>
    </div>
  );
}

function StatCard({ label, value }: { label: string; value: number }) {
  return (
    <div className="bg-card border border-border rounded-lg p-6 text-center">
      <div className="text-3xl font-bold text-foreground">{value}</div>
      <div className="text-muted-foreground mt-2">{label}</div>
    </div>
  );
}

function RecentVideos({ videos }: { videos: VideoDto[] }) {
  return (
    <div className="bg-card border border-border rounded-lg overflow-hidden">
      <table className="min-w-full text-left">
        <thead className="bg-muted text-muted-foreground">
          <tr>
            <th className="px-4 py-3">File Name</th>
            <th className="px-4 py-3">Status</th>
            <th className="px-4 py-3"></th>
          </tr>
        </thead>
        <tbody>
          {videos.map((v) => (
            <tr
              key={v.id}
              className="border-b border-border hover:bg-muted/50 transition-colors"
            >
              <td className="px-4 py-3 text-foreground">{v.fileName}</td>
              <td className="px-4 py-3 text-muted-foreground">
                {v.processingStatus}
              </td>
              <td className="px-4 py-3 text-right">
                <a
                  href={`/videos/${v.id}`}
                  className="text-primary hover:text-primary/80 hover:underline"
                >
                  View
                </a>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function RecentSightings({ sightings }: { sightings: PlateSightingDto[] }) {
  return (
    <div className="bg-card border border-border rounded-lg overflow-hidden">
      <table className="min-w-full text-left">
        <thead className="bg-muted text-muted-foreground">
          <tr>
            <th className="px-4 py-3">Plate</th>
            <th className="px-4 py-3">State</th>
            <th className="px-4 py-3">Timestamp</th>
          </tr>
        </thead>
        <tbody>
          {sightings.map((s) => (
            <tr
              key={s.id}
              className="border-b border-border hover:bg-muted/50 transition-colors"
            >
              <td className="px-4 py-3 font-mono text-foreground">{s.plate}</td>
              <td className="px-4 py-3 text-muted-foreground">{s.state}</td>
              <td className="px-4 py-3 text-muted-foreground">
                {new Date(s.timestamp).toLocaleString()}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}