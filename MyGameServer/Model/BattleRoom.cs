using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Model
{
    class BattleRoom
    {
       
        //public int BattleRoomID { get; set; }
        public ClientPeer first { get; set; }
        public ClientPeer second { get; set; }
    }
}
