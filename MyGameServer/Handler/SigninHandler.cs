using Common;
using MyGameServer.Manager;
using MyGameServer.Model;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class SigninHandler : BaseHandler
    {
        public SigninHandler()
        {
            OpCode = Common.OperationCode.Signin;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string username = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.UserName) as string;
            string password = DicTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParaCode.Password) as string;

            var user = UserManager.Instance.GetUserByName(username);

            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            if (user==null)
            {
                user = new User();
                user.Username = username;
                user.Password = password;
                UserManager.Instance.Add(user);
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
