import { useEffect, useState } from "react";
import { getAllPlates, PlateDto } from "../api/plates";

export default function PlatesPage() {
  const [plates, setPlates] = useState<PlateDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getAllPlates().then((data) => {
      setPlates(data);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div className="text-gray-600">Loading plates...</div>;
  }

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6">Plates</h1>

      <div className="bg-white shadow rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-gray-100 text-gray-700">
            <tr>
              <th className="px-4 py-3">Plate</th>
              <th className="px-4 py-3">State</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>

          <tbody>
            {plates.map((p) => (
              <tr key={p.id} className="border-b hover:bg-gray-50">
                <td className="px-4 py-3 font-mono">{p.plate}</td>
                <td className="px-4 py-3">{p.state}</td>
                <td className="px-4 py-3 text-right">
                  <a
                    href={`/plates/${p.plate}`}
                    className="text-blue-600 hover:underline"
                  >
                    View
                  </a>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}