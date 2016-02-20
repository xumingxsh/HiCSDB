/// <copyright>��־�����ӣ�  1999-2007</copyright>
/// <version>1.0</version>
/// <author>��־</author>
/// <email></email>
/// <log date="2007-04-05">����</log>

using System;
using System.Data;
using System.Data.Common;


namespace Xumingxsh.DB
{
    /// <summary>
    /// ���ݿ���������ࡣ
    /// </summary>
    /// <author>��־</author>
    /// <log date="2007-04-05">����</log>
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
        /// ���ݿ����Ӷ���
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private DbConnection conn;

        /// <summary>
        /// ���������
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
        private DbTransaction trans;

        /// <summary>
        /// ָʾ��ǰ�����Ƿ��������С�
        /// </summary>
        /// <author>��־</author>
        /// <log date="2007-04-05">����</log>
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
        /// ����
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


	

