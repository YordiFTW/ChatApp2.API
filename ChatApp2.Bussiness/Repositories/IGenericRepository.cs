using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using PagedList;

namespace ChatApp2.Bussiness.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void DeleteById(object id);
        void DeleteAllByChatId(int id);
        void DeleteByObj(T obj);
        void Save();

        List<Message> GetAllMessagesByChat(int chatId);

        IPagedList<Message> GetAllMessagesByPaging(int page);

        List<Message> SearchMessagesBySearchTerm(string searchTerm);
    }
}
