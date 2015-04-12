using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Drawing;
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
        public Boolean SeeNotifications { get; set; }
        public String ProfileName { get; set; }
        public bool Public { get; set; }
        public int Radius { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String Url { get; set; }
        public DateTime Timestamp { get; set; }


        public virtual List<Notifications> Notifications { get; set; }
        public virtual Api Api { get; set; }
        public virtual Password Password { get; set; }
        public virtual List<ConcertFollowers> ConcertFollowers { get; set; }
        public virtual List<BandFollowers> BandFollowers { get; set; }
        public virtual List<UserGenre> UserGenre { get; set; }
        public virtual List<Member> Member { get; set; }
        public virtual List<Friends> Friends { get; set; }
        public virtual List<InviteBandNotifications> InviteBandNotifications { get; set; }
        public virtual List<InviteConcertNotifications> InviteConcertNotifications { get; set; }
        public virtual List<AcceptConcertInvitation> AcceptConcertInvitation { get; set; } 

    }
    public class Friends
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int Friend { get; set; }

        public virtual User User { get; set; }
    }
    public class Band
    {
        [Key]
        public int BandId { get; set; }
        public String Area { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String BandName { get; set; }
        public int Followers { get; set; } // fjerne
        public String BitmapUrl { get; set; }
        public String BitmapSmalUrl { get; set; }
        public String Songurl { get; set; }
        public Byte[] Song { get; set; } // fjerne 
        public String SongName { get; set; }
        public String UrlFacebook { get; set; }
        public String UrlSoundCloud { get; set; }
        public String UrlRandom { get; set; }
        public DateTime Timestamp { get; set;} // fjerne


        public virtual List<BandNotifications> BandNotifications { get; set; } 
        public virtual List<InviteBandNotifications> InviteBandNotifications { get; set; } 
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
        public String Area { get; set; }
        public int Xcoordinates { get; set; }
        public int Ycoordinates { get; set; }
        public int Followers { get; set; }
        public Boolean SeeAttends { get; set; }
        public int BandId { get; set; }
        public String LinkToBand { get; set; }
        public String BitmapUrl { get; set; }
        public String BitmapSmalUrl { get; set; }
        public String VenueName { get; set; }
        public DateTime Timestamp { get; set; }


        public virtual List<BandNotifications> BandNotifications { get; set; } 
        public virtual List<InviteConcertNotifications> InviteConcertNotifications { get; set; }
        public virtual Band Band { get; set; }
        public virtual List<ConcertFollowers> ConcertFollowers { get; set; }
        public virtual List<AcceptConcertInvitation> AcceptConcertInvitation { get; set; }  

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
    public class Notifications
    {
        [Key]
        public int NotificationsId { get; set; }
        public DateTime SendtTime { get; set; }
        public Boolean Seen { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
 
        public virtual User User { get; set; }
        public virtual InviteBandNotifications InviteBandNotifications { get; set; }
        public virtual InviteConcertNotifications InviteConcertNotifications { get; set; }
        public virtual BandNotifications BandNotifications { get; set; }
        public virtual FriendRequestNotifications FriendRequestNotifications { get; set; }
        public virtual AcceptConcertInvitation AcceptConcertInvitation { get; set; }
    }

    public class InviteBandNotifications
    {
        [Key, ForeignKey("Notifications")]
        public int NotificationsId { get; set; }
        public int UserId { get; set; }
        public int BandId { get; set; }
        public Boolean Answered { get; set; }


        public virtual Notifications Notifications { get; set; }
        public virtual User User { get; set; }
        public virtual Band Band { get; set; }

    }
    public class InviteConcertNotifications
    {
        [Key, ForeignKey("Notifications")]
        public int NotificationsId { get; set; }
        public int UserId { get; set; }
        public int ConcertId { get; set; }
        public Boolean Accepted { get; set; }


        public virtual Notifications Notifications { get; set; }
        public virtual User User { get; set; }
        public virtual Concert Concert { get; set; }

    }
    public class AcceptConcertInvitation
    {
        [Key, ForeignKey("Notifications")]
        public int NotificationsId { get; set; }
        public int UserId { get; set; }
        public int ConcertId { get; set; }

        public virtual Notifications Notifications { get; set; }
        public virtual User User { get; set; }
        public virtual Concert Concert { get; set; }
    }
    public class BandNotifications
    {
        [Key, ForeignKey("Notifications")]
        public int NotificationsId { get; set; }
        public int ConcertId { get; set; }
        public int BandId { get; set; }


        public virtual Band Band { get; set; }
        public virtual Concert Concert { get; set; }
        public virtual Notifications Notifications { get; set; }
    }
    public class FriendRequestNotifications
    {
        [Key, ForeignKey("Notifications")]
        public int NotificationsId { get; set; }
        public Boolean Accepted { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Notifications Notifications { get; set; }

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("Semitone")
        {
            Database.CreateIfNotExists();
        }
        public DbSet<AcceptConcertInvitation> AcceptConcertInvitationDb { get; set; }
        public DbSet<Notifications> NotificationsDb { get; set; }
        public DbSet<InviteBandNotifications> InviteBandNotificationsDb { get; set; }
        public DbSet<InviteConcertNotifications> InviteConcertNotificationsDb { get; set; }
        public DbSet<BandNotifications> BandNotificationsDb { get; set; }
        public DbSet<FriendRequestNotifications> FriendRequestNotificationsDb { get; set; }
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
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}