using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Models
{
    public class Fundraiser : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DonationTarget { get; set; }

        public ICollection<DonationFundraiser> fundraisersDonation { get; set; }
    
        public Fundraiser(string title, string description, int donationTarget) 
        {
            Title = title;
            Description = description;
            DonationTarget = donationTarget;
        }
    }
}
