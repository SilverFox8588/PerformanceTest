using Domain;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceTest.Models
{
    public class CustomerViewModel
    {
        public IQueryable<CustomerDomainModel> Customer { get; set; }
        public List<CustomerAggregateInfo> CustomerAggregateInfo { get; set; }
    }
}