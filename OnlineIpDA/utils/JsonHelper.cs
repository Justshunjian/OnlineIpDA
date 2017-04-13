using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OnlineIpDA.utils
{
    /// <summary>
    /// 文件名:JsonHelper.cs
    ///	功能描述:JSON工具类
    ///
    /// 作者:吕凤凯
    /// 创建时间:2016/2/24 11:24:44
    /// 
    /// </summary>
    static class JsonHelper
    {
        /// <summary>
        /// 格式化为JSON
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entityList">对象</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize<T>(this IEnumerable<T> entityList) where T : class
        {
            string _jsonString = string.Empty;
            if (entityList != null)
            {
                JavaScriptSerializer _serializerHelper = new JavaScriptSerializer();
                _serializerHelper.MaxJsonLength = int.MaxValue;
                _jsonString = _serializerHelper.Serialize(entityList);
            }
            return _jsonString;
        }

        /// <summary>
        /// 将JSON字符串解析为对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <returns>对象集合</returns>
        public static IEnumerable<T> Deserialize<T>(this string jsonString) where T : class
        {
            IEnumerable<T> _list = null;
            if (!string.IsNullOrEmpty(jsonString))
            {
                JavaScriptSerializer _serializerHelper = new JavaScriptSerializer();
                _list = _serializerHelper.Deserialize<IEnumerable<T>>(jsonString);
            }
            return _list;
        }
    }
}
