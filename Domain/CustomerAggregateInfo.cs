using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CustomerAggregateInfo
    {
        public string RegionName { get; set; }
        public int TotalCustomers { get; set; }
        public double TotalHours { get; set; }
        public double TotalCost { get; set; }
        public int Priority_紧急_Type_0 { get; set; }
        public int Priority_紧急_Type_1 { get; set; }
        public int Priority_重要_Type_0 { get; set; }
        public int Priority_重要_Type_1 { get; set; }
    }
}
