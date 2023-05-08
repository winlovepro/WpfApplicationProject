using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplicationProject.DataAccesses.EF;

namespace WpfApplicationProject.Utilities
{
    public class DataProvider
    {
        private static DataProvider _ins;

        public static DataProvider Ins
        {
            get
            {
                if(_ins == null) _ins = new DataProvider();

                return _ins;
            }

            set { _ins = value; }
        }

        public WpfDBContext DB { get; set; }

        private DataProvider()
        {
            DB = new WpfDBContext();
        }
    }
}
