using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnline.ViewData.Services
{
    using Common;
    using Rest.Messages;

    public interface IViewDataServices
    {
        ViewDataMessage GetViewData(ViewDataRequest request);
        
    }
}
