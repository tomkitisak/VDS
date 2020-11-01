using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;

namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Doctor.RoleName)]
    public class DoctorController : Controller
    {

        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public DoctorController(
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
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["DoctorType"] = _app.GetDoctorType();
            ViewData["PrefixType"] = _app.GetPrefixTypeSelectList();

            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["BenefitTemplate"] = _app.GetBenefitTemplateSelectList();
        }

        //consume db context service, display all hospital
        public IActionResult Index()
        {

            var objs = _context.Doctor
                .AsNoTracking()
                .Include(x => x.DoctorType)
                .Include(x => x.PrefixType)
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        [HttpGet]
        public async Task<IActionResult> LoginOnBehalf(string id)
        {
            Doctor doctor = new Doctor();
            doctor = _context.Doctor.Where(x => x.DoctorId.Equals(id)).FirstOrDefault();
            if (doctor == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "SelfService", new { period = DateTime.Now.ToString("yyyy-MM") });
        }


        //display hospital create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {
            //create new
            if (id == null)
            {
                //dropdownlist 
                FillDropdownListForhospitalForm();

                Doctor newObj = new Doctor();
                ViewBag.ImageDataUrl = "/assets/images/noimage.png";
                return View(newObj);

            }

            //edit object
            Doctor editObj = new Doctor();
            editObj = _context.Doctor.Where(x => x.DoctorId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }

            string imageBase64Data;
            string imageDataURL;
            if (editObj.ImageData == null)
            {
                imageDataURL = "/assets/images/noimage.png";
            }
            else
            {

                imageBase64Data = Convert.ToBase64String(editObj.ImageData);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }


            ViewBag.ImageDataUrl = imageDataURL;


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        //post submitted hospital data. if hospitalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind] Doctor doctor, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = doctor.DoctorId ?? "" });
                }

                //create new
                if (doctor.DoctorId == null)
                {
                    Doctor newdoctor = new Doctor();
                    newdoctor.DoctorId = Guid.NewGuid().ToString();

                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            newdoctor.ImageData = ms.ToArray();
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
                        newdoctor.ImageData = stream.ToArray();
                        stream.Close();
                        stream.Dispose();
                    }

                    newdoctor.PrefixTypeId = doctor.PrefixTypeId;
                    newdoctor.FirstName = doctor.FirstName;
                    newdoctor.LastName = doctor.LastName;

                    newdoctor.Email = doctor.Email;
                    newdoctor.Phone = doctor.Phone;
                    newdoctor.LineId = doctor.LineId;
                    newdoctor.Address1 = doctor.Address1;
                    newdoctor.SubDistrict = doctor.SubDistrict;
                    newdoctor.District = doctor.District;
                    newdoctor.Province = doctor.Province;
                    newdoctor.ZipCode = doctor.ZipCode;
                    newdoctor.MDLicense = doctor.MDLicense;
                    newdoctor.DoctorTypeId = doctor.DoctorTypeId;

                    newdoctor.CreatedBy = await _userManager.GetUserAsync(User);

                    newdoctor.CreatedAtUtc = DateTime.UtcNow;
                    newdoctor.UpdatedBy = newdoctor.CreatedBy;
                    newdoctor.UpdatedAtUtc = newdoctor.CreatedAtUtc;

                    _context.Doctor.Add(newdoctor);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "เพิ่มข้อมูลแพทย์เรียบร้อยแล้ว";
                    return RedirectToAction(nameof(Form), new { id = newdoctor.DoctorId ?? "" });
                }

                //edit existing
                Doctor edidoctor = new Doctor();
                edidoctor = _context.Doctor.Where(x => x.DoctorId.Equals(doctor.DoctorId)).FirstOrDefault();

                if (edidoctor != null)
                {
                    //duplicate hospital Number ID is not allowed

                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            edidoctor.ImageData = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }
                    }
                    else
                    {
                        if (edidoctor.ImageData == null)
                        {
                            var webRoot = _env.WebRootPath;
                            var file1 = System.IO.Path.Combine(webRoot, "assets/images/noimage.png");
                            byte[] imageArray = System.IO.File.ReadAllBytes(file1);
                            MemoryStream stream = new MemoryStream(imageArray);
                            edidoctor.ImageData = stream.ToArray();
                            stream.Close();
                            stream.Dispose();
                        }
                    }


                    edidoctor.PrefixTypeId = doctor.PrefixTypeId;
                    edidoctor.FirstName = doctor.FirstName;
                    edidoctor.LastName = doctor.LastName;

                    edidoctor.Phone = doctor.Phone;
                    edidoctor.Email = doctor.Email;
                    edidoctor.LineId = doctor.LineId;
                    edidoctor.Address1 = doctor.Address1;
                    edidoctor.SubDistrict = doctor.SubDistrict;
                    edidoctor.District = doctor.District;
                    edidoctor.Province = doctor.Province;
                    edidoctor.ZipCode = doctor.ZipCode;
                    edidoctor.MDLicense = doctor.MDLicense;
                    edidoctor.DoctorTypeId = doctor.DoctorTypeId;

                    edidoctor.UpdatedBy = await _userManager.GetUserAsync(User);
                    edidoctor.UpdatedAtUtc = DateTime.UtcNow;
                    _context.Update(edidoctor);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูลแพทย์ เรียบร้อยแล้ว";
                    return RedirectToAction(nameof(Form), new { id = edidoctor.DoctorId ?? "" });
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = doctor.DoctorId ?? "" });
            }
        }



        //display hospital item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var del = _context.Doctor
                .AsNoTracking()


                .Where(x => x.DoctorId.Equals(id)).FirstOrDefault();

            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(del);
        }

        //delete submitted hospital if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("DoctorId")] Doctor doctor)
        {
            try
            {
                var del = _context.Doctor.Where(x => x.DoctorId.Equals(doctor.DoctorId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Doctor.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete hospital success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = doctor.DoctorId ?? "" });
            }
        }

        public IActionResult DesignationIndex()
        {
            var objs = _context.Designation.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Designation create edit form
        [HttpGet]
        public IActionResult DesignationForm(string id)
        {
            //create new
            if (id == null)
            {
                Designation newObj = new Designation();
                return View(newObj);
            }

            //edit Designation
            Designation obj = new Designation();
            obj = _context.Designation.Where(x => x.DesignationId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Designation data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDesignationForm([Bind("DesignationId", "Name", "Description")] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
                }

                //create new
                if (designation.DesignationId == null)
                {
                    if (await _context.Designation.AnyAsync(x => x.Name.Equals(designation.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + designation.Name + " already exist";
                        return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
                    }

                    Designation newObj = new Designation();
                    newObj.DesignationId = Guid.NewGuid().ToString();
                    newObj.Name = designation.Name;
                    newObj.Description = designation.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Designation.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "เพิ่มข้อมูลเรียบร้อยแล้ว";
                    return RedirectToAction(nameof(DesignationForm), new { id = newObj.DesignationId ?? "" });
                }

                //edit existing
                Designation editObj = new Designation();
                Designation existObj = new Designation();
                editObj = await _context.Designation.Where(x => x.DesignationId.Equals(designation.DesignationId)).FirstOrDefaultAsync();
                existObj = await _context.Designation.Where(x => x.Name.Equals(designation.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.DesignationId != existObj.DesignationId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + designation.Name + " already exist";
                        return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
                    }

                }

                editObj.Name = designation.Name;
                editObj.Description = designation.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "แก้ไขข้อมูลเรียบร้อยแล้ว";
                return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> DesignationDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Designation.Where(x => x.DesignationId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDesignationDelete([Bind("DesignationId")] Designation designation)
        {
            try
            {
                var deleteObj = await _context.Designation.Where(x => x.DesignationId.Equals(designation.DesignationId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }




                _context.Designation.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(DesignationIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DesignationDelete), new { id = designation.DesignationId ?? "" });
            }
        }

        public IActionResult DepartmentIndex()
        {
            var objs = _context.Department.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Department create edit form
        [HttpGet]
        public IActionResult DepartmentForm(string id)
        {
            //create new
            if (id == null)
            {
                Department newObj = new Department();
                return View(newObj);
            }

            //edit Department
            Department obj = new Department();
            obj = _context.Department.Where(x => x.DepartmentId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Department data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDepartmentForm([Bind("DepartmentId", "Name", "Description")] Department department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
                }

                //create new
                if (department.DepartmentId == null)
                {
                    if (await _context.Department.AnyAsync(x => x.Name.Equals(department.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + department.Name + " already exist";
                        return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
                    }

                    Department newObj = new Department();
                    newObj.DepartmentId = Guid.NewGuid().ToString();
                    newObj.Name = department.Name;
                    newObj.Description = department.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Department.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(DepartmentForm), new { id = newObj.DepartmentId ?? "" });
                }

                //edit existing
                Department editObj = new Department();
                Department existObj = new Department();
                editObj = await _context.Department.Where(x => x.DepartmentId.Equals(department.DepartmentId)).FirstOrDefaultAsync();
                existObj = await _context.Department.Where(x => x.Name.Equals(department.Name)).FirstOrDefaultAsync();

                if (existObj != null && editObj.DepartmentId != existObj.DepartmentId)
                {
                    TempData[StaticString.StatusMessage] = "Error: " + department.Name + " already exist";
                    return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
                }

                editObj.Name = department.Name;
                editObj.Description = department.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
            }
        }


        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> DepartmentDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Department.Where(x => x.DepartmentId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDepartmentDelete([Bind("DepartmentId")] Department department)
        {
            try
            {
                var deleteObj = await _context.Department.Where(x => x.DepartmentId.Equals(department.DepartmentId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }



                _context.Department.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(DepartmentIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DepartmentDelete), new { id = department.DepartmentId ?? "" });
            }
        }


        public IActionResult SubmitDeleteDoctor(string id)
        {
            try
            {
                var del = _context.Doctor.Where(x => x.DoctorId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Doctor.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลแพทย์เรียบร้อยแล้ว!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index), ControllerContext.ActionDescriptor.ControllerName);
            }
        }


    }
}