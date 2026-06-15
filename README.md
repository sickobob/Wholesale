## Запуск

```bash
cd src/Wholesale.Api
dotnet ef migrations add Initial -p ../Wholesale.Infrastructure -s .   # один раз
dotnet run
```
Админ по умолчанию: `admin@wholesale.local` / `Admin123!` (настраивается в `appsettings.json`, секция `Seed`).
