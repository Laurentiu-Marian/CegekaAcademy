using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Repository
{
    public class DonationFundraiserRepository : BaseRepository<DonationFundraiser>, IDonationFundraiserRepository
    {
        public DonationFundraiserRepository(PetShelterContext context) : base(context)
        {

        }

    }
}
