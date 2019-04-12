using qrbackend.Models.Entities;
using qrbackend.Models.ViewModels;

namespace qrbackend.Api.Repository
{
    public interface IRepository<T> where T : MQResponse
    {
        T Add(T entity);
        T Delete(T entity);
        T Update(T entity);
        T Get(T entity);
    }
}
