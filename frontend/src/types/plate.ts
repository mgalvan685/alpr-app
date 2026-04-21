import { BoundingBox } from "@/types/bounding_box";

export interface PlateDto {
  id: number;
  plate: string;
  issueState?: string;
  boundingBox?: BoundingBox;
}