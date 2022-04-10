﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MyGameServer.Handler;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MyGameServer
{

    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public int playerIndex;
        public int id;
        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {
            GameModel.Instance.PeerList.Add(this);
        }

        //当每个客户端断开时
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            GameModel.Instance.PeerList.Remove(this);
            if (GameModel.Instance.MatchingList.IndexOf(this) >= 0)
            {
                GameModel.Instance.MatchingList.Remove(this);
            }
            foreach(var item in GameModel.Instance.RoomList)
            {
                if (this == item.first || this == item.second)
                {
                    GameModel.Instance.RoomList.Remove(item);
                }
            }
            
        }
        //客户端发起请求的时候
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = DicTool.GetValue(MyGameServer.Instance.HandlerDic, (OperationCode)operationRequest.OperationCode);

            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                MyGameServer.log.Info("找不到操作码：" + (OperationCode)operationRequest.OperationCode);
            }
            return;
            switch (operationRequest.OperationCode)
            {
                case 1:
                    //解析数据
                    var data = operationRequest.Parameters;
                    object intValue;
                    data.TryGetValue(1, out intValue);
                    object StringValue;
                    data.TryGetValue(2, out StringValue);
                    MyGameServer.log.Info("收到客户端的请求，OpCode：1" + intValue.ToString() + ":" + StringValue.ToString());


                    //返回相应
                    OperationResponse opResponse = new OperationResponse(operationRequest.OperationCode);

                    //构造参数
                    var data2 = new Dictionary<byte, object>();
                    data2.Add(1, 100);
                    data2.Add(2, "这个是参数,服务器发来的");
                    opResponse.SetParameters(data2);
                    //返回code，为发送过来的code，返回的参数，为发送过来的参数
                    SendOperationResponse(opResponse, sendParameters);

                    //推送一个Event
                    EventData ed = new EventData(1);
                    ed.Parameters = data2;
                    SendEvent(ed, new SendParameters());
                    break;
                default:
                    break;
            }
        }
    }
}