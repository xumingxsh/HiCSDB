/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace Xumingxsh.DB
{
    /// <summary>
    /// ���ݿ���������ࡣ
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
    partial class DBOperate
    {
        #region �����������

        /// <summary>
        /// ��ȡһ��OdbcCommand����
        /// </summary>
        /// <param name="strSql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <param name="strCommandType">��������</param>
        /// <returns>OdbcCommand����</returns>
        private DbCommand GetPreCommand(string sql, DbParameter[] parameters)
        {
            // ��ʼ��һ��command����
            DbCommand cmdSql = conn.CreateCommand();
            cmdSql.CommandText = sql;

            cmdSql.CommandType = GetCommandType(sql);

            // �ж��Ƿ���������
            if (this.bInTrans) { cmdSql.Transaction = this.trans; }

            if (parameters != null)
            {
                //ָ������������ȡֵ
                foreach (DbParameter sqlParm in parameters)
                {
                    cmdSql.Parameters.Add(sqlParm);
                }
            }

            return cmdSql;
        }
        #endregion

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DbDataAdapter CreateDataAdapter(string sql)
        {
            return creator.CreateDataAdapter(conn, sql);
        }

        #region ���ɲ�������

        /// <summary>
        /// ���ɲ�����
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
        /// ���ݹ�ϣ�����ɲ������顣
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
        /// ����DataRow���ɲ������顣
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
        /// ȡ��SQL�����������͡�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>��������</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public static CommandType GetCommandType(string sql)
        {
            //��¼SQL���Ŀ�ʼ�ַ�
            string topText = "";

            if (sql.Length > 7)
            {
                //ȡ���ַ�����ǰ7λ
                topText = sql.Substring(0, 7).ToUpper();

                // ������Ǵ洢����
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


	

