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
using vds.ViewModels;

namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Hospital.RoleName)]

    public class HospitalController : Controller
    {
        readonly string modelName = "โรงพยาบาล";

       
        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public HospitalController(
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
            ViewData["PrefixType"] = _app.GetPrefixTypeSelectList();
            ViewData["Gender"] = _app.GetGenderSelectList();
            ViewData["MaritalStatus"] = _app.GetMaritalStatusSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["BenefitTemplate"] = _app.GetBenefitTemplateSelectList();
        }

        //consume db context service, display all hospital
        public IActionResult Index()
        {
            var objs = _context.Hospital
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }


        [HttpGet]
        public IActionResult Map(string id)
        {
       
            //create new
            if (id == null)
            {
                //dropdownlist 
                FillDropdownListForhospitalForm();

                ViewBag.ImageDataUrl = "/assets/images/noimage.png";

                HospitalView1 hospitalview = new HospitalView1();
                hospitalview.Hospital = new Hospital();
                hospitalview.Director = new Director();
                hospitalview.Coordinator = new Coordinator();

                return View(hospitalview);

            }

            //edit object
            HospitalView1 editObj = new HospitalView1();
            editObj.Hospital = _context.Hospital.Where(x => x.HospitalId.Equals(id)).FirstOrDefault();
            editObj.Coordinator = _context.Coordinator.Where(x => x.HospitalId.Equals(id)).FirstOrDefault();
            editObj.Director = _context.Director.Where(x => x.HospitalId.Equals(id)).FirstOrDefault();

            if (editObj.Hospital == null)
            {
                return NotFound();
            }
 
            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }



        //display hospital create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {
            string userTypeId = TempData["userTypeId"] != null ? TempData["userTypeId"].ToString() : null;

            ViewBag.userTypeId = userTypeId;

            ViewBag.IsNew = false;
            ViewBag.ImageDataUrl = "/assets/images/noimage.png";
            //create new
            if (id == null)
            {
                //dropdownlist 
                FillDropdownListForhospitalForm();
 
                HospitalView1 hospitalview = new HospitalView1();
                hospitalview.Hospital = new Hospital();
                hospitalview.Director = new Director();
                hospitalview.Coordinator = new Coordinator();
                ViewBag.IsNew = true;
                return View(hospitalview);

            }

            //edit object
            HospitalView1 editObj = new HospitalView1();
            editObj.Hospital = _context.Hospital.Where(x => x.HospitalId.Equals(id)).FirstOrDefault();
            editObj.Coordinator = _context.Coordinator.Where(x => x.HospitalId.Equals(id)).FirstOrDefault();
            editObj.Director = _context.Director.Where(x => x.HospitalId.Equals(id)).FirstOrDefault();

            if (editObj.Hospital==null)
            {
                TempData[StaticString.StatusMessage] = "Error: ไม่พบข้อมูลโรงพยาบาล";
                return RedirectToAction(nameof(Index));
            } 
            else if (editObj.Coordinator==null)
            {

                TempData[StaticString.StatusMessage] = "Error: ไม่พบข้อมูลผู้ประสานงาน กรุณาเพิ่มข้อมูล";
                return RedirectToAction(nameof(Index));
            }else if (editObj.Director==null)
            {

                TempData[StaticString.StatusMessage] = "Error: ไม่พบข้อมูลผู้อำนวยการ กรุณาเพิ่มข้อมูล";
                return RedirectToAction(nameof(Index));
            }
 

            //if (editObj.Hospital == null)
            //{
            //    return NotFound();
            //}
           if (editObj.Coordinator.ImageData != null)
            {
                string imageBase64Data = Convert.ToBase64String(editObj.Coordinator.ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                ViewBag.ImageDataUrl = imageDataURL;
               
            }
 
            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }

        //   public async Task<IActionResult> SubmitForm([Bind(Prefix = "Hospital")] Hospital hospital1, [Bind(Prefix = "Director")] Director director, [Bind(Prefix = "Coordinator")] Coordinator coordinator)

        //  public async Task<IActionResult> SubmitForm(HospitalView hospital1)

        //   public async Task<IActionResult> SubmitForm([Bind(nameof(Director.FirstName), Prefix = "Director")] Director md1)



        //post submitted hospital data. if hospitalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> SubmitForm(IFormFile file, [Bind(Prefix = "Hospital")] Hospital hospital, [Bind(Prefix = "Director")] Director director, [Bind(Prefix = "Coordinator")] Coordinator coordinator)

        {

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = hospital.HospitalId ?? "" });
                }

                //  create new
                if (hospital.HospitalId == null)
                {

                    Hospital newhospital = new Hospital();
                    newhospital.HospitalId = Guid.NewGuid().ToString();

                    newhospital.HospitalName = hospital.HospitalName;

                    newhospital.Address1 = hospital.Address1;
                    newhospital.SubDistrict = hospital.SubDistrict;
                    newhospital.District = hospital.District;
                    newhospital.Province = hospital.Province;
                    newhospital.ZipCode = hospital.ZipCode;

                    newhospital.Size = hospital.Size;
                    newhospital.OperatingRoom = hospital.OperatingRoom;
                    newhospital.Service = hospital.Service;


                    newhospital.CreatedBy = await _userManager.GetUserAsync(User);
                    newhospital.CreatedAtUtc = DateTime.UtcNow;
                    newhospital.UpdatedBy = newhospital.CreatedBy;
                    newhospital.UpdatedAtUtc = newhospital.CreatedAtUtc;

                    _context.Hospital.Add(newhospital);

                    //  Add Director
                    Director newdirector = new Director();

                    newdirector.DirectorId = Guid.NewGuid().ToString();

                    newdirector.PrefixTypeId = director.PrefixTypeId;
                    newdirector.FirstName = director.FirstName;
                    newdirector.LastName = director.LastName;

                    newdirector.Address1 = director.Address1;
                    newdirector.SubDistrict = director.SubDistrict;
                    newdirector.District = director.District;
                    newdirector.Province = director.Province;
                    newdirector.ZipCode = director.ZipCode;

                    newdirector.Phone = director.Phone;
                    newdirector.Email = director.Email;
                    newdirector.LineId = director.LineId;
                    newdirector.HospitalId = newhospital.HospitalId;

                    newdirector.CreatedBy = await _userManager.GetUserAsync(User);
                    newdirector.CreatedAtUtc = DateTime.UtcNow;
                    newdirector.UpdatedBy = newdirector.CreatedBy;
                    newdirector.UpdatedAtUtc = newdirector.CreatedAtUtc;

                    //  newdirector.HospitalId = newhospital.HospitalId;
                    _context.Director.Add(newdirector);

                    //  Add Coordinator
                    Coordinator newcoordinator = new Coordinator();
                    newcoordinator.CoordinatorId = Guid.NewGuid().ToString();

                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            newcoordinator.ImageData = ms.ToArray();
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
                        newcoordinator.ImageData = stream.ToArray();
                        stream.Close();
                        stream.Dispose();
                    }



                    newcoordinator.PrefixTypeId = coordinator.PrefixTypeId;
                    newcoordinator.FirstName = coordinator.FirstName;
                    newcoordinator.LastName = coordinator.LastName;

                    newcoordinator.Address1 = coordinator.Address1;
                    newcoordinator.SubDistrict = coordinator.SubDistrict;
                    newcoordinator.District = director.District;
                    newcoordinator.Province = coordinator.Province;
                    newcoordinator.ZipCode = coordinator.ZipCode;

                    newcoordinator.Phone = coordinator.Phone;
                    newcoordinator.Email = coordinator.Email;
                    newcoordinator.LineId = coordinator.LineId;

                    newcoordinator.HospitalId = newhospital.HospitalId;
                    newcoordinator.DesignationId = coordinator.DesignationId;
                    newcoordinator.DepartmentId = coordinator.DepartmentId;


                    newcoordinator.CreatedBy = await _userManager.GetUserAsync(User);
                    newcoordinator.CreatedAtUtc = DateTime.UtcNow;
                    newcoordinator.UpdatedBy = newcoordinator.CreatedBy;
                    newcoordinator.UpdatedAtUtc = newcoordinator.CreatedAtUtc;

                    _context.Coordinator.Add(newcoordinator);


                    //  Add Data to Database
                    _context.SaveChanges();

                    //  dropdownlist
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูล" + modelName + "เรียบร้อยแล้ว.";

                    return RedirectToAction(nameof(Form), new { id = newhospital.HospitalId ?? "" });
                }




                ////edit existing
                Hospital edithospital = new Hospital();
                Director editdirector = new Director();
                Coordinator editcoordinator = new Coordinator();

                edithospital = _context.Hospital.Where(x => x.HospitalId.Equals(hospital.HospitalId)).FirstOrDefault();
                editdirector = _context.Director.Where(x => x.HospitalId.Equals(hospital.HospitalId)).FirstOrDefault();
                editcoordinator = _context.Coordinator.Where(x => x.HospitalId.Equals(hospital.HospitalId)).FirstOrDefault();

                if (edithospital != null)
                {

                    edithospital.HospitalName = hospital.HospitalName;

                    edithospital.Address1 = hospital.Address1;
                    edithospital.SubDistrict = hospital.SubDistrict;
                    edithospital.District = hospital.District;
                    edithospital.Province = hospital.Province;
                    edithospital.ZipCode = hospital.ZipCode;

                    edithospital.Service = hospital.Service;
                    edithospital.OperatingRoom = hospital.OperatingRoom;
                    edithospital.Size = hospital.Size;

                    edithospital.UpdatedBy = await _userManager.GetUserAsync(User);
                    edithospital.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(edithospital);


                    // Update  Director
                    editdirector.PrefixTypeId = director.PrefixTypeId;
                    editdirector.FirstName = director.FirstName;
                    editdirector.LastName = director.LastName;

                    editdirector.Address1 = director.Address1;
                    editdirector.SubDistrict = director.SubDistrict;
                    editdirector.District = director.District;
                    editdirector.Province = director.Province;
                    editdirector.ZipCode = director.ZipCode;

                    editdirector.Phone = director.Phone;
                    editdirector.Email = director.Email;
                    editdirector.LineId = director.LineId;

                    editdirector.UpdatedBy = await _userManager.GetUserAsync(User);
                    editdirector.UpdatedAtUtc = DateTime.UtcNow;

                    
                    _context.Update(editdirector);


                    // Update Coordinator
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            editcoordinator.ImageData = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }
                    }
                    else
                    {

                        if (editcoordinator.ImageData == null)
                        {
                            var webRoot = _env.WebRootPath;
                            var file1 = System.IO.Path.Combine(webRoot, "assets/images/noimage.png");
                            byte[] imageArray = System.IO.File.ReadAllBytes(file1);
                            MemoryStream stream = new MemoryStream(imageArray);
                            editcoordinator.ImageData = stream.ToArray();
                            stream.Close();
                            stream.Dispose();
                        }
                    }

                    editcoordinator.PrefixTypeId = coordinator.PrefixTypeId;
                    editcoordinator.FirstName = coordinator.FirstName;
                    editcoordinator.LastName = coordinator.LastName;

                    editcoordinator.Address1 = coordinator.Address1;
                    editcoordinator.SubDistrict = coordinator.SubDistrict;
                    editcoordinator.District = director.District;
                    editcoordinator.Province = coordinator.Province;
                    editcoordinator.ZipCode = coordinator.ZipCode;

                    editcoordinator.Phone = coordinator.Phone;
                    editcoordinator.Email = coordinator.Email;
                    editcoordinator.LineId = coordinator.LineId;

                    editcoordinator.DesignationId = coordinator.DesignationId;
                    editcoordinator.DepartmentId = coordinator.DepartmentId;

                    editcoordinator.UpdatedBy = await _userManager.GetUserAsync(User);
                    editcoordinator.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editcoordinator);

                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(Form), new { id = edithospital.HospitalId ?? "" });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = hospital.HospitalId ?? "" });
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

            var del = _context.Hospital
                .AsNoTracking()


                .Where(x => x.HospitalId.Equals(id)).FirstOrDefault();

            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(del);
        }



        public IActionResult SubmitDeleteHospital(string id)
        {
            try
            {
                var del = _context.Hospital
                              .AsNoTracking()
                              .Where(x => x.HospitalId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Hospital.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลโรงพยาบาลเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }



        //delete submitted hospital if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("hospitalId")] Hospital hospital)
        {
            try
            {
                var del = _context.Hospital.Where(x => x.HospitalId.Equals(hospital.HospitalId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Hospital.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete hospital success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = hospital.HospitalId ?? "" });
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

                    TempData[StaticString.StatusMessage] = "Create new item success.";
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

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
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

    }
}