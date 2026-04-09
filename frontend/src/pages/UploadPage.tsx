import { useState } from "react";
import { uploadVideo } from "../api/videos";
import { useNavigate } from "react-router-dom";

export default function UploadPage() {
  const [dragging, setDragging] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [progress, setProgress] = useState(0);

  const navigate = useNavigate();

  async function handleFiles(files: FileList | null) {
    if (!files || files.length === 0) return;

    const file = files[0];
    setUploading(true);

    try {
      // Optional: show fake progress for UX
      const interval = setInterval(() => {
        setProgress((p) => Math.min(p + 5, 90));
      }, 200);

      const video = await uploadVideo(file);

      clearInterval(interval);
      setProgress(100);

      // Redirect to the new video
      navigate(`/videos/${video.id}`);
    } catch (err) {
      console.error(err);
      alert("Upload failed");
      setUploading(false);
    }
  }

  function onDrop(e: React.DragEvent) {
    e.preventDefault();
    setDragging(false);
    handleFiles(e.dataTransfer.files);
  }

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6">Upload Video</h1>

      <div
        onDragOver={(e) => {
          e.preventDefault();
          setDragging(true);
        }}
        onDragLeave={() => setDragging(false)}
        onDrop={onDrop}
        className={`border-2 border-dashed rounded-lg p-10 text-center cursor-pointer transition
          ${dragging ? "border-blue-500 bg-blue-50" : "border-border"}
        `}
        onClick={() => document.getElementById("fileInput")?.click()}
      >
        <p className="text-muted-foreground">
          Drag & drop a video file here, or click to select
        </p>
      </div>

      <input
        id="fileInput"
        type="file"
        accept="video/*"
        className="hidden"
        onChange={(e) => handleFiles(e.target.files)}
      />

      {uploading && (
        <div className="mt-6">
          <div className="h-4 bg-muted rounded">
            <div
              className="h-4 bg-blue-600 rounded transition-all"
              style={{ width: `${progress}%` }}
            />
          </div>
          <p className="text-muted-foreground mt-2">{progress}%</p>
        </div>
      )}
    </div>
  );
}