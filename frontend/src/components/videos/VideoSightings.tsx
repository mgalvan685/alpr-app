import { PlateSightingDto } from "../../api/plates";

interface Props {
  sightings: PlateSightingDto[];
}

export function VideoSightings({ sightings }: Props) {
  if (sightings.length === 0) {
    return (
      <div className="bg-white shadow rounded-lg p-6 text-gray-600">
        No plate detections found for this video.
      </div>
    );
  }

  return (
    <div className="bg-white shadow rounded-lg overflow-hidden">
      <table className="min-w-full text-left">
        <thead className="bg-gray-100 text-gray-700">
          <tr>
            <th className="px-4 py-3">Plate</th>
            <th className="px-4 py-3">State</th>
            <th className="px-4 py-3">Confidence</th>
            <th className="px-4 py-3">Frame</th>
            <th className="px-4 py-3">Timestamp</th>
          </tr>
        </thead>

        <tbody>
          {sightings.map((s) => (
            <tr key={s.id} className="border-b hover:bg-gray-50">
              <td className="px-4 py-3 font-mono">{s.plate}</td>
              <td className="px-4 py-3">{s.state}</td>
              <td className="px-4 py-3">{(s.confidence * 100).toFixed(1)}%</td>
              <td className="px-4 py-3">{s.frameNumber}</td>
              <td className="px-4 py-3">{new Date(s.timestamp).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}