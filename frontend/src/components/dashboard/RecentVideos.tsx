import { VideoDto } from "@/api/videos";

export function RecentVideos({ videos }: { videos: VideoDto[] }) {
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
