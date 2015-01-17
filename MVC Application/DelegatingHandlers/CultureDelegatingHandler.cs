using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using log4net;

namespace Monei.MvcApplication.DelegatingHandlers
{
	public class CultureDelegatingHandler :DelegatingHandler
	{
		ILog logger = LogManager.GetLogger(typeof(CultureDelegatingHandler));

		protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			logger.InfoFormat("CultureDelegatingHandler");


			IDictionary<string, string> headers = request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value);


			request.Headers.Remove("Accept-Language");
			request.Headers.Add("Accept-Language", "it-CH");

			//request.Headers.AcceptLanguage.Contains()
			//if (headers.ContainsKey("language"))
			//{
			//	string[] languages = headers["Accepetd"]
			//}
			//request.Se .Headers.AcceptLanguage.

			return base.SendAsync(request, cancellationToken);
		}
	}
}