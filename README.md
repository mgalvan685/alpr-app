# alpr

This project is a full‑stack Automatic License Plate Recognition (ALPR) system designed to process video, extract plate sightings, store structured encounter data, and present it through a tactical, mission‑ready dashboard. It combines a .NET backend, a React/shadcn‑powered frontend, and a modular pipeline for video ingestion, frame extraction, and plate analysis, giving you an end‑to‑end foundation similar to modern commercial ALPR platforms.

## Features

### 🔍 End‑to‑End ALPR Pipeline
A complete ingestion and analysis workflow: upload a video, extract frames, run ALPR detection, and store structured sightings with timestamps and metadata.

### 🎥 Video Upload & Processing
Users can upload MP4/MOV files through the dashboard. The backend registers a processing job, extracts frames via FFmpeg, and routes them through the ALPR engine.

### 🧠 Native C++ ALPR Engine
A high‑performance detection module built in C++17, integrated via P/Invoke. Supports:
- Plate localization  
- Character recognition  
- Confidence scoring  
- Bounding box output  

### 📊 Tactical Dashboard UI
A mission‑ready frontend built with React, Vite, Tailwind, and shadcn/ui. Includes:
- Video list and processing status  
- Plate sightings table  
- Plate detail pages with historical sightings  
- Clean, high‑contrast, operator‑focused design  

### 🗄️ Database‑Backed Sightings Store
All detections are persisted using EF Core and PostgreSQL, enabling:
- Fast queries  
- Historical lookups  
- Aggregated reporting  

### 🔧 Modular Backend Architecture
A .NET 8 API with:
- Typed DTOs  
- Background worker for long‑running jobs  
- Clean REST endpoints  
- Predictable, stable serialization  

### 📡 System Health & Metrics
Endpoints for monitoring:
- Worker status  
- Queue depth  
- Processing throughput  
- API health  

### 🧩 Clean Separation of Concerns
Backend, frontend, and ALPR engine live in isolated root‑level folders, enabling:
- Independent builds  
- Clear ownership boundaries  
- Easier debugging and deployment  

## Folder Structure

The repository is organized into three top‑level projects — backend API, frontend dashboard, and native ALPR engine — each isolated for clean builds, clear ownership, and predictable deployments.

.
├── backend/                # .NET 8 API, EF Core models, worker, controllers
│   ├── Controllers/        # REST endpoints (Videos, Plates, System)
│   ├── Services/           # Business logic and pipeline orchestration
│   ├── Workers/            # Background processing (FFmpeg + ALPR engine)
│   ├── DTOs/               # Request/response contracts
│   ├── Database/           # EF Core models, migrations, DbContext
│   ├── Infrastructure/     # Storage, configuration, helpers
│   └── appsettings*.json   # Environment configuration
│
├── frontend/               # React + Vite + Tailwind + shadcn/ui dashboard
│   ├── src/
│   │   ├── components/     # Reusable UI components
│   │   ├── pages/          # Route-level views (Videos, Plates, Dashboard)
│   │   ├── lib/            # API client, utilities, hooks
│   │   └── styles/         # Global styles and theme config
│   └── public/             # Static assets
│
├── alpr-engine/            # Native C++17 ALPR engine
│   ├── include/            # Public headers
│   ├── src/                # Detection + OCR implementation
│   ├── CMakeLists.txt      # Build configuration
│   └── build/              # Compiled output (DLL/SO/Dylib)
│
└── storage/                # Local development storage
    ├── videos/             # Uploaded video files
    └── frames/             # Extracted frames for processing

## Environment Variables

The system relies on a small set of environment variables to configure database access, storage paths, and ALPR engine integration. These can be set using `appsettings.Development.json`, user secrets, or your deployment environment.

### Backend (`/backend`)

| Variable | Description |
|---------|-------------|
| `ASPNETCORE_ENVIRONMENT` | Sets the environment (Development, Staging, Production). |
| `DATABASE_CONNECTION_STRING` | PostgreSQL connection string used by EF Core. |
| `STORAGE_ROOT` | Root directory for video uploads and extracted frames. |
| `FFMPEG_PATH` | Absolute path to the FFmpeg executable (if not on PATH). |
| `ALPR_ENGINE_PATH` | Path to the compiled ALPR engine shared library (DLL/SO/Dylib). |
| `FRAME_EXTRACTION_RATE` | Number of frames per second to extract during processing. |

### Frontend (`/frontend`)

| Variable | Description |
|---------|-------------|
| `VITE_API_BASE_URL` | Base URL of the backend API (e.g., `http://localhost:5000`). |

### ALPR Engine (`/alpr-engine`)

Most configuration is handled at compile time, but you may optionally define:

| Variable | Description |
|---------|-------------|
| `ALPR_DEBUG` | Enables verbose logging for engine development. |
| `ALPR_MODEL_PATH` | Path to detection/OCR model files (if externalized). |

---

### Example: Local Development Setup

```bash
# Backend
setx DATABASE_CONNECTION_STRING "Host=localhost;Port=5432;Database=alpr;Username=postgres;Password=postgres"
setx STORAGE_ROOT "C:\alpr-storage"
setx ALPR_ENGINE_PATH "C:\path\to\alpr-engine.dll"

# Frontend
setx VITE_API_BASE_URL "http://localhost:5000"
```

