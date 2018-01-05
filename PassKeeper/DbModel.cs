namespace PassKeeper
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class DbModel : DbContext
	{
		public DbModel()
			: base("name=DbModel")
		{
		}

		public DbModel(string connString) : base(connString)
		{			
		}

		public virtual DbSet<APP_USER> APP_USER { get; set; }
		public virtual DbSet<PASSWORD_HISTORY> PASSWORD_HISTORY { get; set; }
		public virtual DbSet<USER_DATA> USER_DATA { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<APP_USER>()
				.Property(e => e.EMAIL)
				.IsUnicode(false);

			modelBuilder.Entity<APP_USER>()
				.Property(e => e.PASSWORD)
				.IsUnicode(false);

			modelBuilder.Entity<APP_USER>()
				.HasMany(e => e.USER_DATA)
				.WithRequired(e => e.APP_USER)
				.HasForeignKey(e => e.APP_USER_ID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<PASSWORD_HISTORY>()
				.Property(e => e.PASSWORD_HIST)
				.IsUnicode(false);

			modelBuilder.Entity<USER_DATA>()
				.Property(e => e.SERV_NAME)
				.IsUnicode(false);

			modelBuilder.Entity<USER_DATA>()
				.Property(e => e.SERV_PASSWORD)
				.IsUnicode(false);

			modelBuilder.Entity<USER_DATA>()
				.Property(e => e.COMMENT)
				.IsUnicode(false);

			modelBuilder.Entity<USER_DATA>()
				.HasMany(e => e.PASSWORD_HISTORY)
				.WithRequired(e => e.USER_DATA)
				.HasForeignKey(e => e.DATA_ID)
				.WillCascadeOnDelete(false);
		}
	}
}
