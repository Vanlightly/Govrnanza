using Govrnanza.Registry.WebApi.Model;
using Govrnanza.Registry.WebApi.Model.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Govrnanza.Registry.WebApi.Infrastructure.Database
{
    public class RegistryDbContext : DbContext
    {
        public RegistryDbContext(DbContextOptions<RegistryDbContext> options) : base(options)
        {}

        public virtual DbSet<Api> Apis { get; set; }
        public virtual DbSet<BusinessDomain> BusinessDomains { get; set; }
        public virtual DbSet<BusinessSubDomain> BusinessSubDomains { get; set; }
        public virtual DbSet<ApiTag> ApiTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
