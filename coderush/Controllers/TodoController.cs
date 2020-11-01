using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.ViewModels;

namespace vds.Controllers
{
    [Authorize(Roles = Services.App.Pages.Todo.RoleName)]
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public TodoController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
             Services.App.ICommon app
            )
        {
            _context = context;
            _userManager = userManager;
            _app = app;
        }

        //fill viewdata as dropdownlist datasource for form
        private void FillDropdownListWithData()
        {
            ViewData["TodoType"] = _app.GetTodoTypeSelectList();
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();

        }

        //consume db context service, display all todo items
        public IActionResult Index(string period)
        {

            try
            {
                var todos = _context.Job
                    .Where(x => x.JobStatus.Status.Equals(3))
                    .Include(x => x.JobStatus)
                    .Include(x => x.Hospital)
                     .OrderBy(x => x.AppStartDate).ToList();


                var appData = _context.Job
                    .Where(x => x.JobStatus.Status.Equals(3))
                    .Include(x => x.JobStatus)
                    .Include(x => x.Hospital)
                    .Select(x => new
                    {
                        Id = x.JobId,
                        Subject = x.Name,
                        Location = x.Hospital.HospitalName,
                        StartTime = Convert.ToDateTime(x.AppStartDate, new System.Globalization.CultureInfo("en-US")),
                        EndTime = Convert.ToDateTime(x.AppEndDate, new System.Globalization.CultureInfo("en-US")),
                        CategoryColor = "#1aaa55"
                    }).ToList();

                ViewBag.appointments = appData;

                return View(todos);
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }


        }

        public IActionResult Index1()
        {

            return View();
        }

        //display todo create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            FillDropdownListWithData();

            //create new
            if (id == null)
            {


                Todo newTodo = new Todo();
                newTodo.StartDate = DateTime.Now;
                newTodo.EndDate = newTodo.StartDate.AddDays(3);
                return View(newTodo);
            }

            //edit todo
            Todo todo = new Todo();
            todo = _context.Todo.Where(x => x.TodoId.Equals(id)).FirstOrDefault();

            if (todo == null)
            {
                return NotFound();
            }



            return View(todo);

        }

        //post submitted todo data. if todo.TodoId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "TodoId",
            "TodoItem",
            "Description",
            "OnBehalfId",
            "IsDone",
            "TodoTypeId",
            "StartDate",
            "EndDate")]Todo todo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
                }

                //create new
                if (todo.TodoId == null)
                {
                    Todo newTodo = new Todo();
                    newTodo.TodoId = Guid.NewGuid().ToString();
                    newTodo.TodoItem = todo.TodoItem;
                    newTodo.Description = todo.Description;
                    newTodo.OnBehalfId = todo.OnBehalfId;
                    newTodo.TodoTypeId = todo.TodoTypeId;
                    newTodo.StartDate = todo.StartDate;
                    newTodo.EndDate = todo.EndDate;
                    newTodo.IsDone = todo.IsDone;
                    newTodo.CreatedBy = await _userManager.GetUserAsync(User);
                    newTodo.CreatedAtUtc = DateTime.UtcNow;
                    newTodo.UpdatedBy = newTodo.CreatedBy;
                    newTodo.UpdatedAtUtc = newTodo.CreatedAtUtc;

                    _context.Todo.Add(newTodo);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new todo item success.";
                    return RedirectToAction(nameof(Form), new { id = newTodo.TodoId ?? "" });
                }

                //edit existing
                Todo editTodo = new Todo();
                editTodo = _context.Todo.Where(x => x.TodoId.Equals(todo.TodoId)).FirstOrDefault();
                editTodo.TodoItem = todo.TodoItem;
                editTodo.Description = todo.Description;
                editTodo.OnBehalfId = todo.OnBehalfId;
                editTodo.TodoTypeId = todo.TodoTypeId;
                editTodo.StartDate = todo.StartDate;
                editTodo.EndDate = todo.EndDate;
                editTodo.IsDone = todo.IsDone;
                editTodo.UpdatedBy = await _userManager.GetUserAsync(User);
                editTodo.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editTodo);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing todo item success.";
                return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
            }
        }

        //display todo item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            FillDropdownListWithData();
            var todo = _context.Todo.Include(x => x.TodoType).Where(x => x.TodoId.Equals(id)).FirstOrDefault();
            return View(todo);
        }

        //delete submitted todo item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("TodoId")] Todo todo)
        {
            try
            {
                var deleteTodo = _context.Todo.Where(x => x.TodoId.Equals(todo.TodoId)).FirstOrDefault();
                if (deleteTodo == null)
                {
                    return NotFound();
                }

                _context.Todo.Remove(deleteTodo);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete todo item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = todo.TodoId ?? "" });
            }
        }

        //display of TodoType
        public IActionResult TodoTypeIndex()
        {
            var objs = _context.TodoType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display TodoType create edit form
        [HttpGet]
        public IActionResult TodoTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                TodoType newObj = new TodoType();
                return View(newObj);
            }

            //edit TodoType
            TodoType obj = new TodoType();
            obj = _context.TodoType.Where(x => x.TodoTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted TodoType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTodoTypeForm([Bind("TodoTypeId", "Name", "Description")] TodoType TodoType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
                }

                //create new
                if (TodoType.TodoTypeId == null)
                {
                    if (await _context.TodoType.AnyAsync(x => x.Name.Equals(TodoType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + TodoType.Name + " already exist";
                        return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
                    }

                    TodoType newObj = new TodoType();
                    newObj.TodoTypeId = Guid.NewGuid().ToString();
                    newObj.Name = TodoType.Name;
                    newObj.Description = TodoType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.TodoType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(TodoTypeForm), new { id = newObj.TodoTypeId ?? "" });
                }

                //edit existing
                TodoType editObj = new TodoType();
                TodoType existObj = new TodoType();
                editObj = await _context.TodoType.Where(x => x.TodoTypeId.Equals(TodoType.TodoTypeId)).FirstOrDefaultAsync();
                existObj = await _context.TodoType.Where(x => x.Name.Equals(TodoType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.TodoTypeId != existObj.TodoTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + TodoType.Name + " already exist";
                        return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
                    }

                }

                editObj.Name = TodoType.Name;
                editObj.Description = TodoType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> TodoTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.TodoType.Where(x => x.TodoTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTodoTypeDelete([Bind("TodoTypeId")] TodoType TodoType)
        {
            try
            {
                var deleteObj = await _context.TodoType.Where(x => x.TodoTypeId.Equals(TodoType.TodoTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //check existing todo
                Todo objCheck = new Todo();
                objCheck = await _context.Todo
                    .Where(x => x.TodoTypeId.Equals(deleteObj.TodoTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(TodoTypeDelete), new { id = TodoType.TodoTypeId ?? "" });
                }

                _context.TodoType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(TodoTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TodoTypeDelete), new { id = TodoType.TodoTypeId ?? "" });
            }
        }




        /*==================================================== FORM2 ==============================================================*/
        //display hospital create edit form
        [HttpGet]
        public IActionResult FormJobView(string id, string cap)
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
            if (editObj.Job.JobStatus.Status == 2)
            {
                editObj.Job.AppEntryDate = DateTime.Now;
            }

            if (editObj.Job == null)
            {
                return NotFound();
            }


            return View(editObj);

        }


        //display hospital create edit form
        [HttpGet]
        public IActionResult DoctorDetail(string id)
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

            FillDropdownListForhospitalForm();


            return View(editObj);

        }

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


            FillDropdownListForhospitalForm();


            return View(editObj);

        }







    }
}