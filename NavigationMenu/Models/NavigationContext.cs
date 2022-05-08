using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace NavigationMenu.Models
{
    internal class NavigationContext:DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<EquipmentEntity> EquipmentEntities { get; set; }

    }
}
