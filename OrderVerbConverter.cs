using Newtonsoft.Json;
using System;

public class OrderVerbConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        OrderVerb orderVerb = (OrderVerb)value;

        writer.WriteValue(orderVerb.ToString());

        // switch (orderVerb)
        // {
        //     case OrderVerb.AuthorizationSuccess:
        //         writer.WriteValue("AuthorizationSuccess");
        //         break;
        // }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var enumString = (string)reader.Value;

        switch (enumString.ToLower())
        {
            case "authorization_failure":
                return OrderVerb.AuthorizationFailure;
            case "authorization_pending":
                return OrderVerb.AuthorizationPending;
            case "authorization_success":
                return OrderVerb.AuthorizationSuccess;
            case "cancelled":
                return OrderVerb.Cancelled;
            case "capture_failure":
                return OrderVerb.CaptureFailure;
            case "capture_pending":
                return OrderVerb.CapturePending;
            case "capture_success":
                return OrderVerb.CaptureSuccess;
            case "closed":
                return OrderVerb.Closed;
            case "confirmed":
                return OrderVerb.Confirmed;
            case "fulfillment_cancelled":
                return OrderVerb.FulfillmentCancelled;
            case "fulfillment_pending":
                return OrderVerb.FulfillmentPending;
            case "fulfillment_success":
                return OrderVerb.FulfillmentSuccess;
            case "mail_sent":
                return OrderVerb.MailSent;
            case "placed":
                return OrderVerb.Placed;
            case "re_opened":
                return OrderVerb.ReOpened;
            case "refund_failure":
                return OrderVerb.RefundFailure;
            case "refund_pending":
                return OrderVerb.RefundPending;
            case "refund_success":
                return OrderVerb.RefundSuccess;
            case "restock_line_items":
                return OrderVerb.RestockLineItems;
            case "sale_failure":
                return OrderVerb.SaleFailure;
            case "sale_pending":
                return OrderVerb.SalePending;
            case "sale_success":
                return OrderVerb.SalesSuccess;
            case "update":
                return OrderVerb.Update;
            case "void_failure":
                return OrderVerb.VoidFailure;
            case "void_pending":
                return OrderVerb.VoidPending;
            case "void_success":
                return OrderVerb.VoidSuccess;
            case "sms_sent":
                return OrderVerb.SmsSent;
            case "payments_charge":
                return OrderVerb.PaymentsCharge;
            case "changed":
                return OrderVerb.Changed;
            case "emv_sale_success":
                return OrderVerb.EmvSaleSuccess;
            case "follow_up":
                return OrderVerb.FollowUp;
            case "refund_restock":
                return OrderVerb.RefundRestock;
            case "comment":
                return OrderVerb.Comment;
            case "inventory_management_failure":
                return OrderVerb.InventoryManagementFailure;
        }

        // we have a problem...
        Console.WriteLine($"... Unknown verb found {enumString}");
        return OrderVerb.UNKNOWN;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string);
    }
}