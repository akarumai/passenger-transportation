using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace passenger_transportation
{
    public class StaffListJsonReport
    {
        private class JsonStaff
        {
            [JsonProperty("staff")]
            public Staff[] Staff { get; set; }
        }
        public void Export_Json(ObservableCollection<Staff> data)
        {
            var staff = new List<JsonStaff>();
            foreach (var person in data)
            {
                var obj = new JsonStaff
                {
                    Staff = new Staff[]
                    {
                        new Staff
                        {
                         Id= person.Id,
                         ShortId = person.ShortId,
                         LastName = person.LastName,
                         Name = person.Name,
                         Patronymic = person.Patronymic,
                         BirthdayDate = person.BirthdayDate,
                         ContactPhone = person.ContactPhone,
                         Department = person.Department
                        }
                    }
                };
                staff.Add(obj);
            }
            var json = JsonConvert.SerializeObject(staff, Formatting.Indented);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDir.Contains("bin"))
            {
                int index = baseDir.IndexOf("bin");
                baseDir = baseDir.Substring(0, index);
            }
            string pathToDir = $"{baseDir}\\Reports";
            if (Directory.Exists(pathToDir) == false)
            {
                Directory.CreateDirectory(pathToDir);
            }
            File.WriteAllText($"{pathToDir}\\report.json", json);
        }
    }
}
