using qrbackend.Api.Repository;
using qrbackend.Api.Services.BrokerHelper;
using qrbackend.Models.Entities;
using System;

namespace qr_backend.Repository
{
    public class RepositoryMQ<T> : IRepository<T> where T: MQResponse
    {
        readonly IBroker _broker;

        public RepositoryMQ(IBroker broker)
        {
            _broker = broker;
        }

        public T Add(T entity)
        {
            return GetResponse(entity);
        }

        public T Delete(T entity)
        {
            return GetResponse(entity);
        }

        public T Get(T entity)
        {
            return GetResponse(entity);
        }

        public T Update(T entity)
        {
            return GetResponse(entity);
        }

        private T GetResponse(T entity)
        {
            try
            {
                return _broker.SendBroker<T>(entity);
            }
            catch (Exception ex)
            {
                entity.Code = "01";
                entity.Messages = ex.Message;
            }

            return entity;
        }
    }
}
