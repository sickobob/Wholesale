# Wholesale Platform — тестовое задание

REST API платформы оптовых закупок. .NET 10, PostgreSQL, EF Core (code-first), Clean Architecture.

## Запуск

```bash
docker compose up -d          # PostgreSQL
cd src/Wholesale.Api
dotnet ef migrations add Initial -p ../Wholesale.Infrastructure -s .   # один раз
dotnet run
```

Документация API: `http://localhost:5000/scalar/v1`, схема — `/openapi/v1.json` (типизирована, пригодна для swagger-typescript-api и пр.).

Админ по умолчанию: `admin@wholesale.local` / `Admin123!` (настраивается в `appsettings.json`, секция `Seed`).

## Архитектура

- **Domain** — сущности, енумы, бизнес-правила (переходы статусов заказа инкапсулированы в `Order`).
- **Application** — CQRS на MediatR: команды/запросы по фичам, валидация AbstractValidator'ами через единый `ValidationBehavior`, AutoMapper-профили, интерфейсы инфраструктуры.
- **Infrastructure** — EF Core + Npgsql, generic-репозиторий + единый UnitOfWork, `AuditInterceptor` (аудит + soft-delete), JWT, PBKDF2-хешер, email-заглушка (лог).
- **Api** — контроллеры с типизированными ответами, permission-авторизация, ProblemDetails-middleware.

## Ключевые решения

- **Аудит и soft-delete** реализованы один раз в `AuditInterceptor` + глобальном query-фильтре: любая новая сущность-наследник `AuditableEntity` получает их автоматически.
- **Права (Permissions)** — справочник в БД + claims в JWT. Endpoint'ы закрыты `[HasPermission("...")]`; политики создаются динамически (`PermissionPolicyProvider`). Набор прав редактируется per-user; при регистрации выдаётся стандартный набор роли. Трейд-офф: изменение прав применяется при перевыпуске токена (время жизни — 60 мин). Альтернатива — читать права из БД в AuthorizationHandler.
- **Статусная модель заказа**: AwaitingPayment → Paid → Shipped → Completed; отмена возможна только из AwaitingPayment.
- **Снапшот цены** в `OrderItem.UnitPrice` — изменение цены товара не искажает историю заказов.
- **Email** — заглушка `LoggingEmailSender` за интерфейсом `IEmailSender`; замена на SMTP не затрагивает Application-слой.
