using PetShelterDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public class Fundraiser : IEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DonationTarget { get; set; }

        public int CurrentDonation = 0;

        private readonly IRegistry<Donations> fundraiserDonationsRegistry;

        public Fundraiser()
        {

        }

        public Fundraiser(string title, string description, int donationTarget)
        {
            Title = title;
            Description = description;
            DonationTarget = donationTarget;
            fundraiserDonationsRegistry = new Registry<Donations>(new Database());
        }

        public void RegisterFundraise(Donations donation)
        {
            fundraiserDonationsRegistry.Register(donation);
        }

        public IReadOnlyList<Donations> GetAllFundraiserDonations()
        {
            return fundraiserDonationsRegistry.GetAll().Result;
        }
    }
}
