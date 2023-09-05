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

        /* CONVENSION: Define a DbSet for each entity of the application */
        /* Các khai báo của dự án TCCB - VEA */
        //#region Personal Profile - Staff
        //public virtual DbSet<UploadFile> UploadFiles { get; set; }
        //public virtual DbSet<ProfileStaff> PersonalProfiles { get; set; }

        //public virtual DbSet<OrganizationUnitStaff> OrganizationUnitStaffs { get; set; }

        //public virtual DbSet<Category> Categories { get; set; }

        //public virtual DbSet<BonusInfomation> BonusInfomations { get; set; }

        //#region Trainning Information
        //public DbSet<TrainningInfo> TrainningInfos { get; set; }
        //#endregion

        //#region Working Process
        //public DbSet<WorkingProcess> WorkingProcesses { get; set; }
        //#endregion

        //#region RelationShip
        //public DbSet<RelationShip> RelationShips { get; set; }
        //#endregion

        //#region Salary Process
        //public DbSet<SalaryProcess> SalaryProcesses { get; set; }
        //#endregion    

        //#region Commendation
        //public DbSet<Commendation> Commendations { get; set; }
        //#endregion

        //#region Staff Plainning
        //public DbSet<StaffPlainning> StaffPlainnings { get; set; }
        //#endregion

        //#region Go Abroad
        //public DbSet<GoAbroad> GoAbroads { get; set; }
        //#endregion

        //#region Communist Party Process
        //public DbSet<CommunistPartyProcess> CommunistPartyProcesses { get; set; }
        //#endregion

        //#region Document
        //public DbSet<Document> Documents { get; set; }
        //#endregion

        //#region Property Declaration
        //public DbSet<PropertyDeclaration> PropertyDeclarations { get; set; }
        //#endregion

        //#region Civil Servant + Salary Level
        //public virtual DbSet<CivilServant> CivilServants { get; set; }
        //public virtual DbSet<SalaryLevel> SalaryLevels { get; set; }
        //#endregion
        //public DbSet<RecruitmentInfomation> RecruitmentInfomations { get; set; }
        //public DbSet<AssessedByYear> AssessedByYear { get; set; }
        //#endregion

        #region OMS
        public DbSet<Work> workItems { get; set; }
        public DbSet<DayOff> dayOffs { get; set; }
        public DbSet<WorkGroup> workGroups { get; set; }
        public DbSet<ProfileWork> profileWorks { get; set; }
        public DbSet<ScheduleWork> scheduleWorks { get; set; }
        public DbSet<UserWork> userWorks { get; set; }
        public DbSet<UploadFileCV> uploadFileCVs { get; set; }

        #endregion

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
        public DbSet<Expert> expert { get; set; }
        public DbSet<Assignment> assignments { get; set; }

		public DbSet<PurchasesRequest> purchasesRequest { get; set; }

		public DbSet<PurchasesRequestDetail> purchasesRequestDetail { get; set; }
		public DbSet<PurchasesSynthesise> purchasesSynthesises { get; set; }

        public DbSet<Subsidiary> subsidiaries { get; set; }
        public DbSet<Quote> quotes { get; set; }
        public DbSet<QuoteRelationship> quoteRelationships { get; set; }
        public DbSet<QuoteSynthesise> quoteSynthesises { get; set; }

        public DbSet<PurchaseAssignment> purchaseAssignments { get; set; }

        public DbSet<Contract> contracts { get; set; }
        public DbSet<Order> orders { get; set; }

        public DbSet<SendMailSupplier> sendMailSuppliers { get; set; }
        public DbSet<Transfer> transfers { get; set; }
        public DbSet<TransferDetail> transferDetails { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<QuoteRequest> quoteRequest { get; set; }

        public DbSet<UserWorkCount> UserWorkCounts { get; set; }
        public DbSet<ImportRequestSubsidiary> importRequestSubsidiaries { get; set; }

        public DbSet<ImportRequestSubsidiaryDetail> importRequestSubsidiaryDetails { get; set; }

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
