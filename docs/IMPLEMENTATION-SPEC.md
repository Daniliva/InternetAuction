# InternetAuction.Rules — спецификация реализации

Все публичные методы на ветке `spec` бросают `NotImplementedException("TODO")`; подробные правила — в XML-комментариях рядом с каждым методом.
Тесты: `Rules/InternetAuction.Rules.Tests/BiddingTests.cs` и `AccountTests.cs`.

## Порядок (каждый шаг — свои тесты зелёные)

1. **`Money`** — `IsValidBidAmount`: >0, ≤ `MaxBid` (1 000 000 000), не более 2 знаков после запятой. `MinimumStep`: max(1.00, 1 % от цены с округлением ВВЕРХ до копеек) (50→1.00; 250→2.50; 333.33→3.34).
2. **`AuctionLifecycle`** — `StateAt`: отменён → `Cancelled` всегда; до `Start` — `Planned`; `[Start, End)` — `Active`; с `End` — `Closed`. `Winner`: наибольшая ставка, при равенстве — более ранняя; пусто → null. `ShouldClose`: состояние `Closed`.
3. **`BiddingRules.CurrentPrice` / `MinimumNextBid`** — без ставок минимум = стартовая цена, иначе цена + `MinimumStep`.
4. **`BiddingRules.Decide`** — проверки строго в порядке: сумма → отмена → не начался → закрыт → владелец лота → уже лидер → ниже минимума. Принятая ставка в последние 2 минуты переносит конец на `now + 2 мин` (никогда не раньше исходного конца). Время берётся из `now`, не из ставки.
5. **`AuctionBook`** — `Open` (end > start, повтор id → ArgumentException), `Place` (проверка и запись под одним `lock`), `Cancel` (идемпотентно), `Snapshot` (копия). Неизвестный id → `KeyNotFoundException`. Гонка двух одинаковых ставок: принимается ровно одна.
6. **`PasswordMigration`** — `LooksHashed` (Base64, ≥29 байт, первый байт 0x00/0x01), `Check` (см. XML-описание; «хэш как пароль» не должен пускать).
7. **`LoginThrottle`** — блокировка после N подряд неудач на 15 минут; ключи без учёта регистра; во время блокировки счётчик/срок не продлеваются; после истечения счёт с 1; успех сбрасывает счётчик, но не снимает активную блокировку.

## Подключение к приложению (после того как модуль зелёный)

- `BiddingService.AddAsync`: открыть транзакцию, загрузить аукцион + ставки, собрать `AuctionSnapshot`, вызвать `BiddingRules.Decide(…, DateTime.UtcNow)`; при `Accepted` — сохранить ставку и `NewEnd`, `SaveAsync()`; конкурентность — `[Timestamp] RowVersion` на аукционе и повтор при `DbUpdateConcurrencyException`. Отказ → `InternetException` с понятным текстом по `BidRejection`.
- `AccontController.LogIn`: `LoginThrottle.IsLocked` → отказ; `PasswordMigration.Check(user.PasswordHash, collection.Password, hasher)`; при `NewHash != null` сохранить его; при неудаче — `RegisterFailure`, вернуть форму с ошибкой (а не редирект); исправить `&` на `&&`. `IPasswordHasher` — обёртка над `Microsoft.AspNetCore.Identity.PasswordHasher<TUser>`.
- Закрытие по времени: `BackgroundService` раз в N секунд выбирает аукционы, для которых `ShouldClose`, и фиксирует `Winner`.

## Что проверено, а что нет

Проверено: модуль собирается и тестируется (84 теста) на .NET 8. Не проверено: основное решение на `netcoreapp3.1` здесь собрать нельзя, поэтому подключение к `BiddingService`/`AccontController` не выполнялось.
