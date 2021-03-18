--- 1. Instrukcja uruchomienia aplikacji ---

- Zainstaluj Visual Studio 2019
- Uruchom plik rozwiązania (DotNetWMS.sln)
- Zainstaluj poniższe biblioteki:
 - Microsoft.EntityFrameworkCore
 - Microsoft.EntityFrameworkCore.SqlServer
 - Microsoft.EntityFrameworkCore.Tools
 - Microsoft.VisualStudio.Web.CodeGeneration.Design
 - Microsoft.AspNetCore.Identity.EntityFrameworkCore
 - jQuery.Validation.Globalize
 - NLog.Web.AspNetCore
- W konsoli PMC wpisz 'Update-Database' w celu utworzenia bazy danych na podstawie wprowadzonych migracji
- Po uruchomieniu aplikacji wybierz „Zarejestruj się” (identyfikator składa się z 12 dowolnych cyfr)
- Pod poniższą ścieżką otwórz plik nlog-all-(shortdate) -> DotNetWMS\bin\Debug\netcoreapp3.0\nlog_logs
- Na dole będzie dostępny link aktywacyjny, należy go skopiować i uruchomić w przeglądarce internetowej
- Konto zostało aktywowane. Kliknij na przycisk „Zaloguj się”
- Wprowadź login – jest to 5 pierwszych liter nazwiska, 3 litery imienia i 4 cyfry identyfikatora
- Kliknij na „Listę ról” w sekcji „Panel Administracyjny”
- W roli „Moderator” wybierz „Przypisz użytkownika”
- Kliknij na checkbox obok nazwy użytkownika i kliknij „Zaktualizuj”
- Wyloguj się z aplikacji i zaloguj się ponownie
- Po zalogowaniu zostaną wyświetlone wszystkie opcje dostępne w aplikacji

--- 2. Karta Projektu ---

Zespół projektowy:

Kamil Szydłowski - .NetCore 3.0, C#, Backend
Krzysztof Wcisło - HTML/CSS/jQuery, Frontend


Cele:
Stworzenie prostej i intuicyjnej aplikacji webowej, umożliwiającej cyfrowe zarządzanie majątkiem firmy.

Cele szczegółowe:
- Wprowadzenie na rynek nowej aplikacji webowej WMS, skierowanej głównie do klientów z branży budowlanej, hurtowni i wypożyczalni.
- Rozwijanie aplikacji, w oparciu o własne doświadczenia oraz potrzeby i sugestie zgłaszane przez użytkowników
- Personalizowanie aplikacji w zależności od potrzeb różnych branż i rynków
- Doskonalenie kodu aplikacji, aby spełniała wymagania klientów (opisane poniżej) niezależnie od środowiska na którym będzie uruchamiana

Potrzeby klientów, na które ma odpowiadać aplikacja:
- podgląd stanu magazynów
- wymiana międzymagazynowa
- wydania zewnętrze
- przyjęcie towaru z zewnątrz
- inwentaryzacja
- przypisanie właściciela/opiekuna do składników majątku
- przypisanie czasowe do klienta zewnętrznego (wypożyczenie, dzierżawa)
- generowanie kodów kreskowych/QR
- obsługa skanera kodów kreskowych/QR
- obsługa baz danych (klienci, pracownicy, magazyny, sprzęt itp.)
- kontrola stanu sprzętu np. daty gwarancji, okresu leasingowego, statusu naprawy
- raportowanie z uwzględnieniem informacji istotnych dla klienta

Wymagania, jakie musi spełniać aplikacja:
- szybkość działania – przejście pomiędzy kolejnymi widokami nie powinno trwać dłużej niż 1 sekunda
- intuicyjność obsługi – osoby mające wcześniej styczność z systemami WMS nie powinny mieć problemu z opanowaniem podstawowych procesów biznesowych tuż po uruchomieniu aplikacji 
- niezawodność – aplikacja nie spowoduje błędu skutkującego usunięciem zasobów klienta z bazy danych, a także deszyfracją danych wrażliwych. Dopuszczalne są 2 błędy krytyczne (tj. skutkujące zatrzymaniem aplikacji) w ciągu roku, inne niż wymienione powyżej
- responsywność – GUI aplikacji powinno być dopasowane do urządzenia na którym jest program jest uruchamiany. Zmiana urządzenia przez klienta nie będzie mieć wpływu na wygodę korzystania z aplikacji
- komunikaty o błędach czytelne dla użytkownika – jeżeli podczas korzystania z aplikacji użytkownik wykona niedozwoloną operację, zostanie poinformowany o błędzie w jasny i precyzyjny sposób. Miernikiem powyższego będzie częstotliwość kontaktów klienta z prośbą o interpretacje błędów.

Plan realizacji:
- Stworzenie dokumentacji wraz z mockupem
- Projekt na platformie Azure
- Repozytorium na GitHub i zapewnienie dostępu dla członków zespołu
- Stworzenie klas (modeli) i konfiguracja pod Entity Framework Core
- Prototyp bazy danych do obsługi podstawowych funkcji
- Prototypy widoków w celu powiązania z akcjami i kontrolerami
- Dodawanie kolejnych kontrolerów i akcji
- Udoskonalanie bazy danych wraz z dodawaniem kolejnych funkcji
- Stworzenie bazy danych do obsługi uwierzytelnienia i autoryzacji w oparciu o IdentityDbContext
- Konfiguracja logowania z zewnętrznych serwisów np. Google, Facebook
- Wdrożenie wyszukiwarki z użyciem AJAX/jQuery
- Obsługa błędów (dodanie różnych widoków dla użytkownika i dewelopera)
- Testy jednostkowe
- Testy w Selenium
- Stopniowe wprowadzanie docelowego Layoutu do widoków
- Wdrożenie na platformę i testy końcowe
- Instrukcja obsługi

