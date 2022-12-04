using Microsoft.EntityFrameworkCore;
using System.Windows;

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
                db.SaveChanges();
            }
        }
        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
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
                    staff.BirthdayDate= StaffWindow.Staff.BirthdayDate;
                    staff.ContactPhone = StaffWindow.Staff.ContactPhone;
                    staff.Department = StaffWindow.Staff.Department;

                    db.SaveChanges();
                    staffList.Items.Refresh();
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
            db.SaveChanges();
        }
    }
}