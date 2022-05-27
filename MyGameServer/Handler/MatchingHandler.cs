using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MyGameServer.Handler;
using MyGameServer.Model;
using Photon.SocketServer;


namespace MyGameServer.Handler
{
    class MatchingHandler : BaseHandler
    {
        
#pragma warning disable CS0169 // 从不使用字段“MatchingHandler.room”
        private BattleRoom room;
#pragma warning restore CS0169 // 从不使用字段“MatchingHandler.room”

        public MatchingHandler()
        {
            OpCode = Common.OperationCode.Matching;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {

            // 取出 key 为 ParaType 的参数，判断一下我们是什么类型的请求
            ParaCode code = (ParaCode)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.ParaType);
            if (code == ParaCode.Matching_Start)
            {
               
                //没有参数，直接返回成功,进入等待
                OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                response.ReturnCode = (short)Common.ReturnCode.Success;
                peer.SendOperationResponse(response, sendParameters);
                peer.HeroName = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.HeroType) as string;
                MyGameServer.log.Info("收到匹配请求的英雄："+peer.HeroName);
                //添加进入数组 保存所选英雄
                GameModel.Instance.MatchingList.Add(peer);
                MyGameServer.log.Info("Matching_Start:" + GameModel.Instance.MatchingList.Count);
                //1V1匹配判断队列长度是否满足
                if (GameModel.Instance.MatchingList.Count >= 2)
                {
                    EventData ed1;
                    EventData ed2;
                    Dictionary<byte, object> data;

                    //如果满足，开始战斗Matching_confirm

                    //创建房间
                    BattleRoom room = new BattleRoom();
                    room.first = GameModel.Instance.MatchingList[0];
                    room.second = GameModel.Instance.MatchingList[1];
                    GameModel.Instance.RoomList.Add(room);
                    MyGameServer.log.Info("添加room："+room.first+" "+room.second);
                    foreach (var RoomItem in GameModel.Instance.RoomList)
                    {
                        MyGameServer.log.Info(RoomItem.first);
                        MyGameServer.log.Info(RoomItem.second);
                    }

                    //推送一个Event 推送两个事件，针对每一个人推送
                    GameModel.Instance.MatchingList[0].playerIndex = 1;
                    //参数是自己的playerindex HeroName
                    string index = GameModel.Instance.MatchingList[0].playerIndex.ToString();
                    string hero = GameModel.Instance.MatchingList[0].HeroName.ToString();
                    ed1 = new EventData((byte)operationRequest.OperationCode);
                    data = new Dictionary<byte, object>();
                    data.Add((byte)ParaCode.ParaType, ParaCode.Matching_confirm);
                    data.Add((byte)ParaCode.Matching_confirm, index+","+ hero+","+ GameModel.Instance.MatchingList[1].HeroName.ToString());
                    ed1.Parameters = data;
                    GameModel.Instance.MatchingList[0].SendEvent(ed1, new SendParameters());
                    MyGameServer.log.Info("推送匹配事件:" + index + "," + hero + "," + GameModel.Instance.MatchingList[1].HeroName.ToString());

                    //推送第二个个Event
                    GameModel.Instance.MatchingList[1].playerIndex = 2;
                    //参数是自己的playerindex HeroName
                    string index2 = GameModel.Instance.MatchingList[1].playerIndex.ToString();
                    string hero2 = GameModel.Instance.MatchingList[1].HeroName.ToString();
                    ed2 = new EventData((byte)operationRequest.OperationCode);
                    data = new Dictionary<byte, object>();
                    data.Add((byte)ParaCode.ParaType, ParaCode.Matching_confirm);
                    data.Add((byte)ParaCode.Matching_confirm, index2 + "," + hero2+"," + GameModel.Instance.MatchingList[0].HeroName.ToString()); 
                    ed2.Parameters = data;
                    GameModel.Instance.MatchingList[1].SendEvent(ed2, new SendParameters());
                    MyGameServer.log.Info("推送匹配事件:" + index2 + "," + hero2 + GameModel.Instance.MatchingList[0].HeroName.ToString());

                    //匹配完成后，删除已完成匹配的两个客户端；
                    GameModel.Instance.MatchingList = new List<ClientPeer>();
                }
                else
                {
                    //如果不满足，添加进list，等待
                }
            }



        }

    }
}
