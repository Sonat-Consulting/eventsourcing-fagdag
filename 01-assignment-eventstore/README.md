## Oppgave - Utvide evensourcingen med nye commands og events

### Intro
Oppgaven er å legge til og håndtere kommandoen `CompleteHaircut` med tilhørende event `HaircutCompleted` i eventsourcingen. 

Startpunktet er at `HaircutCreated`og `HaircutStarted` allerede er implementert.  Så du kan titte på dem for inspirasjon.

`HaircutCreated` oppstår når kunden kjøper en klipp.

`HaircutStarted` oppstår når kunden havner i stolen og klippen starter. 

Vi skal da lage `HaircutCompleted` som betyr at klipp er ferdig og frisør klar til nye utfordringer.

Du kan forberede deg ved å bruke debug funksjonen til å steppe gjennom kommandoene `CreateHaircut` og `StartHaircut` hvis du vil ha kontekst for hvordan ting funker.

Oppgaven er strukturert slik at det gis minimal info oppe i dagen, og mer detaljer skjult i hint.  Dette fordi vi alle har forskjellig utgangspunkt. Noen vil foretrekke å finne ut av tingene selv, mens andre trenger mer informasjon. Det er fullt lov å bruke hintene fra første øyeblikk hvis du er lost i C# eller ikke aner hvordan komme i gang.

### Oppgaven

1. Åpne løsningen `Clippers.EventFlow.sln` fra katalog `01-assignment-eventstore` i ditt kodeverktøy (typisk Visual Studio eller Rider). 
2. Legg til håndtering av completed haircut slik at man kan kalle `CompleteHaircut` i API, at aggregatet/entiteten `HaircutModel`er oppdatert i henhold til `Completed` og at eventet `HaircutCompleted` er lagret i EventStore.
3. Krav om følgende ekstra egenskaper på eventet:
    - DateTime `CompletedAt`.
4. Hendelsen skal gi `status` `completed` i aggregatet/entiteten `HaircutModel` (mutasjonen)

[Hint 1 (endringer i aggregatet)](./hint01.md)

[Hint 2 (service)](./hint02.md)

[Hint 3 (endepunkt)](./hint03.md)

[Hint 4 (det du kommer til å glemme)](./hint04.md)

### Teste
Du kan starte løsningen, og bruke API i browser til å kjøre kommandoer med "try it out" knappen.

Du kan bruke Postman eller annen API verktøy som alternativ.

Du kan titte i databasen med Data explorer: https://localhost:8081/_explorer/index.html.

Du kan teste med å bruke Svelte GUI som følger med i github katalogen `clippers-svelte-gui`.

For at GUI skal fungere, må `Cippers.EventFlow.Projections`prosjektet også kjøre. Dette ligger ved siden av `Clippers.EventFlow`. Du kan f.eks. åpne det i IDE og starte det der.

for å starte `clippers-svelte-gui` må du stå i `clippers-svelte-gui` katalogen  kjøre `npm install` (kun første gang) og `npm run dev`.

### Ekstra
1. Legg også til `CancelHaircut` på samme lest.
    - Krav om følgende ekstra egenskaper på eventet:
        - DateTime `CancelledAt`.
    - Cancelled skal gi status `cancelled` i aggregatet/entiteten `HaircutModel`
2. Lag enhetstester i klassen `HaircutModelTests` i prosjektet `Clippers.Test.Unit` og kjør testene.
    - Hvordan kjøre tester varierer med IDE, men der er som regel en `TestExplorer` og høyreklikk alternativer.
3. Sett breakpoints og step gjennom koden for å se hvordan det oppfører seg. Hvis du er usikker på tastatur shortcuts for "step over" og "step into" osv, finner du de som regel i en "debug" meny i IDE'en. 

