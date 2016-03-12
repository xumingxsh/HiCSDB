using System;
using System.Data.Common;

namespace HiCSDB
{
    /// <summary>
    /// ADO.NET对象创建接口
    /// </summary>
    internal interface IDBCreator
    {
        /// <summary>
        /// 创建连接器
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        DbConnection CreateConn(string conn);

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        DbDataAdapter CreateDataAdapter(DbConnection conn, string sql);

        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name, object value);
    }
}
