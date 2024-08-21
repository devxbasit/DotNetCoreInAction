using System.Text.Json.Serialization;

namespace FoodOrdering.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderState: byte
{
    Ordered,
    Preparing,
    AwaitingDelivery,
    Completed

}
