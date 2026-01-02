# Kanban Board (ASP.NET Core)

Projekt przedstawia prostą tablicę **Kanban** z trzema predefiniowanymi kolumnami:

- Todo
- Doing
- Done

Aplikacja umożliwia:
- przeglądanie zadań w kolumnach,
- dodawanie nowych zadań,
- przenoszenie zadań pomiędzy kolumnami,
- zachowanie kolejności zadań (`ord`) po odświeżeniu strony.

Backend został zrealizowany w **ASP.NET Core Web API**, a frontend jako **minimalny HTML + JavaScript** (bez frameworków).

---

## Funkcjonalności

### Kolumny
- Trzy predefiniowane kolumny: `Todo`, `Doing`, `Done`
- Kolumny posiadają pole `ord` określające kolejność
- Kolumny nie są tworzone ani edytowane przez API (inicjalizacja przez SQL)

### Zadania
- Dodawanie zadań (`title`, `colId`)
- Automatyczne ustawianie `ord = MAX + 1` w danej kolumnie
- Przenoszenie zadania do innej kolumny (`colId`, `ord`)
- Stabilna kolejność zadań po przenoszeniu

---

## Model danych

```sql
Columns(
  Id   INT PRIMARY KEY,
  Name NVARCHAR(50),
  Ord  INT
)

Tasks(
  Id    INT PRIMARY KEY,
  Title NVARCHAR(200),
  ColId INT → Columns.Id,
  Ord   INT
)
```
- Columns – predefiniowane kolumny tablicy Kanban
- Tasks – zadania przypisane do kolumn przez `ColId`
- Unikalność `(ColId, Ord)` gwarantuje stabilną kolejność zadań

---

## Technologie

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- HTML + JavaScript (bez frameworków)

---

## Uruchomienie projektu

- Sklonuj repozytorium
- Skonfiguruj połączenie do SQL Server w `appsettings.json`
- Utwórz bazę danych i uruchom skrypt SQL inicjalizujący kolumny
- Uruchom aplikację:
```
dotnet run
```
Aplikacja dostępna pod adresem:
```
https://localhost:XXXX
```
Swagger:
```
https://localhost:XXXX/swagger/
```
UI:
```
https://localhost:XXXX/index.html
```

XXXX odpowiada portowi, który może się różnić przy każdym uruchomieniu.

---

## UI

Minimalny interfejs użytkownika znajduje się w:
```
wwwroot/index.html
```
UI umożliwia:
- wyświetlanie kolumn Kanban
- przeglądanie zadań w kolumnach
- przenoszenie zadań do kolejnej kolumny
- automatyczne odświeżanie widoku po operacjacach