--- 3. Analiza SWOT, przegląd rynku ---

Analiza SWOT:

Mocne strony:
- niska cena
- prostota aplikacji
- brak potrzeby instalacji
- obsługa na desktopach i telefonach
- obsługa kodów QR
- elastyczne podejście do potrzeb klienta
- gotowość wsparcia 24/7

Słabe strony:
- nierozpoznawalna marka
- brak reprezentatywnej siedziby firmy
- niewielki zespół
- niewielkie zasoby finansowe, które mogą ograniczyć odpowiedź na potrzeby klienta

Szanse:
- Poprawiająca się sytuacja materialna Polaków
- Dobra koniunktura w produkcji, handlu i budownictwie (otwartość na usprawnienia i nowe technologie)
- Wzrost zatrudnienia (konieczność kontroli przydzielania majątku)
- Duża liczba inwestycji w sektorze budowlanym (atomizacja materiałów i sprzętu)
- Zwiększenie ilości oddziałów i zakładów firm z w/w branż (konieczność kontroli przepływu majątku między oddziałami)

Zagrożenia:
- istniejące rozwiązania (np. Comarch WMS, PWSK, WMS.net)
- przywiązanie firm do dotychczasowych rozwiązań
- firmy w podanych branżach często korzystają ze starszego sprzętu

Przegląd rynku:
Nazwa: AGILERO WMS, AGILERO WES
Link: https://www.agilero.pl/
Opis, możliwości: Agilero oferuje podział na systemy WMS i WES w zależności od klienta. Dobór jest poprzedzony wywiadem z klientem i określeniem jego potrzeb. System jest tworzony jest wspólnie z klientem, równolegle z procesem wdrożenia rozwiązań biznesowych
Wady: Oferta skierowana jest raczej do dużych klientów, wymagających indywidualnego podejścia i kompleksowego doradztwa. Trudno oszacować samodzielnie koszty takiego systemu, bez doświadczenia w pracy z innym. Brak ogólnodostępnej wersji demonstracyjnej.

Nazwa: ASSECO SOFTLAB WMS
Link: https://softlab.com.pl/rozwiazania/softlab-wms-by-asseco/
Opis, możliwości: Aplikacja posiada wszystkie potrzebne funkcjonalności nowoczesnych systemów WMS m.in. picking, integrację z firmami kurierskimi i narzędzia do automatyzacji procesów. Cena ustalana jest z klientem po analizie jego potrzeb.
Wady: Brak screenów prezentujących interfejs aplikacji lub ogólnodostępnej wersji demonstracyjnej. Dopasowanie interfejsu do potrzeb klienta może okazać się kosztowne.

Nazwa: COMARCH WMS
Link: https://www.comarch.pl/erp/comarch-wms/
Opis, możliwości: Aplikacja poza standardową funkcjonalnością posiada szerokie możliwości integracji z pozostałymi systemami firmy Comarch w ramach jednej bazy danych (np. Comarch ERP). W internacie można znaleźć mnóstwo filmów instruktażowych, które umożliwiają zapoznanie się z funkcjami a także określenie swoich potrzeb przez klienta. Aplikacja jest dość prosta i intuicyjna w obsłudze.
Wady: Rozwiązania skierowane raczej dla dużych klientów. Aby w pełni wykorzystać funkcjonalności należy wykupić zwykle kilka rozwiązań. Nie sposób odnaleźć cenę pakietu WMS w e-sklepie, co z pewnością zniechęci część klientów

Nazwa: PROGRAM WMS (SoftwareStudio)
Link: https://www.softwarestudio.com.pl/
Opis, możliwości: Program posiada wszystkie najpotrzebniejsze funkcjonalności, w tym obsługę kodów kreskowych. Jest rozwijany od wielu lat. Na stronie internetowej jest dostępna wersja demo, bez konieczności pobierania i zakładania konta.
Wady: Przestarzałe materiały instruktażowe, przedstawiające wczesną wersję aplikacji. Problem jest mało czytelny, brakuje także odwołań do plików pomocy i tooltipów.

Nazwa: OptiMES System Do Zarządzania Produkcją
Link: https://optimes.syneo.pl/
Opis, możliwości: System poza podstawową funkcjonalnością obsługuje kody kreskowe i mobilne terminale, łatwe śledzenie każdego etapu produkcji (paszportyzacja) a także powiadomienia i alarmy. Oferuje również integracje z kilkoma systemami zewnętrznymi. Program posiada możliwość pobrania wersji demo i korzystania przez 14 dni po dokonaniu rejestracji. Cennik i funkcjonalności zawarte w danym pakiecie dostępne na stronie
Wady: Program WMS jest częścią składową systemu do zarządzania produkcją i aby z niego skorzystać należy wykupić cały pakiet

--- 4. User Stories ---

1. Rejestracja nowych użytkowników, logowanie, nadawanie uprawnień przez administratora systemu

