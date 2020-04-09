# region  版权所有
/* ================================================
 * 
 * 
 * 创建：2014-02-08
 * 描述：Rest 工具类
 * ================================================
 */
# endregion

using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace WZT.EC.Infrastructure
{
    /// <summary>
    /// Rest 工具类
    /// </summary>
    public class RestHelper
    {
        #region Get 方式提交数据

        /// <summary>
        /// Get 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        public static string GetData(string url)
        {
            return GetData(url, ContentType.Json);
        }

        /// <summary>
        /// Get 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        public static string GetData(Uri uri)
        {
            return GetData(uri, ContentType.Json);
        }

        /// <summary>
        /// Get 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        public static string GetData(string url, ContentType contentType)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            return GetData(request, contentType);
        }

        /// <summary>
        /// Get 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        public static string GetData(Uri uri, ContentType contentType)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            return GetData(request, contentType);
        }

        #endregion Get 方式提交数据

        #region Post 方式提交数据

        /// <summary>
        /// Post 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="data">需要提交的数据（JSON 格式）</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        public static string PostData(string url, string data)
        {
            return PostData(url, data, ContentType.Json);
        }



        /// <summary>
        /// Post 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="data">需要提交的数据</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        private static string PostData(string url, string data, ContentType contentType)
        {
            Debug.Log(url);
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            return PostData(request, data, contentType);
        }

        /// <summary>
        /// Post 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <param name="data">需要提交的数据</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        public static string PostData(string url, string data, WebHeaderCollection header, 
                        ContentType contentType=ContentType.Json)
        {
            var request = WebRequest.Create(new Uri(url,UriKind.Absolute)) as HttpWebRequest;
            request.Method = "POST";
            request.Headers = header;
         
            return PostData(request, data, contentType);
        }
        
        #endregion Post 方式提交数据

        #region Put 方式提交数据

        /// <summary>
        /// Put 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="data">需要提交的数据（JSON 格式）</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        private static string PutData(string url, string data)
        {
            return PutData(url, data, ContentType.Json);
        }

        /// <summary>
        /// Put 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <param name="data">需要提交的数据（JSON 格式）</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        private static string PutData(Uri uri, string data)
        {
            return PutData(uri, data, ContentType.Json);
        }

        /// <summary>
        /// Put 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="data">需要提交的数据</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        private static string PutData(string url, string data, ContentType contentType)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "PUT";
            return PostData(request, data, contentType);
        }

        /// <summary>
        /// Put 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <param name="data">需要提交的数据</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        private static string PutData(Uri uri, string data, ContentType contentType)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "PUT";
            return PostData(request, data, contentType);
        }

        #endregion Put 方式提交数据

        #region Delete 方式提交数据

        /// <summary>
        /// Delete 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="data">需要提交的数据（JSON 格式）</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        private static string DeleteData(string url, string data)
        {
            return DeleteData(url, data, ContentType.Json);
        }

        /// <summary>
        /// Delete 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <param name="data">需要提交的数据（JSON 格式）</param>
        /// <returns>接口返回的内容（JSON 格式）</returns>
        private static string DeleteData(Uri uri, string data)
        {
            return DeleteData(uri, data, ContentType.Json);
        }

        /// <summary>
        /// Delete 方式提交数据
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <param name="data">需要提交的数据</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        private static string DeleteData(string url, string data, ContentType contentType)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "DELETE";
            return PostData(request, data, contentType);
        }

        /// <summary>
        /// Delete 方式提交数据
        /// </summary>
        /// <param name="uri">URI 对象</param>
        /// <param name="data">需要提交的数据</param>
        /// <param name="contentType">返回数据格式类型</param>
        /// <returns>接口返回的内容</returns>
        private static string DeleteData(Uri uri, string data, ContentType contentType)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "DELETE";
            return PostData(request, data, contentType);
        }

        #endregion Delete 方式提交数据

        #region 私有方法

        private static string GetContentType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Json:
                    return "application/json";
                case ContentType.Xml:
                    return "text/xml";
                case ContentType.Html:
                    return "text/html";
                default:
                    return string.Empty;
            }
        }

        private static string GetData(HttpWebRequest request, ContentType contentType)
        {
            StreamReader reader = null;
            var result = string.Empty;
            request.Accept = GetContentType(contentType);
            request.ContentType = GetContentType(contentType);
            request.Method = "GET";

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    reader = new StreamReader(response.GetResponseStream());
                    result = reader == null ? string.Empty : reader.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(request.Method + "方式提交数据失败", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private static string PostData(HttpWebRequest request, string data, ContentType contentType)
        {
            //StreamReader reader = null;
            var result = string.Empty;

            try
            {
                request.Accept = GetContentType(contentType);
                request.ContentType = GetContentType(contentType);
                var arrData = Encoding.UTF8.GetBytes(data);
                request.ContentLength = arrData.Length;
              

                using (var postStream = request.GetRequestStream())
                {
                    postStream.Write(arrData, 0, arrData.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    /* reader = new StreamReader(response.GetResponseStream());
                     result = reader == null ? string.Empty : reader.ReadToEnd();*/
                    result = GetResponseBody(response);
                }

                return result;
            }
            catch (WebException ex)
            {
                if (ex.Status==WebExceptionStatus.ProtocolError)
                {
                    throw new Exception(ex.Message);
                }
                var errResp = ex.Response as HttpWebResponse;

                using (var stream = errResp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        string content = sr.ReadToEnd();
                        string messager = XDocument.Parse(content).Root.Descendants().ElementAt(7).Value;
                        throw new Exception(messager);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(request.Method + "方式提交数据失败", ex);
            }
            finally
            {
              /*  if (reader != null)
                {
                    reader.Close();
                }*/
            }
        }
        private static string GetResponseBody(HttpWebResponse response)
        {
            string responseBody = string.Empty;
            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(
                    response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader =
                        new StreamReader(stream, Encoding.UTF8))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }

        #endregion 私有方法

       
        
    }


    /// <summary>
    /// Http Content Type 标头
    /// </summary>
    public enum ContentType : byte
    {
        /// <summary>
        /// Json 格式
        /// </summary>
        [Description("Json 格式")]
        Json = 1,

        /// <summary>
        /// Xml 格式
        /// </summary>
        [Description("Xml 格式")]
        Xml = 2,

        /// <summary>
        /// Html 格式
        /// </summary>
        [Description("Html 格式")]
        Html = 3
    }
}