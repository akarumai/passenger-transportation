using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.IO;

namespace passenger_transportation
{
    public class StaffListExcelReport
    {
        public void Export_Excel(ObservableCollection<Staff> data)
        {
            InputWindow InputWindow = new InputWindow();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Лист1");
            string[] headers = { "Id", "Короткий Id", "Фамилия", "Имя", "Отчество", "Дата рождения", "Телефон", "Отдел" };
            for (int i = 1; i <= headers.Length; i++)
            {
                sheet.Cells[1, i].Value = headers[i - 1];
            }
            int currString = 2;
            foreach (var person in data)
            {
                sheet.Cells[currString, 1].Value = person.Id;
                sheet.Cells[currString, 2].Value = person.ShortId;
                sheet.Cells[currString, 3].Value = person.LastName;
                sheet.Cells[currString, 4].Value = person.Name;
                sheet.Cells[currString, 5].Value = person.Patronymic;
                sheet.Cells[currString, 6].Value = person.BirthdayDate;
                sheet.Cells[currString, 7].Value = person.ContactPhone;
                sheet.Cells[currString, 8].Value = person.Department;
                currString += 1;
            }
            for (int i = 1; i <= headers.Length; i++)
            {
                sheet.Column(i).AutoFit();
            }
            if (InputWindow.ShowDialog() == true)
            {
                string strPath = InputWindow.pathInput.Text;
                if (File.Exists(strPath))
                    File.Delete(strPath);
                FileStream objFileStrm = File.Create(strPath);
                objFileStrm.Close();
                File.WriteAllBytes(strPath, package.GetAsByteArray());

            }
            package.Dispose();
        }
    }
}
