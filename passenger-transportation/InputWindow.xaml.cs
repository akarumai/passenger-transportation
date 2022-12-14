using System.Windows;

namespace passenger_transportation
{
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
