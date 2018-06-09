using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shopify2EventStore
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static EventStoreClient esClient = new EventStoreClient();

        static void Main(string[] args)
        {
            try
            {
                // Setup Http Client
                client.BaseAddress = new Uri("https://god-feet.myshopify.com/admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var byteArray = Encoding.ASCII.GetBytes("b44e2b63faf1a5a8fd4159f51ddb4e18:0d500f6b7fd4a0abd9370a64d1b5c7ef");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Connect to Event Store
                esClient.Connect();

                // Get all Orders (as events)
                GetAllOrders();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        static async void GetAllOrders()
        {
            try
            {
                // need to page through events
                var currentPage = 1;
                var limit = 100;    // # of events at a time
                var numOfEvents = 0;

                do
                {
                    Console.WriteLine($"Getting page {currentPage} of Orders...");
                    var response = await GetEvents($"events.json?filter=order&limit={limit}&page={currentPage++}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var eventResults = JsonConvert.DeserializeObject<EventResults>(json);
                        numOfEvents = eventResults.Events.Count;
                        Console.WriteLine($"... found {numOfEvents} events");
                        if (numOfEvents > 0)
                        {
                            await ProcessOrders(eventResults);
                        }
                    }
                } while (numOfEvents > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task<HttpResponseMessage> GetEvents(string path)
        {
            return await client.GetAsync(path);
        }

        static async Task GetOrderEvents(ShopifyEvent orderEvent)
        {
            try
            {
                var response = await GetEvents($"{orderEvent.Path}/events.json");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var eventResults = JsonConvert.DeserializeObject<EventResults>(json);
                    if (eventResults.Events.Count > 0)
                    {
                        await ProcessOrderEvents(orderEvent.SubjectId, eventResults);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task ProcessOrders(EventResults eventResults)
        {
            Console.WriteLine($"Processing {eventResults.Events.Count} Orders...");
            foreach (var ev in eventResults.Events)
            {
                Console.WriteLine($"Processing {ev.SubjectType}-{ev.SubjectId}...");
                Console.WriteLine("====================");
                await GetOrderEvents(ev);
                Console.WriteLine();
            }
        }

        static async Task ProcessOrderEvents(long orderId, EventResults results)
        {
            foreach (var ev in results.Events)
            {
                Console.WriteLine($"   {ev.Verb} - {ev.CreatedAt}");
                esClient.PublishOrderEvent(ev);
            }
        }
    }
}
