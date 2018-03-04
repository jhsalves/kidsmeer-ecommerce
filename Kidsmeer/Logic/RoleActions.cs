using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kidsmeer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Kidsmeer.Logic
{
    internal class RoleActions
    {
        internal void CreateAdmin()
        {
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            var roleStore = new RoleStore<IdentityRole>(context);

            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            if (!roleMgr.RoleExists("Administrator"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole("Administrator"));
                if (!IdRoleResult.Succeeded)
                {
                    //problem on creating administrator role
                }
            }

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser()
            {
                UserName = "Admin2",
                Email = "admin@admin.com"
            };
            IdUserResult = userMgr.Create(appUser, "191289");

            if (IdUserResult.Succeeded)
            {
                IdUserResult = userMgr.AddToRole(appUser.Id, "Administrator");
                if (!IdUserResult.Succeeded) {
                    //problem on add  user to role
                }

            }
            else {
                    //user creation failed
            }
        }
    }
}