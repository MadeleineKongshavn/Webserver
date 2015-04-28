using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class GenreManager
    {
        public async Task<GenreClass> GetBandsById(int id)
        {
            DbGenre db=new DbGenre();
            return await db.getGenreById(id);
        }


        public async Task<List<GenreClass> >GetGenres()
        {
            DbGenre db = new DbGenre();
            return await db.getGenres();
        }

        public async Task<GenreClass> GetGenreByName(string name)
        {
            DbGenre db = new DbGenre();
            return await db.getGenreByName(name);

        }

        public async Task<bool> AddGenre(string name)
        {
            DbGenre db = new DbGenre();
            return await db.addGenre(name);
        }
    }
}
