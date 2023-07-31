using System;

namespace IdentityApi.Global
{
    public class GlobalConstant
    {
        public class EmailAccount
        {
            public struct Gmail
            {
                public static string Username = "";
                public static string Password = "";
                public static string host = "smtp.gmail.com";
                public static int PortSSL = Convert.ToInt32(465);//for ssl
                public static int PortTSL = Convert.ToInt32(587);//for tsl
                public static string SendFromEmail = "3pl@dunblare.com";
                public static string SendFromAs = "DUNBLARE IMPORT-EXPORT INC";
            }
            public struct SendGrid
            {
                //public static string ApiKey = "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";
                public static string apiKey = "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";
                public static string ApiKeyName = "12SkiesLawnMo";
                public static string Username = "matthew.gordon@12skiestech.com";
                public static string Password = "12SkiesSendgrid!";
                public static string host = "smtp.sendgrid.net";
                public static int PortSSL = Convert.ToInt32(465);//for ssl
                public static int PortTSL = Convert.ToInt32(587);//for tsl
                public static string SendFromEmail = "matthew.gordon@12skiestech.com";
                public static string SendFromAs = "SourceMatrix INC";
            }
            // 

            //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
        }
    }
}
