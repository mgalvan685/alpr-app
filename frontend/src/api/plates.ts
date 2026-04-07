import { apiGet } from "./apiClient";

export interface PlateSummaryDto {
  plate: string;
  state: string;
  totalCount: number;
  lastSeen: string;
}

export interface PlateSightingDto {
  id: number;
  plate: string;
  state: string;
  timestamp: string;
  frameNumber: number;
  confidence: number;
  videoId: number;
}

export function getPlates() {
  return apiGet<PlateSummaryDto[]>("/api/plates");
}

export function getPlate(plate: string) {
  return apiGet<PlateSightingDto[]>(`/api/plates/${plate}`);
}

export function getPlateSightingsForVideo(videoId: number) {
  return apiGet<PlateSightingDto[]>(`/api/videos/${videoId}/plates`);
}

export function getSightingsForPlate(plate: string) {
  return apiGet<PlateSightingDto[]>(`/api/plates/${plate}/sightings`);
}

export interface PlateDto {
  id: number;
  plate: string;
  state: string;
}

export function getAllPlates() {
  return apiGet<PlateDto[]>("/api/plates");
}

export function getAllSightings() {
  return apiGet<PlateSightingDto[]>("/api/plates/sightings");
}