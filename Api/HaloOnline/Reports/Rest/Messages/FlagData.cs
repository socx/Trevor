using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Domain
{
    public class FlagMessage
    {
        public FlagDataEntry Current { get; set; }
        public FlagDataEntry Previous { get; set; }
    }

    public class FlagDataEntry
    {
        public string Period { get; set; }
        public int AmberFlags { get; set; }
        public int RedFlags { get; set; }
    }
}