using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

namespace HiCSDB
{
    /// <summary>
    /// 根据给定对象生成参数数组的辅助类
    /// </summary>
    public class DBParamHelper
    {
        /// <summary>
        /// 根据哈希表生成参数数组。
        /// </summary>
        /// <param name="db"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static DbParameter[] CreateParameters(DBOperate db, Hashtable hash)
        {
            int count = hash.Count;
            if (count < 1)
            {
                return null;
            }
            DbParameter[] paramers = new DbParameter[count];
            int i = 0;
            foreach (object key in hash.Keys)
            {
                paramers[i] = db.CreateParameter(Convert.ToString(key), hash[key]);
                i++;
            }
            return paramers;
        }

        /// <summary>
        /// 根据数据字典生成参数数组
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="db">生成参数数组的对象</param>
        /// <param name="dict">存储参数对象的字典</param>
        /// <returns>参数数组</returns>
        public static DbParameter[] CreateParameters<T>(DBOperate db, IDictionary<string, T> dict)
        {
            int count = dict.Count;
            if (count < 1)
            {
                return null;
            }
            DbParameter[] paramers = new DbParameter[count];
            int i = 0;
            foreach (KeyValuePair<string, T> it in dict)
            {
                paramers[i] = db.CreateParameter(it.Key, it.Value);
                i++;
            }
            return paramers;
        }

        /// <summary>
        /// 为不支持的类提供异常处理
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="db">生成参数数组的对象</param>
        /// <param name="t">包含生成参数数组所需属性的对象</param>
        /// <returns>参数数组</returns>
        public static DbParameter[] CreateParameters<T>(DBOperate db, T t)
        {
            throw new NotImplementedException("this type for CreateParameters is not support");
        }

        /// <summary>
        /// 根据DataRow生成参数数组。
        /// </summary>
        /// <param name="db">创建参数的对象</param>
        /// <param name="dr">包含参数的DataRow</param>
        /// <returns>参数数组</returns>
        public static DbParameter[] CreateParameters(DBOperate db, DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            int count = dr.Table.Columns.Count;
            if (count < 1)
            {
                return null;
            }

            DbParameter[] paramers;
            paramers = new DbParameter[count];
            for (int i = 0; i < count; i++)
            {
                paramers[i] = db.CreateParameter(
                    dr.Table.Columns[i].ColumnName,
                    dr[dr.Table.Columns[i].ColumnName]);
            }

            return paramers;
        }

        /// <summary>
        /// 根据DataTable生成参数数组（只使用第一行）
        /// </summary>
        /// <param name="db">生成参数的对象</param>
        /// <param name="dt">包含参数的DayaTable，只取第一行</param>
        /// <returns>参数数组</returns>
        public static DbParameter[] CreateParameters(DBOperate db, DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                return null;
            }
            return DBParamHelper.CreateParameters(db, dt.Rows[0]);
        }
    }
}
