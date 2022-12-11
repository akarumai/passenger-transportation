using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO.Packaging;

namespace passenger_transportation
{
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        // при загрузке окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // гарантируем, что база данных создана
            db.Database.EnsureCreated();
            // загружаем данные из БД
            db.Staff.Load();
            // и устанавливаем данные в качестве контекста
            DataContext = db.Staff.Local.ToObservableCollection();
        }

        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow StaffWindow = new StaffWindow(new Staff());
            if (StaffWindow.ShowDialog() == true)
            {
                Staff User = StaffWindow.Staff;
                db.Staff.Add(User);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException exception) { }
               
            }
        }
        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                // получаем выделенный объект
                Staff? staff = staffList.SelectedItem as Staff;
                // если ни одного объекта не выделено, выходим
                if (staff is null) return;

                StaffWindow StaffWindow = new StaffWindow(new Staff
                {
                    Id = staff.Id,
                    ShortId = staff.ShortId,
                    LastName = staff.LastName,
                    Name = staff.Name,
                    Patronymic = staff.Patronymic,
                    BirthdayDate = staff.BirthdayDate,
                    ContactPhone = staff.ContactPhone,
                    Department = staff.Department
                });

                if (StaffWindow.ShowDialog() == true)
                {
                    // получаем измененный объект
                    staff = db.Staff.Find(StaffWindow.Staff.Id);
                    if (staff != null)
                    {
                        staff.ShortId = StaffWindow.Staff.ShortId;
                        staff.LastName = StaffWindow.Staff.LastName;
                        staff.Name = StaffWindow.Staff.Name;
                        staff.Patronymic = StaffWindow.Staff.Patronymic;
                        staff.BirthdayDate = StaffWindow.Staff.BirthdayDate;
                        staff.ContactPhone = StaffWindow.Staff.ContactPhone;
                        staff.Department = StaffWindow.Staff.Department;

                        db.SaveChanges();
                        staffList.Items.Refresh();
                    }
                }
            }
        }
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделенный объект
            Staff? user = staffList.SelectedItem as Staff;
            // если ни одного объекта не выделено, выходим
            if (user is null) return;
            db.Staff.Remove(user);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException exception) { }
        }
        // Экспорт в json
        private void Export_Json_Click(object sender, RoutedEventArgs e)
        {
            InputWindow InputWindow = new InputWindow();
            var staff = new List<JsonStaff>();
            foreach (var person in db.Staff.Local.ToObservableCollection())
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

            if (InputWindow.ShowDialog() == true)
            {
                string strPath = InputWindow.pathInput.Text;
                if (File.Exists(strPath))
                    File.Delete(strPath);
                File.WriteAllText(strPath, json);
            }
        }
        public class JsonStaff
        {
            [JsonProperty("staff")]
            public Staff[] Staff { get; set; }
        }
        // Экспорт в xlsx
        private void Export_Xlsx_Click(object sender, RoutedEventArgs e)
        {
            InputWindow InputWindow = new InputWindow();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Лист1");
            string[] headers = { "Id", "Короткий Id", "Фамилия", "Имя", "Отчество", "Дата рождения", "Телефон", "Отдел" };
            for (int i = 1; i <= headers.Length; i++)
            {
                sheet.Cells[1, i].Value = headers[i-1];
            }
            int currString = 2;
            foreach (var person in db.Staff.Local.ToObservableCollection())
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