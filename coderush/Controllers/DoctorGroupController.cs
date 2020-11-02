using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.ViewModels;


namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.DoctorGroup.RoleName)]
    public class DoctorGroupController : Controller
    {
        readonly string modelName = "กลุ่มแพทย์";


        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public DoctorGroupController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            Services.Security.ICommon security,
            Services.App.ICommon app,
            SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment env
            )
        {
            _context = context;
            _userManager = userManager;
            _security = security;
            _app = app;
            _signInManager = signInManager;
            _env = env;

        }

        //fill viewdata as dropdownlist datasource for hospital form
        private void FillDropdownListForhospitalForm()
        {
            ViewData["Designation"] = _app.GetDesignationSelectList();
            ViewData["DoctorType"] = _app.GetDoctorTypeSelectList();
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["PrefixType"] = _app.GetPrefixTypeSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["BenefitTemplate"] = _app.GetBenefitTemplateSelectList();
        }

        //consume db context service, display all hospital
        public IActionResult Index()
        {

            DoctorGroupView1 dgv = new DoctorGroupView1();

            //var objs =  _context.DoctorGroup
            //    .AsNoTracking()
            //    .Include(x => x.DoctorType)
            //    .OrderByDescending(x => x.CreatedAtUtc).ToList();


            var objs = _context.DoctorGroup
               .AsNoTracking()
               .Include(x => x.DoctorType)
               .Join(
                    _context.GroupCoordinator
                    .Include(X => X.PrefixType),
                    dg => dg.DoctorGroupId,
                    gc => gc.DoctorGroupId,
                    (dg, gc) => new DoctorGroupView1
                    {
                        DoctorGroupId = dg.DoctorGroupId,
                        DoctorGroupName = dg.DoctorGroupName,
                        DoctorTypeId = dg.DoctorTypeId,
                        DoctorType = dg.DoctorType,
                        PrefixTypeId = gc.PrefixTypeId,
                        PrefixType = gc.PrefixType,
                        FirstName = gc.FirstName,
                        LastName = gc.LastName,
                        PhoneNo = gc.Phone,
                        ImageData = gc.ImageData

                    }
                ).ToList();



            return View(objs);



        }



        [HttpGet]
        // public IActionResult SelectDoctor(string FName) => FName is null ? GetAllDoctor() : SearchDoctor(FName);
        public IActionResult SelectDoctor(string doctorgroupid)
        {


            string ID = ViewBag.doctorGroupId = doctorgroupid;
           
            var doctorsInGroupReady = _context.DoctorGroupDoctor
               .AsNoTracking()
               .Where(x => x.DoctorGroupId.Equals(ID)).ToList();

            var doctorIdInGroupReady = doctorsInGroupReady.Select(x => x.DoctorId).ToArray();

            ViewBag.GroupName = "";
            var doctors = _context.Doctor
                     .AsNoTracking()
                     .Include(x => x.DoctorType)
                     .Include(x => x.PrefixType)
                     .Where(x => !doctorIdInGroupReady.Contains(x.DoctorId))
                     .ToList();

            List<DoctorSelectedViewModel> ds = new List<DoctorSelectedViewModel>();
 
            foreach (var item in doctors)
            {

                ds.Add(new DoctorSelectedViewModel
                {
                    DoctorGroupId =ID,
                    DoctorId = item.DoctorId,
                    PrefixTypeId = item.PrefixTypeId,
                    PrefixType = item.PrefixType,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Phone = item.Phone,
                    Email = item.Email,
                    LineId = item.LineId,
                    ImageData = item.ImageData,
                    DoctorTypeId = item.DoctorTypeId,
                    DoctorType = item.DoctorType

                });
            }


            return View(ds);

        }
        public IActionResult GetAllDoctor()
        {
            List<DoctorSelectedViewModel> dt = new List<DoctorSelectedViewModel>();

            //  Doctor doctor = new Doctor();
            ViewBag.GroupName = "";
            var doctors = _context.Doctor
                     .AsNoTracking()
                     .Include(x => x.DoctorType)
                     .Include(x => x.PrefixType).ToList();

            foreach (var item in doctors)
            {
                dt.Add(new DoctorSelectedViewModel
                {
                    DoctorId = item.DoctorId,
                    PrefixTypeId = item.PrefixTypeId,
                    PrefixType = item.PrefixType,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Phone = item.Phone,
                    Email = item.Email,
                    LineId = item.LineId,
                    ImageData = item.ImageData,
                    DoctorTypeId = item.DoctorTypeId,
                    DoctorType = item.DoctorType
                });

            }
            return View(dt);

        }



        public IActionResult SearchDoctor(string strSearch)
        {

            //  Doctor doctor = new Doctor();
            ViewBag.GroupName = "";
            List<DoctorSelectedViewModel> dt = new List<DoctorSelectedViewModel>();

            var doctors = _context.Doctor
                .AsNoTracking()
                .Include(x => x.DoctorType)
                .Include(x => x.PrefixType)
                .Where(x => x.FirstName.StartsWith(strSearch)).ToList();
            foreach (var item in doctors)
            {
                dt.Add(new DoctorSelectedViewModel
                {
                    DoctorId = item.DoctorId,
                    PrefixTypeId = item.PrefixTypeId,
                    PrefixType = item.PrefixType,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Phone = item.Phone,
                    Email = item.Email,
                    LineId = item.LineId,
                    ImageData = item.ImageData,
                    DoctorTypeId = item.DoctorTypeId,
                    DoctorType = item.DoctorType
                });


            }

            return View(dt);

        }


        //post submitted hospital data. if hospitalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitSelectDoctor(string doctorGroupId, List<DoctorSelectedViewModel> doctor)
        {

            
            int countdoctor = doctor.Where(x => x.DoctorSelect.Selected).Count();
            try
            {
                if (countdoctor > 0)
                {
                    foreach (var item in doctor)
                    {
                        if (item.DoctorSelect.Selected)
                        {
                            DoctorGroupDoctor newdoctor = new DoctorGroupDoctor();
                            newdoctor.DoctorGroupDoctorId = Guid.NewGuid().ToString();
                            newdoctor.DoctorGroupId = item.DoctorGroupId;
                            newdoctor.DoctorId = item.DoctorId;
                            newdoctor.CreatedBy = await _userManager.GetUserAsync(User);
                            newdoctor.CreatedAtUtc = DateTime.UtcNow;
                            newdoctor.UpdatedBy = newdoctor.CreatedBy;
                            newdoctor.UpdatedAtUtc = newdoctor.CreatedAtUtc;
                            _context.DoctorGroupDoctor.Add(newdoctor);
                        }
                    }

                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                }
            }

            catch (Exception ex)
            {

               TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(SearchDoctor), new { id = doctorGroupId ?? "" });
            }

            return RedirectToAction(nameof(Form), new { id = doctorGroupId ?? "" });

        }


        [HttpGet]
        public IActionResult Form(string id)
        {
            //create new
            if (id == null)
            {
                //dropdownlist 
                FillDropdownListForhospitalForm();

                DoctorGroupView doctorgroupview = new DoctorGroupView();
                doctorgroupview.GroupCoordinator = new GroupCoordinator();
                doctorgroupview.DoctorGroup = new DoctorGroup();
                
                ViewBag.ImageDataUrl = "/assets/images/noimage.png";

                ViewBag.DataSource = OrdersDetails.GetAllRecords();
                ViewBag.DropDownData = new Countries().CountriesList();

                return View(doctorgroupview);

            }


            //edit object
            ViewBag.doctorgroupid = id;

            List<DoctorSelectedViewModel> dt = new List<DoctorSelectedViewModel>();

            var doctorsInGroupReady = _context.DoctorGroupDoctor
               .AsNoTracking()
               .Where(x => x.DoctorGroupId.Equals(id)).ToList();

            var doctorIdInGroupReady = doctorsInGroupReady.Select(x => x.DoctorId).ToArray();


            DoctorGroupView editObj = new DoctorGroupView();
            editObj.DoctorGroup = _context.DoctorGroup.Where(x => x.DoctorGroupId.Equals(id)).FirstOrDefault();
            editObj.GroupCoordinator = _context.GroupCoordinator.Where(x => x.DoctorGroupId.Equals(id)).FirstOrDefault();
            editObj.DoctorList = _context.Doctor
                                .AsNoTracking()
                                .Include(x => x.DoctorType)
                                .Include(x => x.PrefixType)
                                .Where(x => doctorIdInGroupReady.Contains(x.DoctorId)).ToList();

            if (editObj.DoctorGroup == null)
            {
                return NotFound();
            }

            if (editObj.GroupCoordinator.ImageData != null)
            {
                string imageBase64Data = Convert.ToBase64String(editObj.GroupCoordinator.ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewBag.ImageDataUrl = imageDataURL;

            }
            else
            {
                ViewBag.ImageDataUrl = "/assets/images/noimage.png";
            }

            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }
         

        //post submitted hospital data. if hospitalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(Prefix = "DoctorGroup")] DoctorGroup doctorgroup, [Bind(Prefix = "GroupCoordinator")] GroupCoordinator groupcoordinator, IFormFile file)
        {

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = doctorgroup.DoctorGroupId ?? "" });
                }

                //  create new Group
                if (doctorgroup.DoctorGroupId == null)
                {

                    DoctorGroup newgroup = new DoctorGroup();
                    newgroup.DoctorGroupId = Guid.NewGuid().ToString();

                    newgroup.DoctorGroupName = doctorgroup.DoctorGroupName;
                    newgroup.DoctorTypeId = doctorgroup.DoctorTypeId;

                    newgroup.CreatedBy = await _userManager.GetUserAsync(User);
                    newgroup.CreatedAtUtc = DateTime.UtcNow;
                    newgroup.UpdatedBy = newgroup.CreatedBy;
                    newgroup.UpdatedAtUtc = newgroup.CreatedAtUtc;

                    _context.DoctorGroup.Add(newgroup);

                    //  Add GroupCo
                    GroupCoordinator newgroupco = new GroupCoordinator();

                    newgroupco.GroupCoordinatorId = Guid.NewGuid().ToString();

                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            newgroupco.ImageData = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }
                    }
                    else
                    {

                        var webRoot = _env.WebRootPath;
                        var file1 = System.IO.Path.Combine(webRoot, "assets/images/noimage.png");
                        byte[] imageArray = System.IO.File.ReadAllBytes(file1);
                        MemoryStream stream = new MemoryStream(imageArray);

                        newgroupco.ImageData = stream.ToArray();
                        stream.Close();
                        stream.Dispose();
                    }


                    newgroupco.PrefixTypeId = groupcoordinator.PrefixTypeId;
                    newgroupco.FirstName = groupcoordinator.FirstName;
                    newgroupco.LastName = groupcoordinator.LastName;

                    newgroupco.Address1 = groupcoordinator.Address1;
                    newgroupco.SubDistrict = groupcoordinator.SubDistrict;
                    newgroupco.District = groupcoordinator.District;
                    newgroupco.Province = groupcoordinator.Province;
                    newgroupco.ZipCode = groupcoordinator.ZipCode;

                    newgroupco.Phone = groupcoordinator.Phone;
                    newgroupco.Email = groupcoordinator.Email;
                    newgroupco.LineId = groupcoordinator.LineId;
                    newgroupco.DoctorGroupId = newgroup.DoctorGroupId;

                    newgroupco.CreatedBy = await _userManager.GetUserAsync(User);
                    newgroupco.CreatedAtUtc = DateTime.UtcNow;
                    newgroupco.UpdatedBy = newgroupco.CreatedBy;
                    newgroupco.UpdatedAtUtc = newgroupco.CreatedAtUtc;

                    //  newdirector.HospitalId = newhospital.HospitalId;
                    _context.GroupCoordinator.Add(newgroupco);

                    //  Add Data to Database
                    _context.SaveChanges();


                    //  dropdownlist
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูล" + modelName + "เรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(Form), new { id = newgroup.DoctorGroupId ?? "" });
                }




                ////edit existing
                DoctorGroup editdoctorgroup = new DoctorGroup();
                GroupCoordinator editgroupcoordinator = new GroupCoordinator();



                editdoctorgroup = _context.DoctorGroup.Where(x => x.DoctorGroupId.Equals(doctorgroup.DoctorGroupId)).FirstOrDefault();
                editgroupcoordinator = _context.GroupCoordinator.Where(x => x.DoctorGroupId.Equals(doctorgroup.DoctorGroupId)).FirstOrDefault();


                if (editdoctorgroup != null)
                {

                    editdoctorgroup.DoctorGroupName = doctorgroup.DoctorGroupName;
                    editdoctorgroup.DoctorTypeId = doctorgroup.DoctorTypeId;

                    editdoctorgroup.UpdatedBy = await _userManager.GetUserAsync(User);
                    editdoctorgroup.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editdoctorgroup);


                    // Update  Group Coordinator
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            editgroupcoordinator.ImageData = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }
                    }
                    else
                    {
                        if (editgroupcoordinator.ImageData == null)
                        {
                            var webRoot = _env.WebRootPath;
                            var file1 = System.IO.Path.Combine(webRoot, "assets/images/noimage.png");
                            byte[] imageArray = System.IO.File.ReadAllBytes(file1);
                            MemoryStream stream = new MemoryStream(imageArray);

                            editgroupcoordinator.ImageData = stream.ToArray();
                            stream.Close();
                            stream.Dispose();
                        }
                    }

                    editgroupcoordinator.PrefixTypeId = groupcoordinator.PrefixTypeId;
                    editgroupcoordinator.FirstName = groupcoordinator.FirstName;
                    editgroupcoordinator.LastName = groupcoordinator.LastName;

                    editgroupcoordinator.Address1 = groupcoordinator.Address1;
                    editgroupcoordinator.SubDistrict = groupcoordinator.SubDistrict;
                    editgroupcoordinator.District = groupcoordinator.District;
                    editgroupcoordinator.Province = groupcoordinator.Province;
                    editgroupcoordinator.ZipCode = groupcoordinator.ZipCode;

                    editgroupcoordinator.Phone = groupcoordinator.Phone;
                    editgroupcoordinator.Email = groupcoordinator.Email;
                    editgroupcoordinator.LineId = groupcoordinator.LineId;

                    editgroupcoordinator.UpdatedBy = await _userManager.GetUserAsync(User);
                    editgroupcoordinator.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editgroupcoordinator);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(Form), new { id = editdoctorgroup.DoctorGroupId ?? "" });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = "1" ?? "" });
            }
        }


        public IActionResult SubmitDeleteDoctorGrooup(string id)
        {
            try
            {
                var del = _context.DoctorGroup.Where(x => x.DoctorGroupId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.DoctorGroup.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลกลุ่มแพทย์เรียบร้อยแล้ว!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
