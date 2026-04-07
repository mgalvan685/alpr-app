import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import {
  getSightingsForPlate,
  PlateSightingDto
} from "../api/plates";

export default function PlateDetailPage() {
  const { plate } = useParams();
  const [sightings, setSightings] = useState<PlateSightingDto[]>([]);
  const [loading, setLoading] = useState(true);

  // Step I: Date range filters
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  useEffect(() => {
    if (!plate) return;

    getSightingsForPlate(plate).then((data) => {
      setSightings(data);
      setLoading(false);
    });
  }, [plate]);

  if (loading) {
    return <div className="text-gray-600">Loading plate...</div>;
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
      <div className="flex gap-4 mb-6">
        <div>
          <label className="block text-gray-700 mb-1">Start Date</label>
          <input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className="border rounded px-3 py-2"
          />
        </div>

        <div>
          <label className="block text-gray-700 mb-1">End Date</label>
          <input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="border rounded px-3 py-2"
          />
        </div>
      </div>

      {/* Sightings Table */}
      <div className="bg-white shadow rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-gray-100 text-gray-700">
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
              <tr key={s.id} className="border-b hover:bg-gray-50">
                <td className="px-4 py-3">{s.state}</td>
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
                    className="text-blue-600 hover:underline"
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
                  className="px-4 py-6 text-center text-gray-600"
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