using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.SqlServer;
using NHibernate;

namespace Monei.Tests.DataAccessLayer.SqlServer
{
	[TestClass]
	public class NHibernateSessionFactory
	{
		[TestMethod]
		public void TestNHibernateSessionFactory()
		{
			//Monei.DataAccessLayer.SqlServer.RepositoryBase
			RegistryRepository repository = new RegistryRepository();
		}

		[TestMethod]
		public void OpenSession()
		{ 
			// todo:
		}

	}
}
