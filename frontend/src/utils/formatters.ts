export function formatResolution(width?: number, height?: number) {
  if (!width || !height) return "-";
  return `${width}×${height}`;
}

export function formatDuration(seconds: number) {
  return `${seconds.toFixed(1)}s`;
}