logowanie_edited

Powyższy diagram prezentuje proces uwierzytelniania i autoryzacji użytkownika. Użytkownik może zarejestrować się samodzielnie w systemie i oczekiwać na nadanie uprawnień przez administratora lub otrzymać konto utworzone odgórnie. Użytkownik otrzymuje dostęp do systemu w zależności od roli przydzielonej przez administratora.
Decyzja o zastosowaniu osobnej bazy danych dla danych użytkowników i aplikacji wynika ze względów bezpieczeństwa.

Scenariusz użycia - nowy użytkownik:

Użytkownik klika przycisk "zarejestruj" i zostaje przeniesiony do okna z formularzem
Użytkownik wypełnia i zatwierdza formularz
Użytkownik otrzymuje link z potwierdzeniem rejestracji
Po kliknięciu zostaje przeniesiony na stronę główną i otrzymuje informację, że musi oczekiwać na nadanie uprawnień
Użytkownik otrzymuje mail z informacją, że dane zostały potwierdzone a uprawnienia zostały nadane.
Użytkownik loguje się do systemu i otrzymuje dostęp uzależniony od nadanych uprawnień
Scenariusz użycia - użytkownik z nadanymi uprawnieniami:

Użytkownik wpisuje adres email/login i hasło nadane w procesie rejestracji i klika przycisk "zaloguj"
Użytkownik zostaje przekierowany do strony głównej
Scenariusz użycia - administrator:

Administrator otrzymuje informację o zarejestrowaniu nowego konta
Administrator loguje się do swojego panelu
Administrator nadaje użytkownikowi odpowiednią rolę lub ręcznie przydziela uprawnienia do poszczególnych modułów
Po zatwierdzeniu email z potwierdzeniem nadania uprawnień przesyłany jest automatycznie do użytkownika

2. Standardowy użytkownik - schemat użycia

user_standard

Powyższy diagram prezentuje zakres uprawnień dla użytkownika standardowego. Użytkownik ten jest uprawniony wyłącznie do przeglądu majątku i kontroli stanu magazynów, jednak nie może samodzielnie decydować o ich stanie (nie może np. przypisać elementu do pracownika, nie może wydać elementu itp.). Użytkownik nie ma dostępu do poufnych danych pracowniczych.

Scenariusz użycia:

Użytkownik loguje się do systemu
Użytkownik wybiera przegląd majątku
Użytkownik widzi listę elementów majątku wraz z przypisaniem do magazynu, siedzimy, użytkownika, serwisu itp.
Użytkownik wybiera przegląd magazynu
Użytkownik widzi wszystkie elementy, jakie zostały przypisane do danego magazynu

3. Użytkownik standard+ - schemat użycia

user-standard-plus_edited

Diagram przedstawia schemat użycia dla użytkowników z podniesionymi uprawnieniami, jednak bez możliwości moderacji zasobów z pominięciem wygenerowania dokumentów (stąd proponowana nazwa - standard+). Ten poziom uprawnień skierowany jest głównie dla osób zajmujących stanowiska, których integralną częścią jest modyfikacja stanu majątku np. dla sprzedawców w hurtowniach, dla kadry kierowniczej w firmach budowlanych lub innych osób pełniących funkcje zarządzania majątkiem.

Scenariusz użycia dla przeglądu magazynów i przeglądu majątku analogiczny jak w profilu standardowym.

Scenariusz użycia - modyfikacja majątku:

