using NHibernate;
using NHibernate.Cfg;
namespace MyGameServer
{
    class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        //实际是返回了_sessionFactory；
        private static ISessionFactory sessionFactory
        {
            //惰性实例化，第一次使用才创建
            get//只读
            {
                if (_sessionFactory == null)
                {
                    var cfg = new Configuration();
                    cfg.Configure();//解析配置文件
                    cfg.AddAssembly("MyGameServer");

                    _sessionFactory = cfg.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }
    }

}
