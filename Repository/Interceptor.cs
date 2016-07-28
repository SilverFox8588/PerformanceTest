using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;

namespace Repository
{
    /// <summary>
    /// Interceptor class.
    /// </summary>
    public class Interceptor : IDbCommandInterceptor
    {
        private const string SetArithAbortOn = "SET ARITHABORT ON;";
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            //if (command.CommandType != CommandType.Text || !(command is SqlCommand))
            //{
            //    return;
            //}

            //if (command.CommandText.StartsWith("select", StringComparison.OrdinalIgnoreCase) && !command.CommandText.Contains("option(recompile)"))
            //{
            //    command.CommandText = string.Format("{0}{1}", command.CommandText, " option(recompile)");
            //}

            //if (command.CommandType == CommandType.Text)
            //{
            //    command.CommandText = string.Format("{0}{1}", SetArithAbortOn, command.CommandText);
            //}
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }
    }
}
