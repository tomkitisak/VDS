using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vds.Migrations
{
    public partial class inidb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    isSuperAdmin = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OperationType = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllowanceType",
                columns: table => new
                {
                    AllowanceTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowanceType", x => x.AllowanceTypeId);
                    table.ForeignKey(
                        name: "FK_AllowanceType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllowanceType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applicant",
                columns: table => new
                {
                    ApplicantId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant", x => x.ApplicantId);
                    table.ForeignKey(
                        name: "FK_Applicant_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applicant_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppraisalType",
                columns: table => new
                {
                    AppraisalTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalType", x => x.AppraisalTypeId);
                    table.ForeignKey(
                        name: "FK_AppraisalType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppraisalType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetType",
                columns: table => new
                {
                    AssetTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetType", x => x.AssetTypeId);
                    table.ForeignKey(
                        name: "FK_AssetType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AwardType",
                columns: table => new
                {
                    AwardTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwardType", x => x.AwardTypeId);
                    table.ForeignKey(
                        name: "FK_AwardType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AwardType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BenefitTemplate",
                columns: table => new
                {
                    BenefitTemplateId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitTemplate", x => x.BenefitTemplateId);
                    table.ForeignKey(
                        name: "FK_BenefitTemplate_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BenefitTemplate_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeductionType",
                columns: table => new
                {
                    DeductionTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionType", x => x.DeductionTypeId);
                    table.ForeignKey(
                        name: "FK_DeductionType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeductionType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Department_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Department_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Designation",
                columns: table => new
                {
                    DesignationId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.DesignationId);
                    table.ForeignKey(
                        name: "FK_Designation_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Designation_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiseaseType",
                columns: table => new
                {
                    DiseaseTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiseaseType", x => x.DiseaseTypeId);
                    table.ForeignKey(
                        name: "FK_DiseaseType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiseaseType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorType",
                columns: table => new
                {
                    DoctorTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    DoctorTypeName = table.Column<string>(nullable: false),
                    SubTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorType", x => x.DoctorTypeId);
                    table.ForeignKey(
                        name: "FK_DoctorType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseType",
                columns: table => new
                {
                    ExpenseTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseType", x => x.ExpenseTypeId);
                    table.ForeignKey(
                        name: "FK_ExpenseType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    HospitalId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    HospitalName = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    OperatingRoom = table.Column<int>(nullable: false),
                    Service = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Long = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.HospitalId);
                    table.ForeignKey(
                        name: "FK_Hospital_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hospital_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InformationType",
                columns: table => new
                {
                    InformationTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationType", x => x.InformationTypeId);
                    table.ForeignKey(
                        name: "FK_InformationType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InformationType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobStatus",
                columns: table => new
                {
                    JobStatusId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AllowAddDoctor = table.Column<bool>(nullable: false),
                    AllowAddPatient = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatus", x => x.JobStatusId);
                    table.ForeignKey(
                        name: "FK_JobStatus_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobStatus_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobVacancy",
                columns: table => new
                {
                    JobVacancyId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobVacancy", x => x.JobVacancyId);
                    table.ForeignKey(
                        name: "FK_JobVacancy_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobVacancy_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Layoff",
                columns: table => new
                {
                    LayoffId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layoff", x => x.LayoffId);
                    table.ForeignKey(
                        name: "FK_Layoff_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Layoff_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                columns: table => new
                {
                    LeaveTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.LeaveTypeId);
                    table.ForeignKey(
                        name: "FK_LeaveType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OnBoarding",
                columns: table => new
                {
                    OnBoardingId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnBoarding", x => x.OnBoardingId);
                    table.ForeignKey(
                        name: "FK_OnBoarding_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnBoarding_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrefixType",
                columns: table => new
                {
                    PrefixTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrefixType", x => x.PrefixTypeId);
                    table.ForeignKey(
                        name: "FK_PrefixType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrefixType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicHoliday",
                columns: table => new
                {
                    PublicHolidayId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHoliday", x => x.PublicHolidayId);
                    table.ForeignKey(
                        name: "FK_PublicHoliday_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicHoliday_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    RegionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionId);
                    table.ForeignKey(
                        name: "FK_Region_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Region_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resignation",
                columns: table => new
                {
                    ResignationId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resignation", x => x.ResignationId);
                    table.ForeignKey(
                        name: "FK_Resignation_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resignation_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThirdParty",
                columns: table => new
                {
                    ThirdPartyId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdParty", x => x.ThirdPartyId);
                    table.ForeignKey(
                        name: "FK_ThirdParty_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThirdParty_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    TicketTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.TicketTypeId);
                    table.ForeignKey(
                        name: "FK_TicketType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TodoType",
                columns: table => new
                {
                    TodoTypeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoType", x => x.TodoTypeId);
                    table.ForeignKey(
                        name: "FK_TodoType_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoType_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BenefitTemplateLine",
                columns: table => new
                {
                    BenefitTemplateLineId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    BenefitTemplateId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    AllowanceTypeId = table.Column<string>(nullable: true),
                    DeductionTypeId = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitTemplateLine", x => x.BenefitTemplateLineId);
                    table.ForeignKey(
                        name: "FK_BenefitTemplateLine_AllowanceType_AllowanceTypeId",
                        column: x => x.AllowanceTypeId,
                        principalTable: "AllowanceType",
                        principalColumn: "AllowanceTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BenefitTemplateLine_BenefitTemplate_BenefitTemplateId",
                        column: x => x.BenefitTemplateId,
                        principalTable: "BenefitTemplate",
                        principalColumn: "BenefitTemplateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BenefitTemplateLine_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BenefitTemplateLine_DeductionType_DeductionTypeId",
                        column: x => x.DeductionTypeId,
                        principalTable: "DeductionType",
                        principalColumn: "DeductionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BenefitTemplateLine_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorGroup",
                columns: table => new
                {
                    DoctorGroupId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    DoctorGroupName = table.Column<string>(nullable: false),
                    DoctorTypeId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorGroup", x => x.DoctorGroupId);
                    table.ForeignKey(
                        name: "FK_DoctorGroup_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorGroup_DoctorType_DoctorTypeId",
                        column: x => x.DoctorTypeId,
                        principalTable: "DoctorType",
                        principalColumn: "DoctorTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorGroup_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Information",
                columns: table => new
                {
                    InformationId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    InformationName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    InformationTypeId = table.Column<string>(nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(nullable: false),
                    ExternalLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Information", x => x.InformationId);
                    table.ForeignKey(
                        name: "FK_Information_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Information_InformationType_InformationTypeId",
                        column: x => x.InformationTypeId,
                        principalTable: "InformationType",
                        principalColumn: "InformationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Information_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    JobId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    WorkDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    JobStatusId = table.Column<string>(nullable: true),
                    HospitalId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Job_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Job_JobStatus_JobStatusId",
                        column: x => x.JobStatusId,
                        principalTable: "JobStatus",
                        principalColumn: "JobStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Coordinator",
                columns: table => new
                {
                    CoordinatorId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PrefixTypeId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LineId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    HospitalId = table.Column<string>(nullable: true),
                    DesignationId = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinator", x => x.CoordinatorId);
                    table.ForeignKey(
                        name: "FK_Coordinator_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coordinator_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coordinator_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "DesignationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coordinator_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coordinator_PrefixType_PrefixTypeId",
                        column: x => x.PrefixTypeId,
                        principalTable: "PrefixType",
                        principalColumn: "PrefixTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Coordinator_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    DirectorId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PrefixTypeId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LineId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    HospitalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.DirectorId);
                    table.ForeignKey(
                        name: "FK_Director_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Director_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Director_PrefixType_PrefixTypeId",
                        column: x => x.PrefixTypeId,
                        principalTable: "PrefixType",
                        principalColumn: "PrefixTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Director_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    DoctorId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PrefixTypeId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    LineId = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    MDLicense = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    DoctorTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK_Doctor_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctor_DoctorType_DoctorTypeId",
                        column: x => x.DoctorTypeId,
                        principalTable: "DoctorType",
                        principalColumn: "DoctorTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctor_PrefixType_PrefixTypeId",
                        column: x => x.PrefixTypeId,
                        principalTable: "PrefixType",
                        principalColumn: "PrefixTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doctor_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PrefixTypeId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    EmployeeIDNumber = table.Column<string>(nullable: true),
                    HospitalId = table.Column<string>(nullable: false),
                    DesignationId = table.Column<string>(nullable: false),
                    DepartmentId = table.Column<string>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "DesignationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_PrefixType_PrefixTypeId",
                        column: x => x.PrefixTypeId,
                        principalTable: "PrefixType",
                        principalColumn: "PrefixTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PrefixTypeId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: false),
                    Problem = table.Column<string>(nullable: false),
                    DiseaseTypeId = table.Column<string>(nullable: false),
                    HospitalId = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patient_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_DiseaseType_DiseaseTypeId",
                        column: x => x.DiseaseTypeId,
                        principalTable: "DiseaseType",
                        principalColumn: "DiseaseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_PrefixType_PrefixTypeId",
                        column: x => x.PrefixTypeId,
                        principalTable: "PrefixType",
                        principalColumn: "PrefixTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicHolidayLine",
                columns: table => new
                {
                    PublicHolidayLineId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PublicHolidayId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PublicHolidayDate = table.Column<DateTime>(nullable: false),
                    PublicHolidayYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHolidayLine", x => x.PublicHolidayLineId);
                    table.ForeignKey(
                        name: "FK_PublicHolidayLine_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicHolidayLine_PublicHoliday_PublicHolidayId",
                        column: x => x.PublicHolidayId,
                        principalTable: "PublicHoliday",
                        principalColumn: "PublicHolidayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicHolidayLine_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ProvinceId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    ProvinceThai = table.Column<string>(nullable: true),
                    ProvinceEng = table.Column<string>(nullable: true),
                    RegionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ProvinceId);
                    table.ForeignKey(
                        name: "FK_Province_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Province_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Province_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupCoordinator",
                columns: table => new
                {
                    GroupCoordinatorId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PrefixTypeId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LineId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    Address1 = table.Column<string>(nullable: false),
                    SubDistrict = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    DoctorGroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCoordinator", x => x.GroupCoordinatorId);
                    table.ForeignKey(
                        name: "FK_GroupCoordinator_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupCoordinator_DoctorGroup_DoctorGroupId",
                        column: x => x.DoctorGroupId,
                        principalTable: "DoctorGroup",
                        principalColumn: "DoctorGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupCoordinator_PrefixType_PrefixTypeId",
                        column: x => x.PrefixTypeId,
                        principalTable: "PrefixType",
                        principalColumn: "PrefixTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupCoordinator_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorGroupDoctor",
                columns: table => new
                {
                    DoctorGroupDoctorId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    DoctorGroupId = table.Column<string>(nullable: true),
                    DoctorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorGroupDoctor", x => x.DoctorGroupDoctorId);
                    table.ForeignKey(
                        name: "FK_DoctorGroupDoctor_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorGroupDoctor_DoctorGroup_DoctorGroupId",
                        column: x => x.DoctorGroupId,
                        principalTable: "DoctorGroup",
                        principalColumn: "DoctorGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorGroupDoctor_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorGroupDoctor_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobDoctor",
                columns: table => new
                {
                    JobDoctorId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    JobId = table.Column<string>(nullable: false),
                    DoctorId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDoctor", x => x.JobDoctorId);
                    table.ForeignKey(
                        name: "FK_JobDoctor_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobDoctor_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobDoctor_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobDoctor_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appraisal",
                columns: table => new
                {
                    AppraisalId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    AppraisalName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    AppraisalTypeId = table.Column<string>(nullable: false),
                    SubmitDate = table.Column<DateTimeOffset>(nullable: false),
                    OnBehalfId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appraisal", x => x.AppraisalId);
                    table.ForeignKey(
                        name: "FK_Appraisal_AppraisalType_AppraisalTypeId",
                        column: x => x.AppraisalTypeId,
                        principalTable: "AppraisalType",
                        principalColumn: "AppraisalTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appraisal_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appraisal_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appraisal_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    AssetId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    AssetName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AssetTypeId = table.Column<string>(nullable: false),
                    PurchaseDate = table.Column<DateTimeOffset>(nullable: false),
                    PurchasePrice = table.Column<decimal>(nullable: false),
                    UsedById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_Asset_AssetType_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetType",
                        principalColumn: "AssetTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asset_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asset_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asset_Employee_UsedById",
                        column: x => x.UsedById,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendanceId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    AttendanceName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OnBehalfId = table.Column<string>(nullable: false),
                    Clock = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendance_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendance_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendance_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Award",
                columns: table => new
                {
                    AwardId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    AwardName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    AwardTypeId = table.Column<string>(nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(nullable: false),
                    AwardRecipientId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Award", x => x.AwardId);
                    table.ForeignKey(
                        name: "FK_Award_Employee_AwardRecipientId",
                        column: x => x.AwardRecipientId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Award_AwardType_AwardTypeId",
                        column: x => x.AwardTypeId,
                        principalTable: "AwardType",
                        principalColumn: "AwardTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Award_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Award_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    ExpenseId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    ExpenseName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    ExpenseTypeId = table.Column<string>(nullable: false),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    ExpenseAmount = table.Column<decimal>(nullable: false),
                    IsCashAdvance = table.Column<bool>(nullable: false),
                    OnBehalfId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expense_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_ExpenseType_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseType",
                        principalColumn: "ExpenseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expense_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expense_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leave",
                columns: table => new
                {
                    LeaveId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    LeaveName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    EmergencyCall = table.Column<string>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    LeaveTypeId = table.Column<string>(nullable: false),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    OnBehalfId = table.Column<string>(nullable: false),
                    IsPaidLeave = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave", x => x.LeaveId);
                    table.ForeignKey(
                        name: "FK_Leave_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leave_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveType",
                        principalColumn: "LeaveTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leave_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leave_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payroll",
                columns: table => new
                {
                    PayrollId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    Periode = table.Column<DateTimeOffset>(nullable: false),
                    OnBehalfId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll", x => x.PayrollId);
                    table.ForeignKey(
                        name: "FK_Payroll_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payroll_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payroll_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    TicketName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsSolve = table.Column<bool>(nullable: false),
                    SolutionNote = table.Column<string>(nullable: true),
                    TicketTypeId = table.Column<string>(nullable: false),
                    SubmitDate = table.Column<DateTimeOffset>(nullable: false),
                    OnBehalfId = table.Column<string>(nullable: false),
                    AgentId = table.Column<string>(nullable: true),
                    ParentTicketThreadId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Ticket_Employee_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ticket_Ticket_ParentTicketThreadId",
                        column: x => x.ParentTicketThreadId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ticket_TicketType_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "TicketTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ticket_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Todo",
                columns: table => new
                {
                    TodoId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    TodoItem = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false),
                    TodoTypeId = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTimeOffset>(nullable: false),
                    EndDate = table.Column<DateTimeOffset>(nullable: false),
                    OnBehalfId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todo", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todo_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Todo_Employee_OnBehalfId",
                        column: x => x.OnBehalfId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Todo_TodoType_TodoTypeId",
                        column: x => x.TodoTypeId,
                        principalTable: "TodoType",
                        principalColumn: "TodoTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Todo_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobPatient",
                columns: table => new
                {
                    JobPatientId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    JobId = table.Column<string>(nullable: false),
                    PatientId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPatient", x => x.JobPatientId);
                    table.ForeignKey(
                        name: "FK_JobPatient_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobPatient_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobPatient_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobPatient_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLineAllowance",
                columns: table => new
                {
                    PayrollLineAllowanceId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    AllowanceTypeId = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLineAllowance", x => x.PayrollLineAllowanceId);
                    table.ForeignKey(
                        name: "FK_PayrollLineAllowance_AllowanceType_AllowanceTypeId",
                        column: x => x.AllowanceTypeId,
                        principalTable: "AllowanceType",
                        principalColumn: "AllowanceTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineAllowance_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineAllowance_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineAllowance_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLineBasic",
                columns: table => new
                {
                    PayrollLineBasicId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLineBasic", x => x.PayrollLineBasicId);
                    table.ForeignKey(
                        name: "FK_PayrollLineBasic_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineBasic_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineBasic_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLineCashAdvance",
                columns: table => new
                {
                    PayrollLineCashAdvanceId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ExpenseTypeId = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLineCashAdvance", x => x.PayrollLineCashAdvanceId);
                    table.ForeignKey(
                        name: "FK_PayrollLineCashAdvance_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineCashAdvance_ExpenseType_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseType",
                        principalColumn: "ExpenseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineCashAdvance_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineCashAdvance_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLineDeduction",
                columns: table => new
                {
                    PayrollLineDeductionId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    DeductionTypeId = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLineDeduction", x => x.PayrollLineDeductionId);
                    table.ForeignKey(
                        name: "FK_PayrollLineDeduction_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineDeduction_DeductionType_DeductionTypeId",
                        column: x => x.DeductionTypeId,
                        principalTable: "DeductionType",
                        principalColumn: "DeductionTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineDeduction_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineDeduction_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLineReimburse",
                columns: table => new
                {
                    PayrollLineReimburseId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ExpenseTypeId = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLineReimburse", x => x.PayrollLineReimburseId);
                    table.ForeignKey(
                        name: "FK_PayrollLineReimburse_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineReimburse_ExpenseType_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseType",
                        principalColumn: "ExpenseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineReimburse_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineReimburse_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLineUnpaidLeave",
                columns: table => new
                {
                    PayrollLineUnpaidLeaveId = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(nullable: false),
                    PayrollId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    LeaveId = table.Column<string>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    UnpaidPerDay = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollLineUnpaidLeave", x => x.PayrollLineUnpaidLeaveId);
                    table.ForeignKey(
                        name: "FK_PayrollLineUnpaidLeave_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineUnpaidLeave_Leave_LeaveId",
                        column: x => x.LeaveId,
                        principalTable: "Leave",
                        principalColumn: "LeaveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollLineUnpaidLeave_Payroll_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "Payroll",
                        principalColumn: "PayrollId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLineUnpaidLeave_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceType_CreatedById",
                table: "AllowanceType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceType_UpdatedById",
                table: "AllowanceType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_CreatedById",
                table: "Applicant",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Applicant_UpdatedById",
                table: "Applicant",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisal_AppraisalTypeId",
                table: "Appraisal",
                column: "AppraisalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisal_CreatedById",
                table: "Appraisal",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisal_OnBehalfId",
                table: "Appraisal",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisal_UpdatedById",
                table: "Appraisal",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalType_CreatedById",
                table: "AppraisalType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalType_UpdatedById",
                table: "AppraisalType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetTypeId",
                table: "Asset",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_CreatedById",
                table: "Asset",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_UpdatedById",
                table: "Asset",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_UsedById",
                table: "Asset",
                column: "UsedById");

            migrationBuilder.CreateIndex(
                name: "IX_AssetType_CreatedById",
                table: "AssetType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AssetType_UpdatedById",
                table: "AssetType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_CreatedById",
                table: "Attendance",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_OnBehalfId",
                table: "Attendance",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_UpdatedById",
                table: "Attendance",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Award_AwardRecipientId",
                table: "Award",
                column: "AwardRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Award_AwardTypeId",
                table: "Award",
                column: "AwardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Award_CreatedById",
                table: "Award",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Award_UpdatedById",
                table: "Award",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AwardType_CreatedById",
                table: "AwardType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AwardType_UpdatedById",
                table: "AwardType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplate_CreatedById",
                table: "BenefitTemplate",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplate_UpdatedById",
                table: "BenefitTemplate",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplateLine_AllowanceTypeId",
                table: "BenefitTemplateLine",
                column: "AllowanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplateLine_BenefitTemplateId",
                table: "BenefitTemplateLine",
                column: "BenefitTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplateLine_CreatedById",
                table: "BenefitTemplateLine",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplateLine_DeductionTypeId",
                table: "BenefitTemplateLine",
                column: "DeductionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitTemplateLine_UpdatedById",
                table: "BenefitTemplateLine",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_CreatedById",
                table: "Coordinator",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_DepartmentId",
                table: "Coordinator",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_DesignationId",
                table: "Coordinator",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_HospitalId",
                table: "Coordinator",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_PrefixTypeId",
                table: "Coordinator",
                column: "PrefixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinator_UpdatedById",
                table: "Coordinator",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionType_CreatedById",
                table: "DeductionType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionType_UpdatedById",
                table: "DeductionType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CreatedById",
                table: "Department",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Department_UpdatedById",
                table: "Department",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Designation_CreatedById",
                table: "Designation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Designation_UpdatedById",
                table: "Designation",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Director_CreatedById",
                table: "Director",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Director_HospitalId",
                table: "Director",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Director_PrefixTypeId",
                table: "Director",
                column: "PrefixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Director_UpdatedById",
                table: "Director",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseType_CreatedById",
                table: "DiseaseType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseType_UpdatedById",
                table: "DiseaseType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_CreatedById",
                table: "Doctor",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_DoctorTypeId",
                table: "Doctor",
                column: "DoctorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_PrefixTypeId",
                table: "Doctor",
                column: "PrefixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_UpdatedById",
                table: "Doctor",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroup_CreatedById",
                table: "DoctorGroup",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroup_DoctorTypeId",
                table: "DoctorGroup",
                column: "DoctorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroup_UpdatedById",
                table: "DoctorGroup",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroupDoctor_CreatedById",
                table: "DoctorGroupDoctor",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroupDoctor_DoctorGroupId",
                table: "DoctorGroupDoctor",
                column: "DoctorGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroupDoctor_DoctorId",
                table: "DoctorGroupDoctor",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroupDoctor_UpdatedById",
                table: "DoctorGroupDoctor",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorType_CreatedById",
                table: "DoctorType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorType_UpdatedById",
                table: "DoctorType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CreatedById",
                table: "Employee",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DesignationId",
                table: "Employee",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_HospitalId",
                table: "Employee",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PrefixTypeId",
                table: "Employee",
                column: "PrefixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UpdatedById",
                table: "Employee",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CreatedById",
                table: "Expense",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseTypeId",
                table: "Expense",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_OnBehalfId",
                table: "Expense",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_UpdatedById",
                table: "Expense",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseType_CreatedById",
                table: "ExpenseType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseType_UpdatedById",
                table: "ExpenseType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCoordinator_CreatedById",
                table: "GroupCoordinator",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCoordinator_DoctorGroupId",
                table: "GroupCoordinator",
                column: "DoctorGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCoordinator_PrefixTypeId",
                table: "GroupCoordinator",
                column: "PrefixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCoordinator_UpdatedById",
                table: "GroupCoordinator",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_CreatedById",
                table: "Hospital",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_UpdatedById",
                table: "Hospital",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Information_CreatedById",
                table: "Information",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Information_InformationTypeId",
                table: "Information",
                column: "InformationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Information_UpdatedById",
                table: "Information",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InformationType_CreatedById",
                table: "InformationType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InformationType_UpdatedById",
                table: "InformationType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Job_CreatedById",
                table: "Job",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Job_HospitalId",
                table: "Job",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_JobStatusId",
                table: "Job",
                column: "JobStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_UpdatedById",
                table: "Job",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDoctor_CreatedById",
                table: "JobDoctor",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobDoctor_DoctorId",
                table: "JobDoctor",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDoctor_JobId",
                table: "JobDoctor",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDoctor_UpdatedById",
                table: "JobDoctor",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobPatient_CreatedById",
                table: "JobPatient",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobPatient_JobId",
                table: "JobPatient",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPatient_PatientId",
                table: "JobPatient",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPatient_UpdatedById",
                table: "JobPatient",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobStatus_CreatedById",
                table: "JobStatus",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobStatus_UpdatedById",
                table: "JobStatus",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobVacancy_CreatedById",
                table: "JobVacancy",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobVacancy_UpdatedById",
                table: "JobVacancy",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Layoff_CreatedById",
                table: "Layoff",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Layoff_UpdatedById",
                table: "Layoff",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_CreatedById",
                table: "Leave",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_LeaveTypeId",
                table: "Leave",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_OnBehalfId",
                table: "Leave",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_UpdatedById",
                table: "Leave",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_CreatedById",
                table: "LeaveType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_UpdatedById",
                table: "LeaveType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OnBoarding_CreatedById",
                table: "OnBoarding",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OnBoarding_UpdatedById",
                table: "OnBoarding",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_CreatedById",
                table: "Patient",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_DiseaseTypeId",
                table: "Patient",
                column: "DiseaseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_HospitalId",
                table: "Patient",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PrefixTypeId",
                table: "Patient",
                column: "PrefixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_UpdatedById",
                table: "Patient",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_CreatedById",
                table: "Payroll",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_OnBehalfId",
                table: "Payroll",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_UpdatedById",
                table: "Payroll",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineAllowance_AllowanceTypeId",
                table: "PayrollLineAllowance",
                column: "AllowanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineAllowance_CreatedById",
                table: "PayrollLineAllowance",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineAllowance_PayrollId",
                table: "PayrollLineAllowance",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineAllowance_UpdatedById",
                table: "PayrollLineAllowance",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineBasic_CreatedById",
                table: "PayrollLineBasic",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineBasic_PayrollId",
                table: "PayrollLineBasic",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineBasic_UpdatedById",
                table: "PayrollLineBasic",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineCashAdvance_CreatedById",
                table: "PayrollLineCashAdvance",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineCashAdvance_ExpenseTypeId",
                table: "PayrollLineCashAdvance",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineCashAdvance_PayrollId",
                table: "PayrollLineCashAdvance",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineCashAdvance_UpdatedById",
                table: "PayrollLineCashAdvance",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineDeduction_CreatedById",
                table: "PayrollLineDeduction",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineDeduction_DeductionTypeId",
                table: "PayrollLineDeduction",
                column: "DeductionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineDeduction_PayrollId",
                table: "PayrollLineDeduction",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineDeduction_UpdatedById",
                table: "PayrollLineDeduction",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineReimburse_CreatedById",
                table: "PayrollLineReimburse",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineReimburse_ExpenseTypeId",
                table: "PayrollLineReimburse",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineReimburse_PayrollId",
                table: "PayrollLineReimburse",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineReimburse_UpdatedById",
                table: "PayrollLineReimburse",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineUnpaidLeave_CreatedById",
                table: "PayrollLineUnpaidLeave",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineUnpaidLeave_LeaveId",
                table: "PayrollLineUnpaidLeave",
                column: "LeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineUnpaidLeave_PayrollId",
                table: "PayrollLineUnpaidLeave",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLineUnpaidLeave_UpdatedById",
                table: "PayrollLineUnpaidLeave",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PrefixType_CreatedById",
                table: "PrefixType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PrefixType_UpdatedById",
                table: "PrefixType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Province_CreatedById",
                table: "Province",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Province_RegionId",
                table: "Province",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Province_UpdatedById",
                table: "Province",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHoliday_CreatedById",
                table: "PublicHoliday",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHoliday_UpdatedById",
                table: "PublicHoliday",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidayLine_CreatedById",
                table: "PublicHolidayLine",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidayLine_PublicHolidayId",
                table: "PublicHolidayLine",
                column: "PublicHolidayId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidayLine_UpdatedById",
                table: "PublicHolidayLine",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Region_CreatedById",
                table: "Region",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Region_UpdatedById",
                table: "Region",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Resignation_CreatedById",
                table: "Resignation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Resignation_UpdatedById",
                table: "Resignation",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdParty_CreatedById",
                table: "ThirdParty",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdParty_UpdatedById",
                table: "ThirdParty",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AgentId",
                table: "Ticket",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CreatedById",
                table: "Ticket",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_OnBehalfId",
                table: "Ticket",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ParentTicketThreadId",
                table: "Ticket",
                column: "ParentTicketThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketTypeId",
                table: "Ticket",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UpdatedById",
                table: "Ticket",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TicketType_CreatedById",
                table: "TicketType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TicketType_UpdatedById",
                table: "TicketType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_CreatedById",
                table: "Todo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_OnBehalfId",
                table: "Todo",
                column: "OnBehalfId");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_TodoTypeId",
                table: "Todo",
                column: "TodoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Todo_UpdatedById",
                table: "Todo",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TodoType_CreatedById",
                table: "TodoType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TodoType_UpdatedById",
                table: "TodoType",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applicant");

            migrationBuilder.DropTable(
                name: "Appraisal");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "Attendance");

            migrationBuilder.DropTable(
                name: "Award");

            migrationBuilder.DropTable(
                name: "BenefitTemplateLine");

            migrationBuilder.DropTable(
                name: "Coordinator");

            migrationBuilder.DropTable(
                name: "Director");

            migrationBuilder.DropTable(
                name: "DoctorGroupDoctor");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "GroupCoordinator");

            migrationBuilder.DropTable(
                name: "Information");

            migrationBuilder.DropTable(
                name: "JobDoctor");

            migrationBuilder.DropTable(
                name: "JobPatient");

            migrationBuilder.DropTable(
                name: "JobVacancy");

            migrationBuilder.DropTable(
                name: "Layoff");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "OnBoarding");

            migrationBuilder.DropTable(
                name: "PayrollLineAllowance");

            migrationBuilder.DropTable(
                name: "PayrollLineBasic");

            migrationBuilder.DropTable(
                name: "PayrollLineCashAdvance");

            migrationBuilder.DropTable(
                name: "PayrollLineDeduction");

            migrationBuilder.DropTable(
                name: "PayrollLineReimburse");

            migrationBuilder.DropTable(
                name: "PayrollLineUnpaidLeave");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "PublicHolidayLine");

            migrationBuilder.DropTable(
                name: "Resignation");

            migrationBuilder.DropTable(
                name: "ThirdParty");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Todo");

            migrationBuilder.DropTable(
                name: "AppraisalType");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AssetType");

            migrationBuilder.DropTable(
                name: "AwardType");

            migrationBuilder.DropTable(
                name: "BenefitTemplate");

            migrationBuilder.DropTable(
                name: "DoctorGroup");

            migrationBuilder.DropTable(
                name: "InformationType");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "AllowanceType");

            migrationBuilder.DropTable(
                name: "DeductionType");

            migrationBuilder.DropTable(
                name: "ExpenseType");

            migrationBuilder.DropTable(
                name: "Leave");

            migrationBuilder.DropTable(
                name: "Payroll");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "PublicHoliday");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "TodoType");

            migrationBuilder.DropTable(
                name: "DoctorType");

            migrationBuilder.DropTable(
                name: "JobStatus");

            migrationBuilder.DropTable(
                name: "DiseaseType");

            migrationBuilder.DropTable(
                name: "LeaveType");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropTable(
                name: "Hospital");

            migrationBuilder.DropTable(
                name: "PrefixType");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
