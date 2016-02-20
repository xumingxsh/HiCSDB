/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-10-18">创建</log>

using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;

namespace Xumingxsh.DB
{
    internal sealed class OracleCreator : IDBCreator
    {
        public DbConnection CreateConn(string conn)
        {
            return new OracleConnection(conn);
        }

        public DbDataAdapter CreateDataAdapter(DbConnection conn, string sql)
        {
            return new OracleDataAdapter(sql, (OracleConnection)conn);
        }

        public DbParameter CreateParameter(string name, object value)
        {
            if (value == null)
            {
                return new OracleParameter(name, System.DBNull.Value);
            }
            return new OracleParameter(name, value);
        }
    }
}
