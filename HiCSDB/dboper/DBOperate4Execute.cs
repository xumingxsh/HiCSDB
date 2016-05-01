/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;


namespace HiCSDB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    partial class DBOperate
    {
        private void OnExecuteFinish(DbConnection connection, DbCommand cmdSql)
        {
            this.CloseAfterExecute(connection);
            cmdSql.Parameters.Clear();
        }
        /// <summary>
        /// 执行添加，修改，删除之类的操作。
        /// </summary>
        /// <param name="sql">sql语句名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的条数</returns>
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
                // 打开数据库连接
                this.Open(connection);
                return cmdSql.ExecuteNonQuery();
            }
            finally
            {
                OnExecuteFinish(connection, cmdSql);
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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
			DbConnection connection = this.Conn;
				
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(connection, sql, parameters);

            try
            {
                // 打开数据库连接
                this.Open(connection);
                return cmdSql.ExecuteScalar();
            }
            finally
            {
                OnExecuteFinish(connection, cmdSql);
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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
			DbConnection connection = this.Conn;
			
            //初始化一个command对象
            DbCommand cmdSql = this.GetPreCommand(connection, sql, parameters);

            try
            {

                // 打开数据库连接
                this.Open(connection);

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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
            //初始化一个DataAdapter对象，一个DataTable对象
            DataTable dt = new DataTable();
			
			DbConnection connection = this.Conn;			
            DbDataAdapter da = this.CreateDataAdapter(connection, sql);
            AddCmdParaers(da.SelectCommand, parameters);

            try
            {
                // 打开数据库连接
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
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
            //初始化一个DataSet对象，一个DataAdapter对象
            DataSet ds = new DataSet();
			
			DbConnection connection = this.Conn;	
            DbDataAdapter da = this.CreateDataAdapter(connection, sql);
            AddCmdParaers(da.SelectCommand, parameters);

            try
            {
                // 打开数据库连接
                this.Open(connection);
                da.Fill(ds);
                return ds;
            }
            finally
            {
                OnExecuteFinish(connection, da.SelectCommand);
            }
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="dt">存储数据的DataTable</param>
        /// <param name="sql">SQL模板（含参数）</param>
        /// <param name="paramers">SQL参数与DataTable列的对应集合</param>
        public void BatchUpdate(DataTable dt, string sql, IDictionary<string, string> paramers)
        {
            if (dt == null || string.IsNullOrWhiteSpace(sql))
            {
                return;
            }
			DbConnection connection = this.Conn;
			
            // 初始化一个command对象
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.CommandType = UtilHelper.GetCommandType(sql);

            //指定各个参数的取值
            foreach (var it in paramers)
            {
                DbParameter param = creator.CreateParameter4DataTable(it.Key, it.Value);
                cmd.Parameters.Add(param);
            }

            DbDataAdapter adapter = creator.CreateDataAdapter();


            bool isAdd = false;
            string lowSql = sql.ToLower().Trim();
            if (lowSql.StartsWith("insert "))
            {
                adapter.InsertCommand = cmd;
                isAdd = true;
            }
            else if (lowSql.StartsWith("update "))
            {
                adapter.UpdateCommand = cmd;
            }
            else
            {
                throw new Exception("BatchUpdate function support sql start with insert or update");
            }

            foreach (DataRow it in dt.Rows)
            {
                it.AcceptChanges();
                if (isAdd)
                {
                    it.SetAdded();
                }
                else
                {
                    it.SetModified();
                }
            }
            try
            {
                //执行更新
                adapter.Update(dt.GetChanges());
                //使DataTable保存更新
                dt.AcceptChanges();
            }
            finally
            {
                if (isAdd)
                {
                    OnExecuteFinish(connection, adapter.InsertCommand);
                }
                else
                {
                    OnExecuteFinish(connection, adapter.UpdateCommand);
                }
            }
        }
    }
}


	

