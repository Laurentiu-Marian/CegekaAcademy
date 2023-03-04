using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;
using PetShelter.Domain.Exceptions;
using PetShelter.Domain.Extensions.DataAccess;
using PetShelter.Domain.Extensions.DomainModel;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Domain.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> GetPerson(int personId)
        {
            var person = await _personRepository.GetById(personId);
            if (person == null)
            {
                return null;
            }

            return person.ToDomainModel();
        }

        public async Task UpdateFundraiserAsync(string personId, Person person)
        {

            var personToUpdate = await _personRepository.GetPersonByIdNumber(personId);
            if (person == null)
            {
                throw new NotFoundException($"Fundraiser with id {personId} not found.");
            }

            personToUpdate.DateOfBirth = person.DateOfBirth;
            personToUpdate.IdNumber = person.IdNumber;
            personToUpdate.Name = person.Name;
            personToUpdate.FundraiserCreatorId = person.FundraiserCreatorId;

            await _personRepository.Update(personToUpdate);
        }
    }
}