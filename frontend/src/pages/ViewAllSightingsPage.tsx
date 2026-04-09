import { useEffect, useState } from "react";
import { getAllSightings, PlateSightingDto } from "../api/plates";

export default function ViewAllSightingsPage() {
  const [sightings, setSightings] = useState<PlateSightingDto[]>([]);
  const [loading, setLoading] = useState(true);

  // Pagination
  const pageSize = 20;
  const [page, setPage] = useState(1);

  useEffect(() => {
    getAllSightings().then((data) => {
      // Sort newest → oldest
      const sorted = data.sort(
        (a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime()
      );
      setSightings(sorted);
      setLoading(false);
    });
  }, []);

  if (loading) {
    return <div className="text-muted-foreground">Loading sightings...</div>;
  }

  // Pagination math
  const start = (page - 1) * pageSize;
  const end = start + pageSize;
  const pageItems = sightings.slice(start, end);

  const totalPages = Math.ceil(sightings.length / pageSize);

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6 text-foreground">
        All Sightings
      </h1>

      {/* Table */}
      <div className="bg-card border border-border rounded-lg overflow-hidden">
        <table className="min-w-full text-left">
          <thead className="bg-muted text-muted-foreground">
            <tr>
              <th className="px-4 py-3">Plate</th>
              <th className="px-4 py-3">State</th>
              <th className="px-4 py-3">Timestamp</th>
              <th className="px-4 py-3">Video</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>

          <tbody>
            {pageItems.map((s) => (
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

                <td className="px-4 py-3 text-muted-foreground">
                  {s.videoId}
                </td>

                <td className="px-4 py-3 text-right">
                  <a
                    href={`/videos/${s.videoId}`}
                    className="text-primary hover:text-primary/80 hover:underline mr-4"
                  >
                    View Video
                  </a>

                  <a
                    href={`/plates/${s.plate}`}
                    className="text-primary hover:text-primary/80 hover:underline"
                  >
                    View Plate
                  </a>
                </td>
              </tr>
            ))}

            {pageItems.length === 0 && (
              <tr>
                <td
                  colSpan={5}
                  className="px-4 py-6 text-center text-muted-foreground"
                >
                  No sightings found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {/* Pagination Controls */}
      <div className="flex items-center justify-between mt-6">
        <button
          onClick={() => setPage((p) => Math.max(1, p - 1))}
          disabled={page === 1}
          className="
            bg-muted
            text-muted-foreground
            px-4 py-2
            rounded-md
            disabled:opacity-40
            disabled:cursor-not-allowed
            hover:bg-muted/70
            transition-colors
          "
        >
          Previous
        </button>

        <div className="text-muted-foreground">
          Page <span className="text-foreground">{page}</span> of{" "}
          <span className="text-foreground">{totalPages}</span>
        </div>

        <button
          onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
          disabled={page === totalPages}
          className="
            bg-muted
            text-muted-foreground
            px-4 py-2
            rounded-md
            disabled:opacity-40
            disabled:cursor-not-allowed
            hover:bg-muted/70
            transition-colors
          "
        >
          Next
        </button>
      </div>
    </div>
  );
}