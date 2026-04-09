import { PlateSightingDto } from "../../api/plates";

export function RecentSightings({
  sightings,
}: {
  sightings: PlateSightingDto[];
}) {
  if (!sightings || sightings.length === 0) {
    return (
      <div className="bg-card border border-border rounded-lg p-4 text-muted-foreground">
        No recent sightings found.
      </div>
    );
  }

  return (
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
              <td className="px-4 py-3 font-mono text-foreground">{s.plate}</td>
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
