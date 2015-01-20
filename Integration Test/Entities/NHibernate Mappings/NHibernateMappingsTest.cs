using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Monei.Test.IntegrationTest.Entities.NHibernateMappings
{
	[TestClass]
	public class NHibernateMappingsTest
	{
		[TestMethod, TestCategory("NHibernate")]
		public void AccountMappingTest()
		{

			Assembly assembly = Assembly.GetAssembly(typeof(Monei.Entities.Account));

			string a = NHibernate.Cfg.Configuration.DefaultHibernateCfgFileName ;

			NHibernate.Cfg.Configuration configuration = new NHibernate.Cfg.Configuration();
			configuration.Configure();
			foreach(var mapping in configuration.ClassMappings)
			{
				Type type = mapping.MappedClass;
			}

			SchemaExport export = new SchemaExport(configuration);
			export.SetOutputFile("NHibernate mappings.sql");
			//export.Create()
			export.Execute(true, false, false);
		}
	}
}
