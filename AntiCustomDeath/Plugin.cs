using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using TShockAPI.Hooks;
using Terraria;
using TerrariaApi.Server;
using System.IO;
using Terraria.DataStructures;

namespace AntiCustomDeath
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Author => "Zoom L1";
        public override string Name => "AntiCustomDeath";
        public Plugin(Main game) : base(game) { }

        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
        }

        public static void OnGetData(GetDataEventArgs args)
        {
            if (args.MsgID == PacketTypes.PlayerDeathV2)
            {
                using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
                {
                    int Index = reader.ReadByte();
                    PlayerDeathReason pdr = PlayerDeathReason.FromReader(reader);

                    if (pdr._sourceCustomReason != null)
                    {
                        args.Handled = true;
                        return;
                    }
                }
                
            }
        }
    }
}
