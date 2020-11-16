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
    [Authorize(Roles = Services.App.Pages.Employee.RoleName)]
    public class EmployeeController : Controller
    {

        private readonly IHostingEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public EmployeeController(
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

        //fill viewdata as dropdownlist datasource for employee form
        private void FillDropdownListForEmployeeForm()
        {

            ViewData["Hospital"] = _app.GetHospitalSelectList();
            ViewData["Designation"] = _app.GetDesignationSelectList();
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["Gender"] = _app.GetGenderSelectList();
            ViewData["PrefixType"] = _app.GetPrefixTypeSelectList();
            ViewData["Supervisor"] = _app.GetEmployeeSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
         
        }

        //consume db context service, display all employee
        public IActionResult Index()
        {
            string userTypeId = object.ReferenceEquals(null, TempData["userTypeId"]) ? TempData["userTypeId"].ToString() : null; 

            if (userTypeId == "1")
            {

            }
            var objs = _context.Coordinator
                .AsNoTracking()
                .Include(x => x.Designation)
                .Include(x => x.Department)
                 .Include(x => x.PrefixType)
                   .Include(x => x.Hospital)
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        [HttpGet]
        public async Task<IActionResult> LoginOnBehalf(string id)
        {
            Employee employee = new Employee();
            employee =  _context.Employee.Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "SelfService", new { period = DateTime.Now.ToString("yyyy-MM") });
        }



        //display employee create edit form
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
                FillDropdownListForEmployeeForm();

                Coordinator newObj = new Coordinator();
              
                ViewBag.IsNew = true;
                return View(newObj);
            }

            //edit object
            Coordinator editObj = new Coordinator();
            editObj = _context.Coordinator.Where(x => x.CoordinatorId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }

            if (editObj.ImageData != null)
            {
                string imageBase64Data = Convert.ToBase64String(editObj.ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

                ViewBag.ImageDataUrl = imageDataURL;
            }
           
            //dropdownlist 
            FillDropdownListForEmployeeForm();

            return View(editObj);

        }

        //post submitted employee data. if employeeId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind]Coordinator coordinator, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = coordinator.CoordinatorId ?? "" });
                }

                //create new
                if (coordinator.CoordinatorId == null)
                {
                    ////duplicate Employee Number ID is not allowed
                    //Employee empNumber = await _context.Employee.Where(x => x.EmployeeIDNumber.Equals(employee.EmployeeIDNumber)).FirstOrDefaultAsync();
                    //if (empNumber != null && !String.IsNullOrEmpty(employee.EmployeeIDNumber))
                    //{
                    //    TempData[StaticString.StatusMessage] = "Error: Employee ID Number Can Not Duplicate. " + employee.EmployeeIDNumber;
                    //    return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                    //}


                    Coordinator nowco = new Coordinator();
                    nowco.CoordinatorId = Guid.NewGuid().ToString();


                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            nowco.ImageData = ms.ToArray();
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
                        nowco.ImageData = stream.ToArray();
                        stream.Close();
                        stream.Dispose();
                    }



                    nowco.PrefixTypeId = coordinator.PrefixTypeId;
                    nowco.FirstName = coordinator.FirstName;
                    nowco.LastName = coordinator.LastName;

                    nowco.Email = coordinator.Email;
                    nowco.Phone = coordinator.Phone;
                    nowco.Address1 = coordinator.Address1;

                    nowco.SubDistrict = coordinator.SubDistrict;
                    nowco.District = coordinator.District;
                    nowco.Province = coordinator.Province;
                    nowco.ZipCode = coordinator.ZipCode;


                    nowco.HospitalId = coordinator.HospitalId;
                    nowco.DesignationId = coordinator.DesignationId;
                    nowco.DepartmentId = coordinator.DepartmentId;


                    nowco.CreatedBy = await _userManager.GetUserAsync(User);
                    nowco.CreatedAtUtc = DateTime.UtcNow;
                    nowco.UpdatedBy = nowco.CreatedBy;
                    nowco.UpdatedAtUtc = nowco.CreatedAtUtc;

                    _context.Coordinator.Add(nowco);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForEmployeeForm();

                    TempData[StaticString.StatusMessage] = "เพิ่มข้อมูลผู้ประสานงานเรียบร้อยแล้ว";
                    return RedirectToAction(nameof(Form), new { id = nowco.CoordinatorId ?? "" });
                }

                //edit existing
                Coordinator editCo = new Coordinator();
                editCo = _context.Coordinator.Where(x => x.CoordinatorId.Equals(coordinator.CoordinatorId)).FirstOrDefault();

                if (editCo != null)
                {
                    ////duplicate Employee Number ID is not allowed
                    //Employee empNumber = await _context.Employee.Where(x => x.EmployeeIDNumber.Equals(employee.EmployeeIDNumber)).FirstOrDefaultAsync();
                    //if (empNumber != null && editEmployee.EmployeeIDNumber != employee.EmployeeIDNumber && !String.IsNullOrEmpty(employee.EmployeeIDNumber))
                    //{
                    //    TempData[StaticString.StatusMessage] = "Error: Employee ID Number Can Not Duplicate. " + employee.EmployeeIDNumber;
                    //    return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                    //}
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            MemoryStream ms = new MemoryStream();
                            file.CopyTo(ms);
                            editCo.ImageData = ms.ToArray();
                            ms.Close();
                            ms.Dispose();
                        }
                    }
                    else
                    {
                        if (editCo.ImageData == null)
                        {
                            var webRoot = _env.WebRootPath;
                            var file1 = System.IO.Path.Combine(webRoot, "assets/images/noimage.png");
                            byte[] imageArray = System.IO.File.ReadAllBytes(file1);
                            MemoryStream stream = new MemoryStream(imageArray);
                            editCo.ImageData = stream.ToArray();
                            stream.Close();
                            stream.Dispose();
                        }
                    }

                    editCo.PrefixTypeId = coordinator.PrefixTypeId;
                    editCo.FirstName = coordinator.FirstName;
                    editCo.LastName = coordinator.LastName;
                    
                    editCo.Email = coordinator.Email;
                    editCo.Phone = coordinator.Phone;
                    editCo.Address1 = coordinator.Address1;

                    editCo.SubDistrict = coordinator.SubDistrict;
                    editCo.District = coordinator.District;
                    editCo.ZipCode = coordinator.ZipCode;


                    editCo.DepartmentId = coordinator.DepartmentId;


                    editCo.UpdatedBy = await _userManager.GetUserAsync(User);
                    editCo.UpdatedAtUtc = DateTime.UtcNow;
                    _context.Update(editCo);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForEmployeeForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูลผู้ประสานงานเรียบร้อยแล้ว";
                    return RedirectToAction(nameof(Form), new { id = coordinator.CoordinatorId ?? "" });
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = coordinator.CoordinatorId ?? "" });
            }
        }


        public IActionResult SubmitDeleteEmployee(string id)
        {
            try
            {
                var del = _context.Coordinator.Where(x => x.CoordinatorId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Coordinator.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลผู้ประสานงานเรียบร้อยแล้ว!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index), ControllerContext.ActionDescriptor.ControllerName);
            }
        }

        //display employee item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var del = _context.Employee
                .AsNoTracking()
                .Include(x => x.Designation)
                .Include(x => x.Department)
                .Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();

            //dropdownlist 
            FillDropdownListForEmployeeForm();

            return View(del);
        }

        //delete submitted employee if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("EmployeeId")] Employee employee)
        {
            try
            {
                var del = _context.Employee.Where(x => x.EmployeeId.Equals(employee.EmployeeId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Employee.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete employee success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = employee.EmployeeId ?? "" });
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

                //cek existing ke employee
                Employee objCheck = new Employee();
                objCheck = await _context.Employee
                    .Where(x => x.DesignationId.Equals(deleteObj.DesignationId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(DesignationDelete), new { id = designation.DesignationId ?? "" });
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

                //cek existing ke employee
                Employee objCheck = new Employee();
                objCheck = await _context.Employee
                    .Where(x => x.DepartmentId.Equals(deleteObj.DepartmentId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(DepartmentDelete), new { id = department.DepartmentId ?? "" });
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