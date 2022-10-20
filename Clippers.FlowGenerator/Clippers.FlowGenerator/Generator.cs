using AutoFixture;
using Clippers.FlowGenerator.Commands;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Clippers.FlowGenerator
{
    public class Generator : IGenerator
    {
        private readonly Random random = new Random();
        private readonly int _minutesFromCreatedToStart = 2;
        private readonly int _minutesFromStartedToCompleted = 5;
        private readonly int _numOfHaircuts = 20;
        private readonly int _duration = 10;
        private string[] fornavnListe = new string[] { "Markus", "Lilly", "Emma", "Noa", "Markus", "Amanda", "Maja", "Vilde", "Nicolai", "Sarah", "Phillip", "Sophie", "Mathilde", "Anna", "Casper", "Astri", "Elias", "Johan", "Noah", "Axel", "Maria", "Johannes", "Iben", "Jonas", "Agnes", "Nora", "Sigrid", "Kasper", "Emma", "Adam", "Astri", "Anna", "Johann", "Viktoria", "Oskar", "Jakob", "Sophie", "Elias", "Kasper", "Theo", "Hanna", "Aleksander", "Oline", "Lea", "Oline", "Ida", "Hannah", "Sigrid", "Ellinor", "Aleksander", "Olav", "Sebastian", "Ellinor", "Kasper", "Astrid", "Bantam", "Haakon", "Jonas", "Liam", "Jacob", "Kaia", "Emma", "Tiril", "Victor", "Håkon", "Victoria", "Felix", "Amelia", "Sophia", "Liam", "Selma", "Herman", "Viktoria", "Johan", "Aegon", "Marie", "Emilie", "Henry", "Emil", "Mathilde", "Eline", "Noah", "Dany", "Matilde", "Amanda", "Ella", "Fredeico", "Mikkel", "Even", "Jonas", "Astri", "Mikaela", "Philip", "Jonas", "Jonna", "Sophie", "Lilly", "Oliver", "Alexander", "Agnes" };
        private static HttpClient client = new HttpClient();
        
        private Fixture fixture = new Fixture();
        public Generator(int numOfHaircuts = 20, int duration = 10, int minutesFromCreatedToStart = 2, int minutesFromStartedToCompleted = 5)
        {
            client.BaseAddress = new Uri("https://localhost:7255/");
            _numOfHaircuts = numOfHaircuts;
            _minutesFromCreatedToStart = minutesFromCreatedToStart;
            _minutesFromStartedToCompleted = minutesFromStartedToCompleted;
        }

        public Task Generate()
        {
           // var now = DateTime.Now;
            //var start = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
            //var end = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);

            var durationSpan = TimeSpan.FromMinutes(_duration);

            var totalmilliseconds = (int)durationSpan.TotalMilliseconds;

            for (int i = 0; i < _numOfHaircuts; i++)
            {
                var createHaircutCommand = fixture.Create<CreateHaircutCommand>();
                createHaircutCommand.DisplayName = getRandomFornavn();
                createHaircutCommand.CreatedAt = DateTime.UtcNow;
                var randomMilliSeconds = random.Next(totalmilliseconds);
                if (i == 0)
                {
                    randomMilliSeconds = 1000;
                }
                i.DelayedExecute(randomMilliSeconds, async (Object source, System.Timers.ElapsedEventArgs e, object input) => await OnTimedEvent(source, e, createHaircutCommand as Object));
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
            string operation = "/createHaircut";
            if (DoesPropertyExist(dyn, "CreatedAt"))
            {
                operation = "/createHaircut";
                HttpResponseMessage response = await client.PostAsJsonAsync(operation, data);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Created Haircut for {dyn.DisplayName}.");
                var result = await response.Content.ReadAsStringAsync();
                var haircutIdWrapper = JsonSerializer.Deserialize<HaircutIdWrapper>(result);
                var startHaircutCommand = fixture.Create<StartHaircutCommand>();
                startHaircutCommand.HaircutId = haircutIdWrapper.HaircutId;
                startHaircutCommand.StartedAt = ((DateTime)dyn.CreatedAt).AddMinutes(_minutesFromCreatedToStart);
                var milliseconds = (int)TimeSpan.FromMinutes(_minutesFromCreatedToStart).TotalMilliseconds;
                startHaircutCommand.HaircutId.DelayedExecute(milliseconds, async (Object source, System.Timers.ElapsedEventArgs e, object input) => await OnTimedEvent(source, e, startHaircutCommand as Object));

            }
            else if (DoesPropertyExist(dyn, "StartedAt"))
            {
                operation = "/startHaircut";
                HttpResponseMessage response = await client.PostAsJsonAsync(operation, data);
                response.EnsureSuccessStatusCode();

                var completeHaircutCommand = fixture.Create<CompleteHaircutCommand>();
                completeHaircutCommand.HaircutId = dyn.HaircutId;
                completeHaircutCommand.CompletedAt = ((DateTime)dyn.StartedAt).AddMinutes(_minutesFromStartedToCompleted);
                var milliseconds = (int)TimeSpan.FromMinutes(_minutesFromStartedToCompleted).TotalMilliseconds;
                completeHaircutCommand.HaircutId.DelayedExecute(milliseconds, async (Object source, System.Timers.ElapsedEventArgs e, object input) => await OnTimedEvent(source, e, completeHaircutCommand as Object));
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
