using System;
using System.Data;

namespace Xumingxsh.DB
{
    internal class UtilHelper
    {     
        /// <summary>
        /// 取得SQL语句的命令类型。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>命令类型</returns>
        /// <author>天志</author>
        /// <log date="2007-04-05">创建</log>
        public static CommandType GetCommandType(string sql)
        {
            //记录SQL语句的开始字符
            string topText = "";

            if (sql.Length > 7)
            {
                //取出字符串的前7位
                topText = sql.Substring(0, 7).ToUpper();

                // 如果不是存储过程
                if (topText.Equals("UPDATE ") || topText.Equals("INSERT ") ||
                    topText.Equals("DELETE ") || topText.Equals("ALTER T") ||
                    topText.Equals("ALTER  ") || topText.Equals("BACKUP ") ||
                    topText.Equals("RESTORE") || topText.Equals("SELECT "))
                {
                    return CommandType.Text;
                }
            }

            return CommandType.StoredProcedure;
        }
    }
}
