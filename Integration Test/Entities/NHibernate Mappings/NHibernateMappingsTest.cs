using System;
using System.Reflection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.IO;

namespace Monei.Test.IntegrationTest.Entities.NHibernateMappings
{
    [TestFixture, Category("Data Access Layer"), Category("NHibernate")]
    public class NHibernateMappingsTest
    {
        [Test]
        public void AccountMappingTest()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Monei.Entities.Account));

            NHibernate.Cfg.Configuration configuration = new NHibernate.Cfg.Configuration();
            configuration.Configure();

            Assert.IsNotEmpty(configuration.ClassMappings, "None class mapping loaded.");

            foreach(var mapping in configuration.ClassMappings)
            {
                Type type = mapping.MappedClass;
            }

            SchemaExport export = new SchemaExport(configuration);
            export.SetOutputFile("NHibernate mappings.sql");
            export.Execute(true, false, false);
            string outputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NHibernate mappings.sql");
            using (var writer = new StreamWriter( File.OpenWrite(outputFile)))
            {
               export.Create(writer, false);
            }

            //bool execute = false;
            //string generatedSql;
        }
    }
}
