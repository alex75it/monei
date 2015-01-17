using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Monei.MvcApplication.DelegatingHandlers
{
	public class QueryStringHandler :DelegatingHandler
	{
		protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			KeyValuePair<String, String> pairs = request.GetQueryNameValuePairs().SingleOrDefault(p => p.Key == "format");
			if (pairs.Key == "format")
			{
				if (pairs.Value == "csv")
				{
					request.Headers.Add("accept", "text/csv");
					//request.Headers.Add("x-api-token", GlobalSettings.Instance.ApiToken);
				}
			}

			return base.SendAsync(request, cancellationToken);
		}

	}
}