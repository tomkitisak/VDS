using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.ViewModels;

namespace vds.Services.Security
{
    //custom service provided for common user and membership activities such as get user , create user etc..
    public class Common : ICommon
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationDbContext _context;

        public Common(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _context = context;
        }

        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                ApplicationUser superAdmin = new ApplicationUser();

                superAdmin = await CreateApplicationUser(
                    new ApplicationUser
                    {
                        Email = _superAdminDefaultOptions.Email,
                        UserName = _superAdminDefaultOptions.Email,
                        EmailConfirmed = true,
                        isSuperAdmin = true
                    }
                    , _superAdminDefaultOptions.Password);

                //loop all the roles and then fill to SuperAdmin so he become powerfull
                ApplicationUser selectedUser = await _userManager.FindByEmailAsync(superAdmin.Email);
                List<string> roles = new List<string>();
                if (selectedUser != null)
                {
                    foreach (var item in typeof(App.Pages).GetNestedTypes())
                    {
                        var roleName = item.Name;
                        if (!await _roleManager.RoleExistsAsync(roleName))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(roleName));
                            roles.Add(roleName);
                        }

                    }

                    await _userManager.AddToRolesAsync(selectedUser, roles);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<String> GetAllRoles()
        {
            try
            {
                List<String> roles = new List<string>();
                foreach (var item in typeof(App.Pages).GetNestedTypes())
                {
                    var roleName = item.Name;
                    roles.Add(roleName);

                }

                return roles;
            }
            catch (Exception)
            {

                throw;
            }
        }

         
        public List<ApplicationUser> GetAllMembers()
        {
            try
            {
                List<ApplicationUser> users = new List<ApplicationUser>();

                users = _context.ApplicationUser.OrderBy(x=>x.UserTypeId).ToList();
                     
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<UserView> GetAllMembers1()
        {
            try
            {
                List<UserView> users = new List<UserView>();

                users = _context.UserType
                      .GroupJoin(_context.ApplicationUser,
                            x => x.UserTypeId,
                            y => y.UserTypeId,
                            (x, y) => new { usertype = x, appuser = y }
                      )
                      .SelectMany(x => x.appuser.DefaultIfEmpty(),
                      (x, y) => new { x = x.usertype, app = y })
                      .Select(x => new UserView
                      {
                          Id = x.app.Id,
                          UserName = x.app.UserName,
                          Email = x.app.Email,
                          EmailConfirmed = x.app.EmailConfirmed,
                          PhoneNumber = x.app.PhoneNumber,
                          isSuperAdmin = x.app.isSuperAdmin,
                          UserTypeId = x.app.UserTypeId,
                          UserLevel = x.x.UserLevel,
                          Name = x.x.Name
                      }).ToList();

                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ApplicationUser GetMemberByApplicationId(string applicationId)
        {
            try
            {
                ApplicationUser appUser = new ApplicationUser();
                appUser = _context.ApplicationUser.Where(x => x.Id.Equals(applicationId)).FirstOrDefault();
                   

                return appUser;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserView GetMemberByApplicationId1(string applicationId)
        {
            try
            {
                UserView appUser = new UserView();
                appUser = //_context.ApplicationUser.Where(x => x.Id.Equals(applicationId)).FirstOrDefault();
                    _context.ApplicationUser.Join(_context.UserType,
                    x => x.UserTypeId,
                    y => y.UserTypeId,
                    (x, y) => new { app = x, usr = y }).Where(x => x.app.Id.Equals(applicationId))
                    .Select(x => new UserView
                    {
                        Id = x.app.Id,
                        UserName = x.app.UserName,
                        Email = x.app.Email,
                        EmailConfirmed = x.app.EmailConfirmed,
                        PhoneNumber = x.app.PhoneNumber,
                        isSuperAdmin = x.app.isSuperAdmin,
                        UserTypeId = x.app.UserTypeId,
                        UserLevel = x.usr.UserLevel,
                        Name = x.usr.Name
                    }).FirstOrDefault();


                return appUser;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<ApplicationUser> CreateApplicationUser(ApplicationUser applicationUser, string password)
        {
            try
            {
                ApplicationUser appUser = new ApplicationUser();

                appUser.Email = applicationUser.Email;
                appUser.UserName = applicationUser.Email;
                appUser.EmailConfirmed = applicationUser.EmailConfirmed;
                appUser.isSuperAdmin = applicationUser.isSuperAdmin;

                await _userManager.CreateAsync(appUser, password);

                return appUser;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApplicationUser> GetCurrentUserLogin(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public string GetCurrentUserLoginId(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
        }

        public async Task<Employee> GetCurrentEmployeeLogin(ClaimsPrincipal user)
        {
            Employee employee = new Employee();
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser = await _userManager.GetUserAsync(user);


            return employee;
        }

        public async Task<string> GetCurrentEmployeeLoginId(ClaimsPrincipal user)
        {
            Employee employee = new Employee();
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser = await _userManager.GetUserAsync(user);


            return employee != null ? employee.EmployeeId : "";
        }


    }
}
