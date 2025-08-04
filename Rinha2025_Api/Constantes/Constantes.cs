using Rinha2025_Api.Infra;

namespace Rinha2025_Api.Constantes
{
    public class Constantes
    {
        public const string CacheKeyDefaultProcessor = "Latencia Payment Processor";

        public const string CacheKeyFallbackProcessor = "";

        public const string URL_DEFAULT_PROCESSOR_HEALTHCHECK = "http://payment-processor:8001/payments/service-health";

        public const string URL_DEFAULT_PROCESSOR = "http://payment-processor:8001/payments";
                

        public const string URL_FALLBACK_PROCESSOR_HEALTHCHECK = "http://payment-processor:8002/payments/service-health";

        public const string URL_FALLBACK_PROCESSOR = "http://payment-processor:8002/payments";
    }
}
