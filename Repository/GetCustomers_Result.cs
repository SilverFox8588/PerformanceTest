//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository
{
    using System;
    
    public partial class GetCustomers_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public byte Type { get; set; }
        public double Cost { get; set; }
        public double Hours { get; set; }
        public string CompanyName { get; set; }
        public Nullable<double> HourlyWage { get; set; }
        public Nullable<bool> IsEnabled { get; set; }
        public string JobTitle { get; set; }
        public string UserName { get; set; }
    }
}