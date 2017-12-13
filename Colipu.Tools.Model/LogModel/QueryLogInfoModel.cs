using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colipu.Tools.Model.LogModel
{
    public class QueryLogInfoModel
    {

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// APPID
        /// </summary>
        public string APPID { set; get; }

        /// <summary>
        /// Level
        /// </summary>
        public string Level { set; get; }

        /// <summary>
        /// Method
        /// </summary>
        public string Method { set; get; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// Title
        /// </summary>
        public string Group { set; get; }

        /// <summary>
        /// Index
        /// </summary>
        public string Index { set; get; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// Exception
        /// </summary>
        public string Exception { set; get; }

        /// <summary>
        /// ServerIP
        /// </summary>
        public string ServerIP { set; get; }

        /// <summary>
        /// StartLogDate
        /// </summary>
        public DateTime? StartLogDate { set; get; }

        /// <summary>
        /// EndLogDate
        /// </summary>
        public DateTime? EndLogDate { set; get; }

        /// <summary>
        /// 每页的条数
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { set; get; }
    }
}
