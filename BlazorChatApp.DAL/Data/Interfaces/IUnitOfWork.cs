using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorChatApp.DAL.Data.Repositories;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IUnitOfWork 
    {
        Task<int> SaveChanges();

    }
}
