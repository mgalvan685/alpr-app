# Hotplates

Hotplates is a high‑performance hotlist and sighting tracking service designed for modern ALPR (Automatic License Plate Recognition) systems. It provides fast lookups, structured hotlist management, and a complete historical record of sightings — enabling real‑time alerts, investigative workflows, and tactical intelligence dashboards.

Hotplates is built for speed, clarity, and operational reliability, using a clean .NET backend, a normalized relational schema, and predictable API contracts.

---

## 🚀 Features

- **Hotlist Management**
  - Create, update, and expire hotlist entries
  - Categorize entries (e.g., stolen, BOLO, wanted, custom categories)
  - Track severity, source, and metadata

- **Sighting History**
  - Every encounter is logged as a `HotlistSighting`
  - Includes timestamp, GPS coordinates, device ID, confidence, and raw metadata
  - Supports investigative queries and timeline reconstruction

- **Real‑Time Readiness**
  - Designed for low‑latency lookups during ALPR inference
  - Supports immediate alerting workflows

- **Clean API Layer**
  - Typed DTOs
  - Predictable REST endpoints
  - Easy integration with ALPR engines, workers, and dashboards

- **Production‑Ready Architecture**
  - Normalized schema
  - Index‑optimized queries
  - Clear separation of hotlist definitions vs. sighting events

---

## 🧱 Architecture Overview

Hotplates consists of two core domain models:

### **HotlistEntry**
Represents a plate that should trigger alerts.

Fields include:
- `id`
- `plate`
- `state`
- `category`
- `description`
- `source`
- `severity`
- `createdAt`
- `expiresAt`
- `lastSeenTimestamp`
- `lastSeenLatitude`
- `lastSeenLongitude`
- `lastSeenDeviceId`

### **HotlistSighting**
Represents each time a hotlisted plate is detected.

Fields include:
- `id`
- `hotlistEntryId`
- `plate`
- `state`
- `timestamp`
- `latitude`
- `longitude`
- `confidence`
- `imageUrl`
- `source`
- `deviceId`
- `rawMetadata`

This separation allows:
- clean auditing
- fast historical queries
- reliable alerting
- accurate last‑seen enrichment

---

## 📁 Folder Structure

/hotplates
/src
/Hotplates.Api        # REST API
/Hotplates.Core       # Domain models, services, interfaces
/Hotplates.Data       # EF Core context, migrations, repositories
/tests
/Hotplates.Tests      # Unit tests
README.md

---

## ⚙️ Setup

### **1. Install Dependencies**
- .NET 8 SDK
- SQL Server / PostgreSQL (depending on your configuration)
- EF Core Tools

### **2. Configure Environment Variables**

Create a `.env` or use your preferred secrets store.

Required values:
- `DB_CONNECTION_STRING`
- `ASPNETCORE_ENVIRONMENT`
- `JWT_SECRET` (if auth enabled)

### **3. Run Migrations**

```bash
dotnet ef database update
```

### **4. Start the API**

```bash
dotnet run --project src/Hotplates.Api
```

API will start on:
http://localhost:5000


## 🔌 API Endpoints
Hotlist Entries
GET /api/hotlist — list all entries

POST /api/hotlist — create entry

GET /api/hotlist/{id} — get entry

PUT /api/hotlist/{id} — update entry

DELETE /api/hotlist/{id} — delete entry

Sightings
POST /api/hotlist/{id}/sightings — record a sighting

GET /api/hotlist/{id}/sightings — list sightings for an entry

GET /api/sightings/recent — recent sightings across all entries

🧪 Development Workflow
Run tests:
```bash
dotnet test
```

Add a migration:
```bash
dotnet ef migrations add <Name>
```

🗺️ Roadmap
🔔 Real‑time alerting via WebSockets

📡 Device‑level analytics

🧠 Plate grouping / clustering

🌎 Geofenced hot zones

📊 Dashboard widgets for investigative timelines

☁️ Cloud deployment templates (Docker + Kubernetes)

📜 License
MIT
