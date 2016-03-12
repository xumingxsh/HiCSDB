/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Collections;
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
        private void OnExecuteFinish(DbCommand cmdSql)
        {
            this.CloseAfterExecute();
            cmdSql.Parameters.Clear();
        }
        /// <summary>
        /// 执行添加，修改，删除之类的操作。
        /// </summary>
        /// <param name="strSql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的条数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters = null)
        {
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);
            try
            {
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
        /// 返回结果集中第一行的第一列。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>返回对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public object ExecuteScalar(string sql, DbParameter[] parameters = null)
        {
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {
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
        /// 返回DataReader。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataReader对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters = null)
        {
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);

            try
            {

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
        /// 返回DataTable。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataTable对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters = null)
        {
            //初始化一个DataAdapter对象，一个DataTable对象
            DataTable dt = new DataTable();
            DbDataAdapter da = this.CreateDataAdapter(sql);
            AddCmdParaers(da.SelectCommand, parameters);
            /*
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(sql, parameters);*/

            try
            {
                //返回DataTable对象
                //da.SelectCommand = cmdSql;

                // 打开数据库连接
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
        /// 返回DataSet对象。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <param name="strTableName">操作表的名称</param>
        /// <returns>DataSet对象</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public DataSet ExecuteDataSet(string sql, DbParameter[] parameters = null)
        {
            //初始化一个DataSet对象，一个DataAdapter对象
            DataSet ds = new DataSet();
            DbDataAdapter da = this.CreateDataAdapter(sql);
            AddCmdParaers(da.SelectCommand, parameters);

            try
            {
                // 打开数据库连接
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


	