Użytkownik loguje się do systemu
Użytkownik wybiera opcję związaną z modyfikacją majątku
Użytkownik chce przekazać element majątku do innego magazynu - wybiera przesunięcie między magazynami (MM)
3-1. Użytkownik wybiera magazyn pierwotny i docelowy, a następnie z listy przypisuje poszczególne elementy
3-2. Użytkownik wybiera firmę/osobę odpowiedzialną za realizację
3-3. Użytkownik generuje dokument potwierdzający wykonanie czynności i drukuje celem przekazania do podpisu
3-4. Użytkownik zapisuje czynność, zostaje ona wpisana do bazy danych. Następuje automatyczny powrót do menu głównego.
Użytkownik chce odnotować, że element majątku zawarty w magazynie został przeznaczony na własne potrzeby firmy - wybiera rozchód wewnętrzny (RW)
4-1. Użytkownik wybiera magazyn, a następnie z listy przypisuje poszczególne elementy
4-2. Użytkownik wybiera czy przedmiot ma być przypisany do oddziału firmy, do osoby, czy zarówno do osoby i oddziału
4-3. Użytkownik generuje dokument potwierdzający wykonanie czynności i drukuje celem przekazania do podpisu
4-4. Użytkownik zapisuje czynność, zostaje ona wpisana do bazy danych. Następuje automatyczny powrót do menu głównego.
Użytkownik chce wydać element majątku zewnętrznemu podmiotowi (np. w celach handlowych, serwisowych itd.) - wybiera wydanie zewnętrzne (WZ)
5-1. Użytkownik wybiera magazyn, a następnie z listy przypisuje poszczególne elementy
5-2. Użytkownik wybiera podmiot zewnętrzny, któremu będzie przekazany majątek
5-3. Użytkownik określa w formularzu cel wydania (np. sprzedaż, wypożyczenie, serwis itp.)
5-4. Użytkownik generuje dokument potwierdzający wykonanie czynności i drukuje celem przekazania do podpisu
5-5. Użytkownik zapisuje czynność, zostaje ona wpisana do bazy danych. Następuje automatyczny powrót do menu głównego.
Użytkownik chce odnotować przyjęcie dobra do magazynu od podmiotu zewnętrznego - wybiera przyjęcie zewnętrzne (PZ)
6-1. Użytkownik określa od kogo zostaje przyjęty majątek i wprowadza je do systemu za pomocą formularza
6-2. Użytkownik określa, czy dostarczony towar będzie przypisany do magazynu czy do firmy
6-3. Użytkownik określa sposób pozyskania majątku (np. kupno, zwrot wypożyczenia, powrót z serwisu itp.)
6-4. Użytkownik generuje dokument potwierdzający wykonanie czynności i drukuje celem przekazania do podpisu
6-5. Użytkownik zapisuje czynność, zostaje ona wpisana do bazy danych. Następuje automatyczny powrót do menu głównego.
Użytkownik chce odnotować przyjęcie dobra, powstałego np. w procesie produkcji, od wewnętrznej jednostki firmy - przyjęcie wewnętrzne (PW)
7-1. Użytkownik dodaje ilość danego elementu lub wprowadza nowy za pomocą formularza
7-2. Użytkownik określa czy element zostaje wprowadzony do magazynu czy przypisany do oddziału firmy
7-3. Użytkownik generuje dokument potwierdzający wykonanie czynności i drukuje celem przekazania do podpisu
7-4. Użytkownik zapisuje czynność, zostaje ona wpisana do bazy danych. Następuje automatyczny powrót do menu głównego.
Użytkownik chce odnotować przyjęcie zwrotu - zwrot wewnętrzny (ZW)
8-1. Użytkownik dodaje ilość danego elementu, który ma zostać zwrócony lub wprowadza nowy za pomocą formularza
8-2. Użytkownik określa czy element zostaje wprowadzony do magazynu czy przypisany do oddziału firmy
8-3. Użytkownik generuje dokument potwierdzający wykonanie czynności i drukuje celem przekazania do podpisu
8-4. Użytkownik zapisuje czynność, zostaje ona wpisana do bazy danych. Następuje automatyczny powrót do menu głównego.

Scenariusz użycia - dodanie/edycja klientów:

Użytkownik loguje się do systemu
Użytkownik wybiera opcję związaną z modyfikacją klientów. Posiada uprawnienia do dodawania klientów i edytowania obecnych.
Użytkownik chce dodać nowego klienta i wybiera stosowną opcję
Następnie uzupełnia formularz i zapisuje dane. Baza danych zostaje zaktualizowana. Następuje powrót do menu głównego.
Użytkownik chce edytować klienta, który został wcześniej wprowadzony do bazy danych
Następnie uzupełnia formularz i zapisuje dane. Baza danych zostaje zaktualizowana. Następuje powrót do menu głównego.

4. Moderator - schemat użycia

user-moderator_edited

Diagram przedstawia schemat użycia aplikacji przez moderatorów tj. użytkowników uprawnionych do modyfikowania wszystkich danych przedsiębiorstwa zawartych w systemie z wyłączeniem nadawania uprawnień innym podmiotom (te wymagają zgody administratora). Profil skierowany jest do osób zaufanych, współpracujących bezpośrednio z zarządem danej spółki, gdyż wprowadzone przez nich zmiany mają wpływ na pracę wszystkich osób korzystających z systemu. Moderator jest uprawniony do wykonywania wszystkich czynności zarezerwowanych dla użytkownika standard+, bez konieczności generowania dokumentów, a ponadto modyfikować kadry, dane magazynów i klientów.

Scenariusz użycia - pełny wgląd, modyfikacja listy pracowników:

Moderator loguje się do systemu
Moderator wybiera opcję związaną z zarządzaniem kadrami
Moderator wybiera podgląd pracowników
Moderator widzi listę pracowników z informacjami o zajmowanym stanowisku, miejscu pracy i posiadanych uprawnieniach
Moderator wybiera opcję związaną z RODO, aby odblokować bardziej szczegółowe dane np. PESEL, rodzaj umowy itp.
Moderator ma możliwość filtrowania danych, sortowania i generowania PDF
Moderator może edytować dane pracowników, wybierając opcję "edytuj" obok rekordu tabeli i skorygować dane, które pojawią się w wyświetlonym formularzu. Po zatwierdzeniu zostaje przeniesiony do listy pracowników (pkt 4)
Moderator może usunąć pracownika, wybierając opcję "usuń" obok rekordu tabeli. Zostaje poproszony o potwierdzenie wykonania operacji z uwagi na skutki, jakie niesie za sobą ta decyzja. Jeżeli użytkownik posiada jakieś przedmioty na stanie zostanie wygenerowany komunikat z błędem, a moderator zostanie przeniesiony do formularza w którym będzie mógł usunąć majątek ze stanu użytkownika. Jeżeli usunięcie powiedzie się, moderator otrzyma potwierdzenie i zostanie przekierowany do listy pracowników (pkt 4)
Moderator może wrócić do okna głównego zarządzania kadrami klikając przycisk "wstecz"
Moderator wybiera opcję związaną z dodaniem nowego pracownika do bazy danych
Moderator uzupełnia formularz, może dodać zdjęcie i skan umowy
Po zatwierdzeniu zostaje przeniesiony do okna głównego zarządzania kadrami. Wybierając "wstecz" cofnie się do menu głównego.

