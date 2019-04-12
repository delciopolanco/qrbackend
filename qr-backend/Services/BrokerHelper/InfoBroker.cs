using qr_backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace qrbackend.Api.Services.BrokerHelper
{
    public class InfoBroker
    {

        public InfoBroker()
        {
            QueueIN = Startup.Configuration["MQConnection:QueueIN"];
            QueueOUT = Startup.Configuration["MQConnection:QueueOUT"];
            QueueManagerName = Startup.Configuration["MQConnection:QueueManagerName"];
            QIp = Startup.Configuration["MQConnection:QIp"];
            QPort = Startup.Configuration["MQConnection:QPort"];
            ChannelInfo = Startup.Configuration["MQConnection:ChannelInfo"];
            TimeOut = int.Parse(Startup.Configuration["MQConnection:TimeOut"]);
        }

        public string xml { get; set; }
        public string QueueOUT { get; set; }
        public string QueueIN { get; set; }
        public string QueueManagerName { get; set; }
        public string QIp { get; set; }
        public string QPort { get; set; }
        public string ChannelInfo { get; set; }
        public int TimeOut { get; set; }
    }
}
