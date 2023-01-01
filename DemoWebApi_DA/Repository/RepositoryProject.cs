using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure;
using DemoWebApi_DAL.Data;
using DemoWebApi_DAL.Interface;
using DemoWebApi_DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi_DAL.Repository
{
    public class RepositoryProject:IRepository<EntityProject>
    {  
        private readonly TrackerContext _context;

        public RepositoryProject(TrackerContext context)
        {
            _context=context;
        }
        public IQueryable<EntityProject> GetAll()
        {
            
               
                try
                {
                   return _context.Projects;
                }
                catch (Exception e)
                {
                    //TODO: log exception here 
                    throw;
                }
            
        }

        public async Task Update(EntityProject project)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: log exception here 
                throw;

            }
        }

        public void Delete(EntityProject project)
        {
            try
            {
                _context.Remove(project);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                //TODO: log exception here
                throw;
            }
        }

        public async Task<EntityProject> Create(EntityProject project)
        {
            try
            {
                _context.Add(project); 
               await _context.SaveChangesAsync();

                return project;
            }
            catch (Exception ex)
            {

                //TODO: log exception here 
                throw;
            }
        }

        public Task<EntityProject?> GetById(int id)
        {
            if (_context.Projects is null)
            {
               throw new Exception("Doesn't have any projects yet");
            }

            try
            {
                var project = _context.Projects.Include(c => c.Tasks).FirstOrDefaultAsync(m => m.ID == id);

                return project;
            }
            catch (Exception e)
            {

                //TODO: log exception here
                throw;
            } 
        }
    }
}
