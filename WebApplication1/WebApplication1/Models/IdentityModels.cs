using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

    }
    
    public class Api
    {
        public int ApiId { get; set; }
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
    public class Password
    {
        public String Email { get; set; }
        public byte[] PasswordSet { get; set; }
        public byte[] Salt { get; set; }
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public String ProfileName { get; set; }
        public bool Notifications { get; set; }
        public bool Public { get; set; }
        public int Radius { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String Url { get; set; }


        public virtual Api Api { get; set; }
        public virtual Password Password { get; set; }
        public virtual List<ConcertFollowers> ConcertFollowers { get; set; }
        public virtual List<BandFollowers> BandFollowers { get; set; }
        public virtual List<UserGenre> UserGenre { get; set; }
        public virtual List<Member> Member { get; set; }
        public virtual List<Message> Message { get; set; }
        public virtual List<Friends> Friends { get; set; }
        public virtual List<ConversationConnection> ConversationConnection { get; set; }

    }
    public class Friends
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int Friend { get; set; }
        public Boolean Blocked { get; set; }
        public DateTime FriendsSince { get; set; }

        public virtual User User { get; set; }
    }

    public class Band
    {
        [Key]
        public int BandId { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String BandName { get; set; }
        public String About { get; set; }
        public int Followers { get; set; }
        public String Url { get; set; }



        public virtual List<Member> Member { get; set; }
        public virtual List<BandGenre> BandGenre { get; set; }
        public virtual List<BandFollowers> BandFollowers { get; set; }
        public virtual List<Concert> Concert { get; set; }
    }

    public class Concert
    {
        [Key]
        public int ConcertId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Xcoordinates { get; set; }
        public int Ycoordinates { get; set; }
        public int Followers { get; set; }
        public Boolean SeeAttends { get; set; }
        public int BandId { get; set; }
        public String LinkToBand { get; set; }
        public String Url { get; set; }



        public virtual Band Band { get; set; }
        public virtual List<ConcertFollowers> ConcertFollowers { get; set; }
    }

    public class Member
    {
        [Key]
        [Column(Order = 0)]
        public int BandId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }

        public virtual Band Band { get; set; }
        public virtual User User { get; set; }

    }

    public class BandGenre
    {
        [Key]
        [Column(Order = 0)]
        public int GenreId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int BandId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Band Band { get; set; }

    }

    public class UserGenre
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual User User { get; set; }


    }
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public String GenreName { get; set; }

        public virtual List<UserGenre> UserGenre { get; set; }
        public virtual List<BandGenre> BandGenre { get; set; }


    }
    public class BandFollowers
    {
        [Key]
        [Column(Order = 0)]
        public int BandId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Band Band { get; set; }
    }
    public class ConcertFollowers
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int ConcertId { get; set; }

        public virtual User User { get; set; }
        public virtual Concert Concert { get; set; }
    }

    public class ConversationConnection
    {
        [Key]
        public int ConnectionId { get; set; }
        public int UserId { get; set; }
        public int ConversationId { get; set; }
        public Boolean HasSeen { get; set; }


        public virtual Conversation Conversation { get; set; }
        public virtual User User { get; set; }
    }
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        public String ConversationName { get; set; }
        public String ConversationType { get; set; }

        public virtual List<ConversationConnection> ConversationConnection { get; set; }
        public virtual List<Message> Message { get; set; }
    }

    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public String Content { get; set; }
        public DateTime SendtTime { get; set; }

        public virtual User User { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("Semitone")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<ConversationConnection> ConversationConnectionDb { get; set; }
        public DbSet<Message> MessageDb { get; set; }
        public DbSet<Conversation> ConversationDb { get; set; }
        public DbSet<Friends> FriendsDb { get; set; }
        public DbSet<Password> PasswordDb { get; set; }
        public DbSet<ConcertFollowers> ConcertFollowersDb { get; set; }
        public DbSet<BandFollowers> BandFollowersDb { get; set; }
        public DbSet<UserGenre> UserGenreDb { get; set; }
        public DbSet<BandGenre> BandGenreDb { get; set; }
        public DbSet<Genre> GenreDb { get; set; }
        public DbSet<Member> MemberDb { get; set; }
        public DbSet<Concert> ConcertDb { get; set; }
        public DbSet<Band> BandDb { get; set; }
        public DbSet<User> UserDb { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<Poststeder>().HasKey(p=>p.Postnr);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}