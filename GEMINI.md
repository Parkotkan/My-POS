# My_POS Project Instructions

This project is a modern Point of Sale (POS) system with a decoupled architecture: an ASP.NET Core 8.0 API backend and a Vue 3 frontend.

## 🏗️ Architecture & Core Principles

- **Decoupled Architecture**: The frontend and backend are completely separate. The backend serves only as a RESTful API, and the frontend is a standalone SPA.
- **Backend**: ASP.NET Core 8.0 using Entity Framework Core with SQL Server.
- **Frontend**: Vue 3 with Vite and Vue Router 4.
- **API First**: All business logic and data access must be handled via the API.

## 📂 Directory Structure

- `/Backend_API`: ASP.NET Core 8.0 Web API project.
- `/Frontend-vue`: Vue 3 frontend application.

## 🛠️ Development Workflow

### Backend (`/Backend_API`)
- **Port**: 5000 (HTTP), 5001 (HTTPS).
- **Commands**:
  - Restore: `dotnet restore`
  - Update Database: `dotnet ef database update`
  - Run: `dotnet run`
- **Configuration**: Connection strings are in `appsettings.json`.

### Frontend (`/Frontend-vue`)
- **Port**: 3000.
- **Commands**:
  - Install: `npm install`
  - Dev Mode: `npm run dev`
  - Build: `npm run build`
- **Configuration**: API URL is configured in `.env` as `VITE_API_URL`.

## 📝 Coding Conventions

### Backend (C# / .NET)
- Follow standard ASP.NET Core and EF Core patterns.
- Controllers should be in `Controllers/Api`.
- Models should be in `Models`.
- Data context is in `Data/ApplicationDbContext.cs`.
- Ensure CORS is configured in `Program.cs` to allow requests from the frontend.

### Frontend (Vue / JavaScript)
- Use Vue 3 Composition API where appropriate.
- Stylesheets are in `src/css`.
- App logic and components are in `src/js` and `src/views`.
- API calls should use the URL from `import.meta.env.VITE_API_URL`.

## 🔐 Security & Configuration

- Never commit sensitive information in `appsettings.json` or `.env`.
- Ensure SQL Server connection strings are properly configured for the local environment.
