namespace CandyFramework.DataAccessLayer.Concrete.EntityFramework.Context
{
    using CandyFramework.Core.Concrete;
    using CandyFramework.Core.Enum;
    using CandyFramework.Core.Interface.Entity;
    using CandyFramework.DataAccessLayer.Concrete.EntityFramework.Mapping;
    using CandyFramework.Entity.Entity.Entity;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CandyContext : DbContext
    {
        public CandyContext()
            : base(Core.Concrete.Common.ConnectionProvider.GetConnectionString())
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new CreateDatabaseIfNotExists<CandyContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<CandyContext, Migrations.Configuration>());

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.UserMapping();
            modelBuilder.UserGroupMapping();
            modelBuilder.SettingMapping();
            base.OnModelCreating(modelBuilder);

            
        }
        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries()
               .Where(
                   x =>
                       x.State == EntityState.Added || x.State == EntityState.Modified ||
                       x.State == EntityState.Deleted)) //Eklenen, d�zenlenen ve silinen kay�tlar
            {
                int userId =Core.Concrete.Common.ConnectionProvider.LogonUser.UserId;
                if (!(item.Entity is IEntity)) continue; // e�er veri taban� nesnesi de�ilse ge� git

                //E�er olu�turma bilgileirnin saylayan bir veri ve veri taban�na insert i�lemi ise
                if (item.Entity is ICreateEntity && item.State == EntityState.Added)
                {
                    ((ICreateEntity)item.Entity).CreateUser = userId.ToString();
                    ((ICreateEntity)item.Entity).CreateDate = DateTime.UtcNow;
                }

                //E�er g�ncelleme bilgileirnin saylayan bir veri ve veri taban�na update i�lemi ise
                if (item.Entity is IUpdateEntity)
                {
                    if (item.State == EntityState.Added || item.State == EntityState.Deleted || item.State == EntityState.Modified)
                    {
                        ((IUpdateEntity)item.Entity).UpdateUser = userId.ToString();
                        ((IUpdateEntity)item.Entity).UpdateDate = DateTime.UtcNow;
                    }
                }

                //E�er silinemez sadece state olarak saklanan bir veri ise ve silme i�lemi olarak geldiysa durumunu g�ncellemeye �ek
                if (item.Entity is IState && item.State == EntityState.Deleted)
                {
                    item.State = EntityState.Modified;
                }
                if (item.Entity is IState && item.State == EntityState.Added)
                {
                    ((IState)item.Entity).State = DbStateEnum.Active;
                }
            }
            //Standart savechange i�lemine devam et.
            return base.SaveChanges();
        }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<UserGroupEntity> UserGroups { get; set; }
        public virtual DbSet<SettingEntity> Settings { get; set; }
    }

}