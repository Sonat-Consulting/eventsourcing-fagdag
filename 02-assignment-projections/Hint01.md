For å løse denne køen, er det naturlig å først lage en klasse som inneholder en kunde, altså ett element i listen:

eks:

```
public class QueueElement
{
    public string HaircutId { get; set; }
    public string DisplayName { get; set; }
    public string Status { get; set; }
}
```
Køen blir da en liste av disse:
`List<QueueElement> Customers`

Viewet vårt blir da en klasse som har en property som heter `Customers` som er denne listen:

```
public class QueueView
{
    public List<QueueElement> Customers { get; set; } = new List<QueueElement>();
}
```
Projeksjonen lages da med dette viet likt so for statisitikkene:

```
public class QueueProjection : Projection<QueueView>
    {
        ...
```
I konstruktøren for projeksjonen må vi registrere alle hendelsene vi trenger. Og hvilke trenger vi?  
Vi trenger i hvert fall `HaircutCreated` for å legge kunden inn i køen.
Så trenger vi `HaircutStarted` for å endre status til `serving`.
Men vi trenger også `HaircutCompleted` for å ta kundene vekk fra køen når de er ferdig klippet.  Husk at oppgaven var å "vise alle som venter og blir klippet.
Tilsvarende for `HaircutCancelled`.

Så vi trenger å registrere alle 4 hendelsene:
```
public QueueProjection()
{
    RegisterHandler<HaircutCreated>(WhenHaircutCreated);
    RegisterHandler<HaircutStarted>(WhenHaircutStarted);
    RegisterHandler<HaircutCompleted>(WhenHaircutCompleted);
    RegisterHandler<HaircutCancelled>(WhenHaircutCancelled);
}
```
Deretter må vi lage 4 When metoder til å manipulere dataene for hver hendelse.  De kan du prøve deg på selv, eller fortsette i [Hint 2](./Hint02.md).