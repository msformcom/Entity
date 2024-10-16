using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace DAL.ImmoBDD.Interceptors
{
    public class TimeStampInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            foreach (var e in eventData.Context!.ChangeTracker.Entries().Where(c => c.State == EntityState.Modified || c.State == EntityState.Added).Where(c => c.Entity is ITimeStamps))
            {
                if (e.State == EntityState.Modified)
                {
                    ((ITimeStamps)e.Entity).LastUpdate = DateTime.Now;
                    e.Property("LastUpdate").IsModified = true;
                }
                if (e.State == EntityState.Added)
                {
                    ((ITimeStamps)e.Entity).CreationDate = DateTime.Now;
                    e.Property("CreationDate").IsModified = true;
                }
            }

            return base.SavingChanges(eventData, result);


        }
    }
}
