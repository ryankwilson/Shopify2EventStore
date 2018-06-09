using System;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;

public class EventStoreClient
{
    private IEventStoreConnection _conn;
    public EventStoreClient()
    {
        _conn = EventStoreConnection.Create(new IPEndPoint(
            IPAddress.Parse("10.0.0.4"),
            1113
        ));
        _conn.Connected += EventStore_Connected;
        _conn.Closed += EventStore_Closed;
        _conn.Disconnected += EventStore_Disconnected;
        _conn.Reconnecting += EventStore_Reconnecting;
        _conn.ErrorOccurred += EventStore_ErrorOccured;
    }

    public void Connect()
    {
        try
        {
            _conn.ConnectAsync().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"**Trouble connecting to Event Store {ex.Message}");
        }
    }

    private void EventStore_ErrorOccured(object sender, ClientErrorEventArgs e)
    {
        Console.WriteLine("** Event Store - Error Occured");
    }

    private void EventStore_Reconnecting(object sender, ClientReconnectingEventArgs e)
    {
        Console.WriteLine("** Event Store - Reconnecting");
    }

    private void EventStore_Disconnected(object sender, ClientConnectionEventArgs e)
    {
        Console.WriteLine("** Event Store - Disconnected");
    }

    private void EventStore_Closed(object sender, ClientClosedEventArgs e)
    {
        Console.WriteLine("** Event Store - Closed");
    }

    private void EventStore_Connected(object sender, ClientConnectionEventArgs e)
    {
        Console.WriteLine("** Event Store - Connected");
    }

    public void PublishOrderEvent(ShopifyEvent domainEvent)
    {
        var eventJson = JsonConvert.SerializeObject(domainEvent);

        _conn.AppendToStreamAsync(
            $"OrderTest-{domainEvent.SubjectId}",
            ExpectedVersion.Any,
            new EventData(
                Guid.NewGuid(),
                domainEvent.Verb.ToString(),
                true,
                Encoding.UTF8.GetBytes(eventJson),
                new byte[] { }
            )).Wait();
    }
}