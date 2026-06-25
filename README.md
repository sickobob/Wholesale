## Запуск

```bash
cd src/Wholesale.Api
dotnet ef migrations add Initial -p ../Wholesale.Infrastructure -s .   # один раз
dotnet run
```
Админ по умолчанию: `admin@wholesale.local` / `Admin123!` (настраивается в `appsettings.json`, секция `Seed`).
<img width="1861" height="684" alt="image" src="https://github.com/user-attachments/assets/3c806bbf-9d83-4e4f-b820-2081b9a7e1dc" />
