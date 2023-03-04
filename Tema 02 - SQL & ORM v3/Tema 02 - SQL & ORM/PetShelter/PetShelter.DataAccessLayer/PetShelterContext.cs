using Microsoft.EntityFrameworkCore;
using PetShelter.DataAccessLayer.Configuration;
using PetShelter.DataAccessLayer.Models;

namespace PetShelter.DataAccessLayer;

public class PetShelterContext : DbContext
{
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<Fundraiser> Fundraisers { get; set; }
    public DbSet<DonationFundraiser> DonationFundraisers { get; set; }

    //Tema 01 Database Class
    public static bool ConnectionIsDown = false;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-2QE8A4D\SQLEXPRESS01;Database=PetShelter;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PetConfiguration());
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new DonationConfiguration());
        modelBuilder.ApplyConfiguration(new FundraiserConfiguration());
        modelBuilder.ApplyConfiguration(new DonationFundraiserConfiguration());
    }
}