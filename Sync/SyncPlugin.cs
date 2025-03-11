using System;
using System.Collections.Generic;
using MCGalaxy;
using MCGalaxy.Maths;
using MCGalaxy.Tasks;
using Newtonsoft.Json;

namespace Sync
{
    public class SyncPlugin : Plugin
    {
        public override String name => "Sync";

        public static readonly Queue<KeyValuePair<string, string>> actions = new Queue<KeyValuePair<string, string>>();

        public override void Load(bool start)
        {
            Server.MainScheduler.QueueRepeat(SendData, 0, TimeSpan.FromMilliseconds(125));
        }

        public void SendData(SchedulerTask task)
        {
            String returnData = GetReturnData();

            Logger.Log(LogType.ConsoleMessage, "Data: " + returnData);
            WebUtils.sendJson("http://localhost:8005", returnData);
        }

        public String GetReturnData()
        {
            List<PlayerInformation> players = new List<PlayerInformation>();

            Player[] onlinePlayers = PlayerInfo.Online.Items;
            foreach (Player p in onlinePlayers)
            {
                Vec3S32 blockCoords = p.Pos.FeetBlockCoords;
                PlayerInformation info = new PlayerInformation
                {
                    n = p.name,
                    l = new Location
                    {
                        x = blockCoords.X,
                        y = blockCoords.Y,
                        z = blockCoords.Z,
                        ya = (p.Rot.RotY / (double)256) * 360,
                        p = (p.Rot.HeadX / (double)256) * 360
                    }
                };

                players.Add(info);
            }

            ReturnData data = new ReturnData
            {
                p = players,
                j = "testServer",
                a = new object[0]
            };

            return JsonConvert.SerializeObject(data);
        }

        public override void Unload(bool start)
        {

        }

    }

    public class PlayerInformation {
        public Location l { get; set; }
        public string n { get; set; }
    }

    public class Location
    {
        public double y { get; set; }
        public double x { get; set; }
        public double ya { get; set; }
        public double z { get; set; }
        public double p { get; set; }
    }

    public class ReturnData
    {
        public List<PlayerInformation> p { get; set; }

        public string j { get; set; }

        public object[] a { get; set; }
    }
}
