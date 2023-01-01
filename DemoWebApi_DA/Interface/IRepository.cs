using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoWebApi_DAL.Model;

namespace DemoWebApi_DAL.Interface
{
    //Create contract for work with database
    public interface IRepository<T>
    {
       IQueryable<T> GetAll();
        Task Update(T entity);
        void Delete(T entity);
        Task<T> Create(T entity);
        Task<T?> GetById(int id);
    }
}
