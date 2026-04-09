import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getAllSightings, PlateSightingDto } from "../api/plates";

export default function PlateDetailPage() {
  const { plate } = useParams();
  const [sightings, setSightings] = useState<PlateSightingDto[]>([]);
  const [loading, setLoading] = useState(true);

  // Step I: Date range filters
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  useEffect(() => {
    if (!plate) return;

    getAllSightings().then((data) => {
      setSightings(data);
      setLoading(false);
    });
  }, [plate]);

  if (loading) {
    return <div className="text-muted-foreground">Loading plate...</div>;
  }

  // Apply date filtering
  const filteredSightings = sightings.filter((s) => {
    const ts = new Date(s.timestamp).getTime();

    if (startDate && ts < new Date(startDate).getTime()) return false;
    if (endDate && ts > new Date(endDate).getTime()) return false;

    return true;
  });

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6">
        Plate: <span className="font-mono">{plate}</span>
      </h1>

      {/* Date Filters */}
      <div className="flex items-center gap-4 mb-6">
        <div className="flex flex-col">
          <label className="text-sm text-muted-foreground mb-1">
            Start Date
          </label>
          <input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className="
        bg-card
        text-foreground
        border border-border
        rounded-md
        px-3 py-2
        focus:outline-none
        focus:ring-2 focus:ring-ring
      "
          />
        </div>

        <div className="flex flex-col">
          <label className="text-sm text-muted-foreground mb-1">End Date</label>
          <input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="
        bg-card
        text-foreground
        border border-border
        rounded-md
        px-3 py-2
        focus:outline-none
        focus:ring-2 focus:ring-ring
      "
          />
        </div>
      </div>

      {/* Sightings Table */}
      <div className="bg-card border border-border rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-muted text-muted-foreground">
            <tr>
              <th className="px-4 py-3">State</th>
              <th className="px-4 py-3">Confidence</th>
              <th className="px-4 py-3">Frame</th>
              <th className="px-4 py-3">Timestamp</th>
              <th className="px-4 py-3">Video</th>
            </tr>
          </thead>

          <tbody>
            {filteredSightings.map((s) => (
              <tr key={s.id} className="border-b hover:bg-muted/50">
                <td className="px-4 py-3">{s.issueState}</td>
                <td className="px-4 py-3">
                  {(s.confidence * 100).toFixed(1)}%
                </td>
                <td className="px-4 py-3">{s.frameNumber}</td>
                <td className="px-4 py-3">
                  {new Date(s.timestamp).toLocaleString()}
                </td>
                <td className="px-4 py-3">
                  <a
                    href={`/videos/${s.videoId}`}
                    className="text-primary hover:underline"
                  >
                    View Video
                  </a>
                </td>
              </tr>
            ))}

            {filteredSightings.length === 0 && (
              <tr>
                <td
                  colSpan={5}
                  className="px-4 py-6 text-center text-muted-foreground"
                >
                  No sightings match this filter.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
