using System.Text.Json.Serialization;

namespace Orderly.Application.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketStatus
{
    Todo,
    InProgress,
    Implemented
}
