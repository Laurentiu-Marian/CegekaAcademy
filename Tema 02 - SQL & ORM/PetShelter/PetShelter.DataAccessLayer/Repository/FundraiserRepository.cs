using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;
using System.Net.WebSockets;

namespace PetShelter.DataAccessLayer.Repository
{
    public class FundraiserRepository : BaseRepository<Fundraiser>, IFundraiserRepository
    {
        //private readonly IBaseRepository<Fundraiser> fundraiserRepository;
        private readonly IBaseRepository<DonationFundraiser> donationFundraiserRepository;
        private readonly IBaseRepository<Donation> donationRepository;

        //public FundraiserRepository()
        //{
        //    //donationFundraiserRepository = new BaseRepository<DonationFundraiser>(new PetShelterContext());
        //    //donationRepository = new BaseRepository<Donation>(new PetShelterContext());
        //}

        public FundraiserRepository(PetShelterContext context) : base(context)
        {
            donationFundraiserRepository = new BaseRepository<DonationFundraiser>(context);
            donationRepository = new BaseRepository<Donation>(context);
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
    }
}
