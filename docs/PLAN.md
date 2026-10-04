# InternetAuction — план доработки (тесты и документация первыми)

Исходный проект собирается на `netcoreapp3.1` (снят с поддержки). Поэтому первым шагом сделан **отдельный модуль чистых правил**
`Rules/InternetAuction.Rules` (netstandard2.1, без БД и без ASP.NET), к которому написаны 84 теста. Ветка `spec` — заглушки + красные тесты,
ветка `reference` — готовая реализация.

## Что нашли в исходном коде (подтверждено чтением)

| # | Где | Проблема |
|---|---|---|
| 1 | `AccontController.LogIn` | `user != null & (...)` — одиночный `&` не прерывает вычисление: при неизвестном e-mail `user.PasswordHash` даёт NullReferenceException (ловится `catch`, пользователь видит пустую форму) |
| 2 | `AccontController.LogIn` | пароль хранится и сравнивается **открытым текстом** (`user.PasswordHash == collection.Password`) |
| 3 | `AccontController.LogIn` | при неверном пароле всё равно `RedirectToAction("Index","User")` — сообщение об ошибке теряется; нет ограничения числа попыток |
| 4 | `BiddingService.AddAsync` | нет ни одной проверки (минимальный шаг, владелец лота, время, отмена) и нет `SaveAsync()` |
| 5 | комментарии в `AccontController.Registration` | в XML-комментариях лежат строки, похожие на e-mail и пароль — убрать из истории и сменить, если они настоящие |
| 6 | платформа | `netcoreapp3.1` — без обновлений безопасности |

## Этапы

1. **Правила торгов** (`Money`, `AuctionLifecycle`, `BiddingRules`) — сделать тесты зелёными.
2. **Атомарность** (`AuctionBook`) — эталон поведения для БД: ставка проверяется и записывается под одной блокировкой.
3. **Миграция паролей** (`PasswordMigration`) — апгрейд «открытый текст → хэш» при первом успешном входе.
4. **Троттлинг входа** (`LoginThrottle`).
5. Подключение к приложению (см. `IMPLEMENTATION-SPEC.md`, раздел «Подключение»): `BiddingService.AddAsync` вызывает `BiddingRules.Decide`
   внутри транзакции с `RowVersion` на аукционе; `LogIn` использует `PasswordMigration` + `LoginThrottle`; закрытие по времени — `BackgroundService`.
6. Обновление до .NET 8, DI вместо `ServiceFactory`, интеграционные тесты (`WebApplicationFactory`), SignalR для живых ставок, CI/Docker.

## Как работать

```
cd Rules
dotnet test        # на ветке spec: 84 красных; цель — 84 зелёных
```
Порядок реализации — в `IMPLEMENTATION-SPEC.md`. Эталон — ветка `reference` (или папка `reference/` в запушенной версии).
