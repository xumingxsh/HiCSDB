using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiCSDB;

namespace HiCSDBTest
{
    [TestClass]
    public class UnitTestDBOperate_MySQL
    {
        const int MySQL = 4;
        private string connString = "";
        public UnitTestDBOperate_MySQL()
        {
            connString = "Server=127.0.0.1;port=3306;Database=information_schema;Uid=root;Pwd=root;";

            DBOperate.AddDBCreator<MySQLCreator>(MySQL);
        }
        
        [TestMethod]
        public void Test_ExecuteDataTable()
        {
            DBOperate db = new DBOperate(connString, MySQL);
            DataTable dt = db.ExecuteDataTable("select table_name from tables limit 10");
            Assert.IsTrue(dt != null);
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void Test_ExecuteScalar()
        {
            DBOperate db = new DBOperate(connString, MySQL);
            object obj = db.ExecuteScalar("select table_name from tables limit 1");
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj is String);
        }

        [TestMethod]
        public void Test_ExecuteTrans()
        {
            DBOperate db = new DBOperate(connString, MySQL);
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
        
        /// <summary>
        /// 测试多数据插入
        /// </summary>
        [TestMethod] 
        public void Test_ExcuteTrans_Mulity()
        {
            string conn = "Server=127.0.0.1;port=3306;Database=test;Uid=root;Pwd=root;";
            string sql = @"CREATE TABLE mulity_test(
	id int not null auto_increment,
	name varchar(50) not null,
	typeid int NOT NULL,
   primary key (id)
) ";

            string insert = "insert into mulity_test(name,typeid) values('this is a test{0}',{0})";
            string delete = "delete from mulity_test";
            string drop = "drop table if exists mulity_test";
            string count_sql = "select count(1) from mulity_test";
            string info = "insert {0} to table({1}),time span:{2}";
            DBOperate db = new DBOperate(conn, MySQL, false);
            int ret = db.ExecuteNonQuery(drop);
            ret = db.ExecuteNonQuery(sql);
            object val = db.ExecuteScalar("select count(1) from mulity_test where id=0");
            Assert.IsTrue(Convert.ToInt32(val) == 0);

            DateTime dt;
            DateTime dt2;
            int count = 500;
            db.Close();
            dt = DateTime.Now;
            db.OnTran((DBOperate op) =>
            {
                for (int i = 0; i < count; i++)
                {
                    op.ExecuteNonQuery(string.Format(insert, i));
                }
                return true;
            });
            val = db.ExecuteScalar(count_sql);
            dt2 = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(
                info, "in Transaction", Convert.ToInt32(val), dt2 - dt);

            db.ExecuteNonQuery(delete);
            dt = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                db.ExecuteNonQuery(string.Format(insert, i));
            }
            val = db.ExecuteScalar(count_sql);
            dt2 = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(
                info, "not in Transaction", Convert.ToInt32(val), dt2 - dt);
                    
            db.ExecuteNonQuery(drop);
            db.Close();
        }

        [TestMethod]
        public void Test_Params()
        {
            DBOperate db = new DBOperate(connString, MySQL);
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
            DBOperate db = new DBOperate(connString, MySQL);
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
            DBOperate db = new DBOperate(connString, MySQL);
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
            DBOperate db = new DBOperate(connString, MySQL);
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
            DBOperate db = new DBOperate(connString, MySQL);
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
            DBOperate db = new DBOperate(connString, MySQL);
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
