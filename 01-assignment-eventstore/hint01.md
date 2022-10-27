**Oppdatere aggregatet/entiteten HaircutModel:**

Anbefaler å begynne kodingen i HaircutModel først.  Den er "innerst", så hvis du begynner i endepunkter eller service, så kan du bare lage "skall" inntil HaircutModel er implementert.

Husk at HaircutModel har både data og oppførsel.

Så vi må legge til nye data og ny oppførsel som det nye eventet bidrar med.
1. Så du må legge til nye data som eventet bidrar med (altså `CompletedAt` datoen). Legg til `CompletedAt`som en property (slik som `StartedAt`).
2. Du må også legge til ny oppførsel, så lag metode `Complete`. 
3. Se på metode `Start` for syntaks.
4. Inni complete metoden må du 
    1. Mutere (endre) HaircutModel sin tilstand i henhold til eventet.
    2. Sørge for lagring av eventet til eventstore
    - Disse to tingene er i dette systemet normalisert til `Apply` metoden. Så du må kalle `Apply` metoden.
    - `Apply` metoden sørger automatisk for at eventet blir lagret, så det trenger du ikke tenke på når du kaller `Apply`.
    - Hvilken mutering/endring dette eventet har på aggregatet/entiteten, er i denne løsningen delegert til en `When` metode.
    - Implementer en `When` metode for `CompletedHaircut`. Completed eventet sin effekt i vårt eksempel, er simpelthen at status skal settes til `completed`.
    - Se på `When`metoden for `Started` haircut for inspirasjon.
    - `Apply` metoden vil automatisk sørge for at `When`metoden blir kalt.
5. Inni `Complete` metoden kan du legge på lett validering (men må ikke). Complete er kun lovlig når en kunde sitter i stolen og blir klippet. Altså har status "`serving`".  Du kan ikke avslutte en klipp med mindre du klipper.
