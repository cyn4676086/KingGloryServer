using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGameServer.Model;
using NHibernate;
using NHibernate.Criterion;


namespace  MyGameServer.Manager
{
    class UserManager
    {
        public static UserManager Instance = new UserManager();
        public void Add(Model.User user)
        {
            //using:{}之间的代码执完毕之后（）里的内存就会被回收
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        internal bool VerifyUser(string username, string password)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                User user = session.CreateCriteria(typeof(User)).
                    Add(Restrictions.Eq("Username", username))
                    .Add(Restrictions.Eq("Password", password))
                    .UniqueResult<User>();
                if (user == null) return false;
                return true;
            }
        }

        public void update(Model.User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }
        public void Remove(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }
        public User GetUserById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var u = session.Get<User>(id);
                    transaction.Commit();
                    return u;
                }
            }
           
        }

        public User GetUserByName(string name)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", name)).UniqueResult<User>();
            }
           
        }
        public IList<User> GetAll()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof(User)).List<User>();
            }
        }
    }
}
