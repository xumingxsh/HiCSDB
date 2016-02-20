/// <copyright>天志（六子）  1999-2007</copyright>
/// <version>1.0</version>
/// <author>天志</author>
/// <email></email>
/// <log date="2007-04-05">创建</log>

using System;
using System.Data;
using System.Data.Common;


namespace Xumingxsh.DB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2007-04-05">创建</log>
    public sealed partial class DBOperate
    {
        public delegate void TransHandler();

        public DBOperate(string connString, int iDBType, bool isCloseAfterExecute)
        {
            this.IsCloseAfterExecute = isCloseAfterExecute;
            creator = DBFactory.GetCreator(iDBType);
            conn = creator.CreateConn(connString);
        }

        public DBOperate(string connString, int iDBType)
        {
            this.IsCloseAfterExecute = true;
            creator = DBFactory.GetCreator(iDBType);
            conn = creator.CreateConn(connString);
        }

        private IDBCreator creator = null;

        /// <summary>
        /// 数据库连接对象。
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private DbConnection conn;

        /// <summary>
        /// 事务处理对象。
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private DbTransaction trans;

        /// <summary>
        /// 指示当前操作是否在事务中。
        /// </summary>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        private bool bInTrans = false;

        private bool IsCloseAfterExecute = true;

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
        /// 事务
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool OnTran(TransHandler handler)
        {
            if (this.bInTrans)
            {
                return false;
            }
            if (!this.bInTrans)
            {
                this.Open();
                trans = conn.BeginTransaction();
                bInTrans = true;
            }

            try
            {
                handler();
                trans.Commit();
                return true;
            }
            catch (System.Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                bInTrans = false;
                this.CloseAfterExecute();
            }
        }
    }
}


	

