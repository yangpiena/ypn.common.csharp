using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ypn.common.csharp
{
    /// <summary>
    /// Http访问帮助类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// HttpWebRequest通过GET
        /// </summary>
        /// <param name="url">URI</param>
        /// <returns></returns>
        public static string GET(string url, string token = "")
        {
            DateTime v_StartDT = DateTime.Now;
            string responseContent = "";
            try
            {
                Console.WriteLine("\n");
                Console.WriteLine("YPN {0} GET   URL: {1}", v_StartDT , url);
                Console.WriteLine("YPN {0} GET token: {1}", v_StartDT , token);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                if (!StringHelper.IsNull(token)) httpWebRequest.Headers["Authorization"] = token;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";
                //对发送的数据不使用缓存
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                responseContent = streamReader.ReadToEnd();

                webRespon.Close();
                streamReader.Close();
            }
            catch (Exception e)
            {
                return JsonHelper.SerializeObjectToJson(new { code = "999", msg = e.Message });
            }
            Console.WriteLine("YPN {0} GET  耗时: {1} 秒", v_StartDT, DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return responseContent;
        }

        /// <summary>
        /// HttpWebRequest通过POST
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string POST(string url, string postData, string token = "")
        {
            DateTime v_StartDT = DateTime.Now;
            string result = "";
            try
            {
                Console.WriteLine("\n");
                Console.WriteLine("YPN {0} POST      url: {1}", v_StartDT, url);
                Console.WriteLine("YPN {0} POST postData: {1}", v_StartDT, postData);
                Console.WriteLine("YPN {0} POST    token: {1}", v_StartDT, token);
                HttpWebRequest req = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/json";
                req.AllowAutoRedirect = true;
                byte[] data = Encoding.UTF8.GetBytes(postData);//把字符串转换为字节
                req.ContentLength = data.Length;//请求长度
                if (!StringHelper.IsNull(token)) req.Headers["Authorization"] = token;
                //获取
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;//响应结果
                Stream stream = res.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return JsonHelper.SerializeObjectToJson(new { code = "999", msg = e.Message, retTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo) });
            }
            Console.WriteLine("YPN {0} POST     耗时: {1} 秒", v_StartDT, DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return result;
        }

        /// <summary>
        /// 通过POST模拟multipart/form-data提交带文件的表单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string PostFile(string url, string file, string cookie = "")
        {
            //1、数据边界
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //2、创建HttpWebRequest请求
            HttpWebRequest myRequest = WebRequest.Create(url) as HttpWebRequest;
            if (!StringHelper.IsNull(cookie)) myRequest.Headers["Cookie"] = cookie;
            myRequest.Method = "POST";
            //3、设置请求ContentType 和 边界字符（边界字符必须和请求数据体的边界字符一致 否则服务器无法解析）
            myRequest.ContentType = "multipart/form-data;boundary=" + boundary;

            //4、添加文件数据描述信息
            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary);
            sb.Append("\r\n");
            //name 为 上传文件的input name
            sb.Append("Content-Disposition: form-data; name='MAX_FILE_SIZE'; filename=\"" + file + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: application/octet-stream"); //此处则为模拟的文件类型，实际情况下浏览器会根据本地文件后缀名判断此类型
            sb.Append("\r\n\r\n");
            Encoding encoding = Encoding.GetEncoding("gbk"); //此处编码须与网页编码一直 否则导致中文路径或文件名乱码 但文件内容不会乱码
            byte[] form_data = encoding.GetBytes(sb.ToString());

            //5、表尾数据
            byte[] foot_data = encoding.GetBytes("\r\n--" + boundary + "--\r\n");


            //6、读取文件
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("--" + boundary);
                sb2.Append("\r\n");
                sb2.Append("Content-Disposition: form-data; name='username';\r\n"); //发送的内容标题
                sb2.Append("\r\n");
                sb2.Append("中文名称"); //发送的内容
                sb2.Append("\r\n"); //每一组数据结束都需要添加换行字符

                sb2.Append("--" + boundary);
                sb2.Append("\r\n");
                sb2.Append("Content-Disposition: form-data; name='pwd';\r\n");
                sb2.Append("\r\n");
                sb2.Append("QADSFDSFA");
                sb2.Append("\r\n");

                byte[] data = encoding.GetBytes(sb2.ToString());

                //6、设置上传数据长度为表头 + 文件 + 表尾长度
                myRequest.ContentLength = form_data.Length + foot_data.Length + fileStream.Length + data.Length;

                //7、得到请求的数据流
                Stream requestStream = myRequest.GetRequestStream();
                //8.1、将字符信息数据写入请求流
                requestStream.Write(data, 0, data.Length);
                //8.2、将文件信息数据写入请求流
                requestStream.Write(form_data, 0, form_data.Length);

                //9、循环读取文件流 并写入请求流
                byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                //10、将结束边界数据写入请求流
                requestStream.Write(foot_data, 0, foot_data.Length);
            }
            //11、发起请求
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            //12、读取请求返回的数据流
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), encoding);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 通过POST模拟multipart/form-data提交带文件的表单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string PostFile(string url, byte[] file, string cookie = "")
        {
            DateTime v_StartDT = DateTime.Now;
            string result = "";
            Console.WriteLine("\nypn....PostFile......url: {0}", url);
            Console.WriteLine("ypn....PostFile...cookie: {0}", cookie);
            try
            {
                //1、数据边界
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

                //2、创建HttpWebRequest请求
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                if (!StringHelper.IsNull(cookie)) request.Headers["Cookie"] = cookie;
                request.Method = "POST";

                //3、设置请求ContentType 和 边界字符（边界字符必须和请求数据体的边界字符一致 否则服务器无法解析）
                request.ContentType = "multipart/form-data;boundary=" + boundary;

                //4、添加文件数据描述信息
                StringBuilder sb = new StringBuilder();
                sb.Append("--" + boundary);
                sb.Append("\r\n");
                //name 为 上传文件的input name
                string filename = "file";
                sb.Append("Content-Disposition: form-data; name='MAX_FILE_SIZE'; filename=\"" + filename + "\"");
                sb.Append("\r\n");
                sb.Append("Content-Type: application/octet-stream"); //此处则为模拟的文件类型，实际情况下浏览器会根据本地文件后缀名判断此类型
                sb.Append("\r\n\r\n");
                Encoding encoding = Encoding.GetEncoding("gbk"); //此处编码须与网页编码一直 否则导致中文路径或文件名乱码 但文件内容不会乱码
                byte[] form_data = encoding.GetBytes(sb.ToString());

                //5、表尾数据
                byte[] foot_data = encoding.GetBytes("\r\n--" + boundary + "--\r\n");

                //6、读取文件
                //StringBuilder sb2 = new StringBuilder();
                //sb2.Append("--" + boundary);
                //sb2.Append("\r\n");
                //sb2.Append("Content-Disposition: form-data; name='username';\r\n"); //发送的内容标题
                //sb2.Append("\r\n");
                //sb2.Append("中文名称"); //发送的内容
                //sb2.Append("\r\n"); //每一组数据结束都需要添加换行字符
                //sb2.Append("--" + boundary);
                //sb2.Append("\r\n");
                //sb2.Append("Content-Disposition: form-data; name='pwd';\r\n");
                //sb2.Append("\r\n");
                //sb2.Append("QADSFDSFA");
                //sb2.Append("\r\n");

                //byte[] data = encoding.GetBytes(sb2.ToString());

                //6、设置上传数据长度为表头 + 文件 + 表尾长度
                //request.ContentLength = form_data.Length + foot_data.Length + file.Length + data.Length;
                request.ContentLength = form_data.Length + foot_data.Length + file.Length;

                //7、得到请求的数据流
                Stream requestStream = request.GetRequestStream();
                //8.1、将字符信息数据写入请求流
                //requestStream.Write(data, 0, data.Length);
                //8.2、将文件信息数据写入请求流
                requestStream.Write(form_data, 0, form_data.Length);

                //9、循环读取文件流 并写入请求流
                requestStream.Write(file, 0, file.Length);

                //10、将结束边界数据写入请求流
                requestStream.Write(foot_data, 0, foot_data.Length);

                //11、发起请求
                HttpWebResponse myResponse = request.GetResponse() as HttpWebResponse;

                //12、读取请求返回的数据流
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), encoding);
                result = sr.ReadToEnd();
            }
            catch (Exception e)
            {
                return JsonHelper.SerializeObjectToJson(new { code = "999", msg = e.Message });
            }
            Console.WriteLine("ypn....PostFile.....耗时: {0} 秒", DateTime.Now.Subtract(v_StartDT).TotalSeconds);
            return result;
        }

        #region 编码解码
        /// <summary>
        /// 解码得到url值
        /// </summary>
        /// <param name="url">string</param>
        /// <returns>string</returns>
        public static string UrlDecode(string url)
        {
            return HttpUtility.UrlDecode(url);
        }
        /// <summary>
        /// 编码传入url
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        #endregion
    }
}