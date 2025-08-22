using MedicialUse.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicialUse
{
    /// <summary>
    /// Interaction logic for ChooseDate.xaml
    /// </summary>
    public partial class ChooseDate : Page
    {
        private string _date;
        private string _time;

        public ChooseDate()
        {
            InitializeComponent();

            this.DataContext = MedicalData.MyViewModel;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _date = datePicker.SelectedDate.ToString();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
               _time = selectedItem.Content.ToString();
            }
        }

        private void Back_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MedicalData.MyViewModel.ChooseDates = _date;
            MedicalData.MyViewModel.Time = _time;

            NavigationService.Navigate(new PhoneNumber());
            //to do time viewmodel-d hadgalah
            

        }
    }
}
