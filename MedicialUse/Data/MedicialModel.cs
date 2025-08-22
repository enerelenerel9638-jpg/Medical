using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicialUse.Data
{
    public class MedicialData : INotifyPropertyChanged
    {
        public MedicialData()
        {

        }
        private string _choosedate;
        private string _serviceName;
        private string _time;

        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public string ChooseDates
        {
            get => _choosedate;
            set
            {
                _choosedate = value;
                OnPropertyChanged(nameof(ChooseDates));
            }
        }


        public string ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value;
                OnPropertyChanged(nameof(ServiceName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

