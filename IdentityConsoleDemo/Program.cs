using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = "aayush@vetwal.com";
            var password = "Password123!";

            var userStore = new CustomUserStore(new CustomUserDbContext());
            var userManager = new UserManager<CustomUser, int>(userStore);

            //CREATING USER
            var creationResult = userManager.Create(new CustomUser { UserName = username }, password);
            Console.WriteLine("Created: {0}", creationResult.Succeeded);

            var user = userManager.FindByName(username);

            //ADDING CLAIM
            //var claimResult = userManager.AddClaim(user.Id, new Claim("given_name", "aayush"));
            //Console.WriteLine("Claim: {0}", claimResult.Succeeded);

            //VERIFYING PASSWORD
            var isMatch = userManager.CheckPassword(user, password);
            Console.WriteLine("Password Match: {0}", isMatch);
        }
    }

    public class CustomUser : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }

    public class CustomUserDbContext : DbContext
    {
        public CustomUserDbContext() : base("DefaultConnection") { }

        public DbSet<CustomUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<CustomUser>();
            user.ToTable("Users");
            user.HasKey(x => x.Id);
            user.Property(x => x.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            user.Property(x => x.UserName).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));
            base.OnModelCreating(modelBuilder);
        }
    }

    public class CustomUserStore : IUserPasswordStore<CustomUser, int>
    {
        private readonly CustomUserDbContext context;

        public CustomUserStore(CustomUserDbContext context)
        {
            this.context = context;
        }

        public Task CreateAsync(CustomUser user)
        {
            context.Users.Add(user);
            return context.SaveChangesAsync();
        }

        public Task DeleteAsync(CustomUser user)
        {
            context.Users.Remove(user);
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<CustomUser> FindByIdAsync(int userId)
        {
            return context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<CustomUser> FindByNameAsync(string userName)
        {
            return context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public Task<string> GetPasswordHashAsync(CustomUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(CustomUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CustomUser user)
        {
            context.Users.Attach(user);
            return context.SaveChangesAsync();
        }
    }
}
