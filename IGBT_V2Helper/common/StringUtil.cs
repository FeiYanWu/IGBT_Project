using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IGBT_V2Helper.common
{
    public static class StringUtil
    {
        public static string ByteToChar(byte byteValue)
        {
            return Convert.ToChar(byteValue).ToString();
        }

        /// <summary>
        /// 将字符串使用指定的分隔符分割
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="separator">分隔符</param>
        public static string[] Split(string source, string separator)
        {
            if (string.IsNullOrWhiteSpace(separator))
            {
                throw new ArgumentException("Invalid separator.", nameof(separator));
            }
            if (separator.Length == 1)
            {
                return source.Split(separator[0]);
            }
            return Regex.Split(source, separator);
        }

        /// <summary>
        /// 返回多个数组之间的拼接
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="separator">分隔符</param>
        public static string Join(string[] source, string separator)
        {
            if (separator == null)
            {
                throw new ArgumentException("Invalid separator.", nameof(separator));
            }
            return string.Join(separator, source);
        }

        /// <summary>
        /// 获取字符串的子串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="count">子字符串的长度</param>
        public static string GetSubString(string source, int startIndex, int count)
        {
            return source.Substring(startIndex, count);
        }

        /// <summary>
        /// 检查字符串中的控制字符
        /// </summary>
        /// <param name="text">待检查的文本</param>
        public static bool HasControlChar(string text)
        {
            if (null == text)
            {
                throw new ArgumentNullException(nameof(text));
            }
            return text.Any(char.IsControl);
        }

        /// <summary>
        /// 检查多个字符串中的控制字符
        /// </summary>
        /// <param name="texts">待检查的文本</param>
        public static bool HasControlChar(string[] texts)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                string text = texts[i];
                if (null == text)
                {
                    throw new ArgumentNullException($"text with index '{i}'");
                }
                if (text.Any(char.IsControl))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查字符串中的控制字符
        /// </summary>
        /// <param name="text">待检查的文本</param>
        public static void CheckControlChar(string text)
        {
            if (null == text)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Any(char.IsControl))
            {
                throw new ArgumentException($"Control character exist in text.");
            }
        }

        /// <summary>
        /// 检查多个字符串中的控制字符
        /// </summary>
        /// <param name="texts">待检查的文本</param>
        public static void CheckControlChar(string[] texts)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                string text = texts[i];
                if (null == text)
                {
                    throw new ArgumentNullException($"text with index '{i}'");
                }
                if (text.Any(char.IsControl))
                {
                    throw new ArgumentException($"Control character exist in text with index '{i}'.");
                }
            }
        }
    }
}
