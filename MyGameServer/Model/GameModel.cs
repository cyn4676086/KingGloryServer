using MyGameServer.Model;
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
        //保存所有连接玩家
        public List<ClientPeer> PeerList = new List<ClientPeer>();
        //匹配池
        public List<ClientPeer> MatchingList = new List<ClientPeer>();
        //房间池
        public List<BattleRoom> RoomList = new List<BattleRoom>();
    }
}
