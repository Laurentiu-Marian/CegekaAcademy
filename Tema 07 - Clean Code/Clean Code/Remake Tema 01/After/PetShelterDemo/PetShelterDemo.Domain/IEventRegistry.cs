using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public interface IEventRegistry<T> where T: IEvent
    {
        Task Register(T instance);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetByName(string name);
        Task<IReadOnlyList<T>> Find(Func<T, bool> filter);
    }
}
