﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KAFE
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CafeEntities : DbContext
    {
        public CafeEntities()
            : base("name=CafeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Check> Check { get; set; }
        public virtual DbSet<Food> Food { get; set; }
        public virtual DbSet<ListOrder> ListOrder { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<RoleUsers> RoleUsers { get; set; }
        public virtual DbSet<StatusOrder> StatusOrder { get; set; }
        public virtual DbSet<StatusUsers> StatusUsers { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tables> Tables { get; set; }
        public virtual DbSet<TypeFood> TypeFood { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WayPay> WayPay { get; set; }
        public virtual DbSet<WorkShift> WorkShift { get; set; }
    }
}
