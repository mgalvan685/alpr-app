import { VideoDto } from "../../api/videos";
import { VideoListItem } from "./VideoListItem";

interface Props {
  videos: VideoDto[];
}

export function VideoList({ videos }: Props) {
  return (
    <div className="bg-card border border-border rounded-lg overflow-hidden">
      <table className="min-w-full text-left">
        <thead className="bg-muted text-muted-foreground">
          <tr>
            <th className="px-4 py-3">File Name</th>
            <th className="px-4 py-3">Resolution</th>
            <th className="px-4 py-3">FPS</th>
            <th className="px-4 py-3">Duration</th>
            <th className="px-4 py-3">Status</th>
            <th className="px-4 py-3"></th>
          </tr>
        </thead>

        <tbody>
          {videos.map((v) => (
            <VideoListItem key={v.id} video={v} />
          ))}
        </tbody>
      </table>
    </div>
  );
}