Scenariusz użycia - pełny wgląd, modyfikacja listy majątku:

Moderator loguje się do systemu
Moderator wybiera opcję związaną z zarządzaniem majątkiem
Moderator wybiera podgląd majątku
Moderator widzi listę elementów majątku wraz z informacjami do kogo jest przypisany, w którym magazynie/oddziale się znajduje itp.
Moderator ma możliwość filtrowania danych, sortowania i generowania PDF
Moderator może zapoznać się ze szczegółami danego elementu, wybierając opcję "podgląd". W tym widoku będzie miał możliwość wygenerowania/sprawdzenia kodu kreskowego/QR.
Moderator może wrócić do okna głównego zarządzania majątkiem klikając przycisk "wstecz"
Moderator może edytować dany element, klikając na opcję "edytuj" obok rekordu tabeli i skorygować dane, które pojawią się w wyświetlonym formularzu. Po zatwierdzeniu zostaje przeniesiony do listy majątku (pkt 4)
Moderator może usunąć dany element, wybierając opcję "usuń" obok rekordu tabeli. Zostaje poproszony o potwierdzenie wykonania operacji z uwagi na skutki, jakie niesie za sobą ta decyzja. Jeżeli przedmiot jest na stanie jakiegoś pracownika, bądź jest on przypisany do jakiegoś oddziału/magazynu zostanie wygenerowany komunikat z błędem, a moderator zostanie przeniesiony do formularza w którym będzie mógł dokonać stosownych zmian. Jeżeli usunięcie powiedzie się, moderator otrzyma potwierdzenie i zostanie przekierowany do listy majątku (pkt 4)
Moderator może dodać nowy element do bazy danych wybierając opcję "dodaj". Kolejne kroki są analogiczne do profilu standard+ z tą różnicą, że nie jest w tym przypadku generowany dokument.
Po zatwierdzeniu moderator zostanie przeniesiony do okna głównego zarządzania majątkiem. Wybierając "wstecz" cofnie się do menu głównego.

Scenariusz użycia - pełny wgląd, modyfikacja listy magazynów:

Moderator loguje się do systemu
Moderator wybiera opcję związaną z zarządzaniem magazynami
Moderator wybiera podgląd danych magazynów
Moderator widzi listę magazynów (nazwa, dane adresowe itp.)
Moderator ma możliwość filtrowania danych, sortowania i generowania PDF
Moderator może zapoznać się ze szczegółowymi danymi nt. magazynu, wybierając opcję "podgląd" obok rekordu w tabeli. W tym widoku będzie miał możliwość przekierowania do listy przedmiotów znajdujących się w wybranym magazynie.
Moderator może wrócić do okna głównego zarządzania magazynami klikając przycisk "wstecz"
Moderator może edytować dane magazynu, klikając na opcję "edytuj" obok rekordu tabeli i skorygować dane, które pojawią się w wyświetlonym formularzu. Po zatwierdzeniu zostaje przeniesiony do listy magazynów (pkt 4)
Moderator może usunąć magazyn z listy, wybierając opcję "usuń" obok rekordu tabeli. Zostaje poproszony o potwierdzenie wykonania operacji z uwagi na skutki, jakie niesie za sobą ta decyzja. Jeżeli w magazynie znajdują się jakieś elementy majątku firmy, zostanie wygenerowany komunikat z błędem, a moderator zostanie przeniesiony do formularza w którym będzie mógł dokonać stosownych zmian. Jeżeli usunięcie powiedzie się, moderator otrzyma potwierdzenie i zostanie przekierowany do listy magazynów (pkt 4)
Moderator może dodać nowy magazyn wybierając opcję "dodaj".
Moderator uzupełnia formularz, może dodać odnośnik do Google Maps w celu wygenerowania miniaturki mapy.
Po zatwierdzeniu moderator zostanie przeniesiony do okna głównego zarządzania magazynami. Wybierając "wstecz" cofnie się do menu głównego.

Scenariusz użycia - pełny wgląd, modyfikacja listy klientów:

Moderator loguje się do systemu
Moderator wybiera opcję związaną z zarządzaniem klientami
Moderator posiada analogiczne możliwości do profilu standard+ w zakresie podglądu, dodawania nowych klientów i edycji obecnych
Ponadto moderator może usunąć klienta, wybierając opcję "usuń" obok rekordu tabeli. Zostaje poproszony o potwierdzenie wykonania operacji z uwagi na skutki, jakie niesie za sobą ta decyzja. Jeżeli klient posiada element majątku firmy na stanie, a powinien on zostać zwrócony, zostanie wygenerowany komunikat z błędem, a moderator zostanie przeniesiony do formularza w którym będzie mógł dokonać stosownych zmian. Jeżeli usunięcie powiedzie się, moderator otrzyma potwierdzenie i zostanie przekierowany do listy klientów.
Każda opisana wyżej czynność, ze względów bezpieczeństwa, zapisywana jest w logach aplikacji i bazie danych.

5. Administrator - schemat użycia

user-admin_edited

