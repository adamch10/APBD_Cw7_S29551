## Jak uruchomić aplikację
1. Otwórz terminal w folderze projektu.
2. Wygeneruj bazę danych i tabele:  `dotnet ef database update`
3.  `dotnet run`

## Jak uruchomić testy
1. Przejdź do folderu z testami lub głównego folderu solucji. 
2. Wykonaj polecenie: `dotnet test`

## Gdzie jest co?
* **Baza danych:** Entity Framework Core połączony z bazą Microsoft SQL Server (LocalDB). Connection String znajduje się w pliku `appsettings.json`, a struktura bazy jest zarządzana przez migracje w folderze `Migrations`.
* **Middleware:** Klasa `RequestTimingMiddleware` w folderze `Middleware`, zarejestrowana w `Program.cs`.
* **Transakcja:** W pliku `Repositories/TicketRepository.cs` w metodzie `CreateTicketWithCommentAsync` (przy użyciu `BeginTransactionAsync`).
* **Testy:** Projekt z testami jednostkowymi xUnit (APBD_Cw7_S29551.Test)

## Odpowiedzi na pytania kontrolne

1. **Dlaczego kolejność middleware w Program.cs ma znaczenie?**
   Ponieważ middleware'y wykonują się w potoku (pipeline). Żądanie HTTP przechodzi przez nie w dół (w kolejności rejestracji), a odpowiedź wraca w górę. Błędna kolejność może zablokować poprawne wykonanie żądania – np. middleware autoryzacji uruchomiony przed CORS odrzuci legalne zapytania z przeglądarki, a middleware do logowania czasu dodany na samym końcu nie zmierzy faktycznego czasu trwania zapytania.

2. **Czym różni się app.Use od app.Run?**
   `app.Use` dodaje do potoku middleware, który wykonuje swoją logikę i może wywołać delegat `next()`, przekazując żądanie do kolejnego elementu. `app.Run` dodaje tzw. *terminal middleware*, który kończy przetwarzanie potoku i natychmiast zwraca odpowiedź (nie wywołuje `next()`).

3. **Dlaczego kontroler nie powinien zawierać całej logiki aplikacji?**
   Łamie to zasadę jednej odpowiedzialności (Single Responsibility Principle - SRP). Kontroler to jedynie warstwa prezentacji dla API. Ma przyjąć ruch, zwalidować wejście i zwrócić wynik. Jeśli zamkniemy w nim logikę biznesową, kod staje się niezwykle trudny do testowania jednostkowego i niemożliwy do użycia w innych częściach systemu.

4. **Co daje test jednostkowy warstwy Service?**
   Daje pewność, że główne reguły biznesowe aplikacji  działają poprawnie, w całkowitej izolacji od infrastruktury i warstwy sieciowej. Dzięki temu testy wykonują się w ułamku sekundy, są deterministyczne i ułatwiają bezpieczne refaktoryzowanie kodu.

5. **Co powinno się stać, jeśli zapis zgłoszenia się uda, ale zapis komentarza zakończy się błędem?**
   Dzięki zastosowaniu transakcji, jeśli zapis komentarza rzuci wyjątkiem, transakcja zostanie cofnięta (Rollback). Spowoduje to wycofanie wszystkich operacji w tej transakcji z bazy danych. W efekcie, osierocone zgłoszenie bez komentarza nie zostanie zapisane.
