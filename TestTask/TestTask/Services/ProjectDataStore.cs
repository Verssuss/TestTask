using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Model;
using Xamarin.Forms;

namespace TestTask.Services
{
    class ProjectDataStore : IDataStore<Project>
    {
        ApplicationDbContext DbContext;
        public ProjectDataStore()
        {
            this.DbContext = DependencyService.Resolve<ApplicationDbContext>();
        }
        public async Task<bool> AddItemAsync(Project item)
        {
            await DbContext.Projects.AddAsync(item).ConfigureAwait(false);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await DbContext.Projects.FirstAsync(x => x.Id == id);
            if (item != null)
            {
                DbContext.Projects.Remove(item);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            return true;
        }
        public async Task<Project> GetItemAsync(int id)
        {
            var item = await DbContext.Projects
                .Include(p => p.Employee)
                .Include(p => p.Leader)
                .Include(p => p.Executors)
                .FirstAsync(x => x.Id == id);
            return item;
        }
        public async Task<IEnumerable<Project>> GetItemsAsync(bool forceRefresh = false)
        {
            var allitems = await DbContext.Projects
                .Include(p => p.Employee)
                .Include(p => p.Leader)
                .Include(p => p.Executors)
                .ToListAsync()
                .ConfigureAwait(false);
            return allitems;
        }
        public async Task<bool> UpdateItemAsync(Project item)
        {
            DbContext.Update(item);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}
