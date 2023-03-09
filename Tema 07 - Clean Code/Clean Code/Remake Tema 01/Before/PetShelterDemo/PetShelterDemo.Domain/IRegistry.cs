﻿namespace PetShelterDemo.Domain
{
    public interface IRegistry<T> where T: INamedEntity
    {
        Task Register(T instance);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetByName(string name);
        Task<T> GetById(string id);
        Task<IReadOnlyList<T>> Find(Func<T, bool> filter);
    }
}
