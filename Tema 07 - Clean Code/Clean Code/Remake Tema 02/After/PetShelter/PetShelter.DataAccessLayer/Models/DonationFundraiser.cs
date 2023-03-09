﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Models
{
    public class DonationFundraiser : IEntity
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
        public int FundraiserId { get; set; }
        public Fundraiser Fundraiser { get;set; }

        public DonationFundraiser(int donationId, int fundraiserId) 
        {
            DonationId = donationId;
            FundraiserId = fundraiserId;
        }

    }
}
