**Endepunkt:**
Lag et nytt endepunkt for CompleteHaircut i prosjektet `Clippers.Api` i klassen `program.cs`

Vi bruker .NET6 sin nye "minimal API" template, som gir mye mindre overhead enn den tradisjonelle MVC tilnærmimgen med Controller osv. Derfor koder vi endepunktene ganske "barebone" rett i `program.cs.`

Kortversjonen av hint her er: "gjør det likt som `/startedHaircut`...

1. Endepunktet vårt skal serve url `/completeHaircut` og levere videre til den nye `ICompleteHaircutService`n vår.
2. Vi skal gjøre POST. Dette gjøres med `app.mapPost` (for GET er det app.mapGet og PUT app.mapPut osv).
3. Første parameter til `app.Map`post er sub-url, altså `/completeHaircut`.
4. Man kan `inline` hvilke datastruktur vi får og service vi skal sende videre med å dekorere med `[FromBody]` og `[FromServices]`. Se på startHaircut `app.MapPost("/startHaircut/, ...` for syntaks.
5. Ikke viktig, men hvis du er interessert så kan du berike den automatisk genererte API dokumentasjonen (swagger) med `WithMetadata` og `Produces` (med flere). Du kan se i final versjonen hvis du vil se eksempel på dette.