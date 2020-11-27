using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.Services;
using vds.Services.Security;
using vds.ViewModels;

namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Membership.RoleName)]
    public class MembershipController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly Services.Security.ICommon _security;
        private readonly IdentityDefaultOptions _identityDefaultOptions;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;
        private IEmailSender _emailSender;

        //dependency injection through constructor, to directly access services
        public MembershipController(
            Services.Security.ICommon security,
            IOptions<IdentityDefaultOptions> identityDefaultOptions,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            Services.App.ICommon app,
              IWebHostEnvironment env
             
            )
        {
            _security = security;
            _identityDefaultOptions = identityDefaultOptions.Value;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _context = context;
            _userManager = userManager;
            _app = app;
            _env = env;
        


        }

        //fill viewdata as dropdownlist datasource for hospital form
        private void FillDropdownListForhospitalForm()
        {
            ViewData["UserTypeId"] = _app.GetUserTypeSelectList();

        }
        public IActionResult Index()
        {

            List<UserView> users = new List<UserView>();
            var myuser = _userManager.GetUserAsync(User);
            ViewBag.UserTypeId = myuser.Result.UserTypeId;

            if (myuser.Result.UserTypeId != "0")
            {
                var appUser = _security.GetMemberByApplicationId1(_userManager.GetUserId(User));
                users.Add(appUser);
            }
            else
            {
                users = _security.GetAllMembers1();
            }

            return View(users);
        }

        //display change profile screen if member founded, otherwise 404
        [HttpGet]
        public IActionResult ChangeProfile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser appUser = new IdentityUser();
            appUser = _security.GetMemberByApplicationId(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        //post submited change profile request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangeProfile([Bind("Id,EmailConfirmed,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                }

                ApplicationUser updatedUser = new ApplicationUser();
                updatedUser = _security.GetMemberByApplicationId(applicationUser.Id);
                if (updatedUser == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                }

                if (_identityDefaultOptions.IsDemo && _superAdminDefaultOptions.Email.Equals(applicationUser.Email))
                {
                    TempData[StaticString.StatusMessage] = "Error: Demo mode can not change super@admin.com data.";
                    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                }

                updatedUser.Email = applicationUser.Email;
                updatedUser.PhoneNumber = applicationUser.PhoneNumber;
                updatedUser.EmailConfirmed = applicationUser.EmailConfirmed;

                _context.Update(updatedUser);
                await _context.SaveChangesAsync();

                TempData[StaticString.StatusMessage] = "บันทึกข้อมูลเรียบร้อย!";
                return RedirectToAction(nameof(ChangeProfile), new { id = updatedUser.Id });
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
            }

        }

        //display change password screen if user founded, otherwise 404
        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = _security.GetMemberByApplicationId(id);
            if (member == null)
            {
                return NotFound();
            }

            ResetPassword cp = new ResetPassword();
            cp.Id = id;
            cp.UserName = member.UserName;

            return View(cp);
        }

        //post submitted change password request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangePassword([Bind("Id,OldPassword,NewPassword,ConfirmPassword")] ResetPassword changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }

                var member = _security.GetMemberByApplicationId(changePassword.Id);

                if (member == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }

                if (_identityDefaultOptions.IsDemo && _superAdminDefaultOptions.Email.Equals(member.Email))
                {
                    TempData[StaticString.StatusMessage] = "Error: Demo mode can not change super@admin.com data.";
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }
                var tokenResetPassword = await _userManager.GeneratePasswordResetTokenAsync(member);
                var changePasswordResult = await _userManager.ResetPasswordAsync(member, tokenResetPassword, changePassword.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    TempData[StaticString.StatusMessage] = "Error: ";
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        TempData[StaticString.StatusMessage] = TempData[StaticString.StatusMessage] + " " + error.Description;
                    }
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }

                TempData[StaticString.StatusMessage] = "Reset password success";
                return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
            }

        }

        //display change role screen if user founded, otherwise 404
        [HttpGet]
        public async Task<IActionResult> ChangeRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = _security.GetMemberByApplicationId(id);
            if (member == null)
            {
                return NotFound();
            }

            var registeredRoles = await _userManager.GetRolesAsync(member);

            ChangeRoles changeRole = new ChangeRoles();
            changeRole.Id = id;
            changeRole.UserName = member.UserName;



            //changeRole.IsSelfServiceRegistered = registeredRoles.Contains("SelfService") ? true : false;
            //changeRole.IsRecruitmentRegistered = registeredRoles.Contains("Recruitment") ? true : false;
            //changeRole.IsAttendanceRegistered = registeredRoles.Contains("Attendance") ? true : false;
            //changeRole.IsLeaveRegistered = registeredRoles.Contains("Leave") ? true : false;
            //changeRole.IsAwardRegistered = registeredRoles.Contains("Award") ? true : false;
            //changeRole.IsInformationRegistered = registeredRoles.Contains("Information") ? true : false;
            //changeRole.IsAssetRegistered = registeredRoles.Contains("Asset") ? true : false;
            //changeRole.IsExpenseRegistered = registeredRoles.Contains("Expense") ? true : false;
            //changeRole.IsPayrollRegistered = registeredRoles.Contains("Payroll") ? true : false;
            //changeRole.IsAppraisalRegistered = registeredRoles.Contains("Appraisal") ? true : false;
            //changeRole.IsTicketRegistered = registeredRoles.Contains("Ticket") ? true : false;

            changeRole.IsDoctorRegistered = registeredRoles.Contains("Doctor") ? true : false;
            changeRole.IsDoctorGroupRegistered = registeredRoles.Contains("DoctorGroup") ? true : false;
            changeRole.IsSettingsRegistered = registeredRoles.Contains("Settings") ? true : false;
            changeRole.IsJobRegistered = registeredRoles.Contains("Job") ? true : false;
            changeRole.IsEmployeeRegistered = registeredRoles.Contains("Employee") ? true : false;
            changeRole.IsTodoRegistered = registeredRoles.Contains("Todo") ? true : false;
            changeRole.IsMembershipRegistered = registeredRoles.Contains("Membership") ? true : false;
            changeRole.IsRoleRegistered = registeredRoles.Contains("Role") ? true : false;
            changeRole.IsHospitalRegistered = registeredRoles.Contains("Hospital") ? true : false;
            changeRole.IsPatientRegistered = registeredRoles.Contains("Patient") ? true : false;



            return View(changeRole);
        }

        //post submitted change role request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangeRole([Bind("Id", "IsTodoRegistered", "IsMembershipRegistered", "IsRoleRegistered", "IsHospitalRegistered", "IsDoctorRegistered", "IsEmployeeRegistered", "IsDoctorGroupRegistered", "IsJobRegistered", "IsPatientRegistered", "IsSettingsRegistered")] ChangeRoles changeRoles)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
                }

                var member = _security.GetMemberByApplicationId(changeRoles.Id);
                if (member == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                    return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
                }

                if (_identityDefaultOptions.IsDemo && _superAdminDefaultOptions.Email.Equals(member.Email))
                {
                    TempData[StaticString.StatusMessage] = "Error: Demo mode can not change super@admin.com data.";
                    return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
                }


                //todo role
                if (changeRoles.IsTodoRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Todo");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Todo");
                }

                //membership role
                if (changeRoles.IsMembershipRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Membership");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Membership");
                }

                //role role
                if (changeRoles.IsRoleRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Role");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Role");
                }

                //SelfService role
                if (changeRoles.IsSelfServiceRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "SelfService");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "SelfService");
                }

                //Doctor role
                if (changeRoles.IsDoctorRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Doctor");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Doctor");
                }


                //Doctor role
                if (changeRoles.IsJobRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Job");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Job");
                }



                //DoctorGroup role
                if (changeRoles.IsDoctorGroupRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "DoctorGroup");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "DoctorGroup");
                }

                //Employee role
                if (changeRoles.IsEmployeeRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Employee");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Employee");
                }

                //Employee role
                if (changeRoles.IsHospitalRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Hospital");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Hospital");
                }

                //Employee role
                if (changeRoles.IsPatientRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Patient");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Patient");
                }


                //Settings role
                if (changeRoles.IsSettingsRegistered)
                {
                    await _userManager.AddToRoleAsync(member, "Settings");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Settings");
                }



                TempData[StaticString.StatusMessage] = "Update success";
                return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
            }

        }

        //display member registration screen
        [HttpGet]
        public IActionResult Register()
        {
            Register reg = new Register();
            reg.EmailConfirmed = true;

            FillDropdownListForhospitalForm();


            return View(reg);
        }



        //post submitted registration request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRegister([Bind("EmailConfirmed,Email,PhoneNumber,Password,ConfirmPassword,UserTypeId")] Register register,bool sendemail)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Register));
                }

                ApplicationUser newMember = new ApplicationUser();
                newMember.Email = register.Email;
                newMember.UserName = register.Email;
                newMember.PhoneNumber = register.PhoneNumber;
                newMember.EmailConfirmed = register.EmailConfirmed;
                newMember.isSuperAdmin = false;
                newMember.UserTypeId = register.UserTypeId;
                var result = await _userManager.CreateAsync(newMember, register.Password);
                // Add Role
                ChangeRoles changeRoles = new ChangeRoles();

                if (register.UserTypeId == "0")
                {
                    changeRoles.IsTodoRegistered = true;
                    changeRoles.IsEmployeeRegistered = true;
                    changeRoles.IsJobRegistered = true;
                    changeRoles.IsDoctorGroupRegistered = true;
                    changeRoles.IsDoctorRegistered = true;
                    changeRoles.IsPatientRegistered = true;
                    changeRoles.IsHospitalRegistered = true;
                    changeRoles.IsMembershipRegistered = true;
                    changeRoles.IsSettingsRegistered = true;


                }
                else
                 if (register.UserTypeId == "1")  // co hospital
                {

                    changeRoles.IsTodoRegistered = true;
                    changeRoles.IsJobRegistered = true;
                    changeRoles.IsEmployeeRegistered = true;
                    changeRoles.IsPatientRegistered = true;
                    changeRoles.IsHospitalRegistered = true;
                    changeRoles.IsMembershipRegistered = true;
                    changeRoles.IsSettingsRegistered = true;
                }
                else
                 if (register.UserTypeId == "2") // co doctorgroup
                {

                    changeRoles.IsTodoRegistered = true;
                    changeRoles.IsJobRegistered = true;
                    changeRoles.IsDoctorGroupRegistered = true;
                    changeRoles.IsDoctorRegistered = true;
                    changeRoles.IsMembershipRegistered = true;
                    changeRoles.IsSettingsRegistered = true;
                }
                else
                 if (register.UserTypeId == "3")  // doctor
                {

                    changeRoles.IsTodoRegistered = true;
                    changeRoles.IsJobRegistered = true;
                    changeRoles.IsDoctorRegistered = true;
                    changeRoles.IsMembershipRegistered = true;
                    changeRoles.IsSettingsRegistered = true;
                }

                //todo role
                if (changeRoles.IsTodoRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Todo");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Todo");
                }

                //membership role
                if (changeRoles.IsMembershipRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Membership");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Membership");
                }

                //role role
                if (changeRoles.IsRoleRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Role");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Role");
                }

                //SelfService role
                if (changeRoles.IsSelfServiceRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "SelfService");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "SelfService");
                }

                //Doctor role
                if (changeRoles.IsDoctorRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Doctor");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Doctor");
                }


                //Doctor role
                if (changeRoles.IsJobRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Job");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Job");
                }

                //DoctorGroup role
                if (changeRoles.IsDoctorGroupRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "DoctorGroup");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "DoctorGroup");
                }

                //Employee role
                if (changeRoles.IsEmployeeRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Employee");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Employee");
                }

                //Employee role
                if (changeRoles.IsHospitalRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Hospital");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Hospital");
                }

                //Employee role
                if (changeRoles.IsPatientRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Patient");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Patient");
                }


                //Settings role
                if (changeRoles.IsSettingsRegistered)
                {
                    await _userManager.AddToRoleAsync(newMember, "Settings");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(newMember, "Settings");
                }
                if (sendemail)
                {
                    _emailSender = new EmailSender(_app, _context, _env);
                    var email = newMember.Email;
                    var subject = "แจ้งรหัสผู้ใช้งาน";
                    var message = "เรียน ผู้เกี่ยวข้อง</br>ตามที่ท่านได้สนใจเข้าร่วมโครงการแพทย์อาสาผ่าตัดในพ้นที่ขาดแคลนนั้น</br>ระบบได้ดำเนินการส่งรหัสผู้ใช้งาน และรหัสผ่านเบื้องต้นมาให้ท่านพร้อมนี้แล้ว</br>รหัสผู้ใช้งาน : <b>"
                        + register.Email + "</b> </br>รหัสผ่านครั้งแรก :<b>" + register.Password + "</b></br>ท่านสามารถเข้าระบบได้จาก Link ต่อไปนี้ คลิก <p><a href=\"http://122.155.7.130:4520\">ศูนย์แพทย์อาสาผ่าตัดในพื้นที่ขาดแคลน</a></p></br>โครงการขอขอบคุณเป็นอย่างยิ่ง</br>กรณีต้องการสอบถามข้อมูลเพิ่มเติมกรุณาติดต่อ 02-123-4567"
                    + " ในวันและเวลาราชการ</br></br><b>โครงการแพทย์อาสาผ่าตัดในพื้นที่ขาดแคลน</b>";
                    await _emailSender.SendEmailAsync(email, subject, message);

                  
                }

                TempData[StaticString.StatusMessage] = "ลงทะเบียนผู้ใช้ใหม่เรียบร้อย!";
                return RedirectToAction(nameof(ChangeProfile), new { id = newMember.Id });

            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Register));
            }

        }


        //display hospital item for deletion
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = _security.GetMemberByApplicationId(id);
            if (id == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(member);

            if (result.Succeeded)
            {
                TempData[StaticString.StatusMessage] = "ลบข้อมูล " + member.UserName + "เรียบร้อยแล้ว!";

            }
            else
            {
                TempData[StaticString.StatusMessage] = "Error: Register new member not success";

            }

            return RedirectToAction(nameof(Index));
        }

    }
}