using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Classes
{
    [DataContractAttribute]
    public class WeatherStructure
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        [DataMember]
        public string city{set;get;} 
        /// <summary>
        /// 城市ID
        /// </summary>
         [DataMember]
        public string cityid{set;get;}  
        /// <summary>
        ///  最低温度
        /// </summary>
         [DataMember]
        public string temp1{set;get;} 
        /// <summary>
        /// 最高温度
        /// </summary>
         [DataMember]
        public string temp2{set;get;} 
        /// <summary>
        /// 天气情况
        /// </summary>
        [DataMember]
        public string weather{set;get;} 
        /// <summary>
        /// 天气图标
        /// </summary>
        [DataMember]
        public string img1{set;get;}
        /// <summary>
        /// 发布时间
        /// </summary>
        [DataMember]
        public string ptime { set; get; }

        
    }
}
