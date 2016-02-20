/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Collections;
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
        private void OnExecuteFinish(DbCommand cmdSql)
        {
            // 如果不在事务中
            if (!this.bInTrans)
            {
                this.CloseAfterExecute();
            }

            cmdSql.Parameters.Clear();
        }
        #region 执行无返回值SQL语句
        /// <summary>
        /// 执行添加，修改，删除之类的操作。
        /// </summary>
        /// <param name="strSql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的条数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters)
        {
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);
            try
            {
                //判断是否在事务中
                if (this.bInTrans)
                {
                    cmdSql.Transaction = this.trans;
                }

                // 打开数据库连接
                this.Open();
                return cmdSql.ExecuteNonQuery();
            }
            finally
            {
                OnExecuteFinish(cmdSql);
            }
        }

        /// <summary>
        /// 执行添加，修改，删除之类的操作。
        /// </summary>
        /// <param name="strSql">sql语句名称</param>
        /// <returns>受影响的条数</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql ,null);
        }

        /// <summary>
        /// 执行添加，修改，删除之类的操作。
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

        #region 返回单个值

        /// <summary>
        /// 返回结果集中第一行的第一列。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>返回对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                //判断是否在事务中
                if (this.bInTrans)
                {
                    cmdSql.Transaction = this.trans;
                }

                // 打开数据库连接
                this.Open();
                return cmdSql.ExecuteScalar();
            }
            finally
            {
                OnExecuteFinish(cmdSql);
            }
        }

        /// <summary>
        /// 返回结果集中第一行的第一列。
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

        #region 返回DataReader

        /// <summary>
        /// 返回DataReader。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataReader对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                //判断是否在事务中
                if (this.bInTrans)
                {
                    cmdSql.Transaction = this.trans;
                }

                // 打开数据库连接
                this.Open();

                //返回DataReader对象
                return cmdSql.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                cmdSql.Parameters.Clear();
            }
        }

        /// <summary>
        /// 返回DataReader。
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

        #region 返回DataTable

        /// <summary>
        /// 返回DataTable。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataTable对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters)
        {
            //初始化一个DataAdapter对象，一个DataTable对象
            DataTable dt = new DataTable();
            DbDataAdapter da = this.CreateDataAdapter(sql);

            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                //返回DataTable对象
                da.SelectCommand = cmdSql;

                // 打开数据库连接
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
        /// 返回DataTable。
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

        #region 返回DataSet

        /// <summary>
        /// 返回DataSet对象。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="strTableName">操作表的名称</param>
        /// <returns>DataSet对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public DataSet ExecuteDataSet(string sql, DbParameter[] parameters)
        {
            //初始化一个DataSet对象，一个DataAdapter对象
            DataSet ds = new DataSet();
            DbDataAdapter da = this.CreateDataAdapter(sql);

            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
                // 返回DataSet对象
                da.SelectCommand = cmdSql;

                // 打开数据库连接
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
        /// 返回DataSet对象。
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


	

