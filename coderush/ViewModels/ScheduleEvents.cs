using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace vds.ViewModels
{
    public class ScheduleEvents
    {
        public List<AppointmentData> GetAppointmentData()
        {
            List<AppointmentData> appData = new List<AppointmentData>();
            appData.Add(new AppointmentData
            {
                Id = 1,
                Subject = "นัด 1",
                Location = "รพ.เอบีซี",
                StartTime = Convert.ToDateTime(DateTime.Now.ToString( new CultureInfo("en-GB"))),
                EndTime = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-GB")).AddHours(5),
                CategoryColor = "#1aaa55"
            });
            appData.Add(new AppointmentData
            {
                Id = 2,
                Subject = "Thule Air Crash Report",
                Location = "Newyork City",
                StartTime = new DateTime(2019, 1, 7, 12, 0, 0),
                EndTime = new DateTime(2019, 1, 7, 14, 0, 0),
                CategoryColor = "#357cd2"
            });

            return appData;
        }

        public class AppointmentData
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public string Location { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string CategoryColor { get; set; }
        }
    }
}
