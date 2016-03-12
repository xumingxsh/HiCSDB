/// <copyright>天志（六子）  1999-2015</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2015-03-04">创建</log>

using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace HiCSDB
{
    /// <summary>
    /// SqlServer数据库操作实现。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2015-03-04">创建</log>
    internal sealed class MySQLCreator : IDBCreator
    {
        public DbConnection CreateConn(string conn)
        {
            return new MySqlConnection(conn);
        }

        public DbDataAdapter CreateDataAdapter(DbConnection conn, string sql)
        {
            return new MySqlDataAdapter(sql, (MySqlConnection)conn);
        }

        public DbParameter CreateParameter(string name, object value)
        {
            if (value == null)
            {
                return new MySqlParameter(name, System.DBNull.Value);
            }
            return new MySqlParameter(name, value);
        }
    }
}
