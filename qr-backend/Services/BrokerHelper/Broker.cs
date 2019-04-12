using System;
using Newtonsoft.Json;
using qrbackend.Api.Helpers;


namespace qrbackend.Api.Services.BrokerHelper
{
    public class Broker : IBroker
    {

        public T SendBroker<T>(dynamic json, int timeOut = 0)
        {

            var data = JsonConvert.SerializeObject(json);

            string _response = string.Empty;

            using (MQManager mqm = GetConnection(new InfoBroker()))
            {
                mqm.mtPutMessage(data);
                _response = mqm.mtGetMessage(timeOut);
            }

            return JsonConvert.DeserializeObject<T>(_response);
        }

        public MQManager GetConnection(InfoBroker infoBroker)
        {
            MQManager mqm = new MQManager(infoBroker);

            mqm.Connect();

            if (!mqm.IsConnected)
                throw new Exception("No se pudo conectar al MQ.");

            return mqm;
        }

        public T GetFromBroker<T>(dynamic json, int timeOut = 0)
        {
            var data = JsonConvert.SerializeObject(json);

            string _response = string.Empty;

            using (MQManager mqm = GetConnection(new InfoBroker()))
            {
                mqm.mtPutMessage(data);
                _response = mqm.mtGetMessage(timeOut);
            }

            return JsonConvert.DeserializeObject<T>(_response);
        }
    }
}
