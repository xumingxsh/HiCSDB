using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace HiCSDB
{
    /// <summary>
    /// ���ݿ���������ࡣ
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
    partial class DBOperate
    {
        private void AddCmdParaers(DbCommand cmd, DbParameter[] parameters)
        {
            if (parameters == null)
            {
                return;
            }

            //ָ������������ȡֵ
            foreach (DbParameter sqlParm in parameters)
            {
                cmd.Parameters.Add(sqlParm);
            }
        }

        private DbCommand GetPreCommand(DbConnection connection, string sql, DbParameter[] parameters)
        {
            // ��ʼ��һ��command����
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
        /// ���ɲ�����
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
        /// ���ɲ�����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name)
        {
            return creator.CreateParameter(name);
        }
    }
}


	

