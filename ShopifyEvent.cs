using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ShopifyEvent
{
   [JsonProperty("arguments", Required=Required.AllowNull)]
   public List<string> Arguments;

   [JsonProperty("body")]
   public string Body;

   [JsonProperty("created_at")]
   public DateTime CreatedAt;

   [JsonProperty("id")]
   public long id;

    [JsonProperty("Description")]
    public string Description;

    [JsonProperty("path")]
    public string Path;

    [JsonProperty("message")]
    public string Message;

    [JsonProperty("subject_id")]
    public long SubjectId;

    [JsonProperty("subject_type")]
    public string SubjectType;

    [JsonProperty("verb")]
    [JsonConverter(typeof(OrderVerbConverter))]
    public OrderVerb Verb;
}