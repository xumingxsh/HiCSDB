using System;
using System.Data.Common;

namespace HiCSDB
{
    /// <summary>
    /// ADO.NET对象创建接口
    /// 2016-05-01 设置为公共接口，用户可以自己实现该类，以提供对某类数据库的支持
    /// </summary>
    public interface IDBCreator
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
        /// 创建适配器
        /// </summary>
        /// <returns></returns>
        DbDataAdapter CreateDataAdapter();

        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isOut"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name, object value, bool isOut = false);

        /// <summary>
        /// 创建参数对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name);
    }

    /// <summary>
    /// 2016-05-01 由于IDBCreator设置为外部可见类,故辅助类也设置为外部可见
    /// 该类为非必须类,以后可能会删除,或者降低可见范围
    /// </summary>
    public static class DBCreatorHelper
    {
        /// <summary>
        /// 创建SQL参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isOut"></param>
        /// <returns></returns>
        public static T CreateParameter<T>(string name, object value, bool isOut) where T : DbParameter,new()
        {
            T t = new T();
            t.ParameterName = name;
            t.Value = (value == null ? System.DBNull.Value : value);

            if (isOut)
            {
                t.Direction = System.Data.ParameterDirection.InputOutput;
            }
            return t;
        }

        /// <summary>
        /// 创建SQL参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T CreateParameter<T>(string name) where T : DbParameter, new()
        {
            T t = new T();
            t.ParameterName = name;
            t.Direction = System.Data.ParameterDirection.Output;
            return t;
        }
    }
}
