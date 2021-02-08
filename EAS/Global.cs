using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAS
{
    static class Global
    {

        private static string _superName = "";

        public static string SuperName
        {
            get { return _superName; }
            set { _superName = value; }
        }
        private static string _superUser = "";

        public static string SuperUser
        {
            get { return _superUser; }
            set { _superUser = value; }
        }

        private static string _superAuth = "";

        public static string SuperAuth
        {
            get { return _superAuth; }
            set { _superAuth = value; }
        }
    }
}
