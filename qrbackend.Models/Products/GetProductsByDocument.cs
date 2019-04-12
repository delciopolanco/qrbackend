using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Models.Products
{
    public class GetProductsByDocument: MQResponse2<List<ClientProduct>>
    {
        public string productNumber { get; set; }

        public string DocumentId { get; set; }

        public GetProductsByDocument()
        {
            FunctionName = "GetProductsByDocument";
        }
    }
}