Diagram przedstawia schemat użycia aplikacji przez administratora. Administrator posiada najwyższy możliwy poziom uprawnień, bardziej skomplikowanych zmian można dokonać tylko poprzez zmiany w kodzie aplikacji. Głównym zadaniem tego profilu jest zarządzanie uprawnieniami i zmiany w stanach poszczególnych zasobów, które powinny odbywać się automatycznie. Administrator posiada również funkcję dodawania stanowisk. Decyzja o umieszczeniu tej możliwości na poziomie administratora wynika z faktu, że z chwilą utworzenia stanowiska powinna zostać przypisana do niego odpowiednia rola. Stanowi to zabezpieczenie przed przydzieleniem użytkownikowi zbyt wysokich uprawnień.

Scenariusz użycia - zarządzanie rolami/uprawnieniami

Administrator loguje się do systemu
Administrator wchodzi do panelu administratora
Administrator wybiera opcję związaną z zarządzaniem uprawnieniami, po której zostanie przekierowany do widoku, w którym będzie mógł zweryfikować utworzone role oraz mieć podgląd do stworzonych uprawnień
Po otwarciu widoku będą widoczne opcje "dodaj rolę", która będzie umożliwiała stworzenie nowej roli i przypisania jej uprawnień. Uprawnienia będą wprowadzone do systemu "na sztywno", a administrator będzie miał do nich dostęp poprzez opcję "podgląd uprawnień".
Przy każdej roli na liście będzie dostępna opcja "zarządzanie rolą", która pozwoli na weryfikację uprawnień przypisanych do danej roli. Z tego poziomu rolę będzie można zmodyfikować i usunąć.
Przy każdej roli będzie także dostępna opcja "zarządzanie użytkownikami", która pozwoli na weryfikację, którym użytkownikom została ona przyporządkowana

Scenariusz użycia - pełny wgląd, modyfikacja stanowisk:

Administrator loguje się do systemu
Administrator wchodzi do panelu administratora
Administrator wybiera opcję związaną z zarządzaniem stanowiskami, po której zostanie przekierowany do widoku, w którym będzie widoczna lista utworzonych stanowisk. Nowe stanowisko będzie możliwe do utworzenia za pomocą opcji "dodaj"
Przy każdym wierszu tabeli będzie dostępna opcja "podgląd". Po wybraniu tej opcji administrator otrzyma widok na którym będzie widoczna nazwa stanowiska, wraz z przypisaną rolą. Powrót możliwy za pomocą przycisku "wstecz".
W tym samym miejscu będzie dostępna opcja "edytuj". Po wybraniu tej opcji administrator otrzyma widok z formularzem umożliwiającym zmianę nazwy stanowiska, a także wyboru roli z listy lub dropdown menu. Powrót możliwy za pomocą przycisku "wstecz".
Obok opcji "edytuj", będzie dostępny przycisk "usuń" służący do zlikwidowania stanowiska z listy i bazy danych. Po wybraniu tej opcji administrator zostanie poproszony w kolejnym widoku o potwierdzenie operacji. Zarówno anulowanie jak i potwierdzenie będą skutkować powrotem do listy stanowisk, jednak po potwierdzeniu wybrane stanowisko zniknie z listy.

Scenariusz użycia - pełny wgląd, modyfikacja stanów majątku:

Jest to opcja umożliwiająca administratorowi ręczną zmianę stanów elementów majątku. W docelowej wersji aplikacji stany będą zmieniać się automatycznie, a wraz z tymi zmianami będą dostępne określone akcje. Przykładowo, jeżeli przedmiot będzie miał status "w naprawie" nie będzie możliwe wydanie zewnętrzne innemu podmiotowi lub jeżeli przedmiot ma status "zamówiony" nie będzie możliwości przypisania go do jednostki. W przypadku błędów administrator będzie mieć furtkę, aby zmieniać statusy bez konieczności ręcznej modyfikacji rekordów w bazie danych.
Administrator loguje się do systemu
Administrator wchodzi do panelu administratora
Administrator wybiera opcję związaną z zarządzaniem stanami.
Administrator widzi listę przedmiotów wraz z podstawowymi danymi i statusami.
Administrator może sortować i filtrować rekordy, aby znaleźć odpowiedni element.
Po dokonaniu zmiany, wybiera przycisk "zapisz" i wraca do głównej strony panelu.

--- 5. Use Cases ---

Use Case dla użytkownika - uprawnienia standardowe:

Aktor - - Warunek początkowy - Warunek końcowy
Użytkownik standardowy - Użytkownik jest zalogowany do systemu - Użytkownik upewnia się, że posiada dostęp do przeglądu majątku
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Logowanie do systemu - 
2. Wybór przeglądu majątku - 2a. Po wyborze pojawia się komunikat o braku uprawnień do modułu
3. Użytkownik widzi listę majątku - 
4. Użytkownik wybiera przegląd magazynów - 4a. Po wyborze pojawia się komunikat o braku uprawnień do modułu
5. Użytkownik widzi wszystkie elementy przypisane do magazynu - 

Use Case dla użytkownika - uprawnienia rozszerzone:

