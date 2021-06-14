using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestTask.Model;
using Xamarin.Forms;

namespace TestTask.Services
{
    class EmployeeDataStore : IDataStore<Employee>
    {
        ApplicationDbContext DbContext;
        public EmployeeDataStore()
        {
            this.DbContext = DependencyService.Resolve<ApplicationDbContext>();
        }
        public async Task<bool> AddItemAsync(Employee item)
        {
            await DbContext.Employees.AddAsync(item).ConfigureAwait(false);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await DbContext.Employees.FirstAsync(x => x.Id == id);
            if (item != null)
            {
                DbContext.Employees.Remove(item);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            return true;
        }

        public async Task<Employee> GetItemAsync(int id)
        {
            var item = await DbContext.Employees
                .Include(x => x.EmployeeProjects)
                .Include(x => x.LeaderProjects)
                .Include(x => x.ExecutorProjects)
                .FirstAsync(x => x.Id == id);
            return item;
        }

        public async Task<IEnumerable<Employee>> GetItemsAsync(bool forceRefresh = false)
        {
            var allitems = await DbContext.Employees
                .Include(x=> x.EmployeeProjects)
                .Include(x=> x.LeaderProjects)
                .Include(x=> x.ExecutorProjects)
             .ToListAsync()
             .ConfigureAwait(false);
            return allitems;
        }

        public async Task<bool> UpdateItemAsync(Employee item)
        {
            DbContext.Update(item);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}
