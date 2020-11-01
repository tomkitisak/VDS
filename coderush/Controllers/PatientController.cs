using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;

namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Patient.RoleName)]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public PatientController(
            ApplicationDbContext context,
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

        //fill viewdata as dropdownlist datasource for employee form
        private void FillDropdownListForEmployeeForm()
        {

            ViewData["Hospital"] = _app.GetHospitalSelectList();
            ViewData["PrefixType"] = _app.GetPrefixTypeSelectList();
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["Gender"] = _app.GetGenderSelectList();
            ViewData["MaritalStatus"] = _app.GetMaritalStatusSelectList();
            ViewData["Supervisor"] = _app.GetEmployeeSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["DiseaseType"] = _app.GetDiseaseTypeSelectList();
        }



        //consume db context service, display all employee
        public IActionResult Index(string id,string userTypeId)
        {
            if  (String.IsNullOrEmpty(id))
            {
                var objs = _context.Patient
               .AsNoTracking()
               .Include(x => x.Hospital)
               .Include(x => x.DiseaseType)
               .Include(x => x.PrefixType)
               .OrderByDescending(x => x.CreatedAtUtc).ToList();
                return View(objs);

            }
            else
            {
                var objs = _context.Patient
              .AsNoTracking()
              .Include(x => x.Hospital)
              .Include(x => x.DiseaseType)
              .Include(x => x.PrefixType)
              .Where(x => x.HospitalId.Equals(id))
              .OrderByDescending(x => x.CreatedAtUtc).ToList();
                ViewBag.HospitalId = id;
                ViewBag.userTypeId = userTypeId;
               return View(objs);

            }
 
        }



        [HttpGet]
        public async Task<IActionResult> LoginOnBehalf(string id)
        {
            Employee employee = new Employee();
            employee = _context.Employee.Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }



            return RedirectToAction("Index", "SelfService", new { period = DateTime.Now.ToString("yyyy-MM") });
        }

        //display employee create edit form
        [HttpGet]
        public IActionResult Form(string patientId,string hospitalId,string userTypeId)
        {
            //  TempData[StaticString.StatusMessage] = "Error: ไม่พบข้อมูลโรงพยาบาล.";
            //   return RedirectToAction(nameof(Index), new { id = id });
            ViewBag.IsNew = false;
            ViewBag.userTypeId = userTypeId;

            //create new
            if (String.IsNullOrEmpty(patientId))
            {
                //dropdownlist 
                FillDropdownListForEmployeeForm();

                Patient newObj = new Patient();
                DateTime bd = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("en-US")));
                newObj.DateOfBirth = bd;

                if (!String.IsNullOrEmpty(hospitalId))
                {
                    newObj.HospitalId = hospitalId;
                   
                }

                ViewBag.IsNew = true;
                
                return View(newObj);
            }

            //edit object
            Patient editObj = new Patient();
            editObj = _context.Patient.Where(x => x.PatientId.Equals(patientId)).FirstOrDefault();
            DateTime obd = Convert.ToDateTime(editObj.DateOfBirth.ToString("dd-MM-yyyy",new System.Globalization.CultureInfo("en-US")));
            editObj.DateOfBirth = obd;
            if (editObj == null)
            {
                return NotFound();
            }

          
            //dropdownlist 
            FillDropdownListForEmployeeForm();

            return View(editObj);

        }



        //post submitted employee data. if employeeId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind] Patient patient, string userTypeId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { patientId = patient.PatientId ?? "" });
                }



                //create new
                if (patient.PatientId == null)
                {
                    //duplicate Employee Number ID is not allowed

                    Patient newPatient = new Patient();

                    newPatient.PatientId = Guid.NewGuid().ToString();
                    newPatient.PrefixTypeId = patient.PrefixTypeId;
                    newPatient.FirstName = patient.FirstName;
                    newPatient.LastName = patient.LastName;
                    newPatient.Gender = patient.Gender;
                    newPatient.DateOfBirth = patient.DateOfBirth.AddYears(543);

                    newPatient.Phone = patient.Phone;
                    newPatient.Address1 = patient.Address1;

                    newPatient.SubDistrict = patient.SubDistrict;
                    newPatient.District = patient.District;
                    newPatient.Province = patient.Province;
                    newPatient.ZipCode = patient.ZipCode;

                    newPatient.HospitalId = patient.HospitalId;
                    newPatient.DiseaseTypeId = patient.DiseaseTypeId;
                    newPatient.Problem = patient.Problem;


                    newPatient.CreatedBy = await _userManager.GetUserAsync(User);
                    newPatient.CreatedAtUtc = DateTime.UtcNow;
                    newPatient.UpdatedBy = newPatient.CreatedBy;
                    newPatient.UpdatedAtUtc = newPatient.CreatedAtUtc;

                    _context.Patient.Add(newPatient);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForEmployeeForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูลผู้ป่วยเรียบร้อยแล้ว";
                    return RedirectToAction(nameof(Form), new { patientId = newPatient.PatientId , userTypeId = userTypeId });
                }

                //edit existing
                Patient editPatient = new Patient();
                editPatient = _context.Patient.Where(x => x.PatientId.Equals(patient.PatientId)).FirstOrDefault();

                if (editPatient != null)
                {
                    editPatient.PrefixTypeId = patient.PrefixTypeId;
                    editPatient.FirstName = patient.FirstName;
                    editPatient.FirstName = patient.FirstName;
                    editPatient.LastName = patient.LastName;
                    editPatient.Gender = patient.Gender;
                    editPatient.DateOfBirth = patient.DateOfBirth.AddYears(543);

                    editPatient.Phone = patient.Phone;
                    editPatient.Address1 = patient.Address1;

                    editPatient.SubDistrict = patient.SubDistrict;
                    editPatient.District = patient.District;
                    editPatient.ZipCode = patient.ZipCode;

                    editPatient.HospitalId = patient.HospitalId;
                    editPatient.DiseaseTypeId = patient.DiseaseTypeId;
                    editPatient.Problem = patient.Problem;

                    editPatient.UpdatedBy = await _userManager.GetUserAsync(User);
                    editPatient.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editPatient);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForEmployeeForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูลผู้ป่วยเรียบร้อยแล้ว";
                    return RedirectToAction(nameof(Form), new { patientId = editPatient.PatientId, userTypeId = userTypeId });
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { patientId = patient.PatientId  });
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


        public IActionResult SubmitDeletePatient(string id)
        {
            try
            {
                var del = _context.Patient.Where(x => x.PatientId.Equals(id)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Patient.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "ลบข้อมูลผู้ป่วยเรียบร้อยแล้ว!";
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