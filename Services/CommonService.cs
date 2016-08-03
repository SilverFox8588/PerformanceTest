using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Domain;
using Repository;
using System.Linq;
using System.Reflection;

namespace Services
{
    public class CommonService
    {
        public IQueryable<CustomerDomainModel> GetCustomers()
        {
            var dataContext = new Entities();
            dataContext.Configuration.AutoDetectChangesEnabled = false;//关闭自动跟踪对象的属性变化
            dataContext.Configuration.LazyLoadingEnabled = false; //关闭延迟加载
            dataContext.Configuration.ProxyCreationEnabled = false; //关闭代理类
            dataContext.Configuration.ValidateOnSaveEnabled = false; //关闭保存时的实体验证
            dataContext.Configuration.UseDatabaseNullSemantics = true; //关闭数据库null比较行为
            var result = dataContext.Customers.AsNoTracking().Select(x =>
            new CustomerDomainModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                PriorityId = x.PriorityId,
                RegionId = x.RegionId,
                Type = x.Type,
                Cost = x.Cost,
                Hours = x.Hours,

                PriorityName = x.Priority != null ? x.Priority.Name : string.Empty,
                RegionName = x.Region.Name,

                FirstUser = new UserDomainModel
                {
                    Id = x.CustomerResponsibleUsers.FirstOrDefault().Id,
                    Name = x.CustomerResponsibleUsers.FirstOrDefault().LoginUser.Name,
                    CompanyName = x.CustomerResponsibleUsers.FirstOrDefault().LoginUser.CompanyName,
                    JobTitle = x.CustomerResponsibleUsers.FirstOrDefault().LoginUser.JobTitle,
                    HourlyWage = x.CustomerResponsibleUsers.FirstOrDefault().LoginUser.HourlyWage,
                    IsEnabled = x.CustomerResponsibleUsers.FirstOrDefault().LoginUser.IsEnabled
                }
            });

            return result;
        }

        public IQueryable<CustomerDomainModel> GetCustomersByFunction()
        {
            var dataContext = new Entities();
            var result = dataContext.GetCustomers().Select(x =>
            new CustomerDomainModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Phone = x.Phone,
                PriorityId = x.PriorityId,
                RegionId = x.RegionId,
                Type = x.Type,
                Cost = x.Cost,
                Hours = x.Hours,

                PriorityName = x.PriorityName,
                RegionName = x.RegionName,

                FirstUser = new UserDomainModel
                {
                    Name = x.UserName,
                    CompanyName = x.CompanyName,
                    JobTitle = x.JobTitle,
                    HourlyWage = x.HourlyWage.Value,
                    IsEnabled = x.IsEnabled.Value
                }
            });

