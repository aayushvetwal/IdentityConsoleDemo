using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
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

            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            //CREATING USER
            //var creationResult = userManager.Create(new IdentityUser(username), password);
            //Console.WriteLine("Created: {0}", creationResult.Succeeded);

            var user = userManager.FindByName(username);

            //ADDING CLAIM
            //var claimResult = userManager.AddClaim(user.Id, new Claim("given_name", "aayush"));
            //Console.WriteLine("Claim: {0}", claimResult.Succeeded);

            //VERIFYING PASSWORD
            var isMatch = userManager.CheckPassword(user, password);
            Console.WriteLine("Password Match: {0}", isMatch);
        }
    }
}
