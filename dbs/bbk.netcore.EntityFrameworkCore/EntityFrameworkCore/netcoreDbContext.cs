using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.MultiTenancy;
using Abp.IdentityServer4;
using bbk.netcore.Friendships;
using bbk.netcore.Chat;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;

namespace bbk.netcore.EntityFrameworkCore
{
	public class netcoreDbContext : AbpZeroDbContext<Tenant, Role, User, netcoreDbContext>, IAbpPersistedGrantDbContext
	{

		#region Inventory
		public DbSet<Warehouse> warehouses { get; set; }
		public DbSet<Producer> producers { get; set; }
		public DbSet<Supplier> suppliers { get; set; }
		public DbSet<Unit> units { get; set; }
		public DbSet<WarehouseType> warehouseTypes { get; set; }
		public DbSet<Items> items { get; set; }
		public DbSet<Rules> rules { get; set; }
		public DbSet<WarehouseItem> warehouseItems { get; set; }
		public DbSet<WarehouseLocationItems> warehouseLocationItems { get; set; }
		public DbSet<ExportRequest> exportRequests { get; set; }
		
		public DbSet<ExportRequestDetail> requestDetails { get; set; }
		public DbSet<Inventory> inventories { get; set; }

		public DbSet<ImportRequest> importRequests { get; set; }

		public DbSet<ImportRequestDetail> importRequestsDetail { get; set; }

		public DbSet<InventoryTicket> inventoryTicket { get; set; }

		public DbSet<InventoryTicketDetail> inventoryTicketDetail { get; set; }


		public DbSet<Subsidiary> subsidiaries { get; set; }

    public DbSet<WarehouseCardDetail> warehouseCardDetails { get; set; }
    public DbSet<WarehouseCard> warehouseCard { get; set; }

    #endregion

    #region Core Systems Additions DBSets
    //public virtual DbSet<BinaryObject> BinaryObjects { get; set; }
    public virtual DbSet<Friendship> Friendships { get; set; }

		public virtual DbSet<ChatMessage> ChatMessages { get; set; }

		public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
		#endregion

		public netcoreDbContext(DbContextOptions<netcoreDbContext> options)
				: base(options)
		{
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			/* Các khai báo của dự án TCCB - VEA */
			#region Personal Profile - Staff
			modelBuilder.Entity<ProfileStaff>(b =>
			{
				b.HasIndex(e => new { e.TenantId });
			});

			modelBuilder.Entity<Category>()
					.HasMany<TrainningInfo>(p => p.Diplomas)
					.WithOne(t => t.Diploma)
					.HasForeignKey(t => t.DiplomaId)
					.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Category>()
					.HasMany<WorkingProcess>(p => p.WorkingTitles)
					.WithOne(t => t.WorkingTitle)
					.HasForeignKey(t => t.WorkingTitleId)
					.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Document>()
					.HasMany<Commendation>(p => p.Commendations)
					.WithOne(t => t.Document)
					.HasForeignKey(t => t.DocumentId)
					.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Document>()
					.HasMany<WorkingProcess>(p => p.WorkingProcesses)
					.WithOne(t => t.Document)
					.HasForeignKey(t => t.DocumentId)
					.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Document>()
					.HasMany<StaffPlainning>(p => p.StaffPlainnings)
					.WithOne(t => t.Document)
					.HasForeignKey(t => t.DocumentId)
					.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Document>()
					.HasMany<GoAbroad>(p => p.GoAbroads)
					.WithOne(t => t.Document)
					.HasForeignKey(t => t.DocumentId)
					.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Category>()
				.HasMany<AssessedByYear>(p => p.SelfAssessments)
				.WithOne(t => t.SelfAssessment)
				.HasForeignKey(t => t.SelfAssessmentId)
				.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<Document>(b =>
			{
				b.HasIndex(e => new { e.TenantId, e.IssuedDate });
				b.HasIndex(e => new { e.TenantId, e.IssuedDate, e.DocumentCategoryEnum });
			});
			#endregion

			#region Core Systems DB Store Procedure, trigger & indexs
			//modelBuilder.Entity<BinaryObject>(b =>
			//{
			//    b.HasIndex(e => new { e.TenantId });
			//});
			modelBuilder.Entity<ChatMessage>(b =>
			{
				b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
				b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
				b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
				b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
			});

			modelBuilder.Entity<Friendship>(b =>
			{
				b.HasIndex(e => new { e.TenantId, e.UserId });
				b.HasIndex(e => new { e.TenantId, e.FriendUserId });
				b.HasIndex(e => new { e.FriendTenantId, e.UserId });
				b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
			});
			#endregion

		}
	}
}
