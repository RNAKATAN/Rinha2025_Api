﻿using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Rinha2025_Api.Domain
{

    public class PaymentInput
    {
        public string? CorrelationId { get; set; }
        public decimal Amount { get; set; }

        public string? RequestedAt { get; set; }

    }


}
