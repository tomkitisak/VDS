using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using vds.ViewModels;
using Syncfusion.EJ2.Linq;

namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Settings.RoleName)]
    public class SettingsController : Controller
    {

        readonly string modelName = "งานร้องขอ";
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        string strController;

        //dependency injection through constructor, to directly access services
        public SettingsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            Services.Security.ICommon security,
            Services.App.ICommon app,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _security = security;
            _app = app;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            return View();
        }


        public IActionResult AddPrefixType(string prefixTypeId)
        {
            ViewBag.SenderController = ControllerContext.ActionDescriptor.ControllerName;
            PrefixTypeView objs = new PrefixTypeView();

            if (prefixTypeId == null)
            {
                
                objs.PrefixType = new PrefixType();
                objs.PrefixTypeList = _context.PrefixType
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ThenBy(y=>y.Name).ToList();
            }
            else
            {

                objs.PrefixType = _context.PrefixType
                    .AsNoTracking()
                   .Where(x=>x.PrefixTypeId.Equals(prefixTypeId)).FirstOrDefault();

                objs.PrefixTypeList = _context.PrefixType
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ToList();


            }

            return View(objs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitAddPrefixType([Bind(Prefix = "PrefixType")] PrefixType prefixType)

        {

            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AddPrefixType), ControllerContext.ActionDescriptor.ControllerName);
                }

                //  create new
                if (prefixType.PrefixTypeId == null)
                {

                    var intOrder = _context.PrefixType.DefaultIfEmpty().Max(x => x.Order);

                    PrefixType newObj = new PrefixType();
                    newObj.PrefixTypeId = Guid.NewGuid().ToString();

                    newObj.Name = prefixType.Name;
                    newObj.Description = prefixType.Description;
                    newObj.Order = prefixType.Order;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PrefixType.Add(newObj);
                    //  Add Data to Database
                    _context.SaveChanges();



                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลคำนำหน้าชื่อเรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(AddPrefixType),   ControllerContext.ActionDescriptor.ControllerName  );

                    //  return RedirectToAction(nameof(AddPrefixType),   ControllerContext.ActionDescriptor.ControllerName , new { prefixTypeId = newObj.PrefixTypeId });
                }


                //////edit existing

                PrefixType editObj = new PrefixType();
                editObj = _context.PrefixType.Where(x => x.PrefixTypeId.Equals(prefixType.PrefixTypeId)).FirstOrDefault();

                if (editObj != null)
                {

                    editObj.Name = prefixType.Name;
                    editObj.Description = prefixType.Description;
                    editObj.Order = prefixType.Order;
                    editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                    editObj.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editObj);
                    _context.SaveChanges();

                    //    //dropdownlist 
                    //    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลคำนำหน้าชื่อเรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(AddPrefixType),  ControllerContext.ActionDescriptor.ControllerName);
                
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddPrefixType), ControllerContext.ActionDescriptor.ControllerName );
            }

        }


        public IActionResult SubmitDeletePrefixType(string prefixTypeId)
        {
            try
            {
                var del = _context.PrefixType.Where(x => x.PrefixTypeId.Equals(prefixTypeId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.PrefixType.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลคำนำหน้าชื่อเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(AddPrefixType));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddPrefixType), ControllerContext.ActionDescriptor.ControllerName);
            }
        }


       //-------------------------------------------- DiseaseType --------------------------------------------------------

        public IActionResult AddDiseaseType(string id)
        {
            ViewBag.SenderController = ControllerContext.ActionDescriptor.ControllerName;
            DiseaseTypeView objs = new DiseaseTypeView();

            if (id == null)
            {

                objs.DiseaseType = new DiseaseType();
                objs.DiseaseTypeList = _context.DiseaseType
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ThenBy(y=>y.Name).ToList();
            }
            else
            {

                objs.DiseaseType = _context.DiseaseType
                    .AsNoTracking()
                   .Where(x => x.DiseaseTypeId.Equals(id)).FirstOrDefault();

                objs.DiseaseTypeList = _context.DiseaseType
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ToList();


            }

            return View(objs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitAddDiseaseType([Bind(Prefix = "DiseaseType")] DiseaseType diseasyType)

        {

            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AddDiseaseType), ControllerContext.ActionDescriptor.ControllerName);
                }

                //  create new
                if (diseasyType.DiseaseTypeId == null)
                {

                    var intOrder = _context.DiseaseType.DefaultIfEmpty().Max(x => x.Order);

                    DiseaseType newObj = new DiseaseType();
                    newObj.DiseaseTypeId = Guid.NewGuid().ToString();

                    newObj.Name = diseasyType.Name;
                    newObj.Description = diseasyType.Description;
                    newObj.Order = diseasyType.Order;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.DiseaseType.Add(newObj);
                    //  Add Data to Database
                    _context.SaveChanges();



                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลโรคเรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(AddDiseaseType), ControllerContext.ActionDescriptor.ControllerName);

                    //  return RedirectToAction(nameof(AddPrefixType),   ControllerContext.ActionDescriptor.ControllerName , new { prefixTypeId = newObj.PrefixTypeId });
                }


                //////edit existing

                DiseaseType editObj = new DiseaseType();
                editObj = _context.DiseaseType.Where(x => x.DiseaseTypeId.Equals(diseasyType.DiseaseTypeId)).FirstOrDefault();

                if (editObj != null)
                {

                    editObj.Name = diseasyType.Name;
                    editObj.Description = diseasyType.Description;
                    editObj.Order = diseasyType.Order;
                    editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                    editObj.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editObj);
                    _context.SaveChanges();

                    //    //dropdownlist 
                    //    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลโรคเรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(AddDiseaseType), ControllerContext.ActionDescriptor.ControllerName);

                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDiseaseType), ControllerContext.ActionDescriptor.ControllerName);
            }

        }

        public IActionResult SubmitDeleteDiseaseType(string id)
        {
            try
            {
                var del = _context.DiseaseType.Where(x => x.DiseaseTypeId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.DiseaseType.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลโรคเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(AddDiseaseType));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDiseaseType), ControllerContext.ActionDescriptor.ControllerName);
            }
        }

        /*-------------------------------------  Department -------------------------------------------------------*/

        public IActionResult AddDepartment(string id)
        {
            ViewBag.SenderController = ControllerContext.ActionDescriptor.ControllerName;
            DepartmentView objs = new DepartmentView();

            if (id == null)
            {

                objs.Department = new Department();
                objs.DepartmentList = _context.Department
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ThenBy(y => y.Name).ToList();
            }
            else
            {

                objs.Department = _context.Department
                    .AsNoTracking()
                   .Where(x => x.DepartmentId.Equals(id)).FirstOrDefault();

                objs.DepartmentList = _context.Department
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ToList();


            }

            return View(objs);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitAddDepartment([Bind(Prefix = "Department")] Department department)

        {

            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AddDepartment), ControllerContext.ActionDescriptor.ControllerName);
                }

                //  create new
                if (department.DepartmentId == null)
                {

                    var intOrder = _context.Department.DefaultIfEmpty().Max(x => x.Order);

                    Department newObj = new Department();
                    newObj.DepartmentId = Guid.NewGuid().ToString();

                    newObj.Name = department.Name;
                    newObj.Description = department.Description;
                    newObj.Order = department.Order;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Department.Add(newObj);
                    //  Add Data to Database
                    _context.SaveChanges();



                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลหน่วยงานเรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(AddDepartment), ControllerContext.ActionDescriptor.ControllerName);

                    //  return RedirectToAction(nameof(AddPrefixType),   ControllerContext.ActionDescriptor.ControllerName , new { prefixTypeId = newObj.PrefixTypeId });
                }


                //////edit existing

                Department editObj = new Department();
                editObj = _context.Department.Where(x => x.DepartmentId.Equals(department.DepartmentId)).FirstOrDefault();

                if (editObj != null)
                {

                    editObj.Name = department.Name;
                    editObj.Description = department.Description;
                    editObj.Order = department.Order;
                    editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                    editObj.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editObj);
                    _context.SaveChanges();

                    //    //dropdownlist 
                    //    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลหน่วยงานรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(AddDepartment), ControllerContext.ActionDescriptor.ControllerName);

                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDepartment), ControllerContext.ActionDescriptor.ControllerName);
            }

        }

        public IActionResult SubmitDeleteDepartment(string id)
        {
            try
            {
                var del = _context.Department.Where(x => x.DepartmentId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Department.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลหน่วยงานเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(AddDepartment));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDepartment), ControllerContext.ActionDescriptor.ControllerName);
            }
        }

        /*-------------------------------------  Designation  -------------------------------------------------------*/

        public IActionResult AddDesignation(string id)
        {
            ViewBag.SenderController = ControllerContext.ActionDescriptor.ControllerName;
            DesignationView objs = new DesignationView();

            if (id == null)
            {

                objs.Designation = new Designation();
                objs.DesignationList = _context.Designation
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ThenBy(y => y.Name).ToList();
            }
            else
            {

                objs.Designation = _context.Designation
                    .AsNoTracking()
                   .Where(x => x.DesignationId.Equals(id)).FirstOrDefault();

                objs.DesignationList = _context.Designation
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ToList();


            }

            return View(objs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitAddDesignation([Bind(Prefix = "Designation")] Designation designation)

        {

            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AddDesignation), ControllerContext.ActionDescriptor.ControllerName);
                }

                //  create new
                if (designation.DesignationId == null)
                {

                    var intOrder = _context.Designation.DefaultIfEmpty().Max(x => x.Order);

                    Designation newObj = new Designation();
                    newObj.DesignationId = Guid.NewGuid().ToString();

                    newObj.Name = designation.Name;
                    newObj.Description = designation.Description;
                    newObj.Order = designation.Order;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Designation.Add(newObj);
                    //  Add Data to Database
                    _context.SaveChanges();



                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลตำแหน่งงานเรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(AddDesignation), ControllerContext.ActionDescriptor.ControllerName);

                    //  return RedirectToAction(nameof(AddPrefixType),   ControllerContext.ActionDescriptor.ControllerName , new { prefixTypeId = newObj.PrefixTypeId });
                }


                //////edit existing

                Designation editObj = new Designation();
                editObj = _context.Designation.Where(x => x.DesignationId.Equals(designation.DesignationId)).FirstOrDefault();

                if (editObj != null)
                {

                    editObj.Name = designation.Name;
                    editObj.Description = designation.Description;
                    editObj.Order = designation.Order;
                    editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                    editObj.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editObj);
                    _context.SaveChanges();

                    //    //dropdownlist 
                    //    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลตำแหน่งงานรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(AddDesignation), ControllerContext.ActionDescriptor.ControllerName);

                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDesignation), ControllerContext.ActionDescriptor.ControllerName);
            }

        }

        public IActionResult SubmitDeleteDesignation(string id)
        {
            try
            {
                var del = _context.Designation.Where(x => x.DesignationId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Designation.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลตำแหน่งงานเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(AddDesignation));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDesignation), ControllerContext.ActionDescriptor.ControllerName);
            }
        }



        /*-------------------------------------  DocTorType  -------------------------------------------------------*/

        public IActionResult AddDoctorType(string id)
        {
            ViewBag.SenderController = ControllerContext.ActionDescriptor.ControllerName;
            DoctorTypeView objs = new DoctorTypeView();

            if (id == null)
            {

                objs.DoctorType = new DoctorType();
                objs.DoctorTypeList = _context.DoctorType
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ThenBy(y => y.DoctorTypeName).ToList();
            }
            else
            {

                objs.DoctorType = _context.DoctorType
                    .AsNoTracking()
                   .Where(x => x.DoctorTypeId.Equals(id)).FirstOrDefault();

                objs.DoctorTypeList = _context.DoctorType
                    .AsNoTracking()
                    .OrderBy(x => x.Order).ToList();


            }

            return View(objs);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitAddDoctorType([Bind(Prefix = "DoctorType")] DoctorType doctorType)

        {

            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AddDoctorType), ControllerContext.ActionDescriptor.ControllerName);
                }

                //  create new
                if (doctorType.DoctorTypeId == null)
                {

                    var intOrder = _context.DoctorType.DefaultIfEmpty().Max(x => x.Order);

                    DoctorType newObj = new DoctorType();
                    newObj.DoctorTypeId = Guid.NewGuid().ToString();

                    newObj.DoctorTypeName = doctorType.DoctorTypeName;
                    newObj.Description = doctorType.Description;
                    newObj.Order = doctorType.Order;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.DoctorType.Add(newObj);
                    //  Add Data to Database
                    _context.SaveChanges();



                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลสาขาความเชี่ยวชาญเรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(AddDoctorType), ControllerContext.ActionDescriptor.ControllerName);

                    //  return RedirectToAction(nameof(AddPrefixType),   ControllerContext.ActionDescriptor.ControllerName , new { prefixTypeId = newObj.PrefixTypeId });
                }


                //////edit existing

                DoctorType editObj = new DoctorType();
                editObj = _context.DoctorType.Where(x => x.DoctorTypeId.Equals(doctorType.DoctorTypeId)).FirstOrDefault();

                if (editObj != null)
                {

                    editObj.DoctorTypeName = doctorType.DoctorTypeName;
                    editObj.Description = doctorType.Description;
                    editObj.Order = doctorType.Order;
                    editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                    editObj.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editObj);
                    _context.SaveChanges();

                    //    //dropdownlist 
                    //    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลสาขาความเชี่ยวชาญเรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(AddDoctorType), ControllerContext.ActionDescriptor.ControllerName);

                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDoctorType), ControllerContext.ActionDescriptor.ControllerName);
            }

        }

        public IActionResult SubmitDeleteDoctorType(string id)
        {
            try
            {
                var del = _context.DoctorType.Where(x => x.DoctorTypeId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.DoctorType.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลสาขาความเชี่ยวชาญเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(AddDoctorType));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AddDoctorType), ControllerContext.ActionDescriptor.ControllerName);
            }
        }
    }
}