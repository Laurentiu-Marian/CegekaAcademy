using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;
using System.Net.WebSockets;

namespace PetShelter.DataAccessLayer.Repository
{
    public class FundraiserRepository : BaseRepository<Fundraiser>, IFundraiserRepository
    {
        private readonly IBaseRepository<DonationFundraiser> donationFundraiserRepository;
        private readonly IBaseRepository<Donation> donationRepository;
        private readonly IBaseRepository<Person> personRepository;

        //public FundraiserRepository()
        //{
        //    //donationFundraiserRepository = new BaseRepository<DonationFundraiser>(new PetShelterContext());
        //    //donationRepository = new BaseRepository<Donation>(new PetShelterContext());
        //}

        public FundraiserRepository(PetShelterContext context) : base(context)
        {
            donationFundraiserRepository = new DonationFundraiserRepository(context);
            donationRepository = new DonationRepository(context);
            personRepository = new PersonRepository(context);
        }

        public int GetDonationsForFundraiserById(int id)
        {
            int sumOfDonations = 0;

            var donationFundraiser = donationFundraiserRepository.GetAll().Result;

            foreach(var dF in donationFundraiser)
            {
                if(dF.FundraiserId==id)
                {
                    sumOfDonations += decimal.ToInt32(donationRepository.GetById(dF.DonationId).Result.Amount);
                }
            }

            return sumOfDonations;
        }

        public async Task<List<DonationFundraiser>> GetAllDonationsWithIdFundraiser(int id)
        {
            return await _context.Set<DonationFundraiser>().Where(x => x.FundraiserId == id).ToListAsync();
        }

        public List<Person?> GetPersonsWhoDonatedFundraiserById(int id)
        {
            var donationsWithIdFundraiser = GetAllDonationsWithIdFundraiser(id).Result;

            List<Person?> persons = new List<Person?>();

            foreach(var donation in donationsWithIdFundraiser)
            {
                var donor = donationRepository.GetById(donation.DonationId).Result;

                var person = personRepository.GetById(donor.DonorId).Result;

                persons.Add(person);

            }

            return persons;
        }
    }
}
