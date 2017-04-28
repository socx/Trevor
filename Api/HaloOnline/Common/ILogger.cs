using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnline.Common
{
    public interface ILogger
    {
        void LogError(Exception exception);
        void LogInfo(string message);
    }


}
