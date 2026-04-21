import { BoundingBox } from "./bounding_box";

export interface PlateSightingDto {
  id: number;
  plate: string;
  issueState?: string;
  timestamp: string;
  frameNumber: number;
  confidence: number;
  videoId: number;
  frameUrl: string;
  boundingBox: BoundingBox;
}