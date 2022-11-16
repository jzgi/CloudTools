using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TencentSDK.Pay.Test
{
    [TestClass]
    public class BaseTenPayTest
    {
        protected ITenPaySetting tenPaySetting;
        public BaseTenPayTest()
        {
            ReadSetting();
        }
        public void ReadSetting()
        {
            string strData = "";
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//得先执行这句，不然无法识别encodeing类型
                //创建一个 StreamReader 的实例来读取文件 ,using 语句也能关闭 StreamReader
                string settingFileName = "appsettings.Test.json";
                if (!File.Exists(settingFileName))
                {
                    settingFileName = "appsettings.json";
                }
                using (System.IO.StreamReader sr = new System.IO.StreamReader(settingFileName, Encoding.Default))
                {
                    //从文件读取并显示行，直到文件的末尾
                    strData = sr.ReadToEnd();
                }
                tenPaySetting = JsonSerializer.Deserialize<TenPaySetting>(strData);
            }
            catch (Exception e)
            {
                // 出错了
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(strData);
        }
    }
}