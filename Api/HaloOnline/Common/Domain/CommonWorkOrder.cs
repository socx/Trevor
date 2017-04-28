using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Common.Domain
{
    public class CommonWorkOrder
    {
        public int WorkOrderID { get; set; }
        public string WorkOrderText { get; set; }
        public string LocationLevel1 { get; set; }
        public string LocationLevel2 { get; set; }
        public string LocationLevel3 { get; set; }
        public string LocationLevel4 { get; set; }
        public string SLA { get; set; }
        public string SLABreach { get; set; }
        public string SLAContract { get; set; }
        public string SLAService { get; set; }
        public string SLAServiceType { get; set; }
        public string Cause { get; set; }
        public string Problem { get; set; }
        public string Remedy { get; set; }
        public string Failure { get; set; }
    }
}