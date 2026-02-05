# MSWMS API Documentation

This folder contains documentation for the MSWMS HTTP API and internal application services.

Files:
- `endpoints.md`
- `services.md`

Quick links:
- Base URL: `https://0.0.0.0:5262`
- Swagger UI: `/swagger`
- SignalR hub: `/api/scanhub`

Authentication summary:
- Supported auth: JWT Bearer or Cookie (`MSWMS.Auth`).
- JWT header: `Authorization: Bearer <token>`
- Cookie auth is set by `/api/Auth/login` or `/api/Auth/register`.
- Unauthorized and forbidden responses are returned as HTTP 403 with JSON `{ "message": "..." }`.

Authorization policies (by role name):
- `RequireAdmin`: `Admin`
- `RequireManager`: `Manager`, `Admin`
- `RequirePicker`: `Picker`, `Manager`, `Admin`
- `RequireDispatcher`: `Dispatcher`, `Admin`
- `RequireLoadingOperator`: `LoadingOperator`, `Manager`, `Dispatcher`, `Admin`
- `RequireManagerOrDispatcher`: `Manager`, `Dispatcher`, `Admin`
- `RequireManagerOrPicker`: `Manager`, `Picker`, `Admin`
- `RequireDispatcherOrLoadingOperator`: `Dispatcher`, `LoadingOperator`, `Admin`
