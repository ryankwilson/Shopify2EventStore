using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Shopify2EventStore
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            try {
            // Get all Orders (as events)
            GetAllOrders();
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

           Console.ReadLine();
        }

        static async void GetAllOrders() 
        {
            try 
            {
                client.BaseAddress = new Uri("https://god-feet.myshopify.com/admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var byteArray = Encoding.ASCII.GetBytes("b44e2b63faf1a5a8fd4159f51ddb4e18:0d500f6b7fd4a0abd9370a64d1b5c7ef");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));


                HttpResponseMessage response = await client.GetAsync("events.json?limit=2&filter=order");
                if (response.IsSuccessStatusCode) {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonObj  = JsonConvert.DeserializeObject<EventResults>(json);
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
