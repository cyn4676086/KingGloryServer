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
        
        private BattleRoom room;

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
                //没有参数，直接返回成功
                OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                response.ReturnCode = (short)Common.ReturnCode.Success;
                peer.SendOperationResponse(response, sendParameters);

                
                MyGameServer.log.Info("Matching_Start:" + GameModel.Instance.MatchingList.Count);
                //添加进入数组
                GameModel.Instance.MatchingList.Add(peer);
                //1V1匹配判断队列长度是否满足
                if (GameModel.Instance.MatchingList.Count >= 2)
                {
                    EventData ed1;
                    EventData ed2;
                    Dictionary<byte, object> data;

                    //如果满足，开始战斗
                    //Matching_confirm

                    //推送一个Event 推送两个事件，针对每一个人推送
                    GameModel.Instance.MatchingList[0].playerIndex = 1;

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

                    ed1 = new EventData((byte)operationRequest.OperationCode);
                    data = new Dictionary<byte, object>();
                    data.Add((byte)ParaCode.ParaType, ParaCode.Matching_confirm);
                    //参数是自己的playerindex
                    data.Add((byte)ParaCode.Matching_confirm, GameModel.Instance.MatchingList[0].playerIndex);
                    ed1.Parameters = data;
                    GameModel.Instance.MatchingList[0].SendEvent(ed1, new SendParameters());

                    //推送第二个个Event
                    GameModel.Instance.MatchingList[1].playerIndex = 2;
                    ed2 = new EventData((byte)operationRequest.OperationCode);
                    data = new Dictionary<byte, object>();
                    data.Add((byte)ParaCode.ParaType, ParaCode.Matching_confirm);
                    //参数是自己的playerindex
                    data.Add((byte)ParaCode.Matching_confirm, GameModel.Instance.MatchingList[1].playerIndex);
                    ed2.Parameters = data;
                    GameModel.Instance.MatchingList[1].SendEvent(ed2, new SendParameters());
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
