/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace Xumingxsh.DB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    partial class DBOperate
    {
        #region 生成命令对象

        /// <summary>
        /// 获取一个OdbcCommand对象
        /// </summary>
        /// <param name="strSql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="strCommandType">命令类型</param>
        /// <returns>OdbcCommand对象</returns>
        private DbCommand GetPreCommand(string sql, DbParameter[] parameters)
        {
            // 初始化一个command对象
            DbCommand cmdSql = conn.CreateCommand();
            cmdSql.CommandText = sql;

            cmdSql.CommandType = GetCommandType(sql);

            // 判断是否在事务中
            if (this.bInTrans) { cmdSql.Transaction = this.trans; }

            if (parameters != null)
            {
                //指定各个参数的取值
                foreach (DbParameter sqlParm in parameters)
                {
                    cmdSql.Parameters.Add(sqlParm);
                }
            }

            return cmdSql;
        }
        #endregion

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DbDataAdapter CreateDataAdapter(string sql)
        {
            return creator.CreateDataAdapter(conn, sql);
        }

        #region 生成参数数组

        /// <summary>
        /// 生成参数。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isOut"></param>
        /// <returns></returns>
        private DbParameter CreateParameter(string name, object value)
        {
            return creator.CreateParameter(name, value);
        }

        /// <summary>
        /// 根据哈希表生成参数数组。
        /// </summary>
        /// <param name="paramDict"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private DbParameter[] CreateParameters(Hashtable hash)
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
                paramers[i] = this.CreateParameter(Convert.ToString(key), hash[key]);
                i++;
            }
            return paramers;
        }

        private DbParameter[] CreateParameters<T>(IDictionary<string, T> dict)
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
                paramers[i] = this.CreateParameter(it.Key, it.Value);
                i++;
            }
            return paramers;
        }

        private DbParameter[] CreateParameters<T>(T dr)
        {
            throw new Exception("this type for CreateParameters is not support");
        }

        /// <summary>
        /// 根据DataRow生成参数数组。
        /// </summary>
        /// <param name="paramDict"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DbParameter[] CreateParameters(DataRow dr)
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
                 paramers[i] = this.CreateParameter(
                     dr.Table.Columns[i].ColumnName, 
                     dr[dr.Table.Columns[i].ColumnName]);
            }

            return paramers;
        }

        private DbParameter[] CreateParameters(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                return null;
            }
            return CreateParameters(dt.Rows[0]);
        }
        #endregion


        /// <summary>
        /// 取得SQL语句的命令类型。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>命令类型</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public static CommandType GetCommandType(string sql)
        {
            //记录SQL语句的开始字符
            string topText = "";

            if (sql.Length > 7)
            {
                //取出字符串的前7位
                topText = sql.Substring(0, 7).ToUpper();

                // 如果不是存储过程
                if (topText.Equals("UPDATE ") || topText.Equals("INSERT ") ||
                    topText.Equals("DELETE ") || topText.Equals("ALTER T") ||
                    topText.Equals("ALTER  ") || topText.Equals("BACKUP ") ||
                    topText.Equals("RESTORE") || topText.Equals("SELECT "))
                {
                    return CommandType.Text;
                }
            }

            return CommandType.StoredProcedure;
        }
    }
}


	

