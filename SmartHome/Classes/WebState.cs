using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Classes
{
    /// <summary>
    /// 判断网络通端情况的类
    /// </summary>
    public class WebState
    {
        /// <summary>
        /// 以调制器方式上网
        /// </summary>
        private const int INTERNET_CONNECTION_MODEM = 1;
        /// <summary>
        /// 以网卡方式上网
        /// </summary>
        private const int INTERNET_CONNECTION_LAN = 2; 

        /// <summary>
        /// 调用Win32API
        /// </summary>
        /// <param name="dwFlag"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("winInet.dll")] 
        private static extern bool InternetGetConnectedState( ref int dwFlag, int dwReserved ); 

        public static bool isConneted()
        {
            System.Int32 dwFlag = new int();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                return false;
            }
            else 
            {
                //通过解调器上网
                if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                {
                   return  true;
                }
                //通过网卡上网
                else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
