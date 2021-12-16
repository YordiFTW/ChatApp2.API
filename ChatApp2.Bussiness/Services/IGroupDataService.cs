using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;

namespace ChatApp2.Bussiness.Services
{
    public interface IGroupDataService
    {
        Group Add(int chatId);

        void Invite(UserAddtoGroup userAddtoGroup);

        void RemoveUserFromGroup(UserRemoveFromGroup userRemoveFromGroup);

        void DeleteEntireGroup(int chatId);
    }
}
