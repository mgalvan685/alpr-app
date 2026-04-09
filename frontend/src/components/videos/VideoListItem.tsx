import { VideoDto } from "../../api/videos";
import { formatDuration, formatResolution } from "../../utils/formatters";

interface Props {
  video: VideoDto;
}

export function VideoListItem({ video }: Props) {
  return (
    <tr className="border-b hover:bg-muted/50">
      <td className="px-4 py-3">{video.fileName}</td>
      <td className="px-4 py-3">
        {formatResolution(video.width, video.height)}
      </td>
      <td className="px-4 py-3">{video.frameRate ?? "-"}</td>
      <td className="px-4 py-3">
        {video.durationSeconds ? formatDuration(video.durationSeconds) : "-"}
      </td>
      <td className="px-4 py-3">
        <span className={`px-2 py-1 rounded text-sm ${statusColor(video.processingStatus)}`}>
          {video.processingStatus}
        </span>
      </td>
      <td className="px-4 py-3 text-right">
        <a
          href={`/videos/${video.id}`}
          className="text-primary hover:underline"
        >
          View
        </a>
      </td>
    </tr>
  );
}

function statusColor(status: string) {
  switch (status) {
    case "COMPLETED":
      return "bg-green-100 text-green-700";
    case "PROCESSING":
      return "bg-yellow-100 text-yellow-700";
    case "FAILED":
      return "bg-red-100 text-red-700";
    default:
      return "bg-muted text-muted-foreground";
  }
}