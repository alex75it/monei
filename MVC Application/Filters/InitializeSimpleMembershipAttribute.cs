using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using Monei.MvcApplication.Models;
using WebMatrix.WebData;


namespace Monei.MvcApplication.Filters
{
	public class InitializeSimpleMembershipAttribute :ActionFilterAttribute
	{
		private static SimpleMembershipInitializer initializer;
		private static object initializerLock = new object();
		private static bool isInitialized;


		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
		{
			// Ensure ASP.Net Simple Membership is initialized only once per app start
			LazyInitializer.EnsureInitialized(ref initializer, ref isInitialized, ref initializerLock);
		}

		private class SimpleMembershipInitializer
		{
			public SimpleMembershipInitializer()
			{
				Database.SetInitializer<UsersContext>(null);

				try 
				{ 
					using (var context = new UsersContext())
					{
						if (!context.Database.Exists())
						{
							// create the SimpleMembership database without Entity Framework migration schema
							((IObjectContextAdapter)context).ObjectContext.CreateDatabase();							
						}
					}

					WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "_UserId", "username", autoCreateTables:true);
				}
				catch (Exception exc)
				{
					throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", exc);
				}

			}
			
		}

	}//class
}