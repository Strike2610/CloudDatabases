﻿using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace ShipOrder {
    public class ShippedResponse {
        [QueueOutput("shipped-orders")]
        public required string[] Messages { get; set; }
        public required HttpResponseData HttpResponse { get; set; }
    }
}