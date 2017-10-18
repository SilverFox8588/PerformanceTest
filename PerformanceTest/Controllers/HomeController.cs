using System;
using System.Data.Entity.Core.Objects;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using Domain;
using PerformanceTest.Models;
using Services;
using System.Linq;
using System.Web.Mvc;

namespace PerformanceTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var service = new CommonService();
            var viewModel = new CustomerViewModel { Customer = service.GetCustomers() };
            return View(viewModel);
        }

        public ActionResult GridViewPartialView()
        {
            var service = new CommonService();
            //var viewModel = new CustomerViewModel { Customer = service.GetCustomers() };
            var viewModel = new CustomerViewModel { Customer = service.GetCustomersByFunction() };
            return PartialView("GridViewPartialView", viewModel);
        }

        public ActionResult AggregateInfo(string filters)
        {
            var service = new CommonService();
            var source = service.GetCustomers();
            CriteriaOperator filterCriteria = CriteriaOperator.Parse(filters);
            CriteriaToExpressionConverter converter = new CriteriaToExpressionConverter();

            if (!ReferenceEquals(filterCriteria, null))
            {
                source = source.AppendWhere(converter, filterCriteria) as IQueryable<CustomerDomainModel>;
            }

            source = source?.Where(x => x.PriorityName == "����" || x.PriorityName == "��Ҫ");
            var results = service.GetCustomerAggregateInfo(source);

            //var results = from c in source
            //              group c by c.RegionName
            //              into g
            //              select new CustomerAggregateInfo
            //              {
            //                  RegionName = g.Key,
            //                  TotalCustomers = g.Count(),
            //                  TotalHours = g.Sum(x => x.Hours),
            //                  TotalCost = g.Sum(x => x.Cost),
            //                  Priority_����_Type_0 = g.Count(x => x.PriorityName == "����" && x.Type == 0),
            //                  Priority_����_Type_1 = g.Count(x => x.PriorityName == "����" && x.Type == 1),
            //                  Priority_��Ҫ_Type_0 = g.Count(x => x.PriorityName == "��Ҫ" && x.Type == 0),
            //                  Priority_��Ҫ_Type_1 = g.Count(x => x.PriorityName == "��Ҫ" && x.Type == 1),
            //              };
            var viewModel = new CustomerViewModel { CustomerAggregateInfo = results.ToList() };

            return PartialView("AggregateInfo", viewModel);
        }

 
        public ActionResult PartialRefresh(int sectionid, string questions)
        {
            try
            {             
                return PartialView("test", string.Format("{0}_{1}", sectionid, questions));
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}