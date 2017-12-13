using System.Linq;
using System.Web.Mvc;
using Colipu.Tools.Business;
using Colipu.Tools.Model.EasyuiModels;
using Colipu.Tools.Model.LogModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Colipu.Tools.Web.Controllers
{
    public class LogController : Controller
    {
        #region View
        // GET: Log
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region doAction

        /// <summary>
        /// GetAllTable
        /// </summary>
        /// <returns></returns>
        public string GetAllTable()
        {
            var data = LogBll.GetAllTable();
            var index = 0;
            var trees = data.Select(item => new Tree { Id = (index++).ToString(), Text = item });
            return JsonConvert.SerializeObject(trees,new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        /// <summary>
        /// QueryLog
        /// </summary>
        /// <param name="queryData1"></param>
        /// <returns></returns>
        public ActionResult QueryLog(QueryLogInfoModel queryData1)
        {
            var data = LogBll.QueryLog(queryData1);
            return Json(new { total = data.Item2, rows = data.Item1 });
        }

        /// <summary>
        /// GetStats
        /// </summary>
        /// <returns></returns>
        public string GetStats()
        {
            var data = LogBll.GetStats();
            var str = JsonConvert.SerializeObject(new { DatabaseStats = data.Item1, CollectionStats = data.Item2 });
            //return Json(new { DatabaseStats = data.Item1, CollectionStats = data.Item2 }, JsonRequestBehavior.AllowGet);
            return str;
        }


        #endregion


    }
}