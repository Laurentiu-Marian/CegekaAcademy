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

        public FundraiserRepository(PetShelterContext context) : base(context)
        {
            donationFundraiserRepository = new DonationFundraiserRepository(context);
            donationRepository = new DonationRepository(context);
            personRepository = new PersonRepository(context);
        }

        public int GetDonationsForFundraiserById(int id)
        {
            int sumOfDonations = 0;

            var donationsForFundraiser = donationFundraiserRepository.GetAll()
                .Result;

            foreach(var donation in donationsForFundraiser)
            {
                if(donation.FundraiserId==id)
                {
                    sumOfDonations += decimal.ToInt32(donationRepository.GetById(donation.DonationId)
                        .Result.Amount);
                }
            }

            return sumOfDonations;
        }

        public async Task<List<DonationFundraiser>> GetAllDonationsWithIdFundraiser(int id)
        {
            return await _context.Set<DonationFundraiser>().Where(x => x.FundraiserId == id)
                .ToListAsync();
        }

        public List<Person?> GetPersonsWhoDonatedFundraiserById(int id)
        {
            var donationsWithIdFundraiser = GetAllDonationsWithIdFundraiser(id).Result;

            List<Person?> persons = new List<Person?>();

            foreach(var donation in donationsWithIdFundraiser)
            {
                var donor = donationRepository.GetById(donation.DonationId)
                    .Result;

                var person = personRepository.GetById(donor.DonorId)
                    .Result;

                persons.Add(person);

            }

            return persons;
        }
    }
}
