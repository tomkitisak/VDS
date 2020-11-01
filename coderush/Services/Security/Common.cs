﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;

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
                users = _context.ApplicationUser.ToList();

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
