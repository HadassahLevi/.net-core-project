using music.Models;
using music.Interfaces;
using System.Text.Json;

namespace music.Services
{
    public class MusicalInstrumentsService : IMusicalInstruments
    {
        List<MusicalInstruments> Products { get; } = new List<MusicalInstruments>();
        private readonly string musicDataPath;
        int nextId = 3;
        public MusicalInstrumentsService()
        {
            this.musicDataPath = Path.Combine("Data" ,"musics.json");
            using (var musicData = File.OpenText(musicDataPath))
            {
                Products = JsonSerializer.Deserialize<List<MusicalInstruments>>(musicData.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        public List<MusicalInstruments> GetAll(int userId) {
            return Products.FindAll(m => m.UserId == userId);
        }

        public MusicalInstruments Get(int id) => Products.FirstOrDefault(p => p.Id == id);

        public void Add(MusicalInstruments MI)
        {

            if (Products.Count == 0)
                MI.Id = 1;
            else
                MI.Id = Products.Max(p => p.Id) + 1;
            Products.Add(MI);
            SaveDataToFile();
        }

        public void Delete(int id)
        {
            var MI = Get(id);
            if(MI is null)
                return;

            Products.Remove(MI);
            SaveDataToFile();
        }

        public void Update(MusicalInstruments MI)
        {
            var index = Products.FindIndex(p => p.Id == MI.Id);
            if(index == -1)
                return;

            Products[index] = MI;
            SaveDataToFile();
        }

        public int Count { get =>  Products.Count();}

        public void SaveDataToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            string jsonData = JsonSerializer.Serialize(Products, options);
            File.WriteAllText(musicDataPath, jsonData);
        }

        public void DeleteAllItemsOfUser(int id)
        {
            var itemsToDelete = Products.Where(p => p.UserId == id).ToList();
            foreach (var item in itemsToDelete)
            {
                Products.Remove(item);
            }
            SaveDataToFile();
        }

    }

}
