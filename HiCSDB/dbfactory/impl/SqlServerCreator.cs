/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

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

        public DbParameter CreateParameter(string name, object value)
        {
            return DBCreatorHelper.CreateParameter<SqlParameter>(name, value);
        }

        public DbParameter CreateParameter4DataTable(string name, string source)
        {
            return DBCreatorHelper.CreateParameter4DataTable<SqlParameter>(name, source);
        }
	}
}
