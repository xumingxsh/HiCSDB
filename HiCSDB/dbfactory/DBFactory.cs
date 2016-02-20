using System;

namespace Xumingxsh.DB
{
    /// <summary>
    /// 数据库对象创建器的生产工厂
    /// </summary>
    internal class DBFactory
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

        public static IDBCreator GetCreator(int type)
        {

            switch (type)
            {
                case DBFactory.MSSQLSERVER:
                    {
                        return new SqlServerCreator();
                    }
                case DBFactory.MySQL:
                    {
                        return new MySQLCreator();
                    }
                case DBFactory.ORACLE:
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
