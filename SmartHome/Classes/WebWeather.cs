using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Classes
{
    class WebWeather
    {

        /// <summary>
        /// 根据城市代码获取天气状况
        /// </summary>
        /// <param name="mCode">城市代码</param>
        public static WeatherStructure getWeatherByCode(string mCode)
        {
            //获取指定城市天气数据
            string wUrl = string.Format("http://www.weather.com.cn/data/cityinfo/{0}.html", mCode);
            HttpWebRequest mRequest = (HttpWebRequest)HttpWebRequest.Create(wUrl);
            HttpWebResponse mResponse = (HttpWebResponse)mRequest.GetResponse();
            mRequest.ContentType = "text/html";
            mRequest.Method = "Get";
            Stream Streams = mResponse.GetResponseStream();
            StreamReader mReader = new StreamReader(Streams, Encoding.UTF8);
            String mResult = mReader.ReadToEnd();
            mReader.Dispose();
            Streams.Dispose();
            mResponse.Close();
            //处理数据
            mResult = mResult.Substring(mResult.IndexOf(":") + 1, mResult.Length - mResult.IndexOf(":") - 2);
            //解析json
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(WeatherStructure));
            MemoryStream Stream = new MemoryStream(Encoding.Unicode.GetBytes(mResult));
            WeatherStructure mWeather = (WeatherStructure)Serializer.ReadObject(Stream);
            return mWeather;
        }

        #region 根据IP定位获得城市代码
        /// <summary>
        /// 根据IP定位获得城市代码
        /// </summary>
        /// <returns></returns>
        public static string getCodeByIP()
        {
            //根据IP地址获取用户所在城市
            string wUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip";
            HttpWebRequest mRequest = (HttpWebRequest)HttpWebRequest.Create(wUrl);
            HttpWebResponse mResponse = (HttpWebResponse)mRequest.GetResponse();
            mRequest.ContentType = "text/html";
            mRequest.Method = "Get";
            Stream Streams = mResponse.GetResponseStream();
            StreamReader mReader = new StreamReader(Streams, Encoding.UTF8);
            String mResult = mReader.ReadToEnd();
            mReader.Dispose();
            Streams.Dispose();
            mResponse.Close();
            //处理返回的json
            mResult = mResult.Substring(mResult.IndexOf("=") + 1, mResult.Length - mResult.IndexOf("=") - 2);
            //解析结果获得城市名
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(IP2Geo));
            MemoryStream Stream = new MemoryStream(Encoding.Unicode.GetBytes(mResult));
            IP2Geo Ip = (IP2Geo)Serializer.ReadObject(Stream);
            //根据城市名获得城市代码
            Xml4DB.XmlDB mDB = Xml4DB.XmlDBFactory.LoadXmlDB(Environment.CurrentDirectory.ToString()+"\\Config\\City.xml");
            List<object> mCitys = mDB.ReadByObject(new CityStructure(Ip.city, null));
            return ((CityStructure)mCitys[0]).CityCode ;
        }
        #endregion

        #region 内部辅助类
        /// <summary>
        /// IP到地理位置的实体类
        /// </summary>
        [DataContractAttribute]
        private class IP2Geo
        {
            [DataMember]
            public string ret{set;get;}
            [DataMember]
            public string start{set;get;}
            [DataMember]
            public string country{set;get;}
            [DataMember]
            public string province{set;get;}
            [DataMember]
            public string city{set;get;}
            [DataMember]
            public string isp{set;get;}
            [DataMember]
            public string type{get;set;}
            [DataMember]
            public string desc{get;set;}
        }

        /// <summary>
        /// 定义城市代码结构类
        /// </summary>
        private class CityStructure
        {
            public string CityName { get; set; }
            public string CityCode { get; set; }

            public CityStructure()
            {

            }

            public CityStructure(string mCityName,string mCityCode)
            {
                this.CityName = mCityName;
                this.CityCode = mCityCode;
            }
        }
        #endregion
    }
}
