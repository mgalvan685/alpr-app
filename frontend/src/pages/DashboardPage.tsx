import { useEffect, useState } from "react";
import { getVideos, VideoDto } from "../api/videos";
import {
  getAllPlates,
  getAllSightings,
  PlateDto,
  PlateSightingDto,
} from "../api/plates";
import { RecentSightings } from "@/components/dashboard/RecentSightings";
import { RecentVideos } from "@/components/dashboard/RecentVideos";
import { StatCard } from "@/components/dashboard/StatsCards";

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
      <div className="mb-10">
        <h2 className="text-xl font-semibold mb-4 text-foreground">
          Recent Sightings
        </h2>
        <RecentSightings sightings={recentSightings} />
      </div>
    </div>
  );
}
