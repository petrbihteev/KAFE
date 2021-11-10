using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAFE
{
    public class Helper
    {
        private static CafeEntities _context;
        public static CafeEntities GetContext()
        {
            if (_context == null)
            {
                _context = new CafeEntities();
            }
            return _context;
        }
        public static int ID_user;
        public static int ID_order;
    }
}
