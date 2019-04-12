using qrbackend.Api.Helpers;

namespace qrbackend.Api.Services.BrokerHelper
{
    public interface IBroker
    {
        MQManager GetConnection(InfoBroker infoBroker);
        T SendBroker<T>(dynamic json, int timeOut = 0);
        T GetFromBroker<T>(dynamic json, int timeOut = 0);
    }
}
