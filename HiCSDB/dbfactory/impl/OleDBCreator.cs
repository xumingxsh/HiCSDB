using System;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace HiCSDB
{
    internal sealed class OleDBCreator : IDBCreator
    {
        public DbConnection CreateConn(string conn)
        {
            return new OleDbConnection(conn);
        }

        public DbDataAdapter CreateDataAdapter(DbConnection conn, string sql)
        {
            return new OleDbDataAdapter(sql, (OleDbConnection)conn);
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        public DbParameter CreateParameter(string name, object value)
        {
            return DBCreatorHelper.CreateParameter<OleDbParameter>(name, value);
        }

        public DbParameter CreateParameter4DataTable(string name, string source)
        {
            return DBCreatorHelper.CreateParameter4DataTable<OleDbParameter>(name, source);
        }
    }
}
