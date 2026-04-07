import { useEffect, useState } from "react";
import { getVideos, VideoDto } from "../api/videos";
import { getAllPlates, getAllSightings, PlateDto, PlateSightingDto } from "../api/plates";

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
    return <div className="text-gray-600">Loading dashboard...</div>;
  }

  const recentVideos = videos.slice(0, 5);
  const recentSightings = sightings.slice(0, 5);

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6">Dashboard</h1>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
        <StatCard label="Total Videos" value={videos.length} />
        <StatCard label="Total Plates" value={plates.length} />
        <StatCard label="Total Sightings" value={sightings.length} />
      </div>

      {/* Recent Videos */}
      <div className="mb-10">
        <h2 className="text-xl font-semibold mb-4">Recent Videos</h2>
        <RecentVideos videos={recentVideos} />
      </div>

      {/* Recent Sightings */}
      <div>
        <h2 className="text-xl font-semibold mb-4">Recent Sightings</h2>
        <RecentSightings sightings={recentSightings} />
      </div>
    </div>
  );
}

function StatCard({ label, value }: { label: string; value: number }) {
  return (
    <div className="bg-white shadow rounded-lg p-6 text-center">
      <div className="text-3xl font-bold text-gray-900">{value}</div>
      <div className="text-gray-600 mt-2">{label}</div>
    </div>
  );
}

function RecentVideos({ videos }: { videos: VideoDto[] }) {
  return (
    <div className="bg-white shadow rounded-lg overflow-hidden">
      <table className="min-w-full text-left">
        <thead className="bg-gray-100 text-gray-700">
          <tr>
            <th className="px-4 py-3">File Name</th>
            <th className="px-4 py-3">Status</th>
            <th className="px-4 py-3"></th>
          </tr>
        </thead>
        <tbody>
          {videos.map((v) => (
            <tr key={v.id} className="border-b hover:bg-gray-50">
              <td className="px-4 py-3">{v.fileName}</td>
              <td className="px-4 py-3">{v.processingStatus}</td>
              <td className="px-4 py-3 text-right">
                <a href={`/videos/${v.id}`} className="text-blue-600 hover:underline">
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
    <div className="bg-white shadow rounded-lg overflow-hidden">
      <table className="min-w-full text-left">
        <thead className="bg-gray-100 text-gray-700">
          <tr>
            <th className="px-4 py-3">Plate</th>
            <th className="px-4 py-3">State</th>
            <th className="px-4 py-3">Timestamp</th>
          </tr>
        </thead>
        <tbody>
          {sightings.map((s) => (
            <tr key={s.id} className="border-b hover:bg-gray-50">
              <td className="px-4 py-3 font-mono">{s.plate}</td>
              <td className="px-4 py-3">{s.state}</td>
              <td className="px-4 py-3">{new Date(s.timestamp).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}