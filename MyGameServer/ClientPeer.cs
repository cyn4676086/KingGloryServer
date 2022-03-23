using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.SocketServer;
using System.Threading.Tasks;
using PhotonHostRuntimeInterfaces;
using MyGameServer.Handler;
using Common;

namespace MyGameServer
{

    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public int id;
        public int playerIndex;

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {
            GameModel.Instance.PeerList.Add(this);
        }
        //每个客户端断开时
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            GameModel.Instance.PeerList.Remove(this);

            //维护匹配池数组,断线即退出
            if (GameModel.Instance.MatchingList.IndexOf(this) >= 0)
            {
                GameModel.Instance.MatchingList.Remove(this);
            }
            MyGameServer.log.Info("一个客户端断开连接");
        }
        //客户端发起请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler=DicTool.GetValue(MyGameServer.Instance.HandlerDic,(OperationCode)operationRequest.OperationCode);
            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest,sendParameters,this);
            } 
            else
            {
                MyGameServer.log.Info("找不到操作码"+ (OperationCode)operationRequest.OperationCode);
            }
            return;
            
        }
    }
}
