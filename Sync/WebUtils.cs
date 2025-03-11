using MCGalaxy;
using System;
using System.IO;
using System.Net;

namespace Sync
{
    class WebUtils
    {

        public static void sendJson(String url, String data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Logger.Log(LogType.ConsoleMessage, "Couldn't write to Server.");
                    }

                }
            } catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

    }
}
