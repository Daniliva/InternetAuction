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

            modelBuilder.Entity<Role>().HasData(new Role { Id = "1", Name = "Customer" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = "2", Name = "Owner" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = "3", Name = "Admin" });

            modelBuilder.Entity<LotCategory>().HasData(new LotCategory { Id = 1, NameCategory = "Arts", DescriptionCategory = "The arts are a very wide range of human practices of creative expression, storytelling and cultural participation. They encompass multiple diverse and plural modes of thinking, doing and being, in an extremely broad range of media. Both highly dynamic and a characteristically constant feature of human life, they have developed into innovative, stylized and sometimes intricate forms." });
            modelBuilder.Entity<LotCategory>().HasData(new LotCategory { Id = 2, NameCategory = "Books", DescriptionCategory = "A book is a medium for recording information in the form of writing or images, typically composed of many pages (made of papyrus, parchment, vellum, or paper) bound together and protected by a cover.[1] The technical term for this physical arrangement is codex (plural, codices). In the history of hand-held physical supports for extended written compositions or records, the codex replaces its predecessor, the scroll. A single sheet in a codex is a leaf and each side of a leaf is a page." });
            modelBuilder.Entity<LotCategory>().HasData(new LotCategory { Id = 3, NameCategory = "Antiques", DescriptionCategory = "A true antique (Latin: antiquus; 'old', 'ancient') is an item perceived as having value because of its aesthetic or historical significance, and often defined as at least 100 years old (or some other limit), although the term is often used loosely to describe any object that is old.[1] An antique is usually an item that is collected or desirable because of its age, beauty, rarity, condition, utility, personal emotional connection, and/or other unique features. It is an object that represents a previous era or time period in human history. Vintage and collectible are used to describe items that are old, but do not meet the 100-year criterion." });

            modelBuilder.Entity<AutctionStatus>().HasData(new AutctionStatus { Id = 1, NameStatus = "Start", DescriptionStatus = "Auction is started" });
            modelBuilder.Entity<AutctionStatus>().HasData(new AutctionStatus { Id = 1, NameStatus = "Finish", DescriptionStatus = "Auction is finished" });
            modelBuilder.Entity<AutctionStatus>().HasData(new AutctionStatus { Id = 1, NameStatus = "Is not started", DescriptionStatus = "Auction isn't started" });
        }
    }
}