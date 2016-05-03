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

        public DbParameter CreateParameter(string name, object value, bool isOut = false)
        {
            return DBCreatorHelper.CreateParameter<OleDbParameter>(name, value, isOut);
        }
        public DbParameter CreateParameter(string name)
        {
            return DBCreatorHelper.CreateParameter<OleDbParameter>(name);
        }
    }
}
