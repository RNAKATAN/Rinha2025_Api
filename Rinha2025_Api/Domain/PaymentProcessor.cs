﻿namespace Rinha2025_Api.Domain
{
    public  class PaymentProcessor
    {
        public int TotalRequests { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalFee { get; set; }

        public decimal FeePorTransaction { get; set; }
    }
}
