using Common.Context.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Context
{
    public static class DbSetExtension
    {
        /// <summary>
        /// Return the DbSet corresponding to the object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_context"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IQueryable<T> GetDbSet<T>(this DbContext _context, T obj) where T : BasicModel
        {
            return (IQueryable<T>)_context.GetType().GetMethod("Set").MakeGenericMethod(obj.GetType()).Invoke(_context, null);
        }

        /// <summary>
        /// Fully loaded an entity and all its navigation properties. Does not load lower levels
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="obj"></param>
        public static void LoadSingleEntity<T>(this DbContext db, T obj) where T : BasicModel
        {
            IQueryable<T> query = db.GetDbSet(obj);
            IEnumerable<INavigation> navigationProperties = db.Model.FindEntityType(obj.GetType()).GetNavigations();

            foreach (INavigation navigationProperty in navigationProperties)
            {
                // Do not load the property if it's already loaded
                if (navigationProperty.PropertyInfo.GetValue(obj) == null)
                {
                    query = query.Include(navigationProperty.Name);
                }               
            }

            query.FirstOrDefault(x => x == obj);
        }
    }

    public static class DbContextEntension
    {
        /// <summary>
        /// Remove an entity and all of its dependents. Entity must have Id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="obj"></param>
        public static void RemoveCascade(this DbContext db, BasicModel obj)
        {
            if (obj.Id == 0 || db.Entry(obj).State == EntityState.Deleted)
            {
                return;
            }

            List<BasicModel> queue = new List<BasicModel>();
            db.Remove(obj);
            db.LoadSingleEntity(obj);          
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                // Make sure model has the right attributes
                bool isValidProperty = Attribute.IsDefined(obj.GetType(), typeof(DbModelAttribute)) && Attribute.IsDefined(property, typeof(OnDeleteCascadeAttribute));
                if (isValidProperty)
                {
                    // Cast to either single object or list. If null, it means there is nothing to delete
                    if (property.GetValue(obj) is BasicModel propertyObject)
                    {                       
                        queue.Add(propertyObject);
                    }
                    else if (property.GetValue(obj) is IEnumerable<BasicModel> propertyObjectList)
                    {
                        queue.AddRange(propertyObjectList);
                    }
                }
            }

            foreach (BasicModel objToRemove in queue)
            {
                db.RemoveCascade(objToRemove);
            }
        }

        public static void RemoveRangeCascade(this DbContext db, IEnumerable<BasicModel> objectList)
        {
            foreach (BasicModel obj in objectList)
            {
                db.RemoveCascade(obj);
            }
        }
    }

    

    public class GamesAndStuffContext : DbContext
    {
        public GamesAndStuffContext(DbContextOptions<GamesAndStuffContext> options)
            : base(options)
        {
           
        }



        public GamesAndStuffContext()
        {
        }

        public DbSet<DiscordUser> DiscordUsers { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }
        public DbSet<ScheduledJob> ScheduledJobs { get; set; }
    }
}
