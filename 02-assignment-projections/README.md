## Oppgave - Projections - lage køen
### Intro
I live kodingen laget vi noen projeksjoner med statistikk. Nå går vi opp et hakk, og skal lage projeksjon som skal brukes av en GUI som viser køen av ventende kunder og de som blir klippet.

En fordel med projeksjoner, er at vi spesialtilpasser data i projeksjonen til formålet. Hvis vi har et annet formål senere, så kan vi lage ny projeksjon.

Vi har allerede laget en GUI som kan benytte projeksjonen, så hvis du navngir ting likt som meg, kan du få kjørt GUI med projeksjonen som du lager selv her.

### Oppgaven
Lag projeksjon for "køen".  Den skal vise alle kunder som venter og alle kunder som blir klippet.

For hver kunde trengs følgende properties (data):
    - HaircutId
    - DisplayName
    - Status
Projeksjonen blir derfor en liste med elementer som hver har de 3 properties over. Property som holder selve listen, skal hete `Customers`.

[Hint 1 - Lage projeksjon](./Hint01.md)

[Hint 2 - When metodene](./Hint02.md)

[Hint 3 - Det du kommer til å glemme](Hint03.md)
### Teste
Når du er ferdig, kan du starte både `Clippers.EventFlow` prosjektet og `Clippers.Projections` prosjektene.

Du kan gjøre `CreateHaircut`, `StartHaircut`, `CompleteHaircut` og `CancelHaircut` på EventFlow API, og sjekke i databasen med Data Explorer: https://localhost:8081/_explorer/index.html

Du kan også bruke GUI. Hvis Projeksjonen din funker og er navngitt rett, skal GUI kunne funke med din implementasjon av projeksjonen.

for å starte `clippers-svelte-gui` må du stå i `clippers-svelte-gui` katalogen kjøre `npm install` (kun første gang) og `npm run dev`.

### Ekstra
Projeksjoner er "billige" å lage, og i et eventsourcing prosjekt med projeksjoner er det vanlig og nyttig å lage nye tilpassede projeksjoner hyppig.

Kanskje du vil foretrekke å lage køen din som en Dictonary ( Javascript Map) på HairCutId i stedet?
`public Dictionary<string, QueueElement> Customers { get; set; } = new Dictionary<string, QueueElement>();`
Da kan du sette HaircutId som Key, og gjøre oppslag direkte uten LINQ spørringer, ala:
`view.Customers[haircutStarted.HaircutId].Status = "serving";`

Du kan jo lage deg en projeksjon som lager køen på denne "dialekten".

Kanskje du vil ha statistikken for antall startede, antall som klippes, antall avsluttede og antall slettede inn på skjermbildet for køen?  Null problem!  Bare legg det til i samme projeksjonen.

Du kan da lage en ny property ved siden av `Customers` som heter noe sånt som `Statistics` og som er en klasse som har disse 4 tallene som properties.  Hvis du legger property `Statistics` over `Customers`i koden, så kommer den også øverst i dokumentet i basen. Så er det litt lettere å lese siden du slipper scrolle under kundelisten.

Du må da også justere When metodene til å sette tallene riktig.

Siste utfordring er å utvide statistikk til også å ha med "antall ventende kunder" og "antall kunder som klippes".  Klarer du tenke ut hvordan de må beregnes i When metodene?










