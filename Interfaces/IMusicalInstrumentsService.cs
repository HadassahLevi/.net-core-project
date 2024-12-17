using music.Models;
using System.Collections.Generic;
using System.Linq;

namespace music.Interfaces
{
    public interface IMusicalInstruments
    {
        List<MusicalInstruments> GetAll(int id);

        MusicalInstruments Get(int id);

        void Add(MusicalInstruments MI);

        void Delete(int id);

        void Update(MusicalInstruments MI);

        int Count { get;}

        void DeleteAllItemsOfUser(int id);
    }
}