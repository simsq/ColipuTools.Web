
using Colipu.Common.Library;
using Colipu.Tools.Model.LogModel;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Colipu.Tools.Business
{
    public class LogBll
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        private static string ConnectionString = AesHelper.Decrypt(ConfigurationManager.ConnectionStrings["LogDB_Write"].ToString());

        #region private methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static MongoDatabase CreateDb()
        {

            MongoUrl mongoUrl = MongoUrl.Create(ConnectionString);
            mongoUrl.ConnectTimeout.Add(new TimeSpan(0, 1, 0));
            MongoClient client = new MongoClient(mongoUrl);
            MongoServer mongoServer = client.GetServer();//MongoServer.Create(mongoUrl);  
            //获取指定数据库
            MongoDatabase db = mongoServer.GetDatabase(mongoUrl.DatabaseName);
            return db;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbName"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static MongoCollection<T> CreateMongoCollection<T>(string tbName, MongoDatabase db = null)
        {
            if (db == null)
                db = CreateDb();
            //获取表
            MongoCollection<T> col = db.GetCollection<T>(tbName);
            return col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private static MongoCollection CreateMongoCollection(string tbName, MongoDatabase db = null)
        {
            if (db == null)
                db = CreateDb();
            //获取表
            MongoCollection col = db.GetCollection(tbName);
            return col;
        }
        #endregion

        private static bool _isRuning = false;
        public static void Jobs()
        {
            if (_isRuning) return;
            Action ac = () =>
            {
                while (true)
                {
                    try
                    {
                        MongoUrl mongoUrl = MongoUrl.Create(ConnectionString);
                        mongoUrl.ConnectTimeout.Add(new TimeSpan(0, 1, 0));
                        MongoClient client = new MongoClient(mongoUrl);
                        MongoServer mongoServer = client.GetServer();//MongoServer.Create(mongoUrl);   
                        var adminDb = mongoServer.GetDatabase("admin");
                        CommandDocument command = new CommandDocument("closeAllDatabases", 1);
                        var result = adminDb.RunCommand(command);
                        Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    Thread.Sleep(6 * 60 * 60 * 1000);
                }

            };
            ac.BeginInvoke(null, null);

            Action deleteLogs = () =>
            {
                while (true)
                {
                    try
                    {
                        var date = int.Parse(DateTime.Now.AddDays(-30).ToString("yyyyMMdd"));
                        var tabs = GetAllTable();
                        var db = CreateDb();
                        foreach (var tab in tabs)
                        {
                            var log = tab.Replace("logs_", string.Empty);
                            if (int.TryParse(log, out int d) && d < date)
                                db.DropCollection(tab);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    Thread.Sleep(12 * 60 * 60 * 1000);
                }
            };
            deleteLogs.BeginInvoke(null, null);
            _isRuning = true;
        }

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Tuple<List<LogInfo>, long> QueryLog(QueryLogInfoModel queryData)
        {
            var db = CreateDb();
            if (queryData.PageNum <= 0)
                queryData.PageNum = 1;
            if (queryData.PageSize <= 0 || queryData.PageSize > 100)
                queryData.PageSize = 20;
            //获取表
            MongoCollection<LogInfo> col = CreateMongoCollection<LogInfo>(queryData.TableName, db);
            List<IMongoQuery> queries = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(queryData.APPID))
                queries.Add(Query<LogInfo>.Where(item => item.APPID == queryData.APPID.Trim()));
            if (queryData.EndLogDate.HasValue)
                queries.Add(Query<LogInfo>.LTE(item => item.LogDate, queryData.EndLogDate));
            if (queryData.StartLogDate.HasValue)
                queries.Add(Query<LogInfo>.GTE(item => item.LogDate, queryData.StartLogDate));
            if (!string.IsNullOrEmpty(queryData.Exception))
                queries.Add(Query<LogInfo>.Where(item => item.Exception.Contains(queryData.Exception.Trim())));
            if (!string.IsNullOrEmpty(queryData.Level))
                queries.Add(Query<LogInfo>.EQ(item => item.Level, queryData.Level.Trim()));
            if (!string.IsNullOrEmpty(queryData.Message))
                queries.Add(Query<LogInfo>.Where(item => item.Message.Contains(queryData.Message.Trim())));
            if (!string.IsNullOrEmpty(queryData.Title))
                queries.Add(Query<LogInfo>.Where(item => item.Title.Contains(queryData.Title.Trim())));
            if (!string.IsNullOrEmpty(queryData.Index))
                queries.Add(Query<LogInfo>.Where(item => item.Index.Contains(queryData.Index.Trim())));
            if (!string.IsNullOrEmpty(queryData.Group))
                queries.Add(Query<LogInfo>.Where(item => item.Group.Contains(queryData.Group.Trim())));
            if (!string.IsNullOrEmpty(queryData.Method))
                queries.Add(Query<LogInfo>.Where(item => item.Method.Contains(queryData.Method.Trim())));
            if (!string.IsNullOrEmpty(queryData.ServerIP))
                queries.Add(Query<LogInfo>.EQ(item => item.ServerIP, queryData.ServerIP.Trim()));
            var finder = queries.Count == 0 ? col.FindAll() : col.Find(Query.And(queries));
            var skip = (queryData.PageNum - 1) * queryData.PageSize;
            var res = finder.SetSkip(skip).SetSortOrder(SortBy<LogInfo>.Descending(item => item.LogDate)).SetLimit(queryData.PageSize);
            Debug.WriteLine(res.Query);
            Debug.WriteLine(res.Options);
            var result = res.ToList();
            var total = queries.Count == 0 ? col.FindAll().Count() : (queryData.PageNum + (result.Count == queryData.PageSize ? 1 : 0)) * queryData.PageSize;
            return new Tuple<List<LogInfo>, long>(result, total);
        }

        /// <summary>
        /// 获取数据库状态
        /// </summary>
        /// <returns></returns>
        public static Tuple<DatabaseStatsResult, List<CollectionStatsResult>> GetStats()
        {
            var db = CreateDb();
            var dbStats = db.GetStats();
            var colsStats = GetAllTable().Select(o => db.GetCollection(o).GetStats()).ToList();
            return new Tuple<DatabaseStatsResult, List<CollectionStatsResult>>(dbStats, colsStats);
        }

        /// <summary>
        /// 获取所有的表名
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAllTable()
        {
            var db = CreateDb();
            return db.GetCollectionNames().Where(o => o.StartsWith("log")).OrderByDescending(o => o);
        }

        public static void AddIndexs(string tbName)
        {
            var db = CreateDb();
            MongoCollection<LogInfo> col = CreateMongoCollection<LogInfo>(tbName, db);
            col.CreateIndex(new IndexKeysDocument("", ""));
        }
        #endregion
    }
}
