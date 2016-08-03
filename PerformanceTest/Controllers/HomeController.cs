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
            var viewModel = new CustomerViewModel { Customer = service.GetCustomers() };
            //var viewModel = new CustomerViewModel { Customer = service.GetCustomersByFunction() };
            return PartialView("GridViewPartialView", viewModel);
        }

        public ActionResult AggregateInfo(string filters)
        {
            var service = new CommonService();
            var source = service.GetCustomers();
            //var source = service.GetCustomersByFunction();
            CriteriaOperator filterCriteria = CriteriaOperator.Parse(filters);
            CriteriaToExpressionConverter converter = new CriteriaToExpressionConverter();

            if (!ReferenceEquals(filterCriteria, null))
            {
                source = source.AppendWhere(converter, filterCriteria) as IQueryable<CustomerDomainModel>;
            }

            source = source?.Where(x => x.PriorityName == "重要" || x.PriorityName == "紧急");
            var results = service.GetCustomerAggregateInfo(source);
            //var results = service.GetCustomerAggregateInfoByFunction(source);

            var viewModel = new CustomerViewModel { CustomerAggregateInfo = results.ToList() };

            return PartialView("AggregateInfo", viewModel);
        }
    }
}