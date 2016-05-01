/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Transactions;

namespace HiCSDB
{
    /// <summary>
    /// ���ݿ���������ࡣ
    /// 2016-05-01 ���IDispose��֧�֣����˳�ʱ���ر����ݿ����ӣ���ֹ�й�ʱδ�رյ�����
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
    public sealed partial class DBOperate: IDisposable 
    {
        /// <summary>
        /// SQL SERVER
        /// </summary>
        public const int MSSQLSERVER = 1;

        /// <summary>
        /// OLEDB���ݿ⡣
        /// </summary>
        public const int OLEDB = 2;

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
        /// ���캯��
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
        /// �����ͷ�ʱ���ر����ݿ�����
        /// �÷���δ���Ƕ��̰߳�ȫ
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
        /// �ر����ݿ�����
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public void Close()
        {
			this.Close(conn);
        }

        /// <summary>
        /// ִ������ע��ʹ�øú���������һ���µ����ݿ�����������ӣ�������ִ�к󣬻�ر����ӣ�
        /// </summary>
        /// <param name="handler">����ִ�к���</param>
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
				// ������꼴�رգ���ÿ�η���ʱ�������µ����ӣ�
				// Ŀ���Ǳ�����̷߳���ʱ��conn��Ա����δ򿪣�
				// �򲻸ùر�ʱ�ر�  ������ 2016-03-17
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
        /// �����ݿ�����
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private void Open(DbConnection connection)
        {
            if (connection.State.Equals(ConnectionState.Closed))
            {
                connection.Open();
            }
        }

        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private void Close(DbConnection connection)
        {
            if (connection.State.Equals(ConnectionState.Open)) { connection.Close(); }
        }

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
        /// ���ݿ����Ӷ���
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private DbConnection conn = null;

        /// <summary>
        /// ADO��ض��󴴽�����
        /// </summary>
        private IDBCreator creator = null;
    }
}


	

