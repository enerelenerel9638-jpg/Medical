using MedicialUse.Data;
using Microsoft.VisualBasic;
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
    /// Interaction logic for Servers.xaml
    /// </summary>
    public partial class Service : Page
    {
        public Service()
        {
            InitializeComponent();
            List.ItemsSource = new List<string> { 
                "Бүтэн биеийн оношилгоо",
                "Хамар хоолой",
                "Шүд, амны хөндий",
                "Мэдрэл",
            
            };
        }

        private void List_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //string hh = MedicalData.MyViewModel.ServiceName; 

            var fe = e.OriginalSource as FrameworkElement;
            if (fe?.DataContext is string model)
            {
                MedicalData.MyViewModel.ServiceName = model;
                NavigationService.Navigate(new ChooseDate());
            }
        }


        
}
}

