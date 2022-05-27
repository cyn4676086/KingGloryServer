using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MyGameServer.Manager;
using Photon.SocketServer;
using Common;
using MyGameServer.Handler;

namespace MyGameServer
{
    //所有server都要继承实现三个基本方法
    public class MyGameServer : ApplicationBase
    {
#pragma warning disable CS0108 // “MyGameServer.Instance”隐藏继承的成员“ApplicationBase.Instance”。如果是有意隐藏，请使用关键字 new。
        public static MyGameServer Instance;
#pragma warning restore CS0108 // “MyGameServer.Instance”隐藏继承的成员“ApplicationBase.Instance”。如果是有意隐藏，请使用关键字 new。

        public static readonly ILogger log = LogManager.GetCurrentClassLogger();
        
        //当有一个客户端连接即执行
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            
            var p = new ClientPeer(initRequest);
            log.Info("一个客户端连接");
            return p;
            
        }
        //初始化
        protected override void Setup()
        {
            Instance = this;
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(
                Path.Combine(this.ApplicationRootPath,"bin_Win64"),"log");
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath,"log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);//photon日志输出
                XmlConfigurator.ConfigureAndWatch(configFileInfo);//读取配置
            }
            log.Info("服务器已启动");
            InitHandler();
        }
        //关闭
        protected override void TearDown()
        {
            log.Info("服务器已关闭");
        }

        public Dictionary<OperationCode, BaseHandler> HandlerDic = new Dictionary<OperationCode, BaseHandler>();

        public void InitHandler()
        {
            //初始化操作码 注册
            LoginHandler loginHandler = new LoginHandler();
            HandlerDic.Add(loginHandler.OpCode, loginHandler);

            SigninHandler signinHandler = new SigninHandler();
            HandlerDic.Add(signinHandler.OpCode, signinHandler);

            ChatHandler chatHandler = new ChatHandler();
            HandlerDic.Add(chatHandler.OpCode, chatHandler);

            BattleFieldHandler battleFieldHandler = new BattleFieldHandler();
            HandlerDic.Add(battleFieldHandler.OpCode, battleFieldHandler);

            MatchingHandler matchingHandler = new MatchingHandler();
            HandlerDic.Add(matchingHandler.OpCode, matchingHandler);
        }
    }
}
