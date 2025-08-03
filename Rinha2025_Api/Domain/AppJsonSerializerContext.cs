using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Rinha2025_Api.Domain
{
    [JsonSerializable(typeof(PaymentInput))]
    public partial class AppJsonSerializerContext : JsonSerializerContext
    { 
    }
}
