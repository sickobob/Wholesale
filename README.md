## Запуск

```bash
cd src/Wholesale.Api
dotnet ef migrations add Initial -p ../Wholesale.Infrastructure -s .   # один раз
dotnet run
```

Документация API: `http://localhost:5000/scalar/v1`, схема — `/openapi/v1.json` (типизирована, пригодна для swagger-typescript-api и пр.).

Админ по умолчанию: `admin@wholesale.local` / `Admin123!` (настраивается в `appsettings.json`, секция `Seed`).
