using System;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;

namespace HiCSDB 
{
    internal sealed class SqlServerCreator : IDBCreator 
	{
        public DbConnection CreateConn(string conn)
        { 
            return new SqlConnection(conn);
        }

        public DbDataAdapter CreateDataAdapter(DbConnection conn, string sql)
        {
            return new SqlDataAdapter(sql, (SqlConnection)conn);
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public DbParameter CreateParameter(string name, object value, bool isOut = false)
        {
            return DBCreatorHelper.CreateParameter<SqlParameter>(name, value, isOut);
        }
        public DbParameter CreateParameter(string name)
        {
            return DBCreatorHelper.CreateParameter<SqlParameter>(name);
        }
	}
}
