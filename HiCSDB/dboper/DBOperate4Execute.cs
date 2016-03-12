/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Collections;
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
        private void OnExecuteFinish(DbCommand cmdSql)
        {
            this.CloseAfterExecute();
            cmdSql.Parameters.Clear();
        }
        /// <summary>
        /// ִ����ӣ��޸ģ�ɾ��֮��Ĳ�����
        /// </summary>
        /// <param name="strSql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters = null)
        {
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);
            try
            {
                // �����ݿ�����
                this.Open();
                return cmdSql.ExecuteNonQuery();
            }
            finally
            {
                OnExecuteFinish(cmdSql);
            }
        }

        /// <summary>
        /// ���ؽ�����е�һ�еĵ�һ�С�
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>���ض���</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public object ExecuteScalar(string sql, DbParameter[] parameters = null)
        {
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                // �����ݿ�����
                this.Open();
                return cmdSql.ExecuteScalar();
            }
            finally
            {
                OnExecuteFinish(cmdSql);
            }
        }

        /// <summary>
        /// ����DataReader��
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>DataReader����</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters = null)
        {
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {

                // �����ݿ�����
                this.Open();

                //����DataReader����
                return cmdSql.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                cmdSql.Parameters.Clear();
            }
        }

        /// <summary>
        /// ����DataTable��
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>DataTable����</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters = null)
        {
            //��ʼ��һ��DataAdapter����һ��DataTable����
            DataTable dt = new DataTable();
            DbDataAdapter da = this.CreateDataAdapter(sql);
            AddCmdParaers(da.SelectCommand, parameters);
            /*
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);*/

            try
            {
                //����DataTable����
                //da.SelectCommand = cmdSql;

                // �����ݿ�����
                this.Open();

                da.Fill(dt);
                return dt;
            }
            finally
            {
                OnExecuteFinish(da.SelectCommand);
            }
        }

        /// <summary>
        /// ����DataSet����
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <param name="strTableName">�����������</param>
        /// <returns>DataSet����</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public DataSet ExecuteDataSet(string sql, DbParameter[] parameters = null)
        {
            //��ʼ��һ��DataSet����һ��DataAdapter����
            DataSet ds = new DataSet();
            DbDataAdapter da = this.CreateDataAdapter(sql);
            AddCmdParaers(da.SelectCommand, parameters);

            try
            {
                // �����ݿ�����
                this.Open();
                da.Fill(ds);
                return ds;
            }
            finally
            {
                OnExecuteFinish(da.SelectCommand);
            }
        }
    }
}


	

