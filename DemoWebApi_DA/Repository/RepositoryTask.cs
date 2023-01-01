using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoWebApi_DAL.Data;
using DemoWebApi_DAL.Interface;
using DemoWebApi_DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi_DAL.Repository
{
    public class RepositoryTask:IRepository<EntityTask>

    {
        private readonly TrackerContext _context;
        public RepositoryTask(TrackerContext context)
        {
            _context = context;
        }
        public IQueryable<EntityTask> GetAll()
        {

            try
            {
                return _context.Tasks;
            }
            catch (Exception e)
            {
                //TODO: log exception here 
                throw;
            }
        }

        public async Task Update(EntityTask task)
        {
            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: log exception here 
                throw;

            }
        }

        public void Delete(EntityTask task)
        {
            try
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: log exception here
                throw;
            }
        }

        public async Task<EntityTask> Create(EntityTask task)
        {

            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {

                //TODO: log exception here 
                throw;
            }
        }

        public Task<EntityTask?> GetById(int id)
        {
            if (_context.Tasks is null)
            {
                throw new Exception("Doesn't have any tasks yet");
            }

            try
            {
                var task = _context.Tasks.Include(x => x.Project).FirstOrDefaultAsync(m => m.ID == id);

                return task;
            }
            catch (Exception e)
            {

                //TODO: log exception here
                throw;
            }
        }
    }
}
