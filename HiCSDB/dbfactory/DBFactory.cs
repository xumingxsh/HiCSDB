using System;

namespace Xumingxsh.DB
{
    /// <summary>
    /// 数据库对象创建器的生产工厂
    /// </summary>
    internal class DBFactory
    {
        public static IDBCreator GetCreator(int type)
        {

            switch (type)
            {
                case DBOperate.MSSQLSERVER:
                    {
                        return new SqlServerCreator();
                    }
                case DBOperate.MySQL:
                    {
                        return new MySQLCreator();
                    }
                case DBOperate.ORACLE:
                    {
                        return new OracleCreator();
                    }
                default:
                    {
                        return new MySQLCreator();
                    }
            }
        }
    }
}
