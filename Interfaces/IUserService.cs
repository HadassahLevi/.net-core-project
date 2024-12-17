using music.Models;
using System.Collections.Generic;
using System.Linq;

namespace music.Interfaces
{
    public interface IUsers
    {
        List<Users> GetAll();

        Users Get(int id);

        void Add(Users user);

        void Delete(int id);

        void Update(Users user);

        int Count { get;}
        
        int ExistUser(string name, string password);
    }
}