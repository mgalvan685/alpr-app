import { apiGet, apiPostForm } from "./apiClient";

export interface VideoDto {
  id: number;
  fileName: string;
  uploadTime: string;
  processingStatus: string;

  // Metadata fields (may be null if processing not complete)
  width?: number;
  height?: number;
  frameRate?: number;
  durationSeconds?: number;
}

export interface VideoMetadataDto {
  codec: string;
  bitrate: number;
  format: string;
  width: number;
  height: number;
  frameRate: number;
  durationSeconds: number;
}

/**
 * GET /api/videos
 * Returns all videos
 */
export function getVideos() {
  return apiGet<VideoDto[]>("/api/videos");
}

/**
 * GET /api/videos/{id}
 * Returns a single video record
 */
export function getVideo(id: number) {
  return apiGet<VideoDto>(`/api/videos/${id}`);
}

/**
 * GET /api/videos/{id}/metadata
 * Returns extracted metadata for a video
 */
export function getVideoMetadata(id: number) {
  return apiGet<VideoMetadataDto>(`/api/videos/${id}/metadata`);
}

/**
 * POST /api/videos/upload
 * Uploads a video file and returns the created VideoDto
 */
export async function uploadVideo(file: File): Promise<VideoDto> {
  const formData = new FormData();
  formData.append("file", file);

  return apiPostForm<VideoDto>("/api/videos/upload", formData);
}