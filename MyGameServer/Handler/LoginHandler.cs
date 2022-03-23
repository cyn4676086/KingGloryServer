using Common;
using MyGameServer.Manager;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class LoginHandler : BaseHandler
    {
        public LoginHandler()
        {
            OpCode = Common.OperationCode.Login;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters,ClientPeer peer)
        {
            string username = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.UserName) as string;
            string password = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.Password) as string;

            bool b =UserManager.Instance.VerifyUser(username, password);
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            if (b)
            {
                response.ReturnCode = (short)Common.ReturnCode.Success;

            }
            else
            {
                response.ReturnCode = (short)Common.ReturnCode.Failed;
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
