import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import {
  getVideo,
  getVideoMetadata,
  VideoDto
} from "../api/videos";

import {
  getPlateSightingsForVideo,
  PlateSightingDto
} from "../api/plates";

import { VideoSightings } from "../components/videos/VideoSightings";

export default function VideoDetailPage() {
  const { id } = useParams();
  const videoId = Number(id);

  const [video, setVideo] = useState<VideoDto | null>(null);
  const [metadata, setMetadata] = useState<any>(null);
  const [sightings, setSightings] = useState<PlateSightingDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function load() {
      try {
        const v = await getVideo(videoId);
        setVideo(v);

        const m = await getVideoMetadata(videoId);
        setMetadata(m);

        const s = await getPlateSightingsForVideo(videoId);
        setSightings(s);
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [videoId]);

  if (loading) {
    return <div className="text-muted-foreground">Loading video...</div>;
  }

  if (!video) {
    return <div className="text-red-600">Video not found.</div>;
  }

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6">{video.fileName}</h1>

      {/* Metadata Card */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <VideoMetadataCard video={video} metadata={metadata} />
      </div>

      {/* Sightings Table */}
      <div className="mt-10">
        <h2 className="text-xl font-semibold mb-4">Plate Sightings</h2>
        <VideoSightings sightings={sightings} />
      </div>
    </div>
  );
}

function VideoMetadataCard({ video, metadata }: any) {
  return (
    <div className="bg-card border border-border rounded-lg p-6">
      <h2 className="text-lg font-semibold mb-4">Metadata</h2>

      <div className="space-y-2 text-muted-foreground">
        <div><strong>Status:</strong> {video.processingStatus}</div>
        <div><strong>Resolution:</strong> {video.width}×{video.height}</div>
        <div><strong>FPS:</strong> {video.frameRate}</div>
        <div><strong>Duration:</strong> {video.durationSeconds}s</div>

        {metadata && (
          <>
            <div><strong>Codec:</strong> {metadata.codec}</div>
            <div><strong>Bitrate:</strong> {metadata.bitrate}</div>
            <div><strong>Format:</strong> {metadata.format}</div>
          </>
        )}
      </div>
    </div>
  );
}