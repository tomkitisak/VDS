using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vds.Models;
using vds.ViewModels;

namespace vds.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //custom entity, override identity user with new column
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //custom entity, for simple todo app
        public DbSet<Todo> Todo { get; set; }
        //custom entity, for log
        public DbSet<Log> Log { get; set; }

        ///HRM
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<AllowanceType> AllowanceType { get; set; }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<DeductionType> DeductionType { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ExpenseType> ExpenseType { get; set; }
        public DbSet<ThirdParty> ThirdParty { get; set; }
        public DbSet<JobVacancy> JobVacancy { get; set; }
        public DbSet<OnBoarding> OnBoarding { get; set; }
        public DbSet<Resignation> Resignation { get; set; }
        public DbSet<Layoff> Layoff { get; set; }
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Award> Award { get; set; }
        public DbSet<AwardType> AwardType { get; set; }
        public DbSet<Information> Information { get; set; }
        public DbSet<InformationType> InformationType { get; set; }
        public DbSet<Appraisal> Appraisal { get; set; }
        public DbSet<AppraisalType> AppraisalType { get; set; }
        public DbSet<TodoType> TodoType { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<PublicHoliday> PublicHoliday { get; set; }
        public DbSet<PublicHolidayLine> PublicHolidayLine { get; set; }
        public DbSet<BenefitTemplate> BenefitTemplate { get; set; }
        public DbSet<BenefitTemplateLine> BenefitTemplateLine { get; set; }
        public DbSet<Payroll> Payroll { get; set; }
        public DbSet<PayrollLineBasic> PayrollLineBasic { get; set; }
        public DbSet<PayrollLineAllowance> PayrollLineAllowance { get; set; }
        public DbSet<PayrollLineDeduction> PayrollLineDeduction { get; set; }
        public DbSet<PayrollLineCashAdvance> PayrollLineCashAdvance { get; set; }
        public DbSet<PayrollLineReimburse> PayrollLineReimburse { get; set; }
        public DbSet<PayrollLineUnpaidLeave> PayrollLineUnpaidLeave { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<Patient> Patient { get; set; }

        public DbSet<Hospital> Hospital { get; set; }

        public DbSet<Director> Director { get; set; }

        public DbSet<Coordinator> Coordinator { get; set; }

        public DbSet<DoctorGroup> DoctorGroup { get; set; }

        public DbSet<DoctorGroupDoctor> DoctorGroupDoctor { get; set; }
        public DbSet<DoctorType> DoctorType { get; set; }

        public DbSet<GroupCoordinator> GroupCoordinator { get; set; }

        public DbSet<Region> Region { get; set; }
        public DbSet<Province> Province { get; set; }

        public DbSet<PrefixType> PrefixType { get; set; }

        public DbSet<Gender> Gender { get; set; }
        public DbSet<DiseaseType> DiseaseType { get; set; }

        public DbSet<Job> Job { get; set; }
        public DbSet<JobStatus> JobStatus { get; set; }
        public DbSet<JobDoctor> JobDoctor { get; set; }
        public DbSet<JobPatient> JobPatient { get; set; }

        public DbSet<UserType> UserType { get; set; }

        public DbSet<EmailSenderConfig> EmailSenderConfig { get; set; }


        // CAll Store Procedure
        public virtual  DbSet<HospitalViewRegionId> HospitalViewRegionId { get; set; }
        public virtual DbSet<HospitalSummary> HospitalSummary { get; set; }

        
    }
}
