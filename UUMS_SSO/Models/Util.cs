using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace UUMS_SSO.Models
{
    public class Util
    {

        public static string jump_type = "1";           //保存跳轉的時候驗證方式1，不驗證，2驗證要登錄
        public static string jump_url = "";             //如果是驗證1那麼不做操作，如果是驗證2，驗證之後跳轉到這個url上面


        /// <summary>
        /// 截取登錄結果中的用戶名或ID
        /// </summary>
        /// <param name="RetMsg">登錄返回值，一個用戶信息封裝的字符串</param>
        /// <param name="str">獲取的結果，把信息存到數組中</param>
        /// <returns></returns>
        public static bool SplitString(string RetMsg, ref string[] str)
        {
            int startIndex = -1;
            int endIndex = -1;
            string ActualLogin = "";
            string UserID = "";
            string[] UserMSG = { "", "" };

            //成功的結果開頭字符是"OK"
            if ("OK".Equals(RetMsg.Substring(0, 2)))
            {
                //判斷數據存儲的開始
                startIndex = RetMsg.IndexOf("ActualLogin:[");

                //判斷是否存在
                if (startIndex > -1)
                {
                    //為了獲取用戶noteID
                    startIndex += 13;

                    //截取末尾的位置
                    endIndex = RetMsg.Substring(startIndex).IndexOf("]");

                    //判斷是否正確
                    if (endIndex > -1)
                    {
                        //獲取noteId  //endIndex += startIndex;
                        ActualLogin = RetMsg.Substring(startIndex, endIndex);

                        //存儲noteId
                        UserMSG[0] = ActualLogin;
                    }
                }

                //獲取userId的位置
                startIndex = RetMsg.IndexOf("UserID:[");

                //判斷是否存在
                if (startIndex > -1)
                {
                    //截取的開始
                    startIndex += 8;

                    //截取的結束位置
                    endIndex = RetMsg.Substring(startIndex).IndexOf("]");

                    //判斷是否存在
                    if (endIndex > -1)
                    {
                        //截取userId //endIndex += startIndex;
                        UserID = RetMsg.Substring(startIndex, endIndex);

                        //存儲userId
                        UserMSG[1] = UserID;
                    }
                }

                //返回結果
                str = UserMSG;

                //返回結果
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string EncodeBase64(string code)
        {
            byte[] bytes = Encoding.GetEncoding(0).GetBytes(code);  //将一组字符编码为一个字节序列. 

            string encode = Convert.ToBase64String(bytes);  //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式. 

            return encode;
        }

        public static string DecodeBase64(string code)
        {
            byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 

            string decode = Encoding.GetEncoding(0).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 

            return decode;
        }

        public static string EncodeUrl(string url, string tag)
        {
            return "/Home/Jump?j=" + HttpUtility.HtmlEncode(url) + tag;
        }

        public static string EncodeUrl(string url)
        {
            return HttpUtility.HtmlEncode(url);
        }



    }
}