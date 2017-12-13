using System.Linq;
using System.Web.Mvc;
using Colipu.Tools.Business;
using Colipu.Tools.Model.EasyuiModels;
using Colipu.Tools.Model.LogModel;

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
        public ActionResult GetAllTable()
        {
            var data = LogBll.GetAllTable();
            var index = 0;
            var trees = data.Select(item => new Tree { Id = (index++).ToString(), Text = item });
            return Json(trees);
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
        public ActionResult GetStats()
        {
            var data = LogBll.GetStats();
            return Json(new { DatabaseStats = data.Item1, CollectionStats = data.Item2 },JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}