import { apiGet } from "./apiClient";

export interface PlateDto {
  id: number;
  plate: string;
  state: string;
}

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

// Plates Controller
// TODO: Refine this. Eventually, this will just get too large and we'll want to filter this list
export function getAllPlates() {
  return apiGet<PlateDto[]>("/api/plates");
}

// TODO: This endpoint is a bit redundant with the one below, consider consolidating them
export function getPlatePlateSummaries() {
  return apiGet<PlateSummaryDto[]>("/api/plates/summaries");
}

export function getPlateByNumber(plate: string) {
  return apiGet<PlateSightingDto[]>(`/api/plates/byplate/${plate}`);
}

export function getPlateById(plate: string) {
  return apiGet<PlateSightingDto[]>(`/api/plates/${plate}`);
}

// Plate Sighting Controller
export function getAllSightings() {
  return apiGet<PlateSightingDto[]>("/api/platesighting");
}

export function getPlateSightingsForVideo(videoId: number) {
  return apiGet<PlateSightingDto[]>(`/api/platesighting/video/${videoId}`);
}

// Plate Summary Controller
export function getPlateSummaries(videoId: number) {
  return apiGet<PlateSummaryDto[]>(`/api/platesummary`);
}

export function getPlateSummaryByPlateNumber(plate: string) {
  return apiGet<PlateSummaryDto[]>(`/api/platesummary/${plate}`);
}