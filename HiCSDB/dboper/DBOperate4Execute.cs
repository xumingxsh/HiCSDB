/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Collections;
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
        private void OnExecuteFinish(DbCommand cmdSql)
        {
            // �������������
            if (!this.bInTrans)
            {
                this.CloseAfterExecute();
            }

            cmdSql.Parameters.Clear();
        }
        #region ִ���޷���ֵSQL���
        /// <summary>
        /// ִ����ӣ��޸ģ�ɾ��֮��Ĳ�����
        /// </summary>
        /// <param name="strSql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters)
        {
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);
            try
            {
                //�ж��Ƿ���������
                if (this.bInTrans)
                {
                    cmdSql.Transaction = this.trans;
                }

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
        /// ִ����ӣ��޸ģ�ɾ��֮��Ĳ�����
        /// </summary>
        /// <param name="strSql">sql�������</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql ,null);
        }

        /// <summary>
        /// ִ����ӣ��޸ģ�ɾ��֮��Ĳ�����
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramDict"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public int ExecuteNonQuery<T>(string sql, T t)
        {
            DbParameter[] parameters = this.CreateParameters(t);
            return this.ExecuteNonQuery(sql, parameters);
        }
        #endregion

        #region ���ص���ֵ

        /// <summary>
        /// ���ؽ�����е�һ�еĵ�һ�С�
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>���ض���</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                //�ж��Ƿ���������
                if (this.bInTrans)
                {
                    cmdSql.Transaction = this.trans;
                }

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
        /// ���ؽ�����е�һ�еĵ�һ�С�
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramDict"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public object ExecuteScalar<T>(string sql, T t)
        {
            DbParameter[] parameters = this.CreateParameters(t);
            return this.ExecuteScalar(sql, parameters);
        }

        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }
        #endregion

        #region ����DataReader

        /// <summary>
        /// ����DataReader��
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>DataReader����</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                //�ж��Ƿ���������
                if (this.bInTrans)
                {
                    cmdSql.Transaction = this.trans;
                }

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
        /// ����DataReader��
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramDict"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader<T>(string sql, T t)
        {
            DbParameter[] parameters = this.CreateParameters(t);
            return this.ExecuteReader(sql, parameters);
        }

        public IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null);
        }
        #endregion

        #region ����DataTable

        /// <summary>
        /// ����DataTable��
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <returns>DataTable����</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters)
        {
            //��ʼ��һ��DataAdapter����һ��DataTable����
            DataTable dt = new DataTable();
            DbDataAdapter da = this.CreateDataAdapter(sql);

            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                //����DataTable����
                da.SelectCommand = cmdSql;

                // �����ݿ�����
                this.Open();

                da.Fill(dt);
                return dt;
            }
            finally
            {
                OnExecuteFinish(cmdSql);
            }
        }

        /// <summary>
        /// ����DataTable��
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramDict"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable<T>(string sql, T t)
        {
            DbParameter[] parameters = this.CreateParameters(t);
            return this.ExecuteDataTable(sql, parameters);
        }
        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, null);
        }
        #endregion

        #region ����DataSet

        /// <summary>
        /// ����DataSet����
        /// </summary>
        /// <param name="sql">sql�������</param>
        /// <param name="parameters">��������</param>
        /// <param name="strTableName">�����������</param>
        /// <returns>DataSet����</returns>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public DataSet ExecuteDataSet(string sql, DbParameter[] parameters)
        {
            //��ʼ��һ��DataSet����һ��DataAdapter����
            DataSet ds = new DataSet();
            DbDataAdapter da = this.CreateDataAdapter(sql);

            //��ʼ��һ��command����
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                // ����DataSet����
                da.SelectCommand = cmdSql;

                // �����ݿ�����
                this.Open();
                da.Fill(ds);
                return ds;
            }
            finally
            {
                OnExecuteFinish(cmdSql);
            }
        }

        /// <summary>
        /// ����DataSet����
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <param name="paramDict"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet<T>(string sql, T t)
        {
            DbParameter[] parameters = this.CreateParameters(t);
            return this.ExecuteDataSet(sql, parameters);
        }
        public DataSet ExecuteDataSet(string sql)
        {
            return ExecuteDataSet(sql);
        }
        #endregion   
    }
}


	

