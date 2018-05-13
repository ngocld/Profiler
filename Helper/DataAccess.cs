using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Profiler.Helpers
{
    public class clsDataAccess
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static int TimeoutSql
        {
            get
            {
                return 900;
            }
        }
        public static bool ExecuteSql(string query, string ConnectionSql)
        {
            var cmd = new SqlCommand();
            var sqlconn = new SqlConnection(ConnectionSql);
            try
            {
                sqlconn.Open();
                cmd.Connection = sqlconn;
                cmd.Transaction = sqlconn.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.CommandTimeout = TimeoutSql;
                Log.InfoFormat("Execute sql: {0}", query);

                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();

                cmd.Dispose();
                sqlconn.Close();

                return true;
            }
            catch (Exception exp)
            {
                Log.ErrorFormat("Exception: {0}", exp.Message);
                cmd.Transaction.Rollback();
                cmd.Dispose();
                sqlconn.Close();
                return false;
            }
        }
        public static bool ExecuteSqlNoTran(string query, string ConnectionSql)
        {
            var cmd = new SqlCommand();
            var sqlconn = new SqlConnection(ConnectionSql);
            try
            {
                sqlconn.Open();
                cmd.Connection = sqlconn;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.CommandTimeout = TimeoutSql;
                Log.InfoFormat("Execute sql: {0}", query);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                sqlconn.Close();

                return true;
            }
            catch (Exception exp)
            {
                Log.ErrorFormat("Exception: {0}", exp.Message);
                cmd.Dispose();
                sqlconn.Close();
                return false;
            }
        }
        public static bool CheckConnectSQL(string ConnectionSql)
        {
            var cmd = new SqlCommand();
            var sqlconn = new SqlConnection(ConnectionSql);
            try
            {
                Log.InfoFormat("Open connection to SQL Server");
                sqlconn.Open();
                sqlconn.Close();

                return true;
            }
            catch (Exception exp)
            {
                Log.ErrorFormat("Exception: {0}", exp.Message);
                cmd.Dispose();
                sqlconn.Close();
                return false;
            }
        }
        public static bool CheckUserIsSysAdmin(string ConnectionSql)
        {
            var cmd = new SqlCommand();
            var sqlconn = new SqlConnection(ConnectionSql);
            var rsData = new DataTable();
            var adpAdapter = new SqlDataAdapter();

            try
            {
                sqlconn.Open();
                cmd.Connection = sqlconn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "IF IS_SRVROLEMEMBER ('sysadmin') = 1 select 'IsSysAdmin' as IsSysAdmin";
                cmd.CommandTimeout = TimeoutSql;

                Log.InfoFormat("Execute sql: {0}", cmd.CommandText);
                adpAdapter.SelectCommand = cmd;
                adpAdapter.Fill(rsData);
                cmd.Dispose();
                sqlconn.Close();
                adpAdapter.Dispose();

                if (rsData.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception exp)
            {
                Log.ErrorFormat("Exception: {0}", exp.Message);
                cmd.Dispose();
                sqlconn.Close();
                adpAdapter.Dispose();
                return false;
            }
        }
        public static DataTable GetDataTableSql(string query, string ConnectionSql)
        {
            var cmd = new SqlCommand();
            var sqlconn = new SqlConnection(ConnectionSql);
            var rsData = new DataTable();
            var adpAdapter = new SqlDataAdapter();

            try
            {
                sqlconn.Open();
                cmd.Connection = sqlconn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.CommandTimeout = TimeoutSql;

                Log.InfoFormat("Execute sql: {0}", query);
                adpAdapter.SelectCommand = cmd;
                adpAdapter.Fill(rsData);
                cmd.Dispose();
                sqlconn.Close();
                adpAdapter.Dispose();
                return rsData;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                cmd.Dispose();
                sqlconn.Close();
                adpAdapter.Dispose();
                return null;
            }
        }
        public static DataRow GetDataRowSql(string query, string ConnectionSql)
        {
            var cmd = new SqlCommand();
            var sqlconn = new SqlConnection(ConnectionSql);
            var rsData = new DataTable();
            var adpAdapter = new SqlDataAdapter();

            try
            {
                sqlconn.Open();
                cmd.Connection = sqlconn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.CommandTimeout = TimeoutSql;

                Log.InfoFormat("Execute sql: {0}", query);
                adpAdapter.SelectCommand = cmd;
                adpAdapter.Fill(rsData);
                cmd.Dispose();
                sqlconn.Close();
                adpAdapter.Dispose();

                if (rsData.Rows.Count > 0)
                    return rsData.Rows[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                cmd.Dispose();
                sqlconn.Close();
                adpAdapter.Dispose();
                return null;
            }
        }
    }
}