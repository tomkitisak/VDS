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



                // Insert PrefixType
                PrefixType prefixtype1 = new PrefixType()
                {
                    Name = "นาย"
                };
                PrefixType prefixtype2 = new PrefixType()
                {
                    Name = "นาง"
                };

                PrefixType prefixtype3 = new PrefixType()
                {
                    Name = "นางสาว"
                };
                PrefixType prefixtype4 = new PrefixType()
                {
                    Name = "นายแพทย์"
                };
                PrefixType prefixtype5 = new PrefixType()
                {
                    Name = "แพทย์หญิง"
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
                    Description = "รอแพทย์"
                };
                JobStatus jobStatus3 = new JobStatus()
                {
                    Status = 3,
                    Description = "รอผ่าตัด"
                };
                JobStatus jobStatus4 = new JobStatus()
                {
                    Status = 4,
                    Description = "ผ่าตัดเรียบร้อย"
                };
                JobStatus jobStatus5 = new JobStatus()
                {
                    Status = 5,
                    Description = "ปิด"
                };
                JobStatus jobStatus6 = new JobStatus()
                {
                    Status = 9,
                    Description = "ยกเลิก"
                };

                await _context.JobStatus.AddAsync(jobStatus1);
                await _context.JobStatus.AddAsync(jobStatus2);
                await _context.JobStatus.AddAsync(jobStatus3);
                await _context.JobStatus.AddAsync(jobStatus4);
                await _context.JobStatus.AddAsync(jobStatus5);
                await _context.JobStatus.AddAsync(jobStatus6);

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



                //insert information type
                InformationType annualReportInformation = new InformationType()
                {
                    Name = "Annual Reports",
                    Description = ""
                };
                InformationType manualInformation = new InformationType()
                {
                    Name = "Manual and Handbooks",
                    Description = ""
                };
                InformationType newsletterInformation = new InformationType()
                {
                    Name = "Newsletters",
                    Description = ""
                };
                InformationType trainingInformation = new InformationType()
                {
                    Name = "Trainings and Seminars",
                    Description = ""
                };
                InformationType whitePapperInformation = new InformationType()
                {
                    Name = "White Pappers",
                    Description = ""
                };
                InformationType caseStudyInformation = new InformationType()
                {
                    Name = "Case Studies",
                    Description = ""
                };
                InformationType blogInformation = new InformationType()
                {
                    Name = "Blogs",
                    Description = ""
                };
                InformationType salesMaterialInformation = new InformationType()
                {
                    Name = "Brochures and Printed Sales Materials",
                    Description = ""
                };

                await _context.InformationType.AddAsync(annualReportInformation);
                await _context.InformationType.AddAsync(manualInformation);
                await _context.InformationType.AddAsync(newsletterInformation);
                await _context.InformationType.AddAsync(trainingInformation);
                await _context.InformationType.AddAsync(whitePapperInformation);
                await _context.InformationType.AddAsync(caseStudyInformation);
                await _context.InformationType.AddAsync(blogInformation);
                await _context.InformationType.AddAsync(salesMaterialInformation);

                await _context.SaveChangesAsync();




                //insert expense type
                ExpenseType marketingExpense = new ExpenseType()
                {
                    Name = "Marketing",
                    Description = ""
                };
                ExpenseType advertisingExpense = new ExpenseType()
                {
                    Name = "Advertising",
                    Description = ""
                };
                ExpenseType promotionExpense = new ExpenseType()
                {
                    Name = "Promotion",
                    Description = ""
                };
                ExpenseType trainingExpense = new ExpenseType()
                {
                    Name = "Training",
                    Description = ""
                };
                ExpenseType seminarExpense = new ExpenseType()
                {
                    Name = "Seminar",
                    Description = ""
                };
                ExpenseType projectExpense = new ExpenseType()
                {
                    Name = "Project",
                    Description = ""
                };
                ExpenseType transportationExpense = new ExpenseType()
                {
                    Name = "Transportation",
                    Description = ""
                };
                ExpenseType accomodationExpense = new ExpenseType()
                {
                    Name = "Accomodation",
                    Description = ""
                };
                ExpenseType mealExpense = new ExpenseType()
                {
                    Name = "Meal",
                    Description = ""
                };
                ExpenseType medicalExpense = new ExpenseType()
                {
                    Name = "Medical",
                    Description = ""
                };

                await _context.ExpenseType.AddAsync(marketingExpense);
                await _context.ExpenseType.AddAsync(advertisingExpense);
                await _context.ExpenseType.AddAsync(promotionExpense);
                await _context.ExpenseType.AddAsync(trainingExpense);
                await _context.ExpenseType.AddAsync(seminarExpense);
                await _context.ExpenseType.AddAsync(projectExpense);
                await _context.ExpenseType.AddAsync(transportationExpense);
                await _context.ExpenseType.AddAsync(accomodationExpense);
                await _context.ExpenseType.AddAsync(mealExpense);
                await _context.ExpenseType.AddAsync(medicalExpense);

                await _context.SaveChangesAsync();




                //insert ticket type
                TicketType attendanceTicket = new TicketType()
                {
                    Name = "Attendance",
                    Description = ""
                };
                TicketType payrollTicket = new TicketType()
                {
                    Name = "Payroll",
                    Description = ""
                };
                TicketType leaveTicket = new TicketType()
                {
                    Name = "Leave",
                    Description = ""
                };
                TicketType reimburseTicket = new TicketType()
                {
                    Name = "Reimburse",
                    Description = ""
                };
                TicketType miscellaneousTicket = new TicketType()
                {
                    Name = "Miscellaneous",
                    Description = ""
                };

                await _context.TicketType.AddAsync(attendanceTicket);
                await _context.TicketType.AddAsync(payrollTicket);
                await _context.TicketType.AddAsync(leaveTicket);
                await _context.TicketType.AddAsync(reimburseTicket);
                await _context.TicketType.AddAsync(miscellaneousTicket);


                await _context.SaveChangesAsync();

                //insert todo type
                TodoType payrollTodo = new TodoType()
                {
                    Name = "Payroll",
                    Description = ""
                };
                TodoType onboardingTodo = new TodoType()
                {
                    Name = "Onboarding",
                    Description = ""
                };
                TodoType recruitmentTodo = new TodoType()
                {
                    Name = "Recruitment",
                    Description = ""
                };

                await _context.TodoType.AddAsync(payrollTodo);
                await _context.TodoType.AddAsync(onboardingTodo);
                await _context.TodoType.AddAsync(recruitmentTodo);


                await _context.SaveChangesAsync();

                //insert department
                Department itDepartment = new Department()
                {
                    Name = "ฝ่ายสารสนเทศ",
                    Description = ""
                };
                Department hrDepartment = new Department()
                {
                    Name = "ฝ่ายทรัพยากรบุคคล",
                    Description = ""
                };
                Department financeDepartment = new Department()
                {
                    Name = "ฝ่ายการเงิน",
                    Description = ""
                };
                Department salesDepartment = new Department()
                {
                    Name = "ฝ่ายขาย",
                    Description = ""
                };
                Department warehouseDepartment = new Department()
                {
                    Name = "ฝ่ายคลังสินค้า",
                    Description = ""
                };

                await _context.Department.AddAsync(itDepartment);
                await _context.Department.AddAsync(hrDepartment);
                await _context.Department.AddAsync(financeDepartment);
                await _context.Department.AddAsync(salesDepartment);
                await _context.Department.AddAsync(warehouseDepartment);


                await _context.SaveChangesAsync();

                //insert designation
                Designation designationVPIT = new Designation()
                {
                    Name = "VP IT / COO (ExComm)",
                    Description = ""
                };
                Designation designationHeadIT = new Designation()
                {
                    Name = "Head of IT",
                    Description = ""
                };
                Designation designationSeniorManagerIT = new Designation()
                {
                    Name = "IT Senior Manager",
                    Description = ""
                };
                Designation designationManagerIT = new Designation()
                {
                    Name = "IT Manager",
                    Description = ""
                };
                Designation designationStaffIT = new Designation()
                {
                    Name = "IT Staff",
                    Description = ""
                };
                Designation designationVPFinance = new Designation()
                {
                    Name = "VP Finance / CEO (ExComm)",
                    Description = ""
                };
                Designation designationHeadFinance = new Designation()
                {
                    Name = "Head of Finance",
                    Description = ""
                };
                Designation designationSeniorManagerFinance = new Designation()
                {
                    Name = "Finance Senior Manager",
                    Description = ""
                };
                Designation designationManagerFinance = new Designation()
                {
                    Name = "Finance Manager",
                    Description = ""
                };
                Designation designationStaffFinance = new Designation()
                {
                    Name = "Finance Staff",
                    Description = ""
                };
                Designation designationVPHR = new Designation()
                {
                    Name = "VP HR (ExComm)",
                    Description = ""
                };
                Designation designationHeadHR = new Designation()
                {
                    Name = "Head of HR",
                    Description = ""
                };
                Designation designationSeniorManagerHR = new Designation()
                {
                    Name = "HR Senior Manager",
                    Description = ""
                };
                Designation designationManagerHR = new Designation()
                {
                    Name = "HR Manager",
                    Description = ""
                };
                Designation designationStaffHR = new Designation()
                {
                    Name = "HR Staff",
                    Description = ""
                };
                Designation designationVPSales = new Designation()
                {
                    Name = "VP Sales / CMO (ExComm)",
                    Description = ""
                };
                Designation designationHeadSales = new Designation()
                {
                    Name = "Head of Sales",
                    Description = ""
                };
                Designation designationSeniorManagerSales = new Designation()
                {
                    Name = "Sales Senior Manager",
                    Description = ""
                };
                Designation designationManagerSales = new Designation()
                {
                    Name = "Sales Manager",
                    Description = ""
                };
                Designation designationStaffSales = new Designation()
                {
                    Name = "Sales Staff",
                    Description = ""
                };
                Designation designationVPWarehouse = new Designation()
                {
                    Name = "VP Warehouse (ExComm)",
                    Description = ""
                };
                Designation designationHeadWarehouse = new Designation()
                {
                    Name = "Head of Warehouse",
                    Description = ""
                };
                Designation designationSeniorManagerWarehouse = new Designation()
                {
                    Name = "Warehouse Senior Manager",
                    Description = ""
                };
                Designation designationManagerWarehouse = new Designation()
                {
                    Name = "Warehouse Manager",
                    Description = ""
                };
                Designation designationStaffWarehouse = new Designation()
                {
                    Name = "Warehouse Staff",
                    Description = ""
                };

                await _context.Designation.AddAsync(designationVPIT);
                await _context.Designation.AddAsync(designationHeadIT);
                await _context.Designation.AddAsync(designationSeniorManagerIT);
                await _context.Designation.AddAsync(designationManagerIT);
                await _context.Designation.AddAsync(designationStaffIT);

                await _context.Designation.AddAsync(designationVPHR);
                await _context.Designation.AddAsync(designationHeadHR);
                await _context.Designation.AddAsync(designationSeniorManagerHR);
                await _context.Designation.AddAsync(designationManagerHR);
                await _context.Designation.AddAsync(designationStaffHR);

                await _context.Designation.AddAsync(designationVPFinance);
                await _context.Designation.AddAsync(designationHeadFinance);
                await _context.Designation.AddAsync(designationSeniorManagerFinance);
                await _context.Designation.AddAsync(designationManagerFinance);
                await _context.Designation.AddAsync(designationStaffFinance);

                await _context.Designation.AddAsync(designationVPSales);
                await _context.Designation.AddAsync(designationHeadSales);
                await _context.Designation.AddAsync(designationSeniorManagerSales);
                await _context.Designation.AddAsync(designationManagerSales);
                await _context.Designation.AddAsync(designationStaffSales);

                await _context.Designation.AddAsync(designationVPWarehouse);
                await _context.Designation.AddAsync(designationHeadWarehouse);
                await _context.Designation.AddAsync(designationSeniorManagerWarehouse);
                await _context.Designation.AddAsync(designationManagerWarehouse);
                await _context.Designation.AddAsync(designationStaffWarehouse);


                await _context.SaveChangesAsync();

                //insert public holiday 2019
                PublicHoliday publicHoliday2019 = new PublicHoliday();
                publicHoliday2019.Name = "2019 Holiday";
                publicHoliday2019.Description = "Federal and Regional";

                _context.PublicHoliday.Add(publicHoliday2019);
                await _context.SaveChangesAsync();

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "New Year's Day",
                    PublicHolidayDate = new DateTime(2019, 1, 1),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Martin Luther King Jr. Day",
                    PublicHolidayDate = new DateTime(2019, 1, 21),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "President's Day",
                    PublicHolidayDate = new DateTime(2019, 2, 18),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Mother's Day",
                    PublicHolidayDate = new DateTime(2019, 5, 12),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Memorial Day",
                    PublicHolidayDate = new DateTime(2019, 5, 27),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Father's Day",
                    PublicHolidayDate = new DateTime(2019, 6, 16),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Independence Day",
                    PublicHolidayDate = new DateTime(2019, 7, 4),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Labor Day",
                    PublicHolidayDate = new DateTime(2019, 9, 2),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Columbus Day",
                    PublicHolidayDate = new DateTime(2019, 10, 14),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "US Indigenous People's Day",
                    PublicHolidayDate = new DateTime(2019, 10, 14),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Veterans Day",
                    PublicHolidayDate = new DateTime(2019, 11, 11),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Thanksgiving",
                    PublicHolidayDate = new DateTime(2019, 11, 28),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Day after Thanksgiving",
                    PublicHolidayDate = new DateTime(2019, 11, 29),
                    PublicHolidayYear = 2019
                });

                _context.PublicHolidayLine.Add(new PublicHolidayLine
                {
                    PublicHoliday = publicHoliday2019,
                    Description = "Chirstmas Day",
                    PublicHolidayDate = new DateTime(2019, 12, 25),
                    PublicHolidayYear = 2019
                });

                await _context.SaveChangesAsync();







            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
