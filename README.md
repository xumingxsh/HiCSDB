# HiCSDB
this is a DB operator program for C#,develop by VS2010.


数据库操作的抽象:
1: 抽象出一下几个接口:
ExecuteNonQuery: 执行操作
ExecuteScalar: 取得第一行第一列
ExecuteDataTable: 取得DataTable
ExecuteDataSet: 返回DataTable
2: SQL语句抽象
抽象为SQL语句和SQL参数
所以所有函数需要重载,或者是用默认参数:
(string sql, DbParameter[] parameters = null)
3: 提供事务支持
OnTran: 
需要的参数是一个以DBOperate为参数的委托
4: 支持
1) 多种数据库
2) 是否执行后立即关闭

这些信息放置在构造函数里面,连同连接字符串,有如下构造函数
DBOperate(string connStr, int iDBType, bool isCloseAfterExecute = true)
5: 需要支持数据库扩展:
这些可以是静态方法.
不同的数据库,支持不同的Command,Adapeter,Connection等,对这些进行抽象,得到一个接口:IDBCreator.
该接口实现方法: CreateConn,CreateDataAdapter,CreateParameter.
支持扩展数据库,需要实现该数据库的IDBCreator接口.
AddDBCreator<T>(int dbType)
