using PetShelterDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    internal sealed class EventRegistry<T> : IEventRegistry<T> where T : IEvent
    {
        private readonly Database database; // use DB after async await
        public EventRegistry(Database database)
        {
            this.database = database;
        }

        public async Task Register(T instance) // Implement this after generic types
        {
            await database.Insert(instance);
        }

        public async Task<IReadOnlyList<T>> GetAll() // Implement this after generic types
        {
            return await database.GetAll<T>();
        }

        public async Task<T> GetByName(string title) // implement after LINQ
        {
            return (await database.GetAll<T>()).Single(o => o.Title == title);
        }

        public async Task<IReadOnlyList<T>> Find(Func<T, bool> filter) // implement after lambda expressions
        {
            return (await database.GetAll<T>()).Where(filter).ToList();
        }
    }
}
