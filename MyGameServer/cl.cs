using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace MyGameServer
{
    //所有server都要继承实现三个基本方法
    public class MyGameServer : ApplicationBase
    {
        //当有一个客户端连接即执行
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            throw new NotImplementedException();
        }

        protected override void Setup()
        {
            throw new NotImplementedException();
        }

        protected override void TearDown()
        {
            throw new NotImplementedException();
        }
    }
}
