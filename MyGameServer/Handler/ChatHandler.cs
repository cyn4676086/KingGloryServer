using Photon.SocketServer;
using Common;
using System.Collections.Generic;

namespace MyGameServer.Handler
{
    class ChatHandler : BaseHandler
    {
        public ChatHandler()
        {
            OpCode = Common.OperationCode.Chat;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string Hello = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.Chat) as string;
            MyGameServer.log.Info("收到客户端发来的聊天内容：" + Hello);
            //返回已经收到消息
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.ReturnCode = (short)Common.ReturnCode.Success;
            peer.SendOperationResponse(response, sendParameters);
            //发送给其他客户端
            EventData ed = new EventData((byte)OperationCode.Chat);
            var data = new Dictionary<byte, object>();
            data.Add((byte)ParaCode.Chat,Hello);
            ed.Parameters = data;
           
            //遍历发送到所有客户端
            foreach(var peerItem in GameModel.Instance.PeerList)
            {
                peerItem.SendEvent(ed, new SendParameters());
            }
        }
    }
}
