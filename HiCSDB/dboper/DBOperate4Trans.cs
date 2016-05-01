/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Transactions;

namespace HiCSDB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// 2016-05-01 添加IDispose的支持，在退出时，关闭数据库连接，防止有过时未关闭的连接
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    public sealed partial class DBOperate: IDisposable 
    {
        /// <summary>
        /// SQL SERVER
        /// </summary>
        public const int MSSQLSERVER = 1;

        /// <summary>
        /// OLEDB数据库。
        /// </summary>
        public const int OLEDB = 2;

        /// <summary>
        /// Oracle数据库。
        /// </summary>
        public const int ORACLE = 6;

        /// <summary>
        /// 执行事务的函数
        /// </summary>
        /// <param name="oper">数据库访问对象</param>
        /// <returns>true：成功，提交；false：失败，回滚</returns>
        public delegate bool TransHandler(DBOperate oper);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="iDBType"></param>
        /// <param name="isCloseAfterExecute"></param>
        public DBOperate(string connStr, int iDBType, bool isCloseAfterExecute = true)
        {
            IsCloseAfterExecute = isCloseAfterExecute;
            creator = GetCreator(iDBType);
            connString = connStr;
            dbType = iDBType;
            if (creator != null)
            {
                conn = creator.CreateConn(connStr);
            }
        }

        /// <summary>
        /// 对象释放时，关闭数据库连接
        /// 该方法未考虑多线程安全
        /// </summary>
        public void Dispose()
        {
            
            if (this.IsCloseAfterExecute)
            {
                return;
            }
            if (conn == null)
            {
                return;
            }
            Close();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public void Close()
        {
			this.Close(conn);
        }

        /// <summary>
        /// 执行事务（注意使用该函数会生成一个新的数据库操作对象病连接，在事务执行后，会关闭连接）
        /// </summary>
        /// <param name="handler">事务执行函数</param>
        public void OnTran(TransHandler handler)
        {
            DBOperate db = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                db = new DBOperate(connString, dbType, false);
                if (handler(db))
                {
                    scope.Complete();
                }
            }
            if (db != null)
            {
                db.Close();
            }
        }
		
		private DbConnection Conn
		{
			get
			{
				// 如果用完即关闭，则每次访问时，创建新的连接，
				// 目的是避免多线程访问时，conn成员被多次打开，
				// 或不该关闭时关闭  徐敏荣 2016-03-17
				if (!this.IsCloseAfterExecute)
                {
                    if (conn == null)
                    {
                        throw new Exception(string.Format("the database type({0}) is can't support,please call AddDBCreator<T>(int dbType) function set it's creator", dbType));
                    }

					return conn;
				}
				else
				{
					DbConnection connection = creator.CreateConn(connString);
					return connection;
				}
			}
		}

        private void CloseAfterExecute(DbConnection connection)
        {
            if (IsCloseAfterExecute)
            {
                this.Close(connection);
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private void Open(DbConnection connection)
        {
            if (connection.State.Equals(ConnectionState.Closed))
            {
                connection.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private void Close(DbConnection connection)
        {
            if (connection.State.Equals(ConnectionState.Open)) { connection.Close(); }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connString = "";

        /// <summary>
        /// 数据库类型
        /// </summary>
        private int dbType = 1;

        /// <summary>
        /// 执行完操作后，是否关闭数据库
        /// </summary>
        private bool IsCloseAfterExecute = true;

        /// <summary>
        /// 数据库连接对象。
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private DbConnection conn = null;

        /// <summary>
        /// ADO相关对象创建工具
        /// </summary>
        private IDBCreator creator = null;
    }
}


	

