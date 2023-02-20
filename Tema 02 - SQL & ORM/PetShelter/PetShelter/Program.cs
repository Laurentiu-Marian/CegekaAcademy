using PetShelter.DataAccessLayer;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

FundraiserRepository fundraiserRepository = new FundraiserRepository(new PetShelterContext());

var fundraiserId = 2;

app.MapGet("/", () => $"Fundraiser with Id: {fundraiserId} has a total of: {fundraiserRepository.GetDonationsForFundraiserById(fundraiserId)}");

app.Run();
