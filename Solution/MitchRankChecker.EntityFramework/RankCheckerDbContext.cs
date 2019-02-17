using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MitchRankChecker.Model;
using MitchRankChecker.Model.Interfaces;

namespace MitchRankChecker.EntityFramework
{
    public class RankCheckerDbContext : DbContext
    {
        public RankCheckerDbContext (DbContextOptions<RankCheckerDbContext> options) : base(options){ }

        public DbSet<SearchEntry> SearchEntries { get; set; }
        public DbSet<RankCheckRequest> RankCheckRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesonSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateTrackingColumns();
            return base.SaveChangesAsync(acceptAllChangesonSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTrackingColumns();
            return base.SaveChanges();
        }

        private void UpdateTrackingColumns()
        {
            ChangeTracker.DetectChanges();
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    DateTime now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = now;
                            break;
                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            goto case EntityState.Modified;
                    }
                }
            }
        }
    }
}