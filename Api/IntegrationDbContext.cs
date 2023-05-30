
using System.Configuration;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Api;

public class IntegrationDbContext: DbContext
{
    public DbSet<RateRank> RateRanks { get; set; } 
    
    public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options): base(options) 
    {}
}