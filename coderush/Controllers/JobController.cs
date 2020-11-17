using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Syncfusion.EJ2.Buttons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.ViewModels;


namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Hospital.RoleName)]

    public class JobController : Controller
    {
        readonly string modelName = "งานร้องขอ";
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public JobController(
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

        //fill viewdata as dropdownlist datasource for hospital form
        private void FillDropdownListForhospitalForm()
        {
            ViewData["Hospital"] = _app.GetHospitalSelectList();
            ViewData["JobStatus"] = _app.GetJobStatusSelectList();
            ViewData["Designation"] = _app.GetDesignationSelectList();
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["PrefixType"] = _app.GetPrefixTypeSelectList();
            ViewData["Gender"] = _app.GetGenderSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["DiseaseType"] = _app.GetDiseaseTypeSelectList();
            ViewData["DoctorType"] = _app.GetDoctorTypeSelectList();

        }


        //======================================================== Index ===========================================================
        public IActionResult Index()
        {
            string hId = TempData["hospitalId"] != null ? TempData["hospitalId"].ToString() : null;
            string uType = TempData["userTypeId"] != null ? TempData["userTypeId"].ToString() : null;

            ViewBag.hospitalId = hId;
            ViewBag.userTypeId = uType;
            try
            {
                if (uType=="1")
                {
                   var objs = _context.Job
                    .Include(x => x.Hospital)
                    .Include(x => x.JobStatus)
                    .AsNoTracking()
                   .Where(x => x.HospitalId.Equals(hId))
                   .OrderByDescending(x => x.CreatedAtUtc).ToList();
                    return View(objs);
                }
                else
                {
                 var objs = _context.Job
                 .Include(x => x.Hospital)
                 .Include(x => x.JobStatus)
                 .AsNoTracking()
                 .OrderByDescending(x => x.CreatedAtUtc).ToList();
                    return View(objs);
                }
              
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }



        //======================================================== FORM#1 ===========================================================

        [HttpGet]
        public IActionResult Form(string id)
        {

            ViewBag.IsNew = false;
            ViewBag.jobId = id;

            ViewBag.Status = 0;

            string hospitalId = TempData["hospitalId"].ToString();
            //create new
            if (id == null)
            {
                //dropdownlist 

                FillDropdownListForhospitalForm();

                JobView newObj = new JobView();
                newObj.DoctorList = new List<Doctor>();
                newObj.Job = new Job();
                newObj.PatientList = new List<Patient>();
                ViewBag.Status = 0;

                if (!String.IsNullOrEmpty(hospitalId))
                {
                    newObj.Job.HospitalId = hospitalId;

                }

                ViewBag.IsNew = true;

                return View(newObj);

            }
 
            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();
            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;
            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));

            //  editObj.Job.AppStartDate = Convert.ToDateTime(DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")));
            //  editObj.Job.AppEndDate = Convert.ToDateTime(DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")));        
            //  editObj.Job.AppEntryDate = Convert.ToDateTime(DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US")));

            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));


            if (editObj.Job.JobStatus.Status == 1)
            {
                editObj.Job.PostDate = DateTime.Now;
            }

            if (editObj.Job == null)
            {
                return NotFound();
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

        public async Task<IActionResult> SubmitForm([Bind] Job job, bool IsChecked1, string userTypeId)

        {
            var status = _context.JobStatus.FirstOrDefault(x => x.Status == 1);

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = job.JobId ?? "" });
                }

                //  create new
                if (job.JobId == null)
                {
                    var jobstatus = _context.JobStatus.Where(x => x.Status == 1).FirstOrDefault();

                    Job newjob = new Job();
                    newjob.JobId = Guid.NewGuid().ToString();

                    newjob.Name = job.Name;
                    newjob.Description = job.Description;
                    newjob.TransDate = Convert.ToDateTime(job.TransDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    newjob.AppStartDate = Convert.ToDateTime(job.AppStartDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    newjob.AppEndDate = Convert.ToDateTime(job.AppEndDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    newjob.PostDate = Convert.ToDateTime(job.PostDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    newjob.AppEntryDate = Convert.ToDateTime(job.AppEntryDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    newjob.HospitalId = job.HospitalId;
                    newjob.JobStatusId = status.JobStatusId;
                    newjob.JobStatus = job.JobStatus;
                    newjob.CreatedBy = await _userManager.GetUserAsync(User);
                    newjob.CreatedAtUtc = DateTime.UtcNow;
                    newjob.UpdatedBy = newjob.CreatedBy;
                    newjob.UpdatedAtUtc = newjob.CreatedAtUtc;

                    _context.Job.Add(newjob);
                    //  Add Data to Database
                    _context.SaveChanges();

                    //  dropdownlist
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobNewEdit), new { id = newjob.JobId } );
                }


                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {

                    editjob.Name = job.Name;
                    editjob.Description = job.Description;
                    editjob.TransDate = Convert.ToDateTime(job.TransDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.AppStartDate = Convert.ToDateTime(job.AppStartDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.AppEndDate = Convert.ToDateTime(job.AppEndDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.PostDate = Convert.ToDateTime(job.PostDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.AppEntryDate = Convert.ToDateTime(job.AppEntryDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.HospitalId = job.HospitalId;

                    if (IsChecked1)
                    {

                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 2).Select(x => x.JobStatusId).FirstOrDefault();
                    }
                    else
                    {

                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 1).Select(x => x.JobStatusId).FirstOrDefault();

                    }

                    editjob.TotalPatients = _context.JobPatient.Where(x => x.JobId.Equals(job.JobId)).Count();
                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobNewEdit), new { id = editjob.JobId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobNewEdit), new { id = job.JobId });
            }
        }


        [HttpGet]
        // public IActionResult SelectDoctor(string FName) => FName is null ? GetAllDoctor() : SearchDoctor(FName);
        public IActionResult SelectDoctors(string strJobId, string userTypeId, string action)
        {


            string ID = ViewBag.jobId = strJobId;
            ViewBag.fromaction = action;
            ViewBag.userTypeId = userTypeId;


            var InJobReady = _context.JobDoctor
               .AsNoTracking()
               .Where(x => x.JobId.Equals(ID)).ToList();

            var IdInJobReady = InJobReady.Select(x => x.DoctorId).ToArray();

            ViewBag.GroupName = "";

            var doctors = _context.Doctor
                     .AsNoTracking()
                     .Include(x => x.DoctorType)
                     .Include(x => x.PrefixType)
                     .Where(x => !IdInJobReady.Contains(x.DoctorId))
                     .ToList();

            List<DoctorSelectedViewModel> ds = new List<DoctorSelectedViewModel>();

            foreach (var item in doctors)
            {

                ds.Add(new DoctorSelectedViewModel
                {
                    DoctorGroupId = ID,
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


        //post submitted hospital data. if hospitalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitSelectDoctor(string jobId,  List<DoctorSelectedViewModel> doctor)
        {

            string OrgAction = TempData["OrgAction"].ToString();
            int countdoctor = doctor.Where(x => x.DoctorSelect.Selected).Count();
            try
            {
                if (countdoctor > 0)
                {
                    foreach (var item in doctor)
                    {
                        if (item.DoctorSelect.Selected)
                        {
                            JobDoctor newdoctor = new JobDoctor();
                            newdoctor.JobDoctorId = Guid.NewGuid().ToString();
                            newdoctor.JobId = jobId;
                            newdoctor.DoctorId = item.DoctorId;
                            newdoctor.CreatedBy = await _userManager.GetUserAsync(User);
                            newdoctor.CreatedAtUtc = DateTime.UtcNow;
                            newdoctor.UpdatedBy = newdoctor.CreatedBy;
                            newdoctor.UpdatedAtUtc = newdoctor.CreatedAtUtc;
                            _context.JobDoctor.Add(newdoctor);
                        }
                    }
                    _context.SaveChanges();

                    int mycount = _context.JobDoctor.Where(x => x.JobId.Equals(jobId)).Count();
                    var update = _context.Job.Where(o => o.JobId == jobId).FirstOrDefault();
                    if (update != null)
                    {
                        update.TotalDoctors = mycount;
                        _context.Update(update);
                    }
                 

                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                }
            }

            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(OrgAction, new { id = jobId });
            }

            return RedirectToAction(OrgAction, new { id = jobId });

        }


        [HttpGet]
        public IActionResult SelectPatients([Bind(Prefix = "Job")] Job job)
        {


            string ID = ViewBag.jobId = job.JobId;
         
            var InReady = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(ID)).ToList();

            var IdArray = InReady.Select(x => x.PatientId).ToArray();

            ViewBag.GroupName = "";
            var patients = _context.Patient
                     .AsNoTracking()
                     .Include(x => x.PrefixType)
                     .Include(x => x.DiseaseType)
                     .Where(x => !IdArray.Contains(x.PatientId) && x.HospitalId.Equals(job.HospitalId)).ToList();

            List<PatientSelectViewModel> ds = new List<PatientSelectViewModel>();

            foreach (var item in patients)
            {

                ds.Add(new PatientSelectViewModel
                {

                    PatientId = item.PatientId,
                    PrefixTypeId = item.PrefixTypeId,
                    PrefixType = item.PrefixType,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Phone = item.Phone,
                    DiseaseType = item.DiseaseType,
                    Problem = item.Problem

                });
            }


            return View(ds);

        }


        //post submitted hospital data. if hospitalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitSelectPatients(string jobId, List<PatientSelectViewModel> patient)
        {
            string orgAction = TempData["OrgAction"].ToString();


            int mycount = patient.Where(x => x.PatientSelect.Selected).Count();
            try
            {
                if (mycount > 0)
                {
                    foreach (var item in patient)
                    {
                        if (item.PatientSelect.Selected)
                        {
                            JobPatient newjobpatient = new JobPatient();
                            newjobpatient.JobPatientId = Guid.NewGuid().ToString();
                            newjobpatient.JobId = jobId;
                            newjobpatient.PatientId = item.PatientId;

                            newjobpatient.CreatedBy = await _userManager.GetUserAsync(User);
                            newjobpatient.CreatedAtUtc = DateTime.UtcNow;
                            newjobpatient.UpdatedBy = newjobpatient.CreatedBy;
                            newjobpatient.UpdatedAtUtc = newjobpatient.CreatedAtUtc;
                            _context.JobPatient.Add(newjobpatient);
                        }
                    }
                    _context.SaveChanges();

                    int countpatient = _context.JobPatient.Where(x => x.JobId.Equals(jobId)).Count();
                    var update = _context.Job.Where(o => o.JobId == jobId).FirstOrDefault();
                    if (update != null)
                    {
                        update.TotalPatients = countpatient;
                        _context.Update(update);
                    }

                    _context.SaveChanges();

                    //_context.Job.Where(x => x.JobId == jobId).up .UpdateFromQuery(x => new Customer { IsActive = false });

                    TempData[StaticString.StatusMessage] = "บันทึกข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                }
            }

            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(orgAction, new { id = jobId  });
            }

            return RedirectToAction(orgAction, new { id = jobId });

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

        //delete submitted hospital if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDeletePrefixType(string prefixTYpeId)
        {
            try
            {
                var del = _context.PrefixType.Where(x => x.PrefixTypeId.Equals(prefixTYpeId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.PrefixType.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete hospital success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = "" ?? "" });
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


      


        //display hospital create edit form
        [HttpGet]
        public IActionResult DoctorDetailView(string id, string jobId)
        {
            ViewBag.jobId = jobId;

            //edit object
            Doctor editObj = new Doctor();
            editObj = _context.Doctor.Where(x => x.DoctorId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }

            string imageBase64Data = Convert.ToBase64String(editObj.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }



        //display hospital create edit form
        [HttpGet]
        public IActionResult PatientDetail(string id)
        {


            //edit object
            Patient editObj = new Patient();
            editObj = _context.Patient.Where(x => x.PatientId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }



        /*==================================================== FORM2 ==============================================================*/
        //display hospital create edit form
        [HttpGet]
        public IActionResult Form2(string id, string cap)
        {

            ViewBag.jobId = id;
            ViewBag.capture = cap;
            ViewBag.Status = 0;



            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();

            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Include(x => x.Hospital)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();
            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;
            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppStartDate = Convert.ToDateTime(editObj.Job.AppStartDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEndDate = Convert.ToDateTime(editObj.Job.AppEndDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppStartDate = Convert.ToDateTime(editObj.Job.AppStartDate, new System.Globalization.CultureInfo("en-US"));

            if (editObj.Job.JobStatus.Status == 2)
            {
                editObj.Job.AppStartDate = DateTime.Now;
            }

            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitForm2([Bind] Job job, bool IsChecked2)

        {
            var status = _context.JobStatus.FirstOrDefault(x => x.Status == 2);

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form2), new { id = job.JobId ?? "" });
                }



                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    // Server is locale Thai  accept  year in BE
                    editjob.AppEntryDate = Convert.ToDateTime(DateTime.Today, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.AppStartDate = Convert.ToDateTime(job.AppStartDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.AppEndDate = Convert.ToDateTime(job.AppEndDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);

                    editjob.Remark1 = job.Remark1;
                    if (IsChecked2)
                    {

                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 3).Select(x => x.JobStatusId).FirstOrDefault();
                    }

                    editjob.TotalDoctors = _context.JobDoctor.Where(x => x.JobId.Equals(job.JobId)).Count();
                    editjob.TotalDoctors = _context.JobDoctor.Where(x => x.JobId.Equals(job.JobId)).Count();

                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(Form2), new { id = job.JobId, cap = "งานร้องขอ" });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form2), new { id = job.JobId, cap = "งานร้องขอ" });
            }
        }

        /*==================================================== Form Job Done Entry VIEW ==============================================================*/
        //display hospital create edit form
        [HttpGet]
        public IActionResult FormJobDoneView(string id)
        {

            ViewBag.jobId = id;
          
            ViewBag.Status = 0;
            TempData["fromTab"] = 4;



            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();

            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Include(x => x.Hospital)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();
            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;
 

            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        /*==================================================== Form Job Done Entry  ==============================================================*/
        //display hospital create edit form
        [HttpGet]
        public IActionResult FormJobDoneEntry(string id)
        {

            ViewBag.jobId = id;            
            ViewBag.Status = 0;
            TempData["fromTab"] = 3;


            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Include(x => x.Hospital)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();
            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;
            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppStartDate = Convert.ToDateTime(editObj.Job.AppStartDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEndDate = Convert.ToDateTime(editObj.Job.AppEndDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEntryDate = Convert.ToDateTime(editObj.Job.AppEntryDate, new System.Globalization.CultureInfo("en-US"));

            editObj.Job.StartDate = DateTime.Now;
            editObj.Job.EndDate = DateTime.Now;
            editObj.Job.JobEndEntryDate = DateTime.Today;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobDoneEntry([Bind] Job job, bool IsChecked3)

        {
            var status = _context.JobStatus.FirstOrDefault(x => x.Status == 3);

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(FormJobDoneEntry), new { id = job.JobId ?? "" });
                }



                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    if (IsChecked3)
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 4).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.StartDate = job.AppStartDate.AddYears(543);
                        editjob.EndDate = job.AppEndDate.AddYears(543);
                        editjob.JobEndEntryDate = DateTime.Now.AddYears(543);
                        editjob.Remark2 = job.Remark2;
                        editjob.IsDone = true;
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobDoneView), new { id = job.JobId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobDoneView), new { id = job.JobId });
            }
        }
        /*==================================================== Form Job Done Entry  ==============================================================*/
        //display hospital create edit form
        [HttpGet]
        public IActionResult FormJobDoneEdit(string id)
        {

            ViewBag.jobId = id;

            ViewBag.Status = 0;
            TempData["fromTab"] = 4;



            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();

            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Include(x => x.Hospital)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();
            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;
            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppStartDate = Convert.ToDateTime(editObj.Job.AppStartDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEndDate = Convert.ToDateTime(editObj.Job.AppEndDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEntryDate = Convert.ToDateTime(editObj.Job.AppEntryDate, new System.Globalization.CultureInfo("en-US"));

            editObj.Job.StartDate = DateTime.Now;
            editObj.Job.EndDate = DateTime.Now;
            editObj.Job.JobEndEntryDate = DateTime.Today;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobDoneEdit([Bind] Job job, bool IsChecked3)

        {
          //  var status = _context.JobStatus.FirstOrDefault(x => x.Status == 3);

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(FormJobDoneEdit), new { id = job.JobId  });
                }



                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    if (IsChecked3)
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 4).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.StartDate = job.AppStartDate.AddYears(543);
                        editjob.EndDate = job.AppEndDate.AddYears(543);
                        editjob.JobEndEntryDate = DateTime.Now.AddYears(543);
                        editjob.Remark2 = job.Remark2;
                        editjob.IsDone = true;
                    }
                    else
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 3).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.StartDate = DateTime.Now.AddYears(543);
                        editjob.EndDate = DateTime.Now.AddYears(543);
                        editjob.JobEndEntryDate = DateTime.Now.AddYears(543);
                        editjob.Remark2 = string.Empty;
                        editjob.IsDone = false;
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobDoneEdit), new { id = job.JobId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobDoneEdit), new { id = job.JobId });
            }
        }


        /*==================================================== FormJobAppointmentView ==============================================================*/
        //display hospital create edit form
        [HttpGet]
        public IActionResult FormJobAppointmentView(string id)
        {

            ViewBag.jobId = id;
           
            ViewBag.Status = 0;
            TempData["fromTab"] = 3;


            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();

            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Include(x => x.Hospital)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();
            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;
            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppStartDate = Convert.ToDateTime(editObj.Job.AppStartDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEndDate = Convert.ToDateTime(editObj.Job.AppEndDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppEntryDate = Convert.ToDateTime(editObj.Job.AppEntryDate, new System.Globalization.CultureInfo("en-US"));

            if (editObj.Job.JobStatus.Status == 2)
            {
                editObj.Job.AppEntryDate = DateTime.Now;
            }

            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobAppointmentView([Bind] Job job, bool IsChecked3)

        {
            var status = _context.JobStatus.FirstOrDefault(x => x.Status == 3);

            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form2), new { id = job.JobId ?? "" });
                }



                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    // Server is locale Thai  accept  year in BE
                    editjob.StartDate = Convert.ToDateTime(job.StartDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.EndDate = Convert.ToDateTime(job.EndDate, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                    editjob.JobEndEntryDate = Convert.ToDateTime(DateTime.Now.ToString(new System.Globalization.CultureInfo("en-US"))).AddYears(543);

                    editjob.Remark2 = job.Remark2;
                    if (IsChecked3)
                    {

                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 4).Select(x => x.JobStatusId).FirstOrDefault();
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobAppointmentView), new { id = job.JobId, cap = "งานร้องขอ" });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobAppointmentView), new { id = job.JobId, cap = "งานร้องขอ" });
            }
        }


        //======================================================== FORM POST JOB FROM STATUS-1 TO STATUS-2 ===========================================================

        [HttpGet]
        public IActionResult FormJobPost(string id, string userTypeId)
        {

            ViewBag.IsNew = false;
            ViewBag.userTypeId = userTypeId;

            ViewBag.jobId = id;
            ViewBag.Status = 0;

            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();

            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;

            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = DateTime.Now;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobPost([Bind] Job job, bool IsChecked1, string userTypeId)

        {
            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(FormJobPost), new { id = job.JobId, userTypeId = userTypeId });
                }

                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    // Server is locale Thai  accept  year in BE
                    editjob.TotalPatients = job.TotalPatients;

                    if (IsChecked1)
                    {
                        editjob.PostDate = job.PostDate.AddYears(543);
                        editjob.IsPosted = true;
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 2).Select(x => x.JobStatusId).FirstOrDefault();
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobPost), new { id = job.JobId, userTypeId = userTypeId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobPost), new { id = job.JobId, userTypeId = userTypeId });
            }
        }


        //======================================================== FORM  JOB  EDIT   ===========================================================

        [HttpGet]
        public IActionResult FormJobPostEdit(string id, string userTypeId)
        {

            ViewBag.IsNew = false;
            ViewBag.userTypeId = userTypeId;

            ViewBag.jobId = id;
            ViewBag.Status = 0;

            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();

            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;

            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = DateTime.Now;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobPostEdit([Bind] Job job, bool IsChecked1, string userTypeId)

        {
            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(FormJobPostEdit), new { id = job.JobId, userTypeId = userTypeId });
                }

                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    // Server is locale Thai  accept  year in BE
                    editjob.TotalPatients = job.TotalPatients;
                    editjob.Name = job.Name;
                    editjob.Description = job.Description;

                    if (IsChecked1)
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 2).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.PostDate = job.PostDate.AddYears(543);
                        editjob.IsPosted = true;
                    }
                    else
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 1).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.PostDate = Convert.ToDateTime(DateTime.Today, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                        editjob.IsPosted = false;
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobPostEdit), new { id = job.JobId, userTypeId = userTypeId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobPostEdit), new { id = job.JobId, userTypeId = userTypeId });
            }
        }


        //======================================================== FORM ADD DOCTOR JOB  ===========================================================

        [HttpGet]
        public IActionResult FormJobAddDoctor(string id)
        {

            ViewBag.IsNew = false;

            TempData["fromTab"] = 2;


            ViewBag.jobId = id;
            ViewBag.Status = 0;

            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();

            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;

            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));


            if (editObj.Job == null)
            {
                return NotFound();
            }

            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }

        //======================================================== FORM ADD DOCTOR JOB  ===========================================================

        [HttpGet]
        public IActionResult FormJobAddDoctorAppointment(string id)
        {

            ViewBag.IsNew = false;
            TempData["fromTab"] = 2;
            ViewBag.jobId = id;
            ViewBag.Status = 0;

            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();

            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;

            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.AppStartDate = DateTime.Now;
            editObj.Job.AppEndDate = DateTime.Now;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobAddDoctorAppointment([Bind] Job job, bool IsChecked2, string userTypeId)

        {
            string orgAction = TempData["OrgAction"].ToString();
            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(orgAction, new { id = job.JobId});
                }

                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {


                    if (IsChecked2)
                    {
                        editjob.AppStartDate = job.AppStartDate.AddYears(543);
                        editjob.AppEndDate = job.AppEndDate.AddYears(543);
                        editjob.IsAppointed = true;
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 3).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.Remark1 = job.Remark1;
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(orgAction, new { id = job.JobId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(orgAction, new { id = job.JobId});
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobAppointment([Bind] Job job, bool IsChecked1, string userTypeId)

        {
            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(FormJobPost), new { id = job.JobId, userTypeId = userTypeId });
                }

                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    // Server is locale Thai  accept  year in BE
                    editjob.TotalPatients = job.TotalPatients;

                    if (IsChecked1)
                    {
                        editjob.PostDate = job.PostDate.AddYears(543);
                        editjob.IsPosted = true;
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 2).Select(x => x.JobStatusId).FirstOrDefault();
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobPost), new { id = job.JobId, userTypeId = userTypeId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobPost), new { id = job.JobId, userTypeId = userTypeId });
            }
        }


        //display hospital create edit form
        [HttpGet]
        public IActionResult FormAddDoctorDetail(string id)
        {


            //edit object
            Doctor editObj = new Doctor();
            editObj = _context.Doctor.Where(x => x.DoctorId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }

            string imageBase64Data = Convert.ToBase64String(editObj.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }


        [HttpGet]
        public IActionResult PatientDetailView(string id, string jobId)
        {

            ViewBag.jobId = jobId;
            

            //edit object
            Patient editObj = new Patient();
            editObj = _context.Patient.Where(x => x.PatientId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return PartialView("_PatientDetaillPartial", editObj);

        }



        public IActionResult SubmitDeleteDoctorFromJob(string id, string jobId)
        {

            string userTypeId = TempData["userTypeId"].ToString();
            string orgAction = TempData["OrgAction"].ToString();

            try
            {
                var del = _context.JobDoctor.Where(x => x.DoctorId.Equals(id) && x.JobId.Equals(jobId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }
                _context.JobDoctor.Remove(del);

                // Update Total Doctor
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(jobId)).FirstOrDefault();
                editjob.TotalDoctors = _context.JobDoctor.Where(x => x.JobId.Equals(jobId)).Count();
                _context.Update(editjob);

                _context.SaveChanges();



                TempData[StaticString.StatusMessage] = "ลบข้อมูลแพทย์ออกจากงาน เรียบร้อยแล้ว!";
                return RedirectToAction(orgAction, new { id = jobId, userTYpeId = userTypeId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(orgAction, new { id = jobId, userTYpeId = userTypeId });
            }
        }



        public IActionResult SubmitDeletePatientFromJob(string id,string jobId)
        {
            string userTypeId = TempData["userTypeId"].ToString();
            string orgAction = TempData["OrgAction"].ToString();
            try
            {
                var del = _context.JobPatient.Where(x => x.PatientId.Equals(id) && x.JobId.Equals(jobId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }
                _context.JobPatient.Remove(del);
                _context.SaveChanges();

                // Update Total Doctor
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(jobId)).FirstOrDefault();
                editjob.TotalPatients = _context.JobPatient.Where(x => x.JobId.Equals(jobId)).Count();
                _context.Update(editjob);

                _context.SaveChanges();



                TempData[StaticString.StatusMessage] = "ลบข้อมูลผู้ป่วยออกจากงาน เรียบร้อยแล้ว!";
                return RedirectToAction(orgAction, new { id = jobId, userTYpeId = userTypeId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(orgAction, new { id = jobId, userTYpeId = userTypeId });
            }
        }


        //=============================================================================================================================================================

        [HttpGet]
        public IActionResult FormJobNew(string id)
        {

            ViewBag.IsNew = false;
          
            ViewBag.jobId = id;
            ViewBag.Status = 0;

            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();

            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;

            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = DateTime.Now;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }

        [HttpGet]
        public IActionResult FormJobNewEdit(string id)
        {

            ViewBag.IsNew = false;
            
            ViewBag.jobId = id;
            ViewBag.Status = 0;

            //edit object
            //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();
            List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

            var patientReadyIn = _context.JobPatient
               .AsNoTracking()
               .Where(x => x.JobId.Equals(id)).ToList();

            var doctorReadyIn = _context.JobDoctor
              .AsNoTracking()
              .Where(x => x.JobId.Equals(id)).ToList();

            var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
            var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

            JobView editObj = new JobView();
            editObj.Job = _context.Job
                 .AsNoTracking()
                 .Include(x => x.JobStatus)
                 .Where(x => x.JobId.Equals(id)).FirstOrDefault();

            editObj.PatientList = _context.Patient
                               .AsNoTracking()
                               .Include(x => x.PrefixType)
                               .Include(x => x.DiseaseType)
                               .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
            editObj.DoctorList = _context.Doctor
                              .AsNoTracking()
                              .Include(x => x.PrefixType)
                              .Include(x => x.DoctorType)
                              .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

            ViewBag.Status = editObj.Job.JobStatus.Status;

            editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
            editObj.Job.PostDate = DateTime.Now;


            if (editObj.Job == null)
            {
                return NotFound();
            }


            //dropdownlist 
            FillDropdownListForhospitalForm();

            return View(editObj);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitFormJobNewEdit([Bind] Job job, bool IsChecked1, string userTypeId)

        {
            try
            {


                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(FormJobNewEdit), new { id = job.JobId, userTypeId = userTypeId });
                }

                ////edit existing
                Job editjob = new Job();
                editjob = _context.Job.Where(x => x.JobId.Equals(job.JobId)).FirstOrDefault();


                if (editjob != null)
                {
                    // Server is locale Thai  accept  year in BE
                    editjob.TotalPatients = job.TotalPatients;
                    editjob.Name = job.Name;
                    editjob.Description = job.Description;

                    if (IsChecked1)
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 2).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.PostDate = job.PostDate.AddYears(543);
                        editjob.IsPosted = true;
                    }
                    else
                    {
                        editjob.JobStatusId = _context.JobStatus.Where(x => x.Status == 1).Select(x => x.JobStatusId).FirstOrDefault();
                        editjob.PostDate = Convert.ToDateTime(DateTime.Today, new System.Globalization.CultureInfo("en-US")).AddYears(543);
                        editjob.IsPosted = false;
                    }


                    editjob.UpdatedBy = await _userManager.GetUserAsync(User);
                    editjob.UpdatedAtUtc = DateTime.UtcNow;

                    _context.Update(editjob);

                    _context.SaveChanges();


                    //dropdownlist 
                    FillDropdownListForhospitalForm();

                    TempData[StaticString.StatusMessage] = "ปรับปรุงข้อมูล" + modelName + "เรียบร้อยแล้ว.";
                    return RedirectToAction(nameof(FormJobNewEdit), new { id = job.JobId, userTypeId = userTypeId });
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(FormJobPostEdit), new { id = job.JobId, userTypeId = userTypeId });
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _context.Patient.Where(x => x.PatientId.Equals(id)).FirstOrDefault();
            return PartialView(model);
        }

        //display hospital create edit form
        [HttpGet]
        public IActionResult DoctorDetailPartial(string id)
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


            return PartialView("_DoctorDetailPartial", editObj);
        }



        ///*==================================================== Form Job Done Entry  ==============================================================*/
        ////display hospital create edit form
        //[HttpGet]
        //public IActionResult FormJobDoneView(string id, string cap)
        //{

        //    ViewBag.jobId = id;
        //    ViewBag.capture = cap;
        //    ViewBag.Status = 0;
        //    TempData["fromTab"] = 4;

        //    //edit object
        //    //ViewBag.Status = _context.Job.Where(x=>x.JobId.Equals(id)).Select(x=>x.JobStatus.Status).SingleOrDefault();

        //    List<PatientSelectViewModel> dt = new List<PatientSelectViewModel>();

        //    var patientReadyIn = _context.JobPatient
        //       .AsNoTracking()
        //       .Where(x => x.JobId.Equals(id)).ToList();

        //    var doctorReadyIn = _context.JobDoctor
        //      .AsNoTracking()
        //      .Where(x => x.JobId.Equals(id)).ToList();

        //    var patientIdReadyIn = patientReadyIn.Select(x => x.PatientId).ToArray();
        //    var doctorIdReadyIn = doctorReadyIn.Select(x => x.DoctorId).ToArray();

        //    JobView editObj = new JobView();
        //    editObj.Job = _context.Job
        //         .AsNoTracking()
        //         .Include(x => x.JobStatus)
        //         .Include(x => x.Hospital)
        //         .Where(x => x.JobId.Equals(id)).FirstOrDefault();
        //    editObj.PatientList = _context.Patient
        //                       .AsNoTracking()
        //                       .Include(x => x.PrefixType)
        //                       .Include(x => x.DiseaseType)
        //                       .Where(x => patientIdReadyIn.Contains(x.PatientId)).ToList();
        //    editObj.DoctorList = _context.Doctor
        //                      .AsNoTracking()
        //                      .Include(x => x.PrefixType)
        //                      .Include(x => x.DoctorType)
        //                      .Where(x => doctorIdReadyIn.Contains(x.DoctorId)).ToList();

        //    ViewBag.Status = editObj.Job.JobStatus.Status;
        //    editObj.Job.TransDate = Convert.ToDateTime(editObj.Job.TransDate, new System.Globalization.CultureInfo("en-US"));
        //    editObj.Job.AppStartDate = Convert.ToDateTime(editObj.Job.AppStartDate, new System.Globalization.CultureInfo("en-US"));
        //    editObj.Job.AppEndDate = Convert.ToDateTime(editObj.Job.AppEndDate, new System.Globalization.CultureInfo("en-US"));
        //    editObj.Job.PostDate = Convert.ToDateTime(editObj.Job.PostDate, new System.Globalization.CultureInfo("en-US"));
        //    editObj.Job.AppEntryDate = Convert.ToDateTime(editObj.Job.AppEntryDate, new System.Globalization.CultureInfo("en-US"));

        //    editObj.Job.StartDate = DateTime.Now;
        //    editObj.Job.EndDate = DateTime.Now;
        //    editObj.Job.JobEndEntryDate = DateTime.Today;


        //    if (editObj.Job == null)
        //    {
        //        return NotFound();
        //    }


        //    //dropdownlist 
        //    FillDropdownListForhospitalForm();

        //    return View(editObj);

        //}







    }
}