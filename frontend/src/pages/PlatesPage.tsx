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
    return <div className="text-muted-foreground">Loading plates...</div>;
  }

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6 text-foreground">Plates</h1>

      <div className="bg-card border border-border rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-muted text-muted-foreground">
            <tr>
              <th className="px-4 py-3">Plate</th>
              <th className="px-4 py-3">State</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>

          <tbody>
            {plates.map((p) => (
              <tr
                key={p.id}
                className="border-b border-border hover:bg-muted/50 transition-colors"
              >
                <td className="px-4 py-3 font-mono text-foreground">
                  {p.plate}
                </td>

                <td className="px-4 py-3 text-muted-foreground">
                  {p.issueState}
                </td>

                <td className="px-4 py-3 text-right">
                  <a
                    href={`/plates/${p.plate}`}
                    className="text-primary hover:text-primary/80 hover:underline"
                  >
                    View
                  </a>
                </td>
              </tr>
            ))}

            {plates.length === 0 && (
              <tr>
                <td
                  colSpan={3}
                  className="px-4 py-6 text-center text-muted-foreground"
                >
                  No plates found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}