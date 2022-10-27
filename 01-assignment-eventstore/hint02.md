**Lage service** - anbefaler at endringen i aggregatet/entiteten `HaircutModel` er ferdigstilt ([hint1](./hint01.md)) før man lager service.

Sevicene våre gjør normalt 3 hovedting:

1. Laster aggregatet/entiteten vår fra Eventstore.
2. Påfører kommandoen på aggregatet/entiteten.
3. Lagrer aggregatet/entiteten til Eventstore.

Så du må:
1. Lage en command klasse for Completed kommandoen som servicen vår skal levere: `CompleteHaircutCommand` med egenskaper:
    - `HaircutId` - nøkkel for aggregatet/eventstømmen som identifiserer hvilken klipp/kunde/frisør som er i spill.
    - `CompletedAt` - dato
2. Lage et Interface for Complete servicen: `ICompleteHaircutService` som har operasjon
 `Task<HaircutModel> CompleteHaircut(CompleteHaircutCommand completeHaircutCommand);`

3. Lage en klasse som implementerer interfacet og arver baseklassen `HaircutServiceBase` (som har felles funksjonalitet for alle Haircut servicene).
`public class CompleteHaircutService : HaircutServiceBase, ICompleteHaircutService`
4. Implementer metoden 
`public async Task<HaircutModel> CompleteHaircut(CompleteHaircutCommand completeHaircutCommand)`:
    - Load HaircutModel fra Eventstore: 
    `var haircut = await LoadHaircut(completeHaircutCommand.HaircutId);`
    - Påfør kommandoen på HaircutModel: 
    `haircut.Complete(completeHaircutCommand.CompletedAt);`
    - Lagre HaircutModel mot Haircut Repository, som da vil lagre eventet vår til eventstore: 
    `return await base.SaveHaircut(haircut);` * 
5. Til slutt må du injisere den nye servicen din i dependency injection. Det gjør du i `program.cs` i prosjektet `Clippers.Api`:
`builder.Services.AddScoped<ICompleteHaircutService, CompleteHaircutService>();`

*Eventet vårt blir lagt på i Entiteten sin `Changes` property av `Complete` metoden vi har laget (i `Apply`). Eventstore Repositoriet vårt kan å lagre alle eventene i `Changes` til selve eventstoren.

Du kan gå ned i repository og kikke, men detaljene der har vi prioritert bort av tidshensyn.

