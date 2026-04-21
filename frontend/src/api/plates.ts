import { PlateSightingDto } from "@/types/plate_sighting_dto";
import { PlateSummaryDto } from "@/types/plate_summary";
import { PlateDto } from "@/types/plate";
import { apiGet } from "./apiClient";

// Plates
export function getAllPlates() {
  return apiGet<PlateDto[]>("/api/plates");
}

export function getPlateSummaries() {
  return apiGet<PlateSummaryDto[]>("/api/plates/summaries");
}

// Sightings
export function getAllSightings() {
  return apiGet<PlateSightingDto[]>("/api/platesighting");
}

export function getSightingsForPlate(plate: string) {
  return apiGet<PlateSightingDto[]>(`/api/platesighting/plate/${plate}`);
}

export function getPlateSightingsForVideo(videoId: number) {
  return apiGet<PlateSightingDto[]>(`/api/platesighting/video/${videoId}`);
}

// Summaries
export function getPlateSummaryByPlateNumber(plate: string) {
  return apiGet<PlateSummaryDto>(`/api/platesummary/${plate}`);
}