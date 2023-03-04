using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;

namespace PetShelter.DataAccessLayer.Repository;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{

    public PersonRepository(PetShelterContext context) : base(context)
    {
    }

    public async Task<Person?> GetPersonByIdNumber(string idNumber)
    {
        var toCheck = await _context.Persons.SingleOrDefaultAsync(p => p.IdNumber == idNumber);
        if (toCheck==default)
        {
            return null;
        }
        return toCheck;
    }
}