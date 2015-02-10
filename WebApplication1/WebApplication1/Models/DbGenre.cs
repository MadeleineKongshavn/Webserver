using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Models;

namespace WebApplication1
{
    public class DbGenre
    {
        public Boolean NewGenre(String name)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Genre gen = new Genre();
                    gen.GenreName = name;
                    db.GenreDb.Add(gen);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {    
                    return false;
                }
            }
        }

        public Genre FindGenre(String name) // Finds a genre
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Genre genre = (from b in db.GenreDb where b.GenreName == name select b).FirstOrDefault();
                    return genre;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Genre FindGenre(int id) // Finds a genre
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Genre genre = (from b in db.GenreDb where b.GenreId == id select b).FirstOrDefault();
                    return genre;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public List<Genre> AllGenres() // Gets all the genres that exist 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<Genre> genres = (from b in db.GenreDb select b).ToList();
                    return genres; 
                }
                catch (Exception)
                {     
                    return null;
                }
            }
        }
    }
}