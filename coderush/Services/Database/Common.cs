using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using vds.Data;
using vds.Models;
using vds.Services.Security;

namespace vds.Services.Database
{
    //special service provided for db initialization / data seed
    public class Common : ICommon
    {
        private readonly ApplicationDbContext _context;
        private readonly Security.ICommon _security;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly App.ICommon _app;

        public Common(ApplicationDbContext context,
            Security.ICommon security,
            RoleManager<IdentityRole> roleManager,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
            App.ICommon app,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _security = security;
            _userManager = userManager;
            _roleManager = roleManager;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _app = app;
        }

        public async Task Initialize()
        {
            try
            {
                _context.Database.EnsureCreated();

                //check for users
                if (_context.ApplicationUser.Any())
                {
                    return; //if user is not empty, DB has been seed
                }

                //init app with super admin user
                await _security.CreateDefaultSuperAdmin();

                //init app with demo data
                await InsertDemoData();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task InsertDemoData()
        {
            try
            {
                //get super admin user
                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin = await _userManager.FindByEmailAsync(_superAdminDefaultOptions.Email);


                // Insert UserType
                UserType ut1 = new UserType()
                {
                    UserTypeId = "0",
                    UserLevel = 0,
                    Name = "ผู้บริหารระบบ",
                    Description = "ผู้บริหารระบบ"
                };
                // Insert UserType
                UserType ut2 = new UserType()
                {
                    UserTypeId = "1",
                    UserLevel = 1,
                    Name = "ผู้ประสานงาน รพ.",
                    Description = "ผู้ประสานงาน รพ."
                };
                // Insert UserType
                UserType ut3 = new UserType()
                {
                    UserTypeId = "2",
                    UserLevel = 2,
                    Name = "ผู้ประสานงานกลุ่มแพทย์",
                    Description = "ผู้ประสานงานกลุ่มแพทย์"
                };
                // Insert UserType
                UserType ut4 = new UserType()
                {

                    UserTypeId = "3",
                    UserLevel = 3,
                    Name = "แพทย์",
                    Description = "แพทย์"
                };
                await _context.UserType.AddAsync(ut1);
                await _context.UserType.AddAsync(ut2);
                await _context.UserType.AddAsync(ut3);
                await _context.UserType.AddAsync(ut4);

                await _context.SaveChangesAsync();



                // Insert PrefixType
                PrefixType prefixtype1 = new PrefixType()
                {
                    Name = "นาย",
                    Description = "นาย"
                };
                PrefixType prefixtype2 = new PrefixType()
                {
                    Name = "นาง",
                    Description = "นาย"
                };

                PrefixType prefixtype3 = new PrefixType()
                {
                    Name = "น.ส.",
                    Description = "นาย"
                };
                PrefixType prefixtype4 = new PrefixType()
                {
                    Name = "นพ.",
                    Description = "นายแพทย์"
                };
                PrefixType prefixtype5 = new PrefixType()
                {
                    Name = "พญ.",
                    Description = "แพทย์หญิง"

                };

                await _context.PrefixType.AddAsync(prefixtype1);
                await _context.PrefixType.AddAsync(prefixtype2);
                await _context.PrefixType.AddAsync(prefixtype3);
                await _context.PrefixType.AddAsync(prefixtype4);
                await _context.PrefixType.AddAsync(prefixtype5);

                await _context.SaveChangesAsync();



                // Insert DiseaseType
                DiseaseType diseaseType1 = new DiseaseType()
                {
                    Name = "โรคหัวใจ"
                };
                DiseaseType diseaseType2 = new DiseaseType()
                {
                    Name = "โรคหลอดเลือดสมอง"
                };
                DiseaseType diseaseType3 = new DiseaseType()
                {
                    Name = "โรควัณโรค"
                };
                await _context.DiseaseType.AddAsync(diseaseType1);
                await _context.DiseaseType.AddAsync(diseaseType2);
                await _context.DiseaseType.AddAsync(diseaseType3);

                await _context.SaveChangesAsync();

                // Insert JobStatus
                JobStatus jobStatus1 = new JobStatus()
                {
                    Status = 1,
                    Description = "เปิด"
                };
                JobStatus jobStatus2 = new JobStatus()
                {
                    Status = 2,
                    Description = "โพสต์"
                };
                JobStatus jobStatus3 = new JobStatus()
                {
                    Status = 3,
                    Description = "คิว"
                };
                JobStatus jobStatus4 = new JobStatus()
                {
                    Status = 4,
                    Description = "ดำเนินการแล้ว"
                };

                await _context.JobStatus.AddAsync(jobStatus1);
                await _context.JobStatus.AddAsync(jobStatus2);
                await _context.JobStatus.AddAsync(jobStatus3);
                await _context.JobStatus.AddAsync(jobStatus4);

                await _context.SaveChangesAsync();


                // Insert Region
                Region r1 = new Region()
                {
                    RegionName = "ภาคกลาง"
                };
                Region r2 = new Region()
                {
                    RegionName = "ภาคตะวันออก"
                };
                Region r3 = new Region()
                {
                    RegionName = "ภาคตะวันออกเฉียงเหนือ"
                };
                Region r4 = new Region()
                {
                    RegionName = "ภาคเหนือ"
                };
                Region r5 = new Region()
                {
                    RegionName = "ภาคใต้"
                };
                Region r6 = new Region()
                {
                    RegionName = "ภาคตะวันตก"
                };

                await _context.Region.AddAsync(r1);
                await _context.Region.AddAsync(r2);
                await _context.Region.AddAsync(r3);
                await _context.Region.AddAsync(r4);
                await _context.Region.AddAsync(r5);
                await _context.Region.AddAsync(r6);

                await _context.SaveChangesAsync();



                // Insert Province

                var region = _context.Region.FirstOrDefault(x => x.RegionName == "ภาคกลาง");

                Province p1 = new Province()
                {

                    ProvinceThai = "กรุงเทพมหานคร",
                    ProvinceEng = "Bangkok",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p1);

                Province p2 = new Province()
                {

                    ProvinceThai = "สมุทรปราการ",
                    ProvinceEng = "Samut Prakarn",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p2);

                Province p3 = new Province()
                {

                    ProvinceThai = "นนทบุรี",
                    ProvinceEng = "Nonthaburi",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p3);

                Province p4 = new Province()
                {

                    ProvinceThai = "ปทุมธานี",
                    ProvinceEng = "Pathum Thani",
                    RegionId = region.RegionId

                };

                await _context.Province.AddAsync(p4);

                Province p5 = new Province()
                {

                    ProvinceThai = "พระนครศรีอยุธยา",
                    ProvinceEng = "Phra Nakhon Si Ayutthaya",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p5);
                Province p6 = new Province()
                {

                    ProvinceThai = "อ่างทอง",
                    ProvinceEng = "Ang Thong",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p6);
                Province p7 = new Province()
                {

                    ProvinceThai = "ลพบุรี",
                    ProvinceEng = "Lop Buri",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p7);
                Province p8 = new Province()
                {

                    ProvinceThai = "สิงห์บุรี",
                    ProvinceEng = "Sing Buri",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p8);
                Province p9 = new Province()
                {

                    ProvinceThai = "ชัยนาท",
                    ProvinceEng = "Chai Nat",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p9);
                Province p10 = new Province()
                {

                    ProvinceThai = "สระบุรี",
                    ProvinceEng = "Saraburi",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p10);

                Province p11 = new Province()
                {

                    ProvinceThai = "นครนายก",
                    ProvinceEng = "Nakhon Nayok",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p11);


                Province p12 = new Province()
                {

                    ProvinceThai = "นครสวรรค์",
                    ProvinceEng = "Nakhon Sawan",
                    RegionId = region.RegionId

                };

                await _context.Province.AddAsync(p12);

                Province p13 = new Province()
                {

                    ProvinceThai = "อุทัยธานี",
                    ProvinceEng = "Uthai Thani",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p13);

                Province p14 = new Province()
                {

                    ProvinceThai = "กำแพงเพชร",
                    ProvinceEng = "Kamphaeng Phet",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p14);
                Province p15 = new Province()
                {

                    ProvinceThai = "สุโขทัย",
                    ProvinceEng = "Sukhothai",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p15);
                Province p16 = new Province()
                {

                    ProvinceThai = "พิษณุโลก",
                    ProvinceEng = "Phitsanulok",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p16);
                Province p17 = new Province()
                {

                    ProvinceThai = "พิจิตร",
                    ProvinceEng = "Phichit",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p17);
                Province p18 = new Province()
                {

                    ProvinceThai = "เพชรบูรณ์",
                    ProvinceEng = "Phetchabun",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p18);

                Province p19 = new Province()
                {

                    ProvinceThai = "สุพรรณบุรี",
                    ProvinceEng = "Suphan Buri",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p19);
                Province p20 = new Province()
                {

                    ProvinceThai = "นครปฐม",
                    ProvinceEng = "Nakhon Pathom",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p20);
                Province p21 = new Province()
                {

                    ProvinceThai = "สมุทรสาคร",
                    ProvinceEng = "Samut Sakhon",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p21);

                Province p22 = new Province()
                {

                    ProvinceThai = "สมุทรสงคราม",
                    ProvinceEng = "Samut Songkhram",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p22);

                region = null;

                region = _context.Region.FirstOrDefault(x => x.RegionName == "ภาคตะวันตก");


                Province p23 = new Province()
                {

                    ProvinceThai = "ตาก",
                    ProvinceEng = "Tak",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p23);

                Province p24 = new Province()
                {

                    ProvinceThai = "ราชบุรี",
                    ProvinceEng = "Ratchaburi",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p24);
                Province p25 = new Province()
                {

                    ProvinceThai = "กาญจนบุรี",
                    ProvinceEng = "กาญจนบุรี",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p25);

                Province p26 = new Province()
                {

                    ProvinceThai = "เพชรบุรี",
                    ProvinceEng = "Phetchaburi",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p26);

                Province p27 = new Province()
                {

                    ProvinceThai = "ประจวบคีรีขันธ์",
                    ProvinceEng = "Prachuap Khiri Khan",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p27);

                region = null;
                region = _context.Region.FirstOrDefault(x => x.RegionName == "ภาคตะวันออก");

                Province p28 = new Province()
                {

                    ProvinceThai = "ชลบุรี",
                    ProvinceEng = "Chon Buri",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p28);

                Province p29 = new Province()
                {

                    ProvinceThai = "ระยอง",
                    ProvinceEng = "Rayong",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p29);
                Province p30 = new Province()
                {

                    ProvinceThai = "จันทบุรี",
                    ProvinceEng = "Chanthaburi",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p30);
                Province p31 = new Province()
                {

                    ProvinceThai = "ตราด",
                    ProvinceEng = "Trat",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p31);
                Province p32 = new Province()
                {

                    ProvinceThai = "ฉะเชิงเทรา",
                    ProvinceEng = "Chachoengsao",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p32);
                Province p33 = new Province()
                {

                    ProvinceThai = "ปราจีนบุรี",
                    ProvinceEng = "Prachin Buri",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p33);

                Province p34 = new Province()
                {

                    ProvinceThai = "สระแก้ว",
                    ProvinceEng = "Sa kaeo",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p34);

                region = null;
                region = _context.Region.FirstOrDefault(x => x.RegionName == "ภาคตะวันออกเฉียงเหนือ");

                Province p35 = new Province()
                {

                    ProvinceThai = "นครราชสีมา",
                    ProvinceEng = "Nakhon Ratchasima",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p35);
                Province p36 = new Province()
                {

                    ProvinceThai = "บุรีรัมย์",
                    ProvinceEng = "Buri Ram",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p36);

                Province p37 = new Province()
                {

                    ProvinceThai = "สุรินทร์",
                    ProvinceEng = "Surin",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p37);

                Province p38 = new Province()
                {

                    ProvinceThai = "ศรีสะเกษ",
                    ProvinceEng = "Si Sa Ket",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p38);
                Province p39 = new Province()
                {

                    ProvinceThai = "อุบลราชธานี",
                    ProvinceEng = "Ubon Ratchathani",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p39);
                Province p40 = new Province()
                {

                    ProvinceThai = "ยโสธร",
                    ProvinceEng = "Yasothon",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p40);
                Province p41 = new Province()
                {

                    ProvinceThai = "ชัยภูมิ",
                    ProvinceEng = "Chaiyaphum",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p41);
                Province p42 = new Province()
                {

                    ProvinceThai = "อำนาจเจริญ",
                    ProvinceEng = "Amnat Charoen",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p42);
                Province p43 = new Province()
                {

                    ProvinceThai = "บึงกาฬ",
                    ProvinceEng = "Bueng Kan",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p43);
                Province p44 = new Province()
                {

                    ProvinceThai = "หนองบัวลำภู",
                    ProvinceEng = "Nong Bua Lam Phu",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p44);
                Province p45 = new Province()
                {

                    ProvinceThai = "ขอนแก่น",
                    ProvinceEng = "Khon Kaen",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p45);
                Province p46 = new Province()
                {

                    ProvinceThai = "อุดรธานี",
                    ProvinceEng = "Udon Thani",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p46);

                Province p47 = new Province()
                {

                    ProvinceThai = "เลย",
                    ProvinceEng = "Loei",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p47);

                Province p48 = new Province()
                {

                    ProvinceThai = "หนองคาย",
                    ProvinceEng = "Nong Khai",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p48);

                Province p49 = new Province()
                {

                    ProvinceThai = "มหาสารคาม",
                    ProvinceEng = "Maha Sarakham",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p49);

                Province p50 = new Province()
                {

                    ProvinceThai = "ร้อยเอ็ด",
                    ProvinceEng = "Roi Et",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p50);
                Province p51 = new Province()
                {

                    ProvinceThai = "กาฬสินธุ์",
                    ProvinceEng = "Kalasin",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p51);
                Province p52 = new Province()
                {

                    ProvinceThai = "สกลนคร",
                    ProvinceEng = "Sakon Nakhon",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p52);
                Province p53 = new Province()
                {

                    ProvinceThai = "นครพนม",
                    ProvinceEng = "Nakhon Phanom",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p53);
                Province p54 = new Province()
                {

                    ProvinceThai = "มุกดาหาร",
                    ProvinceEng = "Mukdahan",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p54);


                region = null;
                region = _context.Region.FirstOrDefault(x => x.RegionName == "ภาคใต้");

                Province p55 = new Province()
                {

                    ProvinceThai = "นครศรีธรรมราช",
                    ProvinceEng = "Nakhon Si Thammarat",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p55);

                Province p56 = new Province()
                {

                    ProvinceThai = "กระบี่",
                    ProvinceEng = "Krabi",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p56);
                Province p57 = new Province()
                {

                    ProvinceThai = "พังงา",
                    ProvinceEng = "Phang-nga",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p57);
                Province p58 = new Province()
                {

                    ProvinceThai = "ภูเก็ต",
                    ProvinceEng = "Phuket",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p58);

                Province p59 = new Province()
                {

                    ProvinceThai = "สุราษฎร์ธานี",
                    ProvinceEng = "Surat Thani",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p59);
                Province p60 = new Province()
                {

                    ProvinceThai = "ระนอง",
                    ProvinceEng = "Ranong",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p60);
                Province p61 = new Province()
                {

                    ProvinceThai = "ชุมพร",
                    ProvinceEng = "Ranong",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p61);
                Province p62 = new Province()
                {

                    ProvinceThai = "สงขลา",
                    ProvinceEng = "Songkhla",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p62);
                Province p63 = new Province()
                {

                    ProvinceThai = "สตูล",
                    ProvinceEng = "Satun",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p63);
                Province p64 = new Province()
                {

                    ProvinceThai = "ตรัง",
                    ProvinceEng = "Trang",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p64);
                Province p65 = new Province()
                {

                    ProvinceThai = "พัทลุง",
                    ProvinceEng = "Phatthalung",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p65);
                Province p66 = new Province()
                {

                    ProvinceThai = "ปัตตานี",
                    ProvinceEng = "Pattani",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p66);
                Province p67 = new Province()
                {

                    ProvinceThai = "ยะลา",
                    ProvinceEng = "Yala",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p67);
                Province p68 = new Province()
                {

                    ProvinceThai = "นราธิวาส",
                    ProvinceEng = "Narathiwat",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p68);
                region = null;
                region = _context.Region.FirstOrDefault(x => x.RegionName == "ภาคเหนือ");


                Province p69 = new Province()
                {

                    ProvinceThai = "เชียงใหม่",
                    ProvinceEng = "Chiang Mai",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p69);

                Province p70 = new Province()
                {

                    ProvinceThai = "ลำพูน",
                    ProvinceEng = "Lamphun",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p70);

                Province p71 = new Province()
                {

                    ProvinceThai = "ลำปาง",
                    ProvinceEng = "Lampang",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p71);

                Province p72 = new Province()
                {

                    ProvinceThai = "อุตรดิตถ์",
                    ProvinceEng = "Uttaradit",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p72);

                Province p73 = new Province()
                {

                    ProvinceThai = "แพร่",
                    ProvinceEng = "Phrae",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p73);

                Province p74 = new Province()
                {

                    ProvinceThai = "น่าน",
                    ProvinceEng = "Nan",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p74);

                Province p75 = new Province()
                {

                    ProvinceThai = "พะเยา",
                    ProvinceEng = "Phayao",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p75);

                Province p76 = new Province()
                {

                    ProvinceThai = "เชียงราย",
                    ProvinceEng = "Chiang Rai",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p76);

                Province p77 = new Province()
                {

                    ProvinceThai = "แม่ฮ่องสอน",
                    ProvinceEng = "Mae Hong Son",
                    RegionId = region.RegionId

                };
                await _context.Province.AddAsync(p77);

                await _context.SaveChangesAsync();


                // Insert DoctorType
                DoctorType t1 = new DoctorType()
                {
                    DoctorTypeName = "แพทย์กระดูก"
                };
                DoctorType t2 = new DoctorType()
                {
                    DoctorTypeName = "ศัลยแพทย์หัวใจ"
                };
                DoctorType t3 = new DoctorType()
                {
                    DoctorTypeName = "ศัลยแพทย์ทั่วไป"
                };
                DoctorType t4 = new DoctorType()
                {
                    DoctorTypeName = "สูตินรีแพทย์"
                };
                DoctorType t5 = new DoctorType()
                {
                    DoctorTypeName = "ศัลยแพทย์ช่องปาก"
                };
                await _context.DoctorType.AddAsync(t1);
                await _context.DoctorType.AddAsync(t2);
                await _context.DoctorType.AddAsync(t3);
                await _context.DoctorType.AddAsync(t4);
                await _context.DoctorType.AddAsync(t5);

                await _context.SaveChangesAsync();


                //insert designation
                Designation designationVPIT = new Designation()
                {
                    Name = "หัวหน้าห้องยา",
                    Description = ""
                };
                Designation designationHeadIT = new Designation()
                {
                    Name = "พยาบาลวิชาชีพ",
                    Description = ""
                };
                Designation designationSeniorManagerIT = new Designation()
                {
                    Name = "หัวหน้างานสารบรรณ",
                    Description = ""
                };
                Designation designationManagerIT = new Designation()
                {
                    Name = "ผู้จัดการสารสนเทศ",
                    Description = ""
                };

                await _context.Designation.AddAsync(designationVPIT);
                await _context.Designation.AddAsync(designationHeadIT);
                await _context.Designation.AddAsync(designationSeniorManagerIT);
                await _context.Designation.AddAsync(designationManagerIT);

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
