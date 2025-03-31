using Microsoft.AspNetCore.Identity;

namespace WesternInn_AA_JH_SFP.Data
{
    public class SeedRoles
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "administrators", "guests" };
            IdentityResult roleResult;


            foreach (var roleName in roleNames)
            {
                // checks if role is non-existant
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // creates roles
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

                // Creates adminstrator user
                var poweruser = new IdentityUser
                {
                    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                    Email = Configuration.GetSection("UserSettings")["UserEmail"]
                };

                string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];
                var user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);
                // check whether the adminstrator user exists
                if (user == null)
                {
                    // create adminstrator user
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                    if (createPowerUser.Succeeded)
                    {
                        // assigns adminstrators role for new user
                        await UserManager.AddToRoleAsync(poweruser, "administrators");
                    }
                }
            }
        }
    }