Aktor - - Warunek początkowy - Warunek końcowy
Użytkownik standardowy - Użytkownik jest zalogowany do systemu - Użytkownik upewnia się, że posiada dostęp do przeglądu majątku
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Logowanie do systemu - 
2. Wybór przeglądu majątku (z uprawnieniami do modyfikacji) - 2a. Po wyborze pojawia się komunikat o braku uprawnień do modułu
2b. Nowy widok uruchamia się, jednak bez uprawnień do modyfikacji
3. Użytkownik wybiera interesującą go akcję - 
4. Użytkownik uzupełnia formularz i wybiera opcję "generuj dokument" 
- 4a. Nie zostały wypełnione wymagane pola - komunikat o błędzie
- 4b. Brak dostępnego klienta na liście - należy utworzyć klienta
- 4c. Brak dostępnego magazynu lub siedziby na liście - zgłoszenie do moderatora
- 4d. Brak dostępnego przedmiotu na liście, który fizycznie istnieje - należy wprowadzić przedmiot
- 4e. Brak możliwości wydania przedmiotu z uwagi na jego status - komunikat o błędzie, zgłoszenie do administratora
5. Użytkownik sprawdza, czy dokument wygenerował się poprawnie 
- 5a. Formularz zawiera błędne dane - należy wybrać "wstecz" i poprawić formularz
- 5b. Formularz wygenerował się niepoprawnie - należy sprawdzić, czy dane w formularzu zostały wpisane poprawnie np. czy nie zawierają pustych spacji
- 5c. PDF zawiera "krzaczki" - należy sprawdzić ustawienia regionalne w systemie
6. Użytkownik drukuje dokument - 6a. Dokument nie drukuje się lub drukuje się niepoprawnie - należy sprawdzić ustawienia drukarki
7. Użytkownik zatwierdza formularz klikając na przycisk "OK"

Aktor - Warunek początkowy - Warunek końcowy
Użytkownik standardowy - Użytkownik jest zalogowany do systemu - Użytkownik upewnia się, że posiada dostęp do przeglądu majątku
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Logowanie do systemu
2. Wybór przeglądu klientów (z uprawnieniami do modyfikacji) 
- 2a. Po wyborze pojawia się komunikat o braku uprawnień do modułu
- 2b. Nowy widok uruchamia się, jednak bez uprawnień do modyfikacji
3. Użytkownik wybiera interesującą go akcję
4. Użytkownik uzupełnia (w przypadku dodania nowego klienta) lub poprawia (w przypadku jego edycji) formularz z danymi klienta 
- 4a. Użytkownik użył niepoprawnego formatu przy wprowadzaniu danych - pojawia się komunikat z błędem przy odpowiednim polu
- 4b. Użytkownik skasował zawartość pola tekstowego i przeszedł do następnego - pojawia się komunikat z błędem, jeżeli pole nie może pozostać puste
5. Użytkownik zatwierdza formularz klikając na przycisk "Zapisz" lub rezygnuje, klikając "Wstecz" - 

Use Case dla moderatora:

Aktor - Warunek początkowy - Warunek końcowy
Moderator - Moderator jest zalogowany do systemu - Moderator potwierdza uprawnienia do modyfikacji danych
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Logowanie do systemu 
2. Moderator wybiera dział, w ramach którego chce dokonać zmian 
- 2a. Po wyborze pojawia się komunikat o braku uprawnień do modułu - konieczny kontakt z administratorem celem sprawdzenia uprawnień
- 2b. Nowy widok uruchamia się, jednak bez uprawnień do modyfikacji - j.w.
- 2c. Brak widocznych działów - j.w.
- 2d. Brak widocznej listy - oznacza, że nie został dodany rekord do bazy danych, należy go utworzyć za pomocą opcji "dodaj"
3. Moderator wybiera jedną z dostępnych opcji modyfikacji - dodaj, usuń, edytuj - 3a. Co najmniej jedna z opcji jest niedostępna - konieczny kontakt z administratorem celem weryfikacji uprawnień przypisanych do roli
4. Moderator uzupełnia (w przypadku dodania nowego klienta) lub poprawia (w przypadku jego edycji) formularz z odpowiednimi danymi 
- 4a. Moderator użył niepoprawnego formatu przy wprowadzaniu danych - pojawia się komunikat z błędem przy odpowiednim polu
- 4b. Moderator skasował zawartość pola tekstowego i przeszedł do następnego - pojawia się komunikat z błędem, jeżeli pole nie może pozostać puste
5. Moderator zatwierdza formularz klikając na przycisk "Zapisz" lub rezygnuje, klikając "Wstecz" - 

Use Case dla administratora:

