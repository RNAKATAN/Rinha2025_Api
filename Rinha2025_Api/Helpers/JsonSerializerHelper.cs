using Rinha2025_Api.Domain;
using System.Net.Http;
using System.Text.Json;

namespace Rinha2025_Api.Helpers
{
    public class JsonSerializerHelper<T>
    {
        public static string Serialize(T ObjetoEntrada)
        {

            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = AppJsonSerializerContext.Default
            };


            return JsonSerializer.Serialize(ObjetoEntrada, options);
        }

        public static T Deserialize(string TextoEntrada)
        {
            T objeto;

            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = AppJsonSerializerContext.Default
            };


            return JsonSerializer.Deserialize<T>(TextoEntrada, options);
        }

    }
}
