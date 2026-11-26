# ApplicationTracker

ApplicationTracker to aplikacja webowa do rejestrowania i analizowania aplikacji wysyłanych podczas poszukiwania pracy. Pozwala przechowywać informacje o ofertach, śledzić kolejne etapy rekrutacji oraz przeglądać statystyki i rekomendacje na jednym dashboardzie.

## Najważniejsze funkcjonalności

### Zarządzanie aplikacjami

- dodawanie aplikacji z informacjami o firmie, stanowisku, lokalizacji, trybie pracy, wynagrodzeniu, źródle oferty, linku i notatkach;
- wyświetlanie listy aplikacji oraz szczegółów wybranej aplikacji;
- edycja i usuwanie aplikacji;
- ochrona przed dodaniem aplikacji z takim samym adresem URL;
- przechowywanie historii zmian statusu wraz z datą i opcjonalną notatką;
- automatyczne ustawienie statusu `Applied` przy tworzeniu aplikacji.

### Statusy i historia rekrutacji

Aplikacja obsługuje następujące statusy:

- `Applied` – aplikacja wysłana;
- `Interview` – etap rozmowy rekrutacyjnej;
- `Offer` – otrzymana oferta;
- `Rejected` – odrzucenie kandydatury;
- `Withdrawn` – wycofanie aplikacji.

Bieżący status jest wyznaczany na podstawie najnowszego wpisu w historii statusów.

### Wyszukiwanie, filtrowanie i sortowanie

Lista aplikacji umożliwia:

- wyszukiwanie po nazwie firmy lub stanowisku;
- filtrowanie po statusie, firmie, źródle i trybie pracy;
- filtrowanie po zakresie `Ghost Score`;
- sortowanie aplikacji, domyślnie od najnowszej daty aplikacji;
- szybkie przejście do szczegółów, edycji lub usunięcia aplikacji.

Obsługiwane tryby pracy to `Remote`, `Hybrid` i `Office`.

### Dashboard i analityka

Dashboard prezentuje między innymi:

- łączną liczbę aplikacji, ofert i podejrzanych ogłoszeń;
- skuteczność rekrutacji;
- rozkład aplikacji według statusów;
- lejek rekrutacyjny: aplikacje, odpowiedzi, rozmowy i oferty;
- oś czasu procesu rekrutacyjnego;
- analizę źródeł aplikacji;
- statystyki i porównania firm;
- ostatnią aktywność;
- rekomendacje dotyczące aplikacji;
- analizę wskaźnika `Ghost Score`.

### Wykrywanie potencjalnych „ghost jobs”

Aplikacje są analizowane pod kątem sygnałów mogących wskazywać na nieaktywne lub wielokrotnie publikowane ogłoszenie. Analiza uwzględnia między innymi:

- historię aplikacji do tej samej firmy;
- brak odpowiedzi;
- powtarzające się publikowanie oferty;
- przebieg rozmowy i otrzymanie oferty.

Na podstawie reguł obliczany jest wynik `Ghost Score` oraz poziom ryzyka:

- niski – wynik poniżej 40;
- średni – wynik od 40 do 59;
- wysoki – wynik od 60.

### Eksport danych

Z poziomu listy aplikacji można pobrać dane w formacie CSV. Eksport zawiera firmę, stanowisko, bieżący status, źródło, tryb pracy oraz datę aplikacji.

## Technologie

- .NET 10;
- ASP.NET Core MVC;
- Entity Framework Core 10;
- Microsoft SQL Server / SQL Server Express;
- Razor Views;
- xUnit do testów.

## Wymagania

Przed uruchomieniem projektu należy zainstalować:

- .NET 10 SDK;
- SQL Server lub SQL Server Express;
- opcjonalnie Visual Studio 2026 z obciążeniem **ASP.NET and web development**.

## Konfiguracja

Domyślne połączenie z bazą danych znajduje się w `ApplicationTracker/appsettings.json`:

```json
{
  "ConnectionStrings": {
	"ApplicationTracker": "Server=.\\SQLEXPRESS;Database=ApplicationTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

Jeżeli SQL Server działa pod inną nazwą lub na innym serwerze, należy zmienić wartość `ConnectionStrings:ApplicationTracker`. Dla lokalnych ustawień deweloperskich można użyć pliku `appsettings.Development.json`.

## Uruchomienie

1. Sklonuj repozytorium i przejdź do katalogu projektu:

   ```powershell
   git clone https://github.com/hgrzes2411/ApplicationTracker.git
   cd ApplicationTracker
   ```

2. Przywróć zależności i zbuduj rozwiązanie:

   ```powershell
   dotnet restore
   dotnet build
   ```

3. Uruchom aplikację:

   ```powershell
   dotnet run --project .\ApplicationTracker\ApplicationTracker.csproj
   ```

   Przy pierwszym uruchomieniu aplikacja automatycznie zastosuje dostępne migracje Entity Framework Core.

4. Otwórz adres HTTPS lub HTTP wyświetlony w terminalu.

## Migracje bazy danych

Migracje znajdują się w `ApplicationTracker/Data/Migrations`. Aplikacja wywołuje `Database.Migrate()` podczas startu, dlatego zwykle nie trzeba uruchamiać ich ręcznie. Można jednak zarządzać nimi z poziomu .NET CLI:

```powershell
dotnet ef migrations list --project .\ApplicationTracker\ApplicationTracker.csproj
dotnet ef database update --project .\ApplicationTracker\ApplicationTracker.csproj
```

## Testy

Projekt testów znajduje się w `ApplicationTracker.Tests` i korzysta z xUnit. Uruchomienie testów:

```powershell
dotnet test
```

## Struktura projektu

- `ApplicationTracker/Controllers` – kontrolery MVC dla widoków głównych, aplikacji i dashboardu;
- `ApplicationTracker/Models` – encje domenowe, statusy, filtry i modele analityczne;
- `ApplicationTracker/Services` – logika aplikacji, statystyki, dashboard, eksport CSV i wykrywanie ghost jobs;
- `ApplicationTracker/Data` – kontekst Entity Framework Core i migracje;
- `ApplicationTracker/Views` – widoki Razor;
- `ApplicationTracker/wwwroot` – arkusze CSS i skrypty JavaScript;
- `ApplicationTracker.Tests` – testy automatyczne.

## Architektura danych

Główną encją jest `JobApplication`, która posiada kolekcję `StatusHistory`. Usunięcie aplikacji usuwa również powiązaną historię statusów. Dane są przechowywane w bazie SQL Server, a konfiguracja usług i zależności odbywa się w `Program.cs`.

## Licencja

W repozytorium nie określono jeszcze licencji projektu.
