using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace HiCSDB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    partial class DBOperate
    {
        private void AddCmdParaers(DbCommand cmd, DbParameter[] parameters)
        {
            if (parameters == null)
            {
                return;
            }

            //指定各个参数的取值
            foreach (DbParameter sqlParm in parameters)
            {
                cmd.Parameters.Add(sqlParm);
            }
        }

        private DbCommand GetPreCommand(DbConnection connection, string sql, DbParameter[] parameters)
        {
            // 初始化一个command对象
            DbCommand cmdSql = connection.CreateCommand();
            cmdSql.CommandText = sql;

            cmdSql.CommandType = UtilHelper.GetCommandType(sql);

            AddCmdParaers(cmdSql, parameters);

            return cmdSql;
        }

        private DbDataAdapter CreateDataAdapter(DbConnection connection, string sql)
        {
            return creator.CreateDataAdapter(connection, sql);
        }

        /// <summary>
        /// 生成参数。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isOut"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, bool isOut = false)
        {
            return creator.CreateParameter(name, value, isOut);
        }

        /// <summary>
        /// 生成参数。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name)
        {
            return creator.CreateParameter(name);
        }
    }
}


	

