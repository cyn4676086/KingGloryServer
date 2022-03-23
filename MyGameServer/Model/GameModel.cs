using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer
{
    class GameModel
    {
        public static GameModel Instance = new GameModel();
        public List<ClientPeer> PeerList = new List<ClientPeer>();//保存所有连接玩家
        internal List<ClientPeer> MatchingList = new List<ClientPeer>();
    }


}
