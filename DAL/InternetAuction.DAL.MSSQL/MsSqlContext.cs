using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL
{
    /// <summary>
    /// The m s s q l context.
    /// </summary>
    public class MsSqlContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>, RoleUser, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        /*  public MsSqlContext(DbContextOptions<MsSqlContext> options)
        : base(options)
          {
          }*/
        private string connectionString;

        public MsSqlContext()
        {
        }

        public MsSqlContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MsSqlContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.Use
            optionsBuilder.UseSqlServer(connectionString);
            //      optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=InternetAuction;Trusted_Connection=True;");
        }

        /// <summary>
        /// Gets or sets the autctions.
        /// </summary>
        /// <value>
        /// The autctions.
        /// </value>
        public DbSet<Autction> Autctions { get; set; }

        /// <summary>
        /// Gets or sets the autction statuses.
        /// </summary>
        /// <value>
        /// The autction statuses.
        /// </value>
        public DbSet<AutctionStatus> AutctionStatuses { get; set; }

        /// <summary>
        /// Gets or sets the biddings.
        /// </summary>
        /// <value>
        /// The biddings.
        /// </value>
        public DbSet<Bidding> Biddings { get; set; }

        /// <summary>
        /// Gets or sets the image ids.
        /// </summary>
        /// <value>
        /// The image ids.
        /// </value>
        public DbSet<ImageId> ImageIds { get; set; }

        /// <summary>
        /// Gets or sets the lots.
        /// </summary>
        /// <value>
        /// The lots.
        /// </value>
        public DbSet<Lot> Lots { get; set; }

        /// <summary>
        /// Gets or sets the lot categories.
        /// </summary>
        /// <value>
        /// The lot categories.
        /// </value>
        public DbSet<LotCategory> LotCategories { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
            modelBuilder.Entity<User>().
                 HasMany(c => c.Lots).
                 WithOne(r => r.Author);

            modelBuilder.Entity<Lot>().
                HasOne(c => c.Author);

            modelBuilder.Entity<Lot>().
                 HasMany(c => c.Photos);

            modelBuilder.Entity<Lot>().
                HasOne(c => c.PhotoCurrent);

            modelBuilder.Entity<User>().
                 HasMany(c => c.Avatars);

            modelBuilder.Entity<User>().
                HasOne(c => c.AvatarCurrent);

            modelBuilder.Entity<LotCategory>().
               HasMany(c => c.Lots).
               WithOne(r => r.Category);

            modelBuilder.Entity<Lot>().
                 HasOne(c => c.Category);

            modelBuilder.Entity<AutctionStatus>().
                HasMany(c => c.Autctions).
                WithOne(r => r.Status);

            modelBuilder.Entity<Autction>().
                HasOne(c => c.Status);

            modelBuilder.Entity<Autction>().
                HasMany(c => c.Biddings).
                WithOne(r => r.Autction);

            modelBuilder.Entity<Bidding>().
                HasOne(c => c.Autction);

            //  modelBuilder.Entity<Autction>().Has(c => c.Lot);//.WithOne(x => x.Lot);
            /* modelBuilder.Entity<Autction>()
               .HasOne(s => s.Lot)
               .WithOne(ad => ad.Autction);*/

            modelBuilder.Entity<Autction>()
      .HasOne(a => a.Lot)
      .WithOne(b => b.Autction)
      .HasForeignKey<Lot>(b => b.AutctionRef);
            /*   modelBuilder.Entity<Autction>().
                   HasOne(c => c.Lot);*/

            modelBuilder.Entity<User>().
                HasMany(c => c.Biddings).
                WithOne(r => r.User);

            modelBuilder.Entity<Bidding>().
                HasOne(c => c.User);

            modelBuilder.Entity<Lot>().
                Property(p => p.CostMin).
                HasColumnType("decimal");

            modelBuilder.Entity<Bidding>().
                Property(p => p.Cost).
                HasColumnType("decimal");
        }
    }
}