using AutoFixture;
using Clippers.Core.Haircut.Events;
using System.Dynamic;
using System.Net.Http.Json;

namespace Clippers.FlowGenerator
{
    public class Generator : IGenerator
    {
        private readonly Random random = new Random();
        private readonly int minutesFromCreatedToStart = 2;
        private readonly int minutesFromStartedToCompleted = 5;
        private string[] fornavnListe = new string[] { "Markus", "Lilly", "Emma", "Noa", "Markus", "Amanda", "Maja", "Vilde", "Nicolai", "Sarah", "Phillip", "Sophie", "Mathilde", "Anna", "Casper", "Astri", "Elias", "Johan", "Noah", "Axel", "Maria", "Johannes", "Iben", "Jonas", "Agnes", "Nora", "Sigrid", "Kasper", "Emma", "Adam", "Astri", "Anna", "Johann", "Viktoria", "Oskar", "Jakob", "Sophie", "Elias", "Kasper", "Theo", "Hanna", "Aleksander", "Oline", "Lea", "Oline", "Ida", "Hannah", "Sigrid", "Ellinor", "Aleksander", "Olav", "Sebastian", "Ellinor", "Kasper", "Astrid", "Bantam", "Haakon", "Jonas", "Liam", "Jacob", "Kaia", "Emma", "Tiril", "Victor", "Håkon", "Victoria", "Felix", "Amelia", "Sophia", "Liam", "Selma", "Herman", "Viktoria", "Johan", "Aegon", "Marie", "Emilie", "Henry", "Emil", "Mathilde", "Eline", "Noah", "Dany", "Matilde", "Amanda", "Ella", "Fredeico", "Mikkel", "Even", "Jonas", "Astri", "Mikaela", "Philip", "Jonas", "Jonna", "Sophie", "Lilly", "Oliver", "Alexander", "Agnes" };
        private static HttpClient client = new HttpClient();
        public int NumPerHour { get; set; }
        private Fixture fixture = new Fixture();
        public Generator(int numPerHour = 240)
        {
            client.BaseAddress = new Uri("https://localhost:7255/");
            NumPerHour = numPerHour;
        }

        public Task Generate()
        {
            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
            var end = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);

            var day = end - start;

            var totalmilliseconds = (int)day.TotalMilliseconds;

            var numOfHaircuts = (int)day.TotalHours * NumPerHour;

            for (int i = 0; i < numOfHaircuts; i++)
            {
                var haircutCreated = fixture.Create<HaircutCreated>();
                haircutCreated.DisplayName = getRandomFornavn();
                haircutCreated.CreatedAt = DateTime.UtcNow;
                var randomMilliSeconds = random.Next(totalmilliseconds);
                haircutCreated.HaircutId.DelayedExecute(randomMilliSeconds, async (Object source, System.Timers.ElapsedEventArgs e, object input) => await OnTimedEvent(source, e, haircutCreated as Object));
            }

            return Task.CompletedTask;
        }

        private string getRandomFornavn()
        {
            return fornavnListe[random.Next(fornavnListe.Count())];
        }

        private async Task OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e, object data)
        {
            var dyn = (dynamic)data;
            string operation = "/purchaseHaircut";
            if (DoesPropertyExist(dyn, "CreatedAt"))
            {
                operation = "/purchaseHaircut";
                HttpResponseMessage response = await client.PostAsJsonAsync(operation, data);
                response.EnsureSuccessStatusCode();

                var haircutStarted = fixture.Create<HaircutStarted>();
                haircutStarted.HaircutId = dyn.HaircutId;
                haircutStarted.StartedAt = ((DateTime)dyn.CreatedAt).AddMinutes(minutesFromCreatedToStart);
                var milliseconds = (int)TimeSpan.FromMinutes(minutesFromCreatedToStart).TotalMilliseconds;
                haircutStarted.HaircutId.DelayedExecute(milliseconds, async (Object source, System.Timers.ElapsedEventArgs e, object input) => await OnTimedEvent(source, e, haircutStarted as Object));

            }
            else if (DoesPropertyExist(dyn, "StartedAt"))
            {
                operation = "/startHaircut";
                HttpResponseMessage response = await client.PostAsJsonAsync(operation, data);
                response.EnsureSuccessStatusCode();

                var haircutCompleted = fixture.Create<HaircutCompleted>();
                haircutCompleted.HaircutId = dyn.HaircutId;
                haircutCompleted.CompletedAt = ((DateTime)dyn.StartedAt).AddMinutes(minutesFromStartedToCompleted);
                var milliseconds = (int)TimeSpan.FromMinutes(minutesFromStartedToCompleted).TotalMilliseconds;
                haircutCompleted.HaircutId.DelayedExecute(milliseconds, async (Object source, System.Timers.ElapsedEventArgs e, object input) => await OnTimedEvent(source, e, haircutCompleted as Object));
            }
            else if (DoesPropertyExist(dyn, "CompletedAt"))
            {
                operation = "/completeHaircut";
                HttpResponseMessage response = await client.PostAsJsonAsync(operation, data);
                response.EnsureSuccessStatusCode();
            }
            else if (DoesPropertyExist(dyn, "CancelledAt"))
            {
                operation = "/cancelHaircut";
                HttpResponseMessage response = await client.PostAsJsonAsync(operation, data);
                response.EnsureSuccessStatusCode();
            }
            // ReSimulate(data as string);
        }

        public static bool DoesPropertyExist(dynamic dyn, string name)
        {
            if (dyn is ExpandoObject)
                return ((IDictionary<string, object>)dyn).ContainsKey(name);

            return dyn.GetType().GetProperty(name) != null;
        }


    }
}
