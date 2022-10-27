## Oppgave - Utvide evensourcingen med nye commands og events

### Intro
Oppgaven er å legge til og håndtere kommandoen `CompleteHaircut` med tilhørende event `HaircutCompleted` i eventsourcingen. 
Startpunktet er at `HaircutCreated`og `HaircutStarted` allerede er implementert.  Så du kan titte på dem for inspirasjon. 
Du kan også bruke debug funksjonen til å steppe gjennom CreateHaircut og StartHaircut hvis du trenger litt kontekst for hvordan ting funker.

Oppgaven er strukturert slik at det gis minimal info oppe i dagen, og mer detaljer skjult i hint.  Dette fordi vi alle har forskjellig utgangspunkt. Det er fullt lov å bruke hintene fra første øyeblikk hvis du er lost i C# for eksempel.

### Oppgaven

1. Åpne løsningen `Clippers.EventFlow.sln` fra katalog `01-assignment-eventstore` i ditt kodeverktøy (typisk Visual Studio eller Rider). 
2. Legg til håndtering av completed haircut slik at man kan kalle `CompleteHaircut` i API, at aggregatet/entiteten `HaircutModel`er oppdatert og at eventet `HaircutCompleted` er lagret i EventStore.
3. Krav om følgende ekstra egenskaper på eventet:
    - DateTime `CompletedAt`.
4. Hendelsen skal gi `status` `completed` i aggregatet/entiteten `HaircutModel`

[Hint 1 (endringer i aggregatet)](./hint01.md)
[Hint 2 (service)](./hint02.md)
[Hint 3 (endepunkt)](./hint03.md)
[Hint 4 (det du kommer til å glemme)](./hint04.md)

### Ekstra
1. Legg også til `CancelHaircut` på samme lest.
    - Krav om følgende ekstra egenskaper på eventet:
        - DateTime `CancelledAt`.
    - Cancelled skal gi status `cancelled` i aggregatet/entiteten `HaircutModel`
2. Lag enhetstester i klassen `HaircutModelTests` i prosjektet `Clippers.Test.Unit` og kjør testene.
    - Hvordan kjøre tester varierer med IDE, men der er som regel en `TestExplorer` og høyreklikk alternativer.
3. Sett breakpoints og step gjennom koden for å se hvordan det oppfører seg. Hvis du er usikker på tatatur shortcuts for "step over" og "step into" osv, finner du de som regel i en "debug" meny i IDE'en. 

