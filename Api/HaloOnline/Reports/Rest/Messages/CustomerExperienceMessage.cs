using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaloOnline.Reports.Rest.Messages
{
    using Common;

    public class CustomerExperienceMessage
    {
        public Periods Periods { get; set; }
        public List<CustomerExperienceChartDataEntry> ChartData { get; set; }
    }

    public class CustomerExperienceChartDataEntry
    {
        public SliderPrompts SliderPrompts { get; set; }
        public IEnumerable<CustomerExperiencePercentage> Percentages { get; set; }
        public List<dynamic> DynamicPercentages { get; set; }
        public CustomerExperienceAverage Averages { get; set; }
        public IEnumerable<CustomerExperienceComment> Comments { get; set; }
        public bool IsAmberFlag { get; set; }
    }

    public class SliderPrompts
    {
        public string Min { get; set; }
        public string Max { get; set; }
    }

    public class CustomerExperiencePercentage
    {
        public decimal Score { get; set; }
        public decimal Current { get; set; }
        public decimal Previous { get; set; }
    }

    public class CustomerExperienceAverage
    {
        public decimal Current { get; set; }
        public decimal Previous { get; set; }
    }

    public class CustomerExperienceComment
    {
        public int Score { get; set; }
        public string Flags { get; set; }
        public string RedFlag { get; set; }
        public string AmberFlag { get; set; }
        public string Comment { get; set; }
    }

}