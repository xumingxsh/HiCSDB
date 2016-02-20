/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Data;
using System.Data.Common;
using System.Transactions;


namespace Xumingxsh.DB
{
    /// <summary>
    /// ���ݿ���������ࡣ
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
    public sealed partial class DBOperate
    {
        /// <summary>
        /// SQL SERVER
        /// </summary>
        public const int MSSQLSERVER = 1;

        /// <summary>
        /// MySQL���ݿ⡣
        /// </summary>
        public const int MySQL = 4;

        /// <summary>
        /// Oracle���ݿ⡣
        /// </summary>
        public const int ORACLE = 6;

        /// <summary>
        /// ִ������ĺ���
        /// </summary>
        /// <param name="oper">���ݿ���ʶ���</param>
        /// <returns>true���ɹ����ύ��false��ʧ�ܣ��ع�</returns>
        public delegate bool TransHandler(DBOperate oper);

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        private string connString = "";

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        private int dbType = 1;

        /// <summary>
        /// ִ����������Ƿ�ر����ݿ�
        /// </summary>
        private bool IsCloseAfterExecute = true;

        /// <summary>
        /// ADO��ض��󴴽�����
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
        /// ���ݿ����Ӷ���
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private DbConnection conn = null;

        private void CloseAfterExecute()
        {
            if (IsCloseAfterExecute)
            {
                this.Close();
            }
        }

        /// <summary>
        /// �����ݿ�����
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private void Open()
        {
            if (conn.State.Equals(ConnectionState.Closed))
            {
                conn.Open();
            }
        }

        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public void Close()
        {
            if (conn.State.Equals(ConnectionState.Open)) { conn.Close(); }
        }

        /// <summary>
        /// ִ������ע��ʹ�øú���������һ���µ����ݿ�����������ӣ�������ִ�к󣬻�ر����ӣ�
        /// </summary>
        /// <param name="handler">����ִ�к���</param>
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


	

