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
    /// 2016-05-01 ���Dispose֧�ֵ�ʮ�����Ӻ���ȥ����,��Ϊ�������������ĵڶ��λ���
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
    public sealed partial class DBOperate//: IDisposable 
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
        /// �ر����ݿ�����
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        public void Close()
        {
			this.Close(conn);
        }

        private DbTransaction trans = null;

        /// <summary>
        /// ִ������ע��ʹ�øú���������һ���µ����ݿ�����������ӣ�������ִ�к󣬻�ر����ӣ�
        /// </summary>
        /// <param name="handler">����ִ�к���</param>
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
        /// ִ������ע��ʹ�øú���������һ���µ����ݿ�����������ӣ�������ִ�к󣬻�ر����ӣ�
        /// </summary>
        /// <param name="handler">����ִ�к���</param>
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
            catch(Exception ex)
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
                // ������꼴�رգ���ÿ�η���ʱ�������µ����ӣ�
                // Ŀ���Ǳ�����̷߳���ʱ��conn��Ա����δ򿪣�
                // �򲻸ùر�ʱ�ر�  ������ 2016-03-17
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


	

