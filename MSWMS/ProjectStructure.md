```
MSWMS/
 ├── Controllers/         # API контроллеры (если используешь ControllerBase)
 ├── Endpoints/           # Если остаёшься на Minimal API – сюда выносят маршруты
 ├── Models/              # DTO, Request/Response классы
 ├── Entities/            # Сущности БД (EF Core, Dapper и т.д.)
 ├── Services/            # Бизнес-логика
 ├── Repositories/        # Доступ к данным
 ├── Infrastructure/      # Вспомогательные классы (логгеры, мапперы, провайдеры)
 ├── Program.cs           # Точка входа
 ├── appsettings.json     # Конфиги
 ```