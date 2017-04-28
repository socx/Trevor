using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Domain
{
    public class Recognition
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<string> Comments { get; set; }
    }
}