using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xumingxsh.DB;

namespace HiCSDBTest
{
    [TestClass]
    public class UnitTestDBOperate_MySQL
    {
        private string connString = "";
        public UnitTestDBOperate_MySQL()
        {
            connString = "Server=127.0.0.1;port=3306;Database=information_schema;Uid=root;Pwd=root;";
        }
        
        [TestMethod]
        public void Test_ExecuteDataTable()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            DataTable dt = db.ExecuteDataTable("select table_name from tables limit 10");
            Assert.IsTrue(dt != null);
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void Test_ExecuteScalar()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            object obj = db.ExecuteScalar("select table_name from tables limit 1");
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
        }

        [TestMethod]
        public void Test_ExecuteTrans()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            db.OnTran((DBOperate op)=>{
                object val = op.ExecuteScalar("Select Count(1) from tables where table_name='CHARACTER_SETS'");
                Assert.IsTrue(Convert.ToInt16(val) == 1);

                try
                {
                    int result = op.ExecuteNonQuery("insert into tables() where table_name='CHARACTER_SETS'");
                    Assert.IsTrue(result == 1);
                }
                catch(Exception ex)
                {
                    ex.ToString();
                }
                return false;
            });
            object ret = db.ExecuteScalar("Select Count(1) from tables where table_name='CHARACTER_SETS'");
            Assert.IsTrue(Convert.ToInt16(ret) == 1);
        }
        
        [TestMethod]
        public void Test_Params()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            DbParameter[] parmter = new DbParameter[1];
            parmter[0] = db.CreateParameter("table_name", "CHARACTER_SETS");
            object obj = db.ExecuteScalar("select table_name from tables where table_name=?table_name", parmter);
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
            Assert.AreEqual(Convert.ToString(obj), "CHARACTER_SETS");
        }
        
        [TestMethod]
        public void Test_Params_DataRow()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            DataTable dt = new DataTable();
            dt.Columns.Add("table_name");
            DataRow dr = dt.Rows.Add();
            dr["table_name"] = "CHARACTER_SETS";
            DbParameter[] parmter = DBParamHelper.CreateParameters(db, dr);
            object obj = db.ExecuteScalar("select table_name from tables where table_name=?table_name", parmter);
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
            Assert.AreEqual(Convert.ToString(obj), "CHARACTER_SETS");
        }

        [TestMethod]
        public void Test_Params_DataTable()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            DataTable dt = new DataTable();
            dt.Columns.Add("table_name");
            DataRow dr = dt.Rows.Add();
            dr["table_name"] = "CHARACTER_SETS";
            DbParameter[] parmter = DBParamHelper.CreateParameters(db, dt);
            object obj = db.ExecuteScalar("select table_name from tables where table_name=?table_name", parmter);
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
            Assert.AreEqual(Convert.ToString(obj), "CHARACTER_SETS");
        }

        [TestMethod]
        public void Test_Params_Dictory()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("table_name", "CHARACTER_SETS");
            DbParameter[] parmter = DBParamHelper.CreateParameters(db, dic);
            object obj = db.ExecuteScalar("select table_name from tables where table_name=?table_name", parmter);
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
            Assert.AreEqual(Convert.ToString(obj), "CHARACTER_SETS");
        }

        [TestMethod]
        public void Test_Params_HashTable()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            Hashtable hs = new Hashtable();
            hs.Add("table_name", "CHARACTER_SETS");
            DbParameter[] parmter = DBParamHelper.CreateParameters(db, hs);
            object obj = db.ExecuteScalar("select table_name from tables where table_name=?table_name", parmter);
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
            Assert.AreEqual(Convert.ToString(obj), "CHARACTER_SETS");
        }

        [TestMethod]
        public void Test_Params_Other()
        {
            DBOperate db = new DBOperate(connString, DBOperate.MySQL);
            Hashtable hs = new Hashtable();
            hs.Add("table_name", "CHARACTER_SETS");
            try
            {
                int x = 0;
                DbParameter[] parmter = DBParamHelper.CreateParameters(db, x);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                ex.ToString();
                Assert.IsTrue(true);
            }
        }
    }
}
