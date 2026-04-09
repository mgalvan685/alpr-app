import { useState } from "react";
import { uploadVideo } from "../api/videos";
import { useNavigate } from "react-router-dom";

export default function UploadPage() {
  const [file, setFile] = useState<File | null>(null);
  const [uploading, setUploading] = useState(false);
  const navigate = useNavigate();

  async function handleUpload() {
    if (!file) return;

    setUploading(true);

    try {
      const video = await uploadVideo(file);
      navigate(`/videos/${video.id}`);
    } catch (err) {
      console.error("Upload failed:", err);
      setUploading(false);
    }
  }

  function handleFileSelect(e: React.ChangeEvent<HTMLInputElement>) {
    if (e.target.files && e.target.files.length > 0) {
      setFile(e.target.files[0]);
    }
  }

  function handleDrop(e: React.DragEvent<HTMLDivElement>) {
    e.preventDefault();
    if (e.dataTransfer.files && e.dataTransfer.files.length > 0) {
      setFile(e.dataTransfer.files[0]);
    }
  }

  return (
    <div>
      <h1 className="text-2xl font-semibold mb-6 text-foreground">Upload Video</h1>

      {/* Upload Box */}
      <div
        onDragOver={(e) => e.preventDefault()}
        onDrop={handleDrop}
        className="
          bg-card
          border border-border
          rounded-lg
          p-10
          text-center
          cursor-pointer
          hover:bg-muted/50
          transition-colors
        "
      >
        <input
          type="file"
          accept="video/*"
          onChange={handleFileSelect}
          className="hidden"
          id="fileInput"
        />

        <label htmlFor="fileInput" className="cursor-pointer">
          <div className="text-lg text-foreground mb-2">
            {file ? file.name : "Drag & drop a video or click to select"}
          </div>
          <div className="text-sm text-muted-foreground">
            Supported formats: MP4, MOV, AVI
          </div>
        </label>
      </div>

      {/* Upload Button */}
      <div className="mt-6">
        <button
          onClick={handleUpload}
          disabled={!file || uploading}
          className="
            bg-primary
            text-primary-foreground
            px-6 py-3
            rounded-md
            font-medium
            disabled:opacity-50
            disabled:cursor-not-allowed
            hover:bg-primary/80
            transition-colors
          "
        >
          {uploading ? "Uploading..." : "Upload Video"}
        </button>
      </div>
    </div>
  );
}