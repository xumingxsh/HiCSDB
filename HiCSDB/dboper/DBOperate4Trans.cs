/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Data;
using System.Data.Common;
using System.Transactions;


namespace Xumingxsh.DB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    public sealed partial class DBOperate
    {
        /// <summary>
        /// SQL SERVER
        /// </summary>
        public const int MSSQLSERVER = 1;

        /// <summary>
        /// MySQL数据库。
        /// </summary>
        public const int MySQL = 4;

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
        /// ADO相关对象创建工具
        /// </summary>
        private IDBCreator creator = null;

        public DBOperate(string connString, int iDBType, bool isCloseAfterExecute)
        {
            this.IsCloseAfterExecute = isCloseAfterExecute;
            creator = DBFactory.GetCreator(iDBType);
            this.connString = connString;
            dbType = iDBType;
            conn = creator.CreateConn(connString);
        }

        public DBOperate(string connString, int iDBType)
            : this(connString, iDBType, true)
        {
        }

        /// <summary>
        /// 数据库连接对象。
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private DbConnection conn = null;

        private void CloseAfterExecute()
        {
            if (IsCloseAfterExecute)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private void Open()
        {
            if (conn.State.Equals(ConnectionState.Closed))
            {
                conn.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public void Close()
        {
            if (conn.State.Equals(ConnectionState.Open)) { conn.Close(); }
        }

        /// <summary>
        /// 执行事务（注意使用该函数会生成一个新的数据库操作对象病连接，在事务执行后，会关闭连接）
        /// </summary>
        /// <param name="handler">事务执行函数</param>
        public void OnTran(TransHandler handler)
        {
            DBOperate db = null;
            using (TransactionScope scope = new TransactionScope())
            {
                db = new DBOperate(connString, dbType, false);
                if (handler(db))
                {
                    scope.Complete();
                }
                //db.Close();
            }
            if (db != null)
            {
                db.Close();
            }
        }
    }
}


	

