using music.Models;
using music.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace music.Services
{
    public class UsersService : IUsers
    {
        List<Users> Users { get; }
        private string usersDataPath ;
        private readonly IMusicalInstruments musicalInstruments;

        public UsersService(IMusicalInstruments musicalInstruments)
        {
            this.musicalInstruments = musicalInstruments;
            this.usersDataPath = Path.Combine("Data" ,"users.json");
            using(var usersData = File.OpenText(usersDataPath))
            {
                Users = JsonSerializer.Deserialize<List<Users>>(usersData.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        public List<Users> GetAll() => Users;

        public Users Get(int id) => Users.FirstOrDefault(p => p.Id == id);

        public void Add(Users user)
        {
            if (Users.Count == 0)
                user.Id = 1;
            else
                user.Id = Users.Max(u => u.Id) + 1;
            Users.Add(user);
            SaveUsersToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
            if(user is null)
                return;
            musicalInstruments.DeleteAllItemsOfUser(id);

            Users.Remove(user);
            SaveUsersToFile();
        }

        public void Update(Users user)
        {
            var index = Users.FindIndex(p => p.Id == user.Id);
            if(index == -1)
                return;

            Users[index] = user;
            SaveUsersToFile();
        }

        public int Count { get =>  Users.Count();}
        

        private void SaveUsersToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonData = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(usersDataPath, jsonData, System.Text.Encoding.UTF8);
        }

        public int ExistUser(string name ,string pass)
        {
            Users user = Users.FirstOrDefault(u => u.Name.Equals(name) && u.Password.Equals(pass));
            if(user != null)
                return user.Id;
            return -1;
        }

    }
}