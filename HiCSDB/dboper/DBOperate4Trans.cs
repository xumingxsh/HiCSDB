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
    /// 2016-05-01 添加Dispose支持的十几分钟后有去除了,因为担心引起垃圾的第二次回收
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    public sealed partial class DBOperate//: IDisposable 
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
        
        private DbTransaction trans = null;

        /// <summary>
        /// 执行事务（注意使用该函数会生成一个新的数据库操作对象病连接，在事务执行后，会关闭连接）
        /// </summary>
        /// <param name="handler">事务执行函数</param>
        public void OnTran(Func<DBOperate, bool> handler)
        {
            DBOperate db = new DBOperate(connString, dbType, false);
            if (db.Conn == null)
            {
                return;
            }

            try
            {
                db.Conn.Open();
                db.trans = db.Conn.BeginTransaction();
                if (db.trans == null)
                {
                    return;
                }
                handler(db);
                db.trans.Commit();
            }
            catch (Exception ex)
            {
                if (db.trans != null)
                {
                    db.trans.Rollback();
                }
                throw ex;
            }
            finally
            {
                db.Close();
                db = null;
            }
        }

        /// <summary>
        /// 执行事务（注意使用该函数会生成一个新的数据库操作对象病连接，在事务执行后，会关闭连接）
        /// </summary>
        /// <param name="handler">事务执行函数</param>
        public bool OnTranEx(Func<DBOperate, bool> handler)
        {
            DBOperate db = new DBOperate(connString, dbType, false);
            if (db.Conn == null)
            {
                return false;
            }

            try
            {
                db.Conn.Open();
                db.trans = db.Conn.BeginTransaction();
                if (db.trans == null)
                {
                    return false;
                }
                if (handler(db))
                {
                    db.trans.Commit();
                    return true;
                }
                db.trans.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                if (db.trans != null)
                {
                    db.trans.Rollback();
                }
                throw ex;
            }
            finally
            {
                db.Close();
                db = null;
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
                        if (creator != null)
                        {
                            conn = creator.CreateConn(connString);
                        }
                        else
                        {
                            throw new Exception(string.Format("the database type({0}) is can't support,please call AddDBCreator<T>(int dbType) function set it's creator", dbType));
                        }
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


	

