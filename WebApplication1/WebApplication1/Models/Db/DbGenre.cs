using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using WebApplication1.Managers;
using WebApplication1.Models.Class;
using WebApplication1.Models;

namespace WebApplication1
{
    public class DbGenre
    {
        public async Task<List<GenreClass>> GetGenres()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var genres = await (from g in db.GenreDb
                                        select new GenreClass
                                        {
                                            GenreId=g.GenreId,
                                            GenreName=g.GenreName                    
                                         }).ToListAsync();
                    return genres.ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<GenreClass> GetGenreById(int genreId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var genres=await(from g in db.GenreDb
                                     where g.GenreId==genreId   
                                     select g).FirstOrDefaultAsync();
                    return new GenreClass { GenreId = genres.GenreId, GenreName = genres.GenreName };
                }
            }
            catch(Exception)
            {
                return null;
            }
        }
        public async Task<GenreClass> GetGenreByName(string genreName)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var genre = await (from g in db.GenreDb
                                       where g.GenreName == genreName
                                       select g).FirstOrDefaultAsync();
                    return new GenreClass { GenreId = genre.GenreId, GenreName = genre.GenreName };
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> AddGenre(string genreName)
        {
            try
            {
                using(var db=new ApplicationDbContext())
                {
                Genre g=new Genre{ GenreName=genreName};
                
                    db.GenreDb.Add(g);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}