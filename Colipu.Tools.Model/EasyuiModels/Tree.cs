using Newtonsoft.Json;
using System.Collections.Generic;

namespace Colipu.Tools.Model.EasyuiModels
{
    public class Tree
    {
        /// <summary>
        /// node id, which is important to load remote data
        /// </summary>     
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { set; get; }


        /// <summary>
        /// node text to show
        /// </summary>
        [JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { set; get; }

        /// <summary>
        ///  node state, 'open' or 'closed', default is 'open'. When set to 'closed', 
        ///  the node have children nodes and will load them from remote site
        /// </summary>
        [JsonProperty(PropertyName = "state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { set; get; }

        /// <summary>
        /// Indicate whether the node is checked selected..
        /// </summary>        
        [JsonProperty(PropertyName = "checked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Checked { set; get; }

        /// <summary>
        /// custom attributes can be added to a node
        /// </summary>      
        [JsonProperty(PropertyName = "attributes", NullValueHandling = NullValueHandling.Ignore)]
        public object Attributes { set; get; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>    
        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public List<Tree> Children { set; get; }
    }
}
