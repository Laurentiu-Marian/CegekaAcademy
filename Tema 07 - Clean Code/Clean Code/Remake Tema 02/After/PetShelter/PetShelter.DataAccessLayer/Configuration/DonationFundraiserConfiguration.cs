using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetShelter.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.DataAccessLayer.Configuration
{
    public class DonationFundraiserConfiguration : IEntityTypeConfiguration<DonationFundraiser>
    {
        public void Configure(EntityTypeBuilder<DonationFundraiser> builder)
        {
            builder.HasKey(p => new { p.Id, p.DonationId, p.FundraiserId });

            builder.HasOne(p => p.Donation).WithMany(p => p.DonationFundraisers).HasForeignKey(p => p.DonationId).IsRequired();

            builder.HasOne(p => p.Fundraiser).WithMany(p => p.FundraisersDonation).HasForeignKey(p => p.FundraiserId).IsRequired();
        }
    }
}
