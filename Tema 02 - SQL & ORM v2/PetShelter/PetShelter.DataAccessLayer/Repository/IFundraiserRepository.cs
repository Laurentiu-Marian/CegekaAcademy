using PetShelter.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Repository
{
    public interface IFundraiserRepository : IBaseRepository<Fundraiser>
    {
        int GetDonationsForFundraiserById(int id);
        Task<List<DonationFundraiser>> GetAllDonationsWithIdFundraiser(int id);
        List<Person?> GetPersonsWhoDonatedFundraiserById(int id);
    }
}
