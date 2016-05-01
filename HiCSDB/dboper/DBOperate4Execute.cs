/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;


namespace HiCSDB
{
    /// <summary>
    /// ���ݿ���������ࡣ
    /// 2016-05-02 ɾ��BatchUpdate�ӿ�(��������DataTable����),
    ///     �ýӿ�Ϊ�ǳ��ýӿ�,�����ṩ֧��
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
    partial class DBOperate
    {
        private void OnExecuteFinish(DbConnection connection, DbCommand cmdSql)
        {
            this.CloseAfterExecute(connection);
            cmdSql.Parameters.Clear();
        }
        /// <summary>
        /// ִ����ӣ��޸ģ�ɾ��֮��Ĳ�����
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return -1;
            }
			DbConnection connection = this.Conn;
			
            DbCommand cmdSql = this.GetPreCommand(connection, sql, parameters);
            try
            {
                // �����ݿ�����
                this.Open(connection);
                return cmdSql.ExecuteNonQuery();
            }
            finally
            {
                OnExecuteFinish(connection, cmdSql);
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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
			DbConnection connection = this.Conn;
				
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(connection, sql, parameters);

            try
            {
                // �����ݿ�����
                this.Open(connection);
                return cmdSql.ExecuteScalar();
            }
            finally
            {
                OnExecuteFinish(connection, cmdSql);
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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
			DbConnection connection = this.Conn;
			
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(connection, sql, parameters);

            try
            {

                // �����ݿ�����
                this.Open(connection);

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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
            //��ʼ��һ��DataAdapter����һ��DataTable����
            DataTable dt = new DataTable();
			
			DbConnection connection = this.Conn;			
            DbDataAdapter da = this.CreateDataAdapter(connection, sql);
            AddCmdParaers(da.SelectCommand, parameters);

            try
            {
                // �����ݿ�����
                this.Open(connection);

                da.Fill(dt);
                return dt;
            }
            finally
            {
                OnExecuteFinish(connection, da.SelectCommand);
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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
            //��ʼ��һ��DataSet����һ��DataAdapter����
            DataSet ds = new DataSet();
			
			DbConnection connection = this.Conn;	
            DbDataAdapter da = this.CreateDataAdapter(connection, sql);
            AddCmdParaers(da.SelectCommand, parameters);

            try
            {
                // �����ݿ�����
                this.Open(connection);
                da.Fill(ds);
                return ds;
            }
            finally
            {
                OnExecuteFinish(connection, da.SelectCommand);
            }
        }
    }
}


	

