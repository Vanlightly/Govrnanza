using Govrnanza.Registry.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace Govrnanza.Registry.Backend.Infrastructure.Database
{
    public class RegistryDbContext : DbContext
    {
        public RegistryDbContext(DbContextOptions<RegistryDbContext> options) : base(options)
        {}

        public virtual DbSet<Api> Apis { get; set; }
        public virtual DbSet<ApiVersion> ApiVersions { get; set; }
        public virtual DbSet<BusinessDomain> BusinessDomains { get; set; }
        public virtual DbSet<BusinessSubDomain> BusinessSubDomains { get; set; }
        public virtual DbSet<ApiTag> ApiTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiVersion>()
                .HasKey(x => new { x.ApiId, x.MajorVersion, x.MinorVersion });
        }
    }
}
