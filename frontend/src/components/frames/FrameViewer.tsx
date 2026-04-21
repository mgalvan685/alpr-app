import { BoundingBox } from "@/types/bounding_box";

import React from "react";

interface FrameViewProps {
  frameUrl: string;
  box?: BoundingBox;
}

export function FrameView({ frameUrl, box }: FrameViewProps) {
  return (
    <div className="relative inline-block">
      <img src={frameUrl} alt="frame" className="max-w-full" />

      {box && (
        <div
          className="absolute border-2 border-red-500"
          style={{
            left: box.x,
            top: box.y,
            width: box.width,
            height: box.height
          }}
        />
      )}
    </div>
  );
}