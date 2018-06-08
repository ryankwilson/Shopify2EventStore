using System.Collections.Generic;

public class EventResults {
    private List<ShopifyEvent> _events;

    public List<ShopifyEvent> Events { get => _events; set => _events = value; }
}