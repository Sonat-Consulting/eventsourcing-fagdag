### Projeksjoner fortsettelse - When metodene

`WhenHaircutCreated`:
Her skal nye kunder legges inn.  Så da adder vi et nytt kunde element til køen. Det kan kodes på mange måter, her er en hvor vi oppretter QueueElement direkte i Add metoden til listen, og bare manuelt mapper info over fra eventet:

```
view.Customers.Add(new QueueElement
{
    HaircutId = haircutCreated.HaircutId,
    DisplayName = haircutCreated.DisplayName,
    Status = "waiting",
});
```

`WhenHaircutStarted`
Her skal vi oppdatere status på kunde som allerede er i køen fra `waiting` til `serving` (kunden havnet i køen på create).

Det kan også gjøres på mange måter, og for å løse dette bør man kunne litt C#.

En måte er å bruke LINQ (.NET spesifikk kode-query språk).  Her bruker jeg LINQ extension metode `First`, som returnerer første element i en liste som treffer en query.  Query her er å matche på HaircutId.

Så hvis HaircutId i eventet matcher haircutId i køen, så har vi funnet elementet vårt. Så setter vi `Status`=`serving` på det elementet:
```
 view.Customers.First(x => x.HaircutId == haircutStarted.HaircutId).Status = "serving";
```
Dette kunne man også løst med en god gammeldags for løkke, eller sikkert andre LINQ metoder.

`WhenHaircutCompleted`
På Completed skal vi slette kunden fra køen. Så da må vi finne elementet i køen og slette det. Elementet finner vi ved å matche HaircutId i eventet med HaircutId i køen.

Jeg har løst det med en OneLiner Linq.  Linq har en extension metode `RemoveAll` hvor man kan gjøre en direkte match på en property i et element i listen. 
```
view.Customers.RemoveAll(x => x.HaircutId == haircutCompleted.HaircutId);
```

`RemoveAll` er greit siden det uansett alltid bare vil være ett element per HaircutId uansett.

`WhenHaircutCancelled`
Denne blir helt lik som Completed. Vi sletter kunden fra listen.
