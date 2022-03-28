using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
namespace MyGameServer.Handler
{
    class BattleFieldHandler : BaseHandler
    {
        public BattleFieldHandler()
        {
            OpCode = Common.OperationCode.BattleFiled;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            // 取出 key 为 ParaType 的参数，判断一下我们是什么类型的请求
            ParaCode code = (ParaCode)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.ParaType);
            if (code == ParaCode.BF_Join)
            {
                //加入的时候，发送PlayerIndex给对方。根据当前的玩家数量
                //返回序号
                OperationResponse response = new OperationResponse(operationRequest.OperationCode);
                //获取序号
                peer.playerIndex = GameModel.Instance.PeerList.Count;

                //参数创建一个dic
                var data2 = new Dictionary<byte, object>();
                data2.Add((byte)ParaCode.BF_Join, peer.playerIndex);

                response.ReturnCode = (short)Common.ReturnCode.Success;
                response.Parameters = data2;
                peer.SendOperationResponse(response, sendParameters);


                //广播到所有的客户端，有新玩家加入
                //获取所有玩家的序列号，保存为一条字符串
                var allplayer = "";
                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    allplayer += peerItem.playerIndex + ",";
                }// 1，2，  

                //推送一个Event
                EventData ed = new EventData((byte)operationRequest.OperationCode);
                var data3 = new Dictionary<byte, object>();
                //注意，这里2个参数
                data3.Add((byte)ParaCode.ParaType, ParaCode.BF_Join);
                data3.Add((byte)ParaCode.BF_Join, allplayer);
                ed.Parameters = data3;


                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    peerItem.SendEvent(ed, new SendParameters());
                }

            }
            else if (code == ParaCode.BF_Move)
            {
                string p = (string)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.BF_Move);
                //转发给所有客户端

                EventData ed = new EventData((byte)operationRequest.OperationCode);
                var data3 = new Dictionary<byte, object>();
                //注意，这里2个参数
                data3.Add((byte)ParaCode.ParaType, ParaCode.BF_Move);
                data3.Add((byte)ParaCode.BF_Move, p);
                ed.Parameters = data3;
                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    peerItem.SendEvent(ed, new SendParameters());
                }
            }
            else if (code == ParaCode.BF_Att)
            {
                string p = (string)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.BF_Att);
                //转发给所有客户端

                EventData ed = new EventData((byte)operationRequest.OperationCode);
                var data3 = new Dictionary<byte, object>();
                //注意，这里2个参数
                data3.Add((byte)ParaCode.ParaType, ParaCode.BF_Att);
                data3.Add((byte)ParaCode.BF_Att, p);
                ed.Parameters = data3;
                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    peerItem.SendEvent(ed, new SendParameters());
                }
            }
            else if (code == ParaCode.BF_Hurt)
            {
                string p = (string)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.BF_Hurt);
                //转发给所有客户端

                EventData ed = new EventData((byte)operationRequest.OperationCode);
                var data3 = new Dictionary<byte, object>();
                //注意，这里2个参数
                data3.Add((byte)ParaCode.ParaType, ParaCode.BF_Hurt);
                data3.Add((byte)ParaCode.BF_Hurt, p);
                ed.Parameters = data3;
                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    peerItem.SendEvent(ed, new SendParameters());
                }
            }
            else if (code == ParaCode.BF_Destory)
            {
                string p = (string)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.BF_Destory);
                //转发给所有客户端

                EventData ed = new EventData((byte)operationRequest.OperationCode);
                var data3 = new Dictionary<byte, object>();
                //注意，这里2个参数
                data3.Add((byte)ParaCode.ParaType, ParaCode.BF_Destory);
                data3.Add((byte)ParaCode.BF_Destory, p);
                ed.Parameters = data3;
                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    peerItem.SendEvent(ed, new SendParameters());
                }
            }
            else if (code == ParaCode.BF_Ending)
            {
                //参数是失败的人
                int index = (int)DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.BF_Ending);
                //转发给所有客户端

                EventData ed = new EventData((byte)operationRequest.OperationCode);
                var data3 = new Dictionary<byte, object>();
                //注意，这里2个参数
                data3.Add((byte)ParaCode.ParaType, ParaCode.BF_Ending);
                data3.Add((byte)ParaCode.BF_Ending, index);
                ed.Parameters = data3;
                foreach (var peerItem in GameModel.Instance.PeerList)
                {
                    peerItem.SendEvent(ed, new SendParameters());
                }
            }

        }
    }
}
