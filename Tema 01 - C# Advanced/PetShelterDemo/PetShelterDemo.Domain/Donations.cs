using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelterDemo.Domain
{
    public class Donations : Person
    {
        public int RONDonation { get; set; } = 0;

        public int EURODonation { get; set; } = 0;

        public Donations()
        {

        }

        public Donations(string name, string idNumber, int ronDonation, int euroDonation) : base(name, idNumber)
        {
            RONDonation = ronDonation;
            EURODonation = euroDonation;
        }
    }
}
