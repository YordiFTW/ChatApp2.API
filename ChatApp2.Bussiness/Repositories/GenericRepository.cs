using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.DbContexts;
using ChatApp2.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp2.Bussiness.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ChatAppDbContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new ChatAppDbContext();
            table = _context.Set<T>();
        }
        public GenericRepository(ChatAppDbContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void DeleteById(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void DeleteAllByChatId(int chatId)
        {
            //T existing = table.Find(chat);
            

            var allObj = GetAll();

            foreach(T singleObj in allObj)
            {
                PropertyInfo prop = singleObj.GetType().GetProperty("ChatId");
                int id = Convert.ToInt32(prop.GetValue(singleObj));
                

                if ( id == chatId)
                {
                    table.Remove(singleObj);
                }
            }
           
        }
        public void DeleteByObj(T obj)
        {
            //T existing = table.Remove(obj);
            table.Remove(obj);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}