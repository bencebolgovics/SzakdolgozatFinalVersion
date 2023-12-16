using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Services.DatabaseServices
{
    public sealed class DbProvider
    {
        private static object _myLock = new object();
        private static DbAdapter _dbSingleton = null;

        private DbProvider() { }

        public static DbAdapter GetInstance()
        {
            if (_dbSingleton is null)
            {
                lock (_myLock)
                {
                    if (_dbSingleton is null)
                    {
                        _dbSingleton = new DbAdapter();
                    }
                }
            }

            return _dbSingleton;
        }
    }
}