## Architecture

The system is organized into three independent but tightly integrated components: a .NET backend API, a React/shadcn‑powered frontend, and a native ALPR processing engine orchestrated by a background worker. Each component is isolated at the repository root to ensure clean builds, predictable deployments, and a Flock‑style separation of concerns.

### Backend (`/backend`)
The backend is a modular .NET API responsible for ingesting video jobs, managing plate sightings, and exposing a clean REST interface to the frontend. It includes:
- A database-backed sightings store with EF Core
- A job queue for video ingestion and processing
- A background worker that extracts frames and invokes the native ALPR engine
- DTO‑driven serialization for predictable, stable API responses

### Frontend (`/frontend`)
The frontend is a React + TypeScript application built with Vite and styled using Tailwind and shadcn/ui. It provides:
- A tactical, mission‑ready dashboard UI
- Pages for video uploads, sightings review, and system status
- A typed API client layer for clean communication with the backend
- A component system aligned with modern public‑safety SaaS design patterns

### ALPR Engine (`/alpr-engine`)
The ALPR engine is a native C++ module responsible for plate detection and character recognition. It is integrated into the .NET worker via P/Invoke and supports:
- Frame-by-frame analysis
- Plate bounding box extraction
- Confidence scoring and metadata output

### Processing Pipeline
Video ingestion flows through a multi‑stage pipeline:
1. **Upload** — User submits a video through the frontend.
2. **Job Creation** — Backend registers the job and queues it for processing.
3. **Frame Extraction** — Worker uses FFmpeg to extract frames at configured intervals.
4. **ALPR Analysis** — Each frame is passed to the native engine for detection.
5. **Persistence** — Valid sightings are stored in the database with timestamps and metadata.
6. **Presentation** — Frontend retrieves sightings and displays them in the dashboard.

This architecture mirrors the structure of modern commercial ALPR platforms: modular, scalable, and optimized for clear operational boundaries.

## API Endpoints

The backend exposes a clean, typed REST API for managing videos, processing jobs, and retrieving plate sightings. All endpoints return structured DTOs to ensure stable, predictable responses for the frontend.

### Videos

#### `GET /videos`
Returns a list of all uploaded videos with basic metadata.

#### `GET /videos/{id}`
Retrieves detailed metadata for a single video, including processing status and timestamps.

#### `POST /videos/upload`
Accepts a multipart/form-data upload and registers a new video ingestion job.

#### `GET /videos/{id}/download`
Downloads the original uploaded video file.

#### `GET /videos/{id}/metadata`
Returns extracted metadata for the video (duration, frame count, processing stats).

#### `DELETE /videos/{id}`
Deletes a video and all associated processing artifacts.

---

### Plates

#### `GET /plates`
Returns a list of all detected plate sightings across all videos.

#### `GET /plates/{plate}`
Retrieves all sightings for a specific license plate, ordered by timestamp.

#### `GET /plates/{plate}/latest`
Returns the most recent sighting for a specific plate.

---

### Health & System

#### `GET /health`
Basic health check for the API and worker.

#### `GET /system/status`
Returns worker status, queue depth, and processing metrics.

---

These endpoints form the contract between the frontend and backend, enabling clean separation of concerns and predictable integration with the ALPR processing pipeline.

## Setup

This project is organized as a multi‑component workspace with separate environments for the backend API, frontend dashboard, and native ALPR engine. Each component can be developed independently, but all three must be built and configured for the full system to run end‑to‑end.

### Prerequisites
- .NET 8 SDK  
- Node.js 20+ and npm  
- CMake + a C++17‑compatible compiler (for the ALPR engine)  
- FFmpeg installed and available on your system PATH  
- PostgreSQL 15+  

---

### 1. Backend Setup (`/backend`)

The backend will:
- Apply EF Core migrations
- Start the API server
- Initialize the job queue and background worker
Environment variables can be configured via appsettings.Development.json or user secrets.

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### 2. Frontend Setup (`/frontend`)

The frontend will start a Vite dev server and automatically proxy API requests to the backend.

```bash
cd frontend
npm install
npm run dev
```

### 3. ALPR Engine Setup (`/alpr-engine')

This produces a native shared library (DLL/SO/Dylib depending on OS) that the .NET worker loads via P/Invoke.
Make sure the compiled library is placed where the backend worker can access it (e.g., /backend/alpr-engine/ or a configured path)


```bash
cd alpr-engine
mkdir build && cd build
cmake ..
cmake --build . --config Release
```

### 4. FFmpeg Setup

The worker relies on FFmpeg for frame extraction. Ensure it is installed and accessible:

```bash
ffmpeg -version
```

### 5. Running the Full System

Start each component in separate terminals:

```bash
# Terminal 1
cd backend
dotnet run

# Terminal 2
cd frontend
npm install
npm run dev
```

Once running, open the dashboard in your browser and upload a video to begin processing.

### 6. Note To Self

I still need to add instructions for Docker setup.