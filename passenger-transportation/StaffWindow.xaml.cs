using System.Windows;

namespace passenger_transportation
{
    public partial class StaffWindow : Window
    {
        public Staff Staff { get; private set; }
        public StaffWindow(Staff staff)
        {
            InitializeComponent();
            Staff = staff;
            DataContext = Staff;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}