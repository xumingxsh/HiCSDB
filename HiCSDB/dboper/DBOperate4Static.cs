using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiCSDB
{
    /// <summary>
    /// 数据库操作抽象类。
    /// 添加支持创建creator的静态函数.
    /// 主要实现支持数据库的静态扩展。
    /// </summary>
    /// <author>天志</author>
    /// <log date="2016-05-01">创建</log>
    public sealed partial class DBOperate
    {
        delegate IDBCreator OnCreatorHandle();

        /// <summary>
        /// 添加新数据库类型的支持
        /// T:数据库创建类的类型
        /// </summary>
        /// <param name="dbType">数据库的报纸：不能重复</param>
        public static void AddDBCreator<T>(int dbType) where T : IDBCreator, new()
        {
            lock (thisLock)
            {
                if (Dic.ContainsKey(dbType))
                {
                    OnCreatorHandle it = Dic[dbType];
                    throw new Exception(string.Format(@"db type({0}) is already exist,
creator is ({1}),so you can,t set it", dbType, it.GetType().ToString()));
                }

                Dic[dbType] = Creator<T>;
            }
        }

        private static IDBCreator Creator<T>() where T : IDBCreator,new()
        {
            return new T();
        }

        private static IDBCreator GetCreator(int dbType)
        {
            if (!Dic.ContainsKey(dbType))
            {
                return null;
            }
            return Dic[dbType]();
        }

        static Dictionary<int, OnCreatorHandle> Dic
        {
            get
            {
                lock (thisLock)
                {
                    if (dic.Count < 1)
                    {
                        dic[MSSQLSERVER] = Creator<SqlServerCreator>;
                        dic[ORACLE] = Creator<OracleCreator>;
                        dic[OLEDB] = Creator<OleDBCreator>;
                    }
                }

                return dic;
            }
        }

        static Object thisLock = new Object();

        /// <summary>
        /// 数据库创建对象字典
        /// </summary>
        static Dictionary<int, OnCreatorHandle> dic = new Dictionary<int, OnCreatorHandle>();
    }
}
