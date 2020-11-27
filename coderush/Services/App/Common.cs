using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.ViewModels;

namespace vds.Services.App
{
    public class Common : ICommon
    {
        private readonly ApplicationDbContext _context;

        public Common(
            ApplicationDbContext context
            )
        {
            _context = context;
        }

        public async Task GenerateSalaryByEmployeeByPeriod(string employeeId, string period, bool isApproved, bool isPaid)
        {
            try
            {
                Employee employee = new Employee();
                employee = await _context.Employee.Where(x => x.EmployeeId.Equals(employeeId)).FirstOrDefaultAsync();
                if (employee != null)
                {
                    //check payroll exist or not, continue if not exist
                    Payroll payroll = new Payroll();
                    payroll = await _context.Payroll
                        .Where(x => x.Periode.ToString("yyyy-MM").Equals(period)
                            && x.OnBehalfId.Equals(employeeId)).FirstOrDefaultAsync();
                    if (payroll == null)
                    {
                        string[] periods = period.Split('-');
                        if (periods.Length == 2)
                        {
                            payroll = new Payroll();
                            //payroll.PayrollName = period + " " + employee.EmployeeIDNumber;
                            payroll.Description = "Payroll for " + employee.FirstName + " " + employee.LastName + " periode " + period;
                            payroll.Periode = new DateTime(Convert.ToInt32(periods[0]), Convert.ToInt32(periods[1]), 1);
                            payroll.OnBehalfId = employeeId;
                            payroll.IsApproved = isApproved;
                            payroll.IsPaid = isPaid;

                            await _context.Payroll.AddAsync(payroll);

                            //add line basic
                            PayrollLineBasic basic = new PayrollLineBasic();
                            basic.PayrollId = payroll.PayrollId;
                            basic.Description = "Basic salary " + employee.FirstName + " " + employee.LastName;

                            await _context.PayrollLineBasic.AddAsync(basic);




                            //add cash advance based on expense
                            foreach (var item in _context.Expense
                                .Where(x => x.OnBehalfId.Equals(employeeId)
                                    && x.FromDate.ToString("yyyy-MM").Equals(period)
                                    && x.IsCashAdvance.Equals(true))
                                .ToList())
                            {
                                PayrollLineCashAdvance ca = new PayrollLineCashAdvance();
                                ca.PayrollId = payroll.PayrollId;
                                ca.Description = item.Description;
                                ca.ExpenseTypeId = item.ExpenseTypeId;
                                ca.Amount = item.ExpenseAmount;
                                await _context.PayrollLineCashAdvance.AddAsync(ca);

                            }

                            //add reimburse based on expense
                            foreach (var item in _context.Expense
                                .Where(x => x.OnBehalfId.Equals(employeeId)
                                    && x.FromDate.ToString("yyyy-MM").Equals(period)
                                    && x.IsCashAdvance.Equals(false))
                                .ToList())
                            {
                                PayrollLineReimburse reimburse = new PayrollLineReimburse();
                                reimburse.PayrollId = payroll.PayrollId;
                                reimburse.Description = item.Description;
                                reimburse.ExpenseTypeId = item.ExpenseTypeId;
                                reimburse.Amount = item.ExpenseAmount;
                                await _context.PayrollLineReimburse.AddAsync(reimburse);
                            }

                            //add line unpaid leave for deduction
                            foreach (var item in _context.Leave
                                .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                                && x.OnBehalfId.Equals(employeeId)
                                && x.IsPaidLeave.Equals(false)).ToList())
                            {
                                PayrollLineUnpaidLeave unpaidLeave = new PayrollLineUnpaidLeave();
                                unpaidLeave.PayrollId = payroll.PayrollId;
                                unpaidLeave.Description = item.Description;
                                unpaidLeave.LeaveId = item.LeaveId;
                                unpaidLeave.Days = (item.ToDate.Date - item.FromDate.Date).Days + 1;
                                unpaidLeave.Amount = unpaidLeave.Days * unpaidLeave.UnpaidPerDay * -1m;
                                await _context.PayrollLineUnpaidLeave.AddAsync(unpaidLeave);
                            }

                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<SelectListItem> GetEmployeeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Employee.AsNoTracking()
                .OrderBy(x => x.EmployeeId)
                .Select(x => new SelectListItem
                {
                    Value = x.EmployeeId,
                    Text = x.FirstName + " " + x.LastName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetDesignationSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Designation.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.DesignationId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetDoctorTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.DoctorType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.DoctorTypeId,
                    Text = x.DoctorTypeName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetJobStatusSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.JobStatus.AsNoTracking()
                .OrderBy(x => x.Status)
                .Select(x => new SelectListItem
                {
                    Value = x.JobStatusId,
                    Text = x.Description


                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };

            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }


        public IEnumerable<SelectListItem> GetHospitalSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Hospital.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.HospitalId,
                    Text = x.HospitalName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }


        public IEnumerable<SelectListItem> GetUserTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.UserType.AsNoTracking()
                .Where(x => x.UserLevel != 0)
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.UserTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }





        public IEnumerable<SelectListItem> GetDoctorType()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.DoctorType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.DoctorTypeId,
                    Text = x.DoctorTypeName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetPrefixTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.PrefixType.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.PrefixTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }



        public IEnumerable<SelectListItem> GetDiseaseTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.DiseaseType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.DiseaseTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetDepartmentSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Department.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.DepartmentId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetSystemUserSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.ApplicationUser.AsNoTracking()
                .OrderBy(x => x.Email)
                .Select(x => new SelectListItem
                {
                    Value = x.Id,
                    Text = x.Email
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetGenderSelectList()
        {
            return new SelectList(GenderList.GetAll());
        }

        public IEnumerable<SelectListItem> GetMaritalStatusSelectList()
        {
            return new SelectList(MaritalStatusList.GetAll());
        }

        public IEnumerable<SelectListItem> GetTicketTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.TicketType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.TicketTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetTicketSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Ticket.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.TicketId,
                    Text = x.TicketName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetTicketSelectListByEmployeeId(string employeeId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Ticket.Where(x => x.OnBehalfId.Equals(employeeId)).AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.TicketId,
                    Text = x.TicketName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetAppraisalTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.AppraisalType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.AppraisalTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetAssetTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.AssetType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.AssetTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetInformationTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.InformationType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.InformationTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetAwardTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.AwardType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.AwardTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetLeaveTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.LeaveType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.LeaveTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetLeaveSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.Leave.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.LeaveId,
                    Text = x.LeaveName
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetExpenseTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.ExpenseType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.ExpenseTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetTodoTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.TodoType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.TodoTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetAllowanceTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.AllowanceType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.AllowanceTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetDeductionTypeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.DeductionType.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.DeductionTypeId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }


        public IEnumerable<SelectListItem> GetBenefitTemplateSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.BenefitTemplate.AsNoTracking()
                .OrderBy(x => x.CreatedAtUtc)
                .Select(x => new SelectListItem
                {
                    Value = x.BenefitTemplateId,
                    Text = x.Name
                }).ToList();
            SelectListItem blankOption = new SelectListItem()
            {
                Value = "",
                Text = ""
            };
            list.Insert(0, blankOption);
            return new SelectList(list, "Value", "Text");
        }

        public TodoSummary GetTodoSummaryByPeriod(string period)
        {
            try
            {
                TodoSummary result = new TodoSummary();

                decimal done = _context.Todo
                    .Where(x => x.StartDate.ToString("yyyy-MM").Equals(period)
                        && x.IsDone.Equals(true))
                    .Count();

                decimal notDone = _context.Todo
                    .Where(x => x.StartDate.ToString("yyyy-MM").Equals(period)
                        && x.IsDone.Equals(false))
                    .Count();

                decimal total = done + notDone;

                result.Done = done.ToString();
                result.DonePercentage = total != 0m ? ((done / total) * 100m).ToString("##") : "0";
                result.NotDone = notDone.ToString();
                result.NotDonePercentage = total != 0m ? ((notDone / total) * 100m).ToString("##") : "0";

                decimal oneDay = _context.Todo
                    .Where(x => x.StartDate.ToString("yyyy-MM").Equals(period)
                        && (x.EndDate.Date - x.StartDate.Date).Days + 1 == 1)
                    .Count();

                decimal moreThanOne = _context.Todo
                    .Where(x => x.StartDate.ToString("yyyy-MM").Equals(period)
                        && (x.EndDate.Date - x.StartDate.Date).Days + 1 > 1)
                    .Count();

                decimal total2 = oneDay + moreThanOne;

                result.OneDay = oneDay.ToString();
                result.OneDayPercentage = total2 != 0m ? ((oneDay / total2) * 100m).ToString("##") : "0";
                result.MoreThanOne = moreThanOne.ToString();
                result.MoreThanOnePercentage = total2 != 0m ? ((moreThanOne / total2) * 100m).ToString("##") : "0";

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public HospitalSummary GetHospitalSummary()
        {
            HospitalSummary hospitalsummary = new HospitalSummary();

            var hospitalsummary1 = _context.HospitalSummary.FromSqlRaw("dbo.HospitalSummary").ToList();

            foreach (var item in hospitalsummary1)
            {
                var Total = item.central + item.east + item.north + item.south + item.northeast + item.west;

                hospitalsummary.central = item.central;
                hospitalsummary.east = item.east;
                hospitalsummary.west = item.west;

                hospitalsummary.south = item.south;
                hospitalsummary.north = item.north;
                hospitalsummary.northeast = item.northeast;

                hospitalsummary.centralPct = item.centralPct;
                hospitalsummary.eastPct = item.eastPct;
                hospitalsummary.westPct = item.westPct;

                hospitalsummary.northPct = item.northPct;
                hospitalsummary.northeastPct = item.northeastPct;
                hospitalsummary.southPct = item.southPct;

            }


            return hospitalsummary;

        }



        public EmailSenderConfig GetEmailSenderConfig()
        {
            try
            {
                EmailSenderConfig result = new EmailSenderConfig();
                result = _context.EmailSenderConfig.FirstOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public EmployeeSummary GetEmployeeSummary()
        {
            try
            {
                EmployeeSummary result = new EmployeeSummary();

                List<Employee> employees = new List<Employee>();
                employees = _context.Employee.ToList();

                int male = 0;
                int female = 0;
                int mosLess = 0;
                int mosMore = 0;
                int totalMaleFemale = 0;
                int totalMosLessMosMore = 0;

                male = employees.Where(x => x.Gender.Equals("Male")).Count();
                female = employees.Where(x => x.Gender.Equals("Female")).Count();

                totalMaleFemale = male + female;
                totalMosLessMosMore = mosLess + mosMore;

                result.Male = male.ToString();
                result.Female = female.ToString();
                result.MalePercentage = totalMaleFemale == 0 ? "0" : (male * 100.0 / totalMaleFemale * 1.0).ToString("##");
                result.FemalePercentage = totalMaleFemale == 0 ? "0" : (female * 100.0 / totalMaleFemale * 1.0).ToString("##");
                result.MosLess = mosLess.ToString();
                result.MosMore = mosMore.ToString();
                result.MosLessPercentage = totalMosLessMosMore == 0 ? "0" : (mosLess * 100.0 / totalMosLessMosMore * 1.0).ToString("##");
                result.MosMorePercentage = totalMosLessMosMore == 0 ? "0" : (mosMore * 100.0 / totalMosLessMosMore * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public PayrollSummary GetPayrollSummaryByPeriod(string period)
        {
            try
            {
                PayrollSummary result = new PayrollSummary();

                decimal totalSalary = _context.Payroll
                    .Include(x => x.LinesBasic)
                    .Include(x => x.LinesAllowance)
                    .Include(x => x.LinesDeduction)
                    .Include(x => x.LinesUnpaidLeave)
                    .Where(x => x.Periode.ToString("yyyy-MM").Equals(period))
                    .Sum(x => (x.LinesBasic.Count > 0 ? x.LinesBasic.Sum(y => y.Amount) : 0m)
                            + (x.LinesAllowance.Count > 0 ? x.LinesAllowance.Sum(y => y.Amount) : 0m)
                            + (x.LinesDeduction.Count > 0 ? x.LinesDeduction.Sum(y => y.Amount) : 0m)
                            + (x.LinesUnpaidLeave.Count > 0 ? x.LinesUnpaidLeave.Sum(y => y.Amount) : 0m));

                decimal paidSalary = _context.Payroll
                    .Include(x => x.LinesBasic)
                    .Include(x => x.LinesAllowance)
                    .Include(x => x.LinesDeduction)
                    .Include(x => x.LinesUnpaidLeave)
                    .Where(x => x.Periode.ToString("yyyy-MM").Equals(period) && x.IsPaid.Equals(true))
                    .Sum(x => (x.LinesBasic.Count > 0 ? x.LinesBasic.Sum(y => y.Amount) : 0m)
                            + (x.LinesAllowance.Count > 0 ? x.LinesAllowance.Sum(y => y.Amount) : 0m)
                            + (x.LinesDeduction.Count > 0 ? x.LinesDeduction.Sum(y => y.Amount) : 0m)
                            + (x.LinesUnpaidLeave.Count > 0 ? x.LinesUnpaidLeave.Sum(y => y.Amount) : 0m));


                decimal totalExpense = _context.Expense
                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period))
                    .Sum(x => x.ExpenseAmount);

                decimal paidExpense = _context.Payroll
                    .Include(x => x.LinesCashAdvance)
                    .Include(x => x.LinesReimburse)
                    .Where(x => x.Periode.ToString("yyyy-MM").Equals(period))
                    .Sum(x =>
                              (x.LinesCashAdvance.Count > 0 ? x.LinesCashAdvance.Sum(y => y.Amount) : 0m)
                            + (x.LinesReimburse.Count > 0 ? x.LinesReimburse.Sum(y => y.Amount) : 0m));

                result.TotalSalary = totalSalary != 0m ? totalSalary.ToString("##,##") : "0";
                result.PaidSalary = paidSalary != 0m ? paidSalary.ToString("##,##") : "0";
                result.TotalExpense = totalExpense != 0m ? totalExpense.ToString("##,##") : "0";
                result.PaidExpense = paidExpense != 0m ? paidExpense.ToString("##,##") : "0";

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ExpenseSummary GetExpenseSummaryByPeriod(string period)
        {
            try
            {
                ExpenseSummary result = new ExpenseSummary();

                List<Expense> expenses = new List<Expense>();
                expenses = _context.Expense
                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int approved = 0;
                int notApproved = 0;
                int cashAdvance = 0;
                int Reimbursement = 0;
                int totalApprovedNotApproved = 0;
                int totalExpense = 0;

                approved = expenses.Where(x => x.IsApproved.Equals(true)).Count();
                notApproved = expenses.Where(x => x.IsApproved.Equals(false)).Count();
                cashAdvance = expenses.Where(x => x.IsCashAdvance.Equals(true)).Count();
                Reimbursement = expenses.Where(x => x.IsCashAdvance.Equals(false)).Count();

                totalApprovedNotApproved = approved + notApproved;
                totalExpense = cashAdvance + Reimbursement;

                result.Approved = approved.ToString();
                result.NotApproved = notApproved.ToString();
                result.ApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (approved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.NotApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (notApproved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.CashAdvance = cashAdvance.ToString();
                result.Reimbursement = Reimbursement.ToString();
                result.CashAdvancePercentage = totalExpense == 0 ? "0" : (cashAdvance * 100.0 / totalExpense * 1.0).ToString("##");
                result.ReimbursementPercentage = totalExpense == 0 ? "0" : (Reimbursement * 100.0 / totalExpense * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PayrollSummary GetPayrollSummaryByPeriodByEmployeeId(string employeeId, string period)
        {
            try
            {
                PayrollSummary result = new PayrollSummary();

                decimal totalSalary = _context.Payroll
                    .Include(x => x.LinesBasic)
                    .Include(x => x.LinesAllowance)
                    .Include(x => x.LinesDeduction)
                    .Include(x => x.LinesUnpaidLeave)
                    .Where(x => x.Periode.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(employeeId))
                    .Sum(x => (x.LinesBasic.Count > 0 ? x.LinesBasic.Sum(y => y.Amount) : 0m)
                            + (x.LinesAllowance.Count > 0 ? x.LinesAllowance.Sum(y => y.Amount) : 0m)
                            + (x.LinesDeduction.Count > 0 ? x.LinesDeduction.Sum(y => y.Amount) : 0m)
                            + (x.LinesUnpaidLeave.Count > 0 ? x.LinesUnpaidLeave.Sum(y => y.Amount) : 0m));

                decimal paidSalary = _context.Payroll
                    .Include(x => x.LinesBasic)
                    .Include(x => x.LinesAllowance)
                    .Include(x => x.LinesDeduction)
                    .Include(x => x.LinesUnpaidLeave)
                    .Where(x => x.Periode.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(employeeId) && x.IsPaid.Equals(true))
                    .Sum(x => (x.LinesBasic.Count > 0 ? x.LinesBasic.Sum(y => y.Amount) : 0m)
                            + (x.LinesAllowance.Count > 0 ? x.LinesAllowance.Sum(y => y.Amount) : 0m)
                            + (x.LinesDeduction.Count > 0 ? x.LinesDeduction.Sum(y => y.Amount) : 0m)
                            + (x.LinesUnpaidLeave.Count > 0 ? x.LinesUnpaidLeave.Sum(y => y.Amount) : 0m));


                decimal totalExpense = _context.Expense
                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(employeeId))
                    .Sum(x => x.ExpenseAmount);

                decimal paidExpense = _context.Payroll
                    .Include(x => x.LinesCashAdvance)
                    .Include(x => x.LinesReimburse)
                    .Where(x => x.Periode.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(employeeId))
                    .Sum(x =>
                              (x.LinesCashAdvance.Count > 0 ? x.LinesCashAdvance.Sum(y => y.Amount) : 0m)
                            + (x.LinesReimburse.Count > 0 ? x.LinesReimburse.Sum(y => y.Amount) : 0m));

                result.TotalSalary = totalSalary != 0m ? totalSalary.ToString("##,##") : "0";
                result.PaidSalary = paidSalary != 0m ? paidSalary.ToString("##,##") : "0";
                result.TotalExpense = totalExpense != 0m ? totalExpense.ToString("##,##") : "0";
                result.PaidExpense = paidExpense != 0m ? paidExpense.ToString("##,##") : "0";

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetPayrollDoughnutByPeriodByEmployeeId(string employeeId, string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<string> list = new List<string>();
                list.Add("Salary");
                list.Add("Paid");
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in list)
                {
                    labels.Add(item);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    if (item.Equals("Salary"))
                    {
                        decimal totalSalary = _context.Payroll
                                                .Include(x => x.LinesBasic)
                                                .Include(x => x.LinesAllowance)
                                                .Include(x => x.LinesDeduction)
                                                .Include(x => x.LinesUnpaidLeave)
                                                .Where(x => x.Periode.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(employeeId))
                                                .Sum(x => (x.LinesBasic.Count > 0 ? x.LinesBasic.Sum(y => y.Amount) : 0m)
                                                        + (x.LinesAllowance.Count > 0 ? x.LinesAllowance.Sum(y => y.Amount) : 0m)
                                                        + (x.LinesDeduction.Count > 0 ? x.LinesDeduction.Sum(y => y.Amount) : 0m)
                                                        + (x.LinesUnpaidLeave.Count > 0 ? x.LinesUnpaidLeave.Sum(y => y.Amount) : 0m));

                        datas.Add((int)totalSalary);

                    }

                    if (item.Equals("Paid"))
                    {
                        decimal paidSalary = _context.Payroll
                                                .Include(x => x.LinesBasic)
                                                .Include(x => x.LinesAllowance)
                                                .Include(x => x.LinesDeduction)
                                                .Include(x => x.LinesUnpaidLeave)
                                                .Where(x => x.Periode.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(employeeId) && x.IsPaid.Equals(true))
                                                .Sum(x => (x.LinesBasic.Count > 0 ? x.LinesBasic.Sum(y => y.Amount) : 0m)
                                                        + (x.LinesAllowance.Count > 0 ? x.LinesAllowance.Sum(y => y.Amount) : 0m)
                                                        + (x.LinesDeduction.Count > 0 ? x.LinesDeduction.Sum(y => y.Amount) : 0m)
                                                        + (x.LinesUnpaidLeave.Count > 0 ? x.LinesUnpaidLeave.Sum(y => y.Amount) : 0m));

                        datas.Add((int)paidSalary);
                    }

                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttendanceSummary GetAttendanceSummaryByPeriod(string period)
        {
            try
            {
                AttendanceSummary result = new AttendanceSummary();

                List<PublicHolidayLine> holidays = new List<PublicHolidayLine>();
                holidays = _context.PublicHolidayLine
                    .Where(x => x.PublicHolidayDate.ToString("yyyy-MM").Equals(period))
                    .OrderBy(x => x.PublicHolidayDate)
                    .ToList();

                List<DateTime> dateHolidays = new List<DateTime>();


                foreach (var item in holidays)
                {
                    dateHolidays.Add(item.PublicHolidayDate.Date);
                }


                int presence = 0;
                int absence = 0;
                int total = 0;

                foreach (var item in _context.Employee)
                {
                    string[] periods = period.Split('-');

                    //bad period format
                    if (period.Length < 2)
                    {
                        return result;
                    }

                    int year = Convert.ToInt32(periods[0]);
                    int month = Convert.ToInt32(periods[1]);

                    DateTime startDate = new DateTime(year, month, 1);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);


                    List<DateTime> attendances = new List<DateTime>();
                    attendances = _context.Attendance
                        .Where(x => x.Clock.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(item.EmployeeId))
                        .GroupBy(x => x.Clock.Date)
                        .Where(x => x.Count() >= 2)
                        .Select(x => x.Key).ToList();


                    for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    {
                        if (
                            date.DayOfWeek != DayOfWeek.Saturday
                            && date.DayOfWeek != DayOfWeek.Sunday
                            )
                        {
                            //not holidays
                            if (!dateHolidays.Contains(date))
                            {
                                //presence
                                if (attendances.Contains(date.Date))
                                {
                                    presence = presence + 1;
                                }
                                else
                                {
                                    //absence
                                    absence = absence + 1;
                                }
                            }

                        }
                    }


                }



                int paid = 0;
                int unpaid = 0;
                int totalPaidUnpaid = 0;

                foreach (var item in _context.Employee)
                {
                    List<Leave> leaves = new List<Leave>();
                    leaves = _context.Leave
                        .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period) && x.OnBehalfId.Equals(item.EmployeeId))
                        .ToList();

                    paid = paid + leaves.Where(x => x.IsPaidLeave.Equals(true)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                    unpaid = unpaid + leaves.Where(x => x.IsPaidLeave.Equals(false)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);

                }


                totalPaidUnpaid = paid + unpaid;
                total = presence + absence;

                result.Presence = presence.ToString();
                result.PresencePercentage = total != 0 ? (presence * 100.0 / total * 1.0).ToString("##") : "0";
                result.Absence = absence.ToString();
                result.AbsencePercentage = total != 0 ? (absence * 100.0 / total * 1.0).ToString("##") : "0";
                result.PaidLeave = paid.ToString();
                result.PaidLeavePercentage = totalPaidUnpaid != 0 ? (paid * 100.0 / totalPaidUnpaid * 1.0).ToString("##") : "0";
                result.UnpaidLeave = unpaid.ToString();
                result.UnpaidLeavePercentage = totalPaidUnpaid != 0 ? (unpaid * 100.0 / totalPaidUnpaid * 1.0).ToString("##") : "0";


                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public SelfServiceAttendanceSummary GetAttendanceSummaryByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                SelfServiceAttendanceSummary result = new SelfServiceAttendanceSummary();

                List<DateTime> attendances = new List<DateTime>();
                attendances = _context.Attendance
                    .Where(x => x.OnBehalfId.Equals(employeeId)
                    && x.Clock.ToString("yyyy-MM").Equals(period))
                    .GroupBy(x => x.Clock.Date)
                    .Where(x => x.Count() >= 2)
                    .Select(x => x.Key).ToList();

                List<PublicHolidayLine> holidays = new List<PublicHolidayLine>();
                holidays = _context.PublicHolidayLine
                    .Where(x => x.PublicHolidayDate.ToString("yyyy-MM").Equals(period))
                    .OrderBy(x => x.PublicHolidayDate)
                    .ToList();

                List<DateTime> dateHolidays = new List<DateTime>();


                foreach (var item in holidays)
                {
                    dateHolidays.Add(item.PublicHolidayDate.Date);
                }

                string[] periods = period.Split('-');

                //bad period format
                if (period.Length < 2)
                {
                    return result;
                }

                int year = Convert.ToInt32(periods[0]);
                int month = Convert.ToInt32(periods[1]);

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                int presence = 0;
                int absence = 0;
                int total = 0;

                for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    if (
                        date.DayOfWeek != DayOfWeek.Saturday
                        && date.DayOfWeek != DayOfWeek.Sunday
                        )
                    {
                        //not holidays
                        if (!dateHolidays.Contains(date))
                        {
                            //presence
                            if (attendances.Contains(date.Date))
                            {
                                presence = presence + 1;
                            }
                            else
                            {
                                //absence
                                absence = absence + 1;
                            }
                        }

                    }
                }

                DateTime today = DateTime.Now;

                List<Attendance> todayAttendances = new List<Attendance>();
                todayAttendances = _context.Attendance
                    .Where(x => x.OnBehalfId.Equals(employeeId)
                    && x.Clock.Date.Equals(today.Date)).ToList();


                total = presence + absence;

                result.Presence = presence.ToString();
                result.PresencePercentage = total != 0 ? (presence * 100.0 / total * 1.0).ToString("##") : "0";
                result.Absence = absence.ToString();
                result.AbsencePercentage = total != 0 ? (absence * 100.0 / total * 1.0).ToString("##") : "0";
                result.ClockInTime = todayAttendances.Count > 0 ? todayAttendances.Min(x => x.Clock).ToString("HH:mm") : "-:-";
                result.ClockInDate = today.ToString("yyyy-MM-dd");
                result.ClockOutTime = todayAttendances.Count > 0 ? todayAttendances.Max(x => x.Clock).ToString("HH:mm") : "-:-";
                result.ClockOutDate = today.ToString("yyyy-MM-dd");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TicketSummary GetTicketSummaryByPeriod(string period)
        {
            try
            {
                TicketSummary result = new TicketSummary();

                List<Ticket> tickets = new List<Ticket>();
                tickets = _context.Ticket
                    .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int solve = 0;
                int notSolve = 0;
                int recurring = 0;
                int notRecurring = 0;
                int totalSolveNotSolve = 0;
                int totalRecurringNotRecurring = 0;

                solve = tickets.Where(x => x.IsSolve.Equals(true)).Count();
                notSolve = tickets.Where(x => x.IsSolve.Equals(false)).Count();
                recurring = tickets.Where(x => !String.IsNullOrEmpty(x.ParentTicketThreadId)).Count();
                notRecurring = tickets.Where(x => String.IsNullOrEmpty(x.ParentTicketThreadId)).Count();

                totalSolveNotSolve = solve + notSolve;
                totalRecurringNotRecurring = recurring + notRecurring;

                result.Solve = solve.ToString();
                result.NotSolve = notSolve.ToString();
                result.SolvePercentage = totalSolveNotSolve == 0 ? "0" : (solve * 100.0 / totalSolveNotSolve * 1.0).ToString("##");
                result.NotSolvePercentage = totalSolveNotSolve == 0 ? "0" : (notSolve * 100.0 / totalSolveNotSolve * 1.0).ToString("##");
                result.Recurring = recurring.ToString();
                result.NotRecurring = notRecurring.ToString();
                result.RecurringPercentage = totalRecurringNotRecurring == 0 ? "0" : (recurring * 100.0 / totalRecurringNotRecurring * 1.0).ToString("##");
                result.NotRecurringPercentage = totalRecurringNotRecurring == 0 ? "0" : (notRecurring * 100.0 / totalRecurringNotRecurring * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public LeaveSummary GetLeaveSummaryByPeriod(string period)
        {
            try
            {
                LeaveSummary result = new LeaveSummary();

                List<Leave> leaves = new List<Leave>();
                leaves = _context.Leave
                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int approved = 0;
                int notApproved = 0;
                int paid = 0;
                int notPaid = 0;
                int totalApprovedNotApproved = 0;
                int totalPaidNotPaid = 0;

                approved = leaves.Where(x => x.IsApproved.Equals(true)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                notApproved = leaves.Where(x => x.IsApproved.Equals(false)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                paid = leaves.Where(x => x.IsPaidLeave.Equals(true)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                notPaid = leaves.Where(x => x.IsPaidLeave.Equals(false)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);

                totalApprovedNotApproved = approved + notApproved;
                totalPaidNotPaid = paid + notPaid;

                result.Approved = approved.ToString();
                result.NotApproved = notApproved.ToString();
                result.ApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (approved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.NotApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (notApproved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.PaidLeave = paid.ToString();
                result.NotPaidLeave = notPaid.ToString();
                result.PaidLeavePercentage = totalPaidNotPaid == 0 ? "0" : (paid * 100.0 / totalPaidNotPaid * 1.0).ToString("##");
                result.NotPaidLeavePercentage = totalPaidNotPaid == 0 ? "0" : (notPaid * 100.0 / totalPaidNotPaid * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public InformationSummary GetInformationSummaryByPeriod(string period)
        {
            try
            {
                InformationSummary result = new InformationSummary();

                List<Information> informations = new List<Information>();
                informations = _context.Information
                    .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int active = 0;
                int notactive = 0;
                int usinglink = 0;
                int notusinglink = 0;
                int totalActiveNotActive = 0;
                int totalUsingLinkNotUsingLink = 0;

                active = informations.Where(x => x.IsActive.Equals(true)).Count();
                notactive = informations.Where(x => x.IsActive.Equals(false)).Count();
                usinglink = informations.Where(x => !String.IsNullOrEmpty(x.ExternalLink)).Count();
                notusinglink = informations.Where(x => String.IsNullOrEmpty(x.ExternalLink)).Count();

                totalActiveNotActive = active + notactive;
                totalUsingLinkNotUsingLink = usinglink + notusinglink;

                result.Active = active.ToString();
                result.NotActive = notactive.ToString();
                result.ActivePercentage = totalActiveNotActive == 0 ? "0" : (active * 100.0 / totalActiveNotActive * 1.0).ToString("##");
                result.NotActivePercentage = totalActiveNotActive == 0 ? "0" : (notactive * 100.0 / totalActiveNotActive * 1.0).ToString("##");
                result.UsingLink = usinglink.ToString();
                result.NotUsingLink = notusinglink.ToString();
                result.UsingLinkPercentage = totalUsingLinkNotUsingLink == 0 ? "0" : (usinglink * 100.0 / totalUsingLinkNotUsingLink * 1.0).ToString("##");
                result.NotUsingLinkPercentage = totalUsingLinkNotUsingLink == 0 ? "0" : (notusinglink * 100.0 / totalUsingLinkNotUsingLink * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AssetSummary GetAssetSummaryByPeriod(string period)
        {
            try
            {
                AssetSummary result = new AssetSummary();

                List<Asset> assets = new List<Asset>();
                assets = _context.Asset
                    .Where(x => x.PurchaseDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int used = 0;
                int notused = 0;
                decimal usedvalue = 0m;
                decimal notusedvalue = 0m;
                int totalUsedNotUsed = 0;
                decimal totalUsedNotUsedValue = 0m;

                used = assets.Where(x => !String.IsNullOrEmpty(x.UsedById)).Count();
                notused = assets.Where(x => String.IsNullOrEmpty(x.UsedById)).Count();
                usedvalue = assets.Where(x => !String.IsNullOrEmpty(x.UsedById)).Sum(x => x.PurchasePrice);
                notusedvalue = assets.Where(x => String.IsNullOrEmpty(x.UsedById)).Sum(x => x.PurchasePrice);

                totalUsedNotUsed = used + notused;
                totalUsedNotUsedValue = usedvalue + notusedvalue;

                result.Used = used.ToString();
                result.NotUsed = notused.ToString();
                result.UsedPercentage = totalUsedNotUsed == 0 ? "0" : (used * 100.0 / totalUsedNotUsed * 1.0).ToString("##");
                result.NotUsedPercentage = totalUsedNotUsed == 0 ? "0" : (notused * 100.0 / totalUsedNotUsed * 1.0).ToString("##");
                result.UsedValue = usedvalue.ToString("##");
                result.NotUsedValue = notusedvalue.ToString("##");
                result.UsedValuePercentage = totalUsedNotUsedValue == 0m ? "0" : (usedvalue * 100m / totalUsedNotUsedValue).ToString("##");
                result.NotUsedValuePercentage = totalUsedNotUsedValue == 0m ? "0" : (notusedvalue * 100m / totalUsedNotUsedValue).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AppraisalSummary GetAppraisalSummaryByPeriod(string period)
        {
            try
            {
                AppraisalSummary result = new AppraisalSummary();

                List<Appraisal> appraisals = new List<Appraisal>();
                appraisals = _context.Appraisal
                    .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int approved = 0;
                int notApproved = 0;
                int male = 0;
                int female = 0;
                int totalApproveNotApprove = 0;
                int totalMaleFemale = 0;

                approved = appraisals.Where(x => x.IsApproved.Equals(true)).Count();
                notApproved = appraisals.Where(x => x.IsApproved.Equals(false)).Count();
                male = appraisals.Where(x => x.OnBehalf.Gender.Equals("Male")).Count();
                female = appraisals.Where(x => x.OnBehalf.Gender.Equals("Female")).Count();

                totalApproveNotApprove = approved + notApproved;
                totalMaleFemale = male + female;

                result.Approved = approved.ToString();
                result.NotApproved = notApproved.ToString();
                result.ApprovedPercentage = totalApproveNotApprove == 0 ? "0" : (approved * 100.0 / totalApproveNotApprove * 1.0).ToString("##");
                result.NotApprovedPercentage = totalApproveNotApprove == 0 ? "0" : (notApproved * 100.0 / totalApproveNotApprove * 1.0).ToString("##");
                result.Male = male.ToString();
                result.Female = female.ToString();
                result.MalePercentage = totalMaleFemale == 0 ? "0" : (male * 100.0 / totalMaleFemale * 1.0).ToString("##");
                result.FemalePercentage = totalMaleFemale == 0 ? "0" : (female * 100.0 / totalMaleFemale * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AwardSummary GetAwardSummaryByPeriod(string period)
        {
            try
            {
                AwardSummary result = new AwardSummary();

                List<Award> awards = new List<Award>();
                awards = _context.Award
                    .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                int approved = 0;
                int notApproved = 0;
                int male = 0;
                int female = 0;
                int totalApproveNotApprove = 0;
                int totalMaleFemale = 0;

                approved = awards.Where(x => x.IsApproved.Equals(true)).Count();
                notApproved = awards.Where(x => x.IsApproved.Equals(false)).Count();
                male = awards.Where(x => x.AwardRecipient.Gender.Equals("Male")).Count();
                female = awards.Where(x => x.AwardRecipient.Gender.Equals("Female")).Count();

                totalApproveNotApprove = approved + notApproved;
                totalMaleFemale = male + female;

                result.Approved = approved.ToString();
                result.NotApproved = notApproved.ToString();
                result.ApprovedPercentage = totalApproveNotApprove == 0 ? "0" : (approved * 100.0 / totalApproveNotApprove * 1.0).ToString("##");
                result.NotApprovedPercentage = totalApproveNotApprove == 0 ? "0" : (notApproved * 100.0 / totalApproveNotApprove * 1.0).ToString("##");
                result.Male = male.ToString();
                result.Female = female.ToString();
                result.MalePercentage = totalMaleFemale == 0 ? "0" : (male * 100.0 / totalMaleFemale * 1.0).ToString("##");
                result.FemalePercentage = totalMaleFemale == 0 ? "0" : (female * 100.0 / totalMaleFemale * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public SelfServiceLeaveSummary GetLeaveSummaryByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                SelfServiceLeaveSummary result = new SelfServiceLeaveSummary();

                List<Leave> leaves = new List<Leave>();
                leaves = _context.Leave
                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                    && x.OnBehalfId.Equals(employeeId))
                    .ToList();

                int approved = 0;
                int notApproved = 0;
                int paid = 0;
                int notPaid = 0;
                int totalApprovedNotApproved = 0;
                int totalPaidNotPaid = 0;

                approved = leaves.Where(x => x.IsApproved.Equals(true)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                notApproved = leaves.Where(x => x.IsApproved.Equals(false)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                paid = leaves.Where(x => x.IsPaidLeave.Equals(true)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);
                notPaid = leaves.Where(x => x.IsPaidLeave.Equals(false)).Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1);

                totalApprovedNotApproved = approved + notApproved;
                totalPaidNotPaid = paid + notPaid;

                result.Approved = approved.ToString();
                result.NotApproved = notApproved.ToString();
                result.ApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (approved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.NotApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (notApproved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.PaidLeave = paid.ToString();
                result.NotPaidLeave = notPaid.ToString();
                result.PaidLeavePercentage = totalPaidNotPaid == 0 ? "0" : (paid * 100.0 / totalPaidNotPaid * 1.0).ToString("##");
                result.NotPaidLeavePercentage = totalPaidNotPaid == 0 ? "0" : (notPaid * 100.0 / totalPaidNotPaid * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SelfServiceExpenseSummary GetExpenseSummaryByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                SelfServiceExpenseSummary result = new SelfServiceExpenseSummary();

                List<Expense> expenses = new List<Expense>();
                expenses = _context.Expense
                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                    && x.OnBehalfId.Equals(employeeId))
                    .ToList();

                int approved = 0;
                int notApproved = 0;
                int cashAdvance = 0;
                int Reimbursement = 0;
                int totalApprovedNotApproved = 0;
                int totalExpense = 0;

                approved = expenses.Where(x => x.IsApproved.Equals(true)).Count();
                notApproved = expenses.Where(x => x.IsApproved.Equals(false)).Count();
                cashAdvance = expenses.Where(x => x.IsCashAdvance.Equals(true)).Count();
                Reimbursement = expenses.Where(x => x.IsCashAdvance.Equals(false)).Count();

                totalApprovedNotApproved = approved + notApproved;
                totalExpense = cashAdvance + Reimbursement;

                result.Approved = approved.ToString();
                result.NotApproved = notApproved.ToString();
                result.ApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (approved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.NotApprovedPercentage = totalApprovedNotApproved == 0 ? "0" : (notApproved * 100.0 / totalApprovedNotApproved * 1.0).ToString("##");
                result.CashAdvance = cashAdvance.ToString();
                result.Reimbursement = Reimbursement.ToString();
                result.CashAdvancePercentage = totalExpense == 0 ? "0" : (cashAdvance * 100.0 / totalExpense * 1.0).ToString("##");
                result.ReimbursementPercentage = totalExpense == 0 ? "0" : (Reimbursement * 100.0 / totalExpense * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SelfServiceTicketSummary GetTicketSummaryByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                SelfServiceTicketSummary result = new SelfServiceTicketSummary();

                List<Ticket> tickets = new List<Ticket>();
                tickets = _context.Ticket
                    .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period)
                    && x.OnBehalfId.Equals(employeeId))
                    .ToList();

                int solve = 0;
                int notSolve = 0;
                int recurring = 0;
                int notRecurring = 0;
                int totalSolveNotSolve = 0;
                int totalRecurringNotRecurring = 0;

                solve = tickets.Where(x => x.IsSolve.Equals(true)).Count();
                notSolve = tickets.Where(x => x.IsSolve.Equals(false)).Count();
                recurring = tickets.Where(x => !String.IsNullOrEmpty(x.ParentTicketThreadId)).Count();
                notRecurring = tickets.Where(x => String.IsNullOrEmpty(x.ParentTicketThreadId)).Count();

                totalSolveNotSolve = solve + notSolve;
                totalRecurringNotRecurring = recurring + notRecurring;

                result.Solve = solve.ToString();
                result.NotSolve = notSolve.ToString();
                result.SolvePercentage = totalSolveNotSolve == 0 ? "0" : (solve * 100.0 / totalSolveNotSolve * 1.0).ToString("##");
                result.NotSolvePercentage = totalSolveNotSolve == 0 ? "0" : (notSolve * 100.0 / totalSolveNotSolve * 1.0).ToString("##");
                result.Recurring = recurring.ToString();
                result.NotRecurring = notRecurring.ToString();
                result.RecurringPercentage = totalRecurringNotRecurring == 0 ? "0" : (recurring * 100.0 / totalRecurringNotRecurring * 1.0).ToString("##");
                result.NotRecurringPercentage = totalRecurringNotRecurring == 0 ? "0" : (notRecurring * 100.0 / totalRecurringNotRecurring * 1.0).ToString("##");

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ChartDoughnut GetTodoDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.TodoType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Todo
                        .Where(x => x.StartDate.ToString("yyyy-MM").Equals(period)
                            && x.TodoTypeId.Equals(item.TodoTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetLeaveDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.LeaveType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Leave
                        .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                            && x.LeaveTypeId.Equals(item.LeaveTypeId))
                        .ToList()
                        .Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1));
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetAwardDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.AwardType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Award
                        .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period)
                            && x.AwardTypeId.Equals(item.AwardTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetInformationDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.InformationType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Information
                        .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period)
                            && x.InformationTypeId.Equals(item.InformationTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetAssetDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.AssetType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Asset
                        .Where(x => x.PurchaseDate.ToString("yyyy-MM").Equals(period)
                            && x.AssetTypeId.Equals(item.AssetTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetExpenseDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.ExpenseType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Expense
                        .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                            && x.ExpenseTypeId.Equals(item.ExpenseTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetExenseDoughnutByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.ExpenseType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Expense
                        .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                            && x.ExpenseTypeId.Equals(item.ExpenseTypeId)
                            && x.OnBehalfId.Equals(employeeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetTicketDoughnutByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.TicketType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Ticket
                        .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period)
                            && x.TicketTypeId.Equals(item.TicketTypeId)
                            && x.OnBehalfId.Equals(employeeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetAppraisalDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.AppraisalType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Appraisal
                        .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period)
                            && x.AppraisalTypeId.Equals(item.AppraisalTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetTicketDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.TicketType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Ticket
                        .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period)
                            && x.TicketTypeId.Equals(item.TicketTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetAttendanceDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                List<PublicHolidayLine> holidays = new List<PublicHolidayLine>();
                holidays = _context.PublicHolidayLine
                    .Where(x => x.PublicHolidayDate.ToString("yyyy-MM").Equals(period))
                    .OrderBy(x => x.PublicHolidayDate)
                    .ToList();

                List<DateTime> dateHolidays = new List<DateTime>();


                foreach (var itm in holidays)
                {
                    dateHolidays.Add(itm.PublicHolidayDate.Date);
                }

                //bad period format
                if (period.Length < 2)
                {
                    return result;
                }

                string[] periods = period.Split('-');
                int year = Convert.ToInt32(periods[0]);
                int month = Convert.ToInt32(periods[1]);

                foreach (var item in _context.Department.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;


                    int absence = 0;

                    foreach (var itm in _context.Employee.Where(x => x.DepartmentId.Equals(item.DepartmentId)))
                    {


                        DateTime startDate = new DateTime(year, month, 1);
                        DateTime endDate = startDate.AddMonths(1).AddDays(-1);


                        List<DateTime> attendances = new List<DateTime>();
                        attendances = _context.Attendance
                            .Where(x => x.Clock.ToString("yyyy-MM").Equals(period)
                            && x.OnBehalfId.Equals(itm.EmployeeId))
                            .GroupBy(x => x.Clock.Date)
                            .Where(x => x.Count() >= 2)
                            .Select(x => x.Key).ToList();


                        for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                        {
                            if (
                                date.DayOfWeek != DayOfWeek.Saturday
                                && date.DayOfWeek != DayOfWeek.Sunday
                                )
                            {
                                //not holidays
                                if (!dateHolidays.Contains(date))
                                {
                                    //absence
                                    if (!attendances.Contains(date.Date))
                                    {
                                        absence = absence + 1;
                                    }
                                }

                            }
                        }


                    }

                    datas.Add(absence);
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetEmployeeDoughnut()
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.Department.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Employee
                        .Where(x => x.DepartmentId.Equals(item.DepartmentId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ChartDoughnut GetDiseaseTypeDoughnut()
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.DiseaseType.ToList())
                {
                    labels.Add(item.Name);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Patient
                        .Where(x => x.DiseaseTypeId.Equals(item.DiseaseTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ChartDoughnut GetDoctorGroupDoughnut()
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.DoctorType.ToList())
                {
                    labels.Add(item.DoctorTypeName);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.DoctorGroup
                        .Where(x => x.DoctorTypeId.Equals(item.DoctorTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetDoctorDoughnut()
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.DoctorType.ToList())
                {
                    labels.Add(item.DoctorTypeName);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(_context.Doctor
                        .Where(x => x.DoctorTypeId.Equals(item.DoctorTypeId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ChartDoughnut GetHospitalDoughnut()
        {
            try
            {

                var HospitalRegion = _context.HospitalViewRegionId.FromSqlRaw("dbo.ListHospitalRegionId").ToList();


                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in _context.Region.ToList())
                {
                    labels.Add(item.RegionName);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    datas.Add(HospitalRegion
                        .Where(x => x.RegionId.Equals(item.RegionId))
                        .Count());
                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public ChartDoughnut GetSelfServiceDoughnutByPeriod(string period, string employeeId)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;
                List<string> cohorts = new List<string>();
                cohorts.Add("Paid Leave");
                cohorts.Add("Not Paid Leave");

                foreach (var item in cohorts)
                {
                    labels.Add(item);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    if (item.Equals("Paid Leave"))
                    {
                        datas.Add(_context.Leave
                            .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                                && x.OnBehalfId.Equals(employeeId)
                                && x.IsPaidLeave.Equals(true))
                            .Count());
                    }
                    else
                    {
                        datas.Add(_context.Leave
                          .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                              && x.OnBehalfId.Equals(employeeId)
                              && x.IsPaidLeave.Equals(false))
                          .Count());
                    }


                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetHolidayVsWorkdayDoughnutByPeriod(string period)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<int> datas = new List<int>();
                int index = 0;

                List<string> list = new List<string>();
                list.Add("Workday");
                list.Add("Holiday");

                string[] periods = period.Split('-');
                int year = Convert.ToInt32(periods[0]);
                int month = Convert.ToInt32(periods[1]);
                DateTime start = new DateTime(year, month, 1);
                DateTime end = start.AddMonths(1).AddDays(-1);

                foreach (var item in list)
                {
                    labels.Add(item);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    if (item.Equals("Workday"))
                    {
                        int workday = 0;

                        for (DateTime date = start.Date; date < end.Date; date = date.AddDays(1))
                        {
                            if (
                                 date.DayOfWeek != DayOfWeek.Saturday
                                 && date.DayOfWeek != DayOfWeek.Sunday
                                )
                            {
                                workday++;
                            }
                        }

                        datas.Add(workday);
                    }


                    if (item.Equals("Holiday"))
                    {
                        int holiday = 0;
                        holiday = _context.PublicHolidayLine
                            .Where(x => x.PublicHolidayDate.ToString("yyyy-MM").Equals(period))
                            .Count();

                        datas.Add(holiday);
                    }


                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ChartDoughnut GetLeaveDoughnutByPeriodByEmployeeId(string period, string employeeId)
        {
            try
            {
                ChartDoughnut result = new ChartDoughnut();
                List<string> labels = new List<string>();
                List<string> colors = new List<string>();
                List<string> list = new List<string>();
                list.Add("Leave");
                list.Add("Absence");
                List<int> datas = new List<int>();
                int index = 0;
                foreach (var item in list)
                {
                    labels.Add(item);

                    colors.Add(ColorList.GetAllRGBA()[index]);
                    index++;

                    if (item.Equals("Leave"))
                    {
                        datas.Add(_context.Leave
                                    .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period)
                                        && x.OnBehalfId.Equals(employeeId))
                                    .ToList()
                                    .Sum(x => (x.ToDate.Date - x.FromDate.Date).Days + 1));
                    }

                    if (item.Equals("Absence"))
                    {
                        List<DateTime> dateHolidays = new List<DateTime>();

                        foreach (var itm in _context.PublicHolidayLine)
                        {
                            dateHolidays.Add(itm.PublicHolidayDate.Date);
                        }

                        List<DateTime> attendances = new List<DateTime>();
                        attendances = _context.Attendance
                            .Where(x => x.OnBehalfId.Equals(employeeId)
                            && x.Clock.ToString("yyyy-MM").Equals(period))
                            .GroupBy(x => x.Clock.Date)
                            .Where(x => x.Count() >= 2)
                            .Select(x => x.Key).ToList();

                        string[] periods = period.Split('-');

                        int year = Convert.ToInt32(periods[0]);
                        int month = Convert.ToInt32(periods[1]);

                        DateTime startDate = new DateTime(year, month, 1);
                        DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                        int absence = 0;

                        for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                        {
                            if (
                                date.DayOfWeek != DayOfWeek.Saturday
                                && date.DayOfWeek != DayOfWeek.Sunday
                                )
                            {
                                //not holidays
                                if (!dateHolidays.Contains(date))
                                {
                                    //presence
                                    if (!attendances.Contains(date.Date))
                                    {
                                        //absence
                                        absence = absence + 1;
                                    }
                                }

                            }
                        }

                        datas.Add(absence);
                    }


                }

                result.Labels = labels.ToArray();
                result.Colors = colors.ToArray();
                result.Data = datas.ToArray();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
