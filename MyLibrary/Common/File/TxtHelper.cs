using CommonLib.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.File
{
    /// <summary>
    /// Txt文件操作类
    /// </summary>
    public class TxtHelper
    {
        #region 私有属性常量
        public const string NewLine = "\r\n";
        #endregion

        #region 公共方法

        #region Read

        /// <summary>
        /// 读取 Txt 数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            return Read(path, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// 读取 Txt 数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string Read(string path, System.Text.Encoding encoding)
        {
            string content = "";
            ExecuteStreamReader(path, encoding, (StreamReader streamReader) =>
            {
                content = streamReader.ReadToEnd();
            });
            return content;
        }

        #endregion

        #region Write

        /// <summary>
        /// 数据写入 Txt 文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">要写入的数据</param>
        /// <param name="append">是否追加</param>
        /// <returns></returns>
        public static bool Write(string path, string content, bool append = false)
        {
            return Write(path, content, append, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// 数据写入 Txt 文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">要写入的数据</param>
        /// <param name="append">是否追加</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static bool Write(string path, string content, bool append, System.Text.Encoding encoding)
        {
            return ExecuteStreamWriter(path, encoding, append, () =>
            {
                return content;
            });
        }

        #endregion

        #endregion

        #region 私有方法

        private static void ExecuteStreamReader(string txtPath, System.Text.Encoding encoding, Action<StreamReader> callback)
        {
            using (StreamReader streamReader = new StreamReader(txtPath, encoding))
            {
                if (callback != null) callback(streamReader);
            }
        }

        private static bool ExecuteStreamWriter(string txtPath, System.Text.Encoding encoding, bool append, Func<string> callback)
        {
            // 根据路径创建目录
            bool createDirectoryStatus = FileHelper.CreateDirectory(txtPath);
            if (!createDirectoryStatus) return false;

            string content = null;
            if (callback != null) content = callback();

            using (StreamWriter streamWriter = new StreamWriter(txtPath, append, encoding))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
            }
            return true;
        }

        #endregion

    }

    #region 逻辑辅助枚举
    public enum TxtTypeEnum
    {
        /// <summary>
        /// 实体带有 TxtT 特性
        /// </summary>
        Attribute = 1,

        /// <summary>
        /// 实体不带有 TxtT 特性
        /// </summary>
        Normal = 2,
    }
    #endregion
}