Aktor - Warunek początkowy - Warunek końcowy
Administrator - Administrator jest zalogowany do systemu, panel administracyjny uruchamia się - Administrator może dodać nową rolę i przydzielić do niej uprawnienia a następnie przypisać ją do konkretnego stanowiska lub użytkownika
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Administrator wybiera opcję związaną z zarządzaniem uprawnieniami
2. Pojawia się widok z listą dostępnych ról, przy każdej z nich jest opcja umożliwiająca edycję uprawnień przypisanych do danej roli oraz weryfikację użytkowników do niej przypisanych 
- 2a. Brak ról na liście - oznacza to, że żadna nie została utworzona, należy stworzyć nową (pkt 3)
- 2b. Obok wiersza nie pojawiają w/w opcje - kontakt z twórcami aplikacji
3. Administrator chce stworzyć nową rolę i przypisać jej uprawnienia (wybiera "dodaj rolę") lub zweryfikować istniejącą (wybiera "zarządzanie rolą") 
- 3a. Brak dostępnych uprawnień do przypisania - oznacza, że nie zostały jeszcze wprowadzone lub jest problem z powiązaniem widoku z bazą danych. Konieczny jest kontakt z twórcami aplikacji
- 3b. Administrator stworzył rolę, ale nie przypisał jej żadnych uprawnień - pojawia się komunikat informujący o braku możliwości stworzenia takiej roli (jest ona utworzona z automatu dla nowych użytkowników)
- 3c. Administrator przypisał uprawnienia w formularzu, ale nie nadał nazwy dla tworzonej roli - pojawia się komunikat, informujący że przed zapisaniem należy podać nazwę roli
- 3d. Administrator wprowadza nazwę dla roli, która już istnieje w systemie - pojawia się komunikat, informujący że rola o tej nazwie już istnieje w bazie danych i należy wybrać inną nazwę
4. Administrator chce sprawdzić komu rola została przypisana - wybiera opcję "zarządzanie użytkownikami" 
- 4a. Brak użytkowników na liście - oznacza, że rola nie została jeszcze przypisana żadnemu użytkownikowi
- 4b. Lista użytkowników do wyboru jest pusta - oznacza, że żaden pracownik nie został jeszcze stworzony

Aktor - Warunek początkowy - Warunek końcowy
Administrator - Administrator jest zalogowany do systemu, panel administracyjny uruchamia się - Administrator zweryfikował możliwość modyfikacji stanowisk
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Administrator wybiera opcję związaną z zarządzaniem stanowiskami
2. Pojawia się widok z listą stanowisk, przy każdym z nich jest opcja umożliwiająca podgląd, edycję i usunięcie danego stanowiska - 2a. Brak stanowisk na liście - oznacza to, że żadne stanowisko nie zostało utworzone, należy stworzyć nowe (pkt 3)
3. Administrator chce dodać nowe stanowisko - wybiera opcję "dodaj stanowisko" 
- 3a. Administrator próbuje zapisać stanowisko, jednak nie została wprowadzona jego nazwa - pojawia się informacja o konieczności wprowadzenia nazwy przed zapisem
- 3b. Administrator wybrał nazwę, która już istnieje w systemie - pojawia się komunikat, informujący że stanowisko o tej nazwie już istnieje w bazie danych i należy wybrać inną nazwę
4. Administrator chce usunąć stanowisko z systemu - wybiera opcję "usuń"
5. Ze względów bezpieczeństwa pojawia się okno z potwierdzeniem operacji - administrator może zatwierdzić usunięcie klikając na "OK" lub zrezygnować klikając "wstecz"

Aktor - Warunek początkowy - Warunek końcowy
Administrator - Administrator jest zalogowany do systemu, panel administracyjny uruchamia się - Administrator zweryfikował możliwość zmiany stanów elementów majątku
Główny scenariusz (powodzenie) - Rozszerzenie (niepowodzenie)
1. Administrator wybiera opcję związaną z zarządzaniem stanami
2. Pojawia się widok z listą przedmiotów, wraz z danymi identyfikacyjnymi oraz dropdown listą z dostępnymi stanami 
- 2a. Brak przedmiotów na liście - oznacza to, że do bazy danych nie zostały jeszcze wprowadzone żadne elementy
- 2b. Brak odpowiedniego stanu do wyboru - kontakt z właścicielem kopii aplikacji lub twórcami
3. Administrator wybiera opcję "zapisz" by wprowadzić zmiany

--- 6. Zagrożenia ---

Zagrożenia związanie z uwierzytelnianiem
- Zbyt łatwe hasła np. zawierające kombinację miesięcy i lat
- Zapisywanie haseł i trzymanie ich w widocznym miejscu lub niedostatecznie zabezpieczonym
- Zgubienie zapisanego hasła
- Zapomnienie loginu lub hasła
- Rzadsza zmiana haseł niż raz na 30 dni

Zagrożenia związane z autoryzacją
- Korzystanie z cudzych danych dostępowych
- Wybór w aplikacji stanowiska pracy nieadekwatnego do rzeczywistego
- Nadanie zbyt wysokich uprawnień
- Zbyt późne odebranie lub zmniejszenie uprawnień (w przypadku zwolnienia lub zmiany stanowiska pracownika)

Zagrożenia związane z danymi wrażliwymi
- Niezabezpieczenie komputera hasłem
- Korzystanie z aplikacji poza miejscem pracy
- Pozostawienie komputera niezablokowanego podczas nieobecności przy stanowisku
- Ustawienie stanowiska w miejscu dostępnym dla osób nieuprawnionych lub postronnych

Zagrożenia związane z atakami zdalnymi
- Korzystanie z aplikacji poza miejscem pracy bez odpowiednich certyfikatów i kluczy zabezpieczeń (tokenów)
- Korzystanie z aplikacji poza domeną
- Brak zainstalowanego programu antywirusowego/firewalla
- Korzystanie z internetu poprzez router z odblokowanymi bądź przekierowanymi portami, na których działa aplikacja
- Korzystanie z nieaktualnej przeglądarki lub wersji systemu operacyjnego

Zagrożenia związane z błędami aplikacji lub awarią serwera
- Brak możliwości zalogowania do systemu
- Brak autoryzacji
- Desynchronizacja widoku z bazą danych
- Utrata wprowadzonych danych (do ostatniego backupu)
- Nieprawidłowe działanie aplikacji spowodowane brakiem lub uszkodzeniem plików
