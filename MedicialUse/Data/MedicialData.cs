using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicialUse.Data
{
    public class MedicalData
    {
        private static MedicialData _myViewModel;

        public static MedicialData MyViewModel
        {
            get
            {
                if (_myViewModel == null)
                    _myViewModel = new MedicialData();
                return _myViewModel;

            }
        }
    }


}