            return result;
        }

        public List<CustomerAggregateInfo> GetCustomerAggregateInfoByFunction(IQueryable<CustomerDomainModel> source)
        {
            string sql = GetSqlStringByFunction(source);
            List<CustomerAggregateInfo> result;
            using (Entities context = new Entities())
            {
                string exeSql = string.Format(@"WITH [SourceCustomer] AS ({0}) 
                SELECT [RegionName], 
                    SUM([Hours]) [TotalHours],
                    SUM([Cost]) [TotalCost],
                    SUM(CASE WHEN [PriorityName] = N'{1}' AND [Type] = 0 THEN 1 ELSE 0 END) [Priority_紧急_Type_0],
                    SUM(CASE WHEN [PriorityName] = N'{1}' AND [Type] = 1 THEN 1 ELSE 0 END) [Priority_紧急_Type_1],
                    SUM(CASE WHEN [PriorityName] = N'{2}' AND [Type] = 0 THEN 1 ELSE 0 END) [Priority_重要_Type_0],
                    SUM(CASE WHEN [PriorityName] = N'{2}' AND [Type] = 1 THEN 1 ELSE 0 END) [Priority_重要_Type_1]
                FROM [SourceCustomer] GROUP BY [RegionName]", sql,
               "紧急", "重要");

                result = context.Database.SqlQuery<CustomerAggregateInfo>(exeSql).ToList();
            }

            return result;
        }

        public List<CustomerAggregateInfo> GetCustomerAggregateInfo(IQueryable<CustomerDomainModel> source)
        {
            string sql = GetSqlStringFromIQueryable(source);
            List<CustomerAggregateInfo> result;
            using (Entities context = new Entities())
            {
                string exeSql = string.Format(@"WITH [SourceCustomer] AS ({0}) 
                SELECT [Name1] AS [RegionName], 
                    SUM([Hours]) [TotalHours],
                    SUM([Cost]) [TotalCost],
                    SUM(CASE WHEN [C1] = N'{1}' AND [Type] = 0 THEN 1 ELSE 0 END) [Priority_紧急_Type_0],
                    SUM(CASE WHEN [C1] = N'{1}' AND [Type] = 1 THEN 1 ELSE 0 END) [Priority_紧急_Type_1],
                    SUM(CASE WHEN [C1] = N'{2}' AND [Type] = 0 THEN 1 ELSE 0 END) [Priority_重要_Type_0],
                    SUM(CASE WHEN [C1] = N'{2}' AND [Type] = 1 THEN 1 ELSE 0 END) [Priority_重要_Type_1]
                FROM [SourceCustomer] GROUP BY [Name1]", sql,
               "紧急", "重要");

                result = context.Database.SqlQuery<CustomerAggregateInfo>(exeSql).ToList();
            }

            return result;
        }

        private string GetSqlStringByFunction(IQueryable<CustomerDomainModel> source)
        {
            string sql = null;
            ObjectQuery<CustomerDomainModel> objectQuery = source as ObjectQuery<CustomerDomainModel>;
            if (objectQuery != null)
            {
                sql = objectQuery.ToTraceString();
                List<ObjectParameter> parameters = objectQuery.Parameters.ToList();

                parameters.ForEach(p =>
                {
                    string value = p.Value == null
                        ? "NULL"
                        : p.Value.ToString();

                    if (p.ParameterType == typeof(bool))
                    {
                        value = value.ToString().Equals("True", StringComparison.InvariantCultureIgnoreCase)
                            ? "1"
                            : "0";
                    }
                    else if (p.ParameterType == typeof(DateTime) || p.ParameterType == typeof(DateTime?))
                    {
                        value = string.Format("'{0}'", value);
                    }
                    else if (p.ParameterType == typeof(string))
                    {
                        value = string.Format("'{0}'", value);
                    }

                    sql = sql.Replace(string.Format("@{0}", p.Name), value);
                });
            }
            else
            {
                sql = GetSqlStringFromIQueryable(source);
            }

            return sql;
        }

        public string GetSqlStringFromIQueryable<TEntity>(IQueryable<TEntity> queryable) where TEntity : class
        {
            try
            {
                var dbQuery = queryable as DbQuery<TEntity>;

                // get the IInternalQuery internal variable from the DbQuery object
                var iqProp = dbQuery.GetType().GetProperty("InternalQuery", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                var iq = iqProp.GetValue(dbQuery);

                // get the ObjectQuery internal variable from the IInternalQuery object
                var oqProp = iq.GetType().GetProperty("ObjectQuery", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                var oq = oqProp.GetValue(iq);

                var objectQuery = oq as ObjectQuery<TEntity>;

                var sqlString = objectQuery.ToTraceString();

                foreach (var objectParam in objectQuery.Parameters)
                {
                    if (objectParam.ParameterType == typeof(string) || objectParam.ParameterType == typeof(DateTime) || objectParam.ParameterType == typeof(DateTime?))
                    {
                        sqlString = sqlString.Replace(string.Format("@{0}", objectParam.Name), string.Format("'{0}'", objectParam.Value.ToString()));
                    }
                    else if (objectParam.ParameterType == typeof(bool) || objectParam.ParameterType == typeof(bool?))
                    {
                        bool val;
                        if (Boolean.TryParse(objectParam.Value.ToString(), out val))
                        {
                            sqlString = sqlString.Replace(string.Format("@{0}", objectParam.Name), string.Format("{0}", val ? 1 : 0));
                        }
                    }
                    else
                    {
                        sqlString = sqlString.Replace(string.Format("@{0}", objectParam.Name), string.Format("{0}", objectParam.Value.ToString()));
                    }
                }

                return sqlString;
            }
            catch (Exception)
            {
                //squash it and just return ToString
                return queryable.ToString();
            }
        }
    }
}
