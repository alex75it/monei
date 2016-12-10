using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Monei.DataAccessLayer.SqlServer;
using Monei.MvcApplication;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Monei.MvcApplication.DependencyInjection;
using Monei.MvcApplication.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Monei.MvcApplication.Filters;

namespace Monei.Test.IntegrationTest.MvcApplication.Api
{
    [TestFixture]
    public class ApiControllerTestBase<TApiController>
    {
        protected ISessionFactoryProvider sessionFactoryProvider = new SessionFactoryProvider();

        public string testAccountGuid = "00000000-0000-0000-0000-000000000000";
        private const string BASE_URL = "http://www.apitest.com/";

        protected HttpMethod GET = HttpMethod.Get;
        protected HttpMethod POST = HttpMethod.Post;
        protected HttpMethod DELETE = HttpMethod.Delete ;
        protected Random random = new Random(DateTime.Now.Millisecond);
        protected TestDataProvider testDataProvider;

        private HttpServer server;
        private HttpClient client;    

        //private WindsorCastleDependencyInjection dependencyInjectionManager;

        public ApiControllerTestBase()
        {            
            testDataProvider = new TestDataProvider();
        }

        /// <summary>
        /// Return a new HttpServer. A new one is created every time because it can be "dirty" from prevous use.
        /// </summary>
        /// <returns></returns>
        public HttpServer InitializeServer()
        {
            server = new HttpServer(GetConfiguration());
            return server;
        }

        [OneTimeSetUp]
        public void Initialize()
        {			
            //InitializeWindsorContainer();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (server != null)
                server.Dispose();

            if (client != null)
                client.Dispose();
        }

        protected HttpClient GetClient()
        {
            client = new HttpClient(InitializeServer());
            return client;
        }

        protected HttpConfiguration GetConfiguration()
        {            
            HttpConfiguration configuration = new HttpConfiguration();
            // configuration.DependencyResolver = dependencyInjectionManager; // cause error on second call of the same controller. 
            // probably controller is not resolved. Impossible to debug. 

            // A call on a browser show the full error, ho to replicate this ?
            /*
             <Error>
                <Message>An error has occurred.</Message>
                <ExceptionMessage>
                An error occurred when trying to create a controller of type 'TokenApiController'. Make sure that the controller has a parameterless public constructor.
                </ExceptionMessage>
                <ExceptionType>System.InvalidOperationException</ExceptionType>
                <StackTrace>
                at System.Web.Http.Dispatcher.DefaultHttpControllerActivator.Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType) at System.Web.Http.Controllers.HttpControllerDescriptor.CreateController(HttpRequestMessage request) at System.Web.Http.Dispatcher.HttpControllerDispatcher.<SendAsync>d__1.MoveNext()
                </StackTrace>
                <InnerException>
                <Message>An error has occurred.</Message>
                <ExceptionMessage>
                Type 'Monei.MvcApplication.Api.TokenApiController' does not have a default constructor
                </ExceptionMessage>
                <ExceptionType>System.ArgumentException</ExceptionType>
                <StackTrace>
                at System.Linq.Expressions.Expression.New(Type type) at System.Web.Http.Internal.TypeActivator.Create[TBase](Type instanceType) at System.Web.Http.Dispatcher.DefaultHttpControllerActivator.GetInstanceOrActivator(HttpRequestMessage request, Type controllerType, Func`1& activator) at System.Web.Http.Dispatcher.DefaultHttpControllerActivator.Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
                </StackTrace>
                </InnerException>
                </Error>
             */

            configuration.DependencyResolver = new WindsorCastleDependencyInjection(new LifestyleSingletonComponentModelContruction());
            WebApiConfig.Register(configuration);

            // pratically undocumented: http://stackoverflow.com/questions/21901808/need-a-complete-sample-to-handle-unhandled-exceptions-using-exceptionhandler-i
            configuration.Services.Replace(typeof(IExceptionHandler), new UnitTestExceptionHandler());
                        
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            configuration.Filters.Add(new ApiControllerExceptionFilterAttribute());

            log4net.Config.XmlConfigurator.Configure();

            return configuration;
        }

        private class UnitTestExceptionHandler : ExceptionHandler
        {
            public Task Handle(ExceptionHandlerContext context, CancellationToken cancellationToken)
            {
                Console.WriteLine("Error. CatchBlock: " + context.CatchBlock);
                string name = context.CatchBlock.Name;
                return null;
            }
        }

        /// <summary>
        /// Create a HttpRequestMessage that can be used with HttpClient.SendAsync() method.		/// 
        /// </summary>
        /// <param name="url">Partial URL of the API <example>api/mycontroller/param</example></param>
        /// <param name="method">GET, POST ...</param>
        /// <param name="content"></param>
        /// <param name="mediaType">HTTP "Header Accept" value, default is for JSON data</param>
        /// <returns></returns>
        protected HttpRequestMessage CreateRequest(string url, HttpMethod method, string mediaType = "application/json")
        {
            var request = new HttpRequestMessage(method, BASE_URL + url);
            request.Headers.Add("account-guid", testAccountGuid);

            // I don't know what this does. Copied from one example.
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return request;
        }

        /// <summary>
        /// Create a HttpRequestMessage that can be used with HttpClient.SendAsync() method.		/// 
        /// </summary>
        /// <typeparam name="T">Type of the class passed in Content of the request</typeparam>
        /// <param name="url">Partial URL of the API <example>api/mycontroller/param</example></param>
        /// <param name="method">GET, POST ...</param>
        /// <param name="content">The object data passed in the request</param>
        /// <param name="mediaType">HTTP "Header Accept" value, default is for JSON data</param>
        /// <returns></returns>
        protected HttpRequestMessage CreateRequest<T>(string url, HttpMethod method,  T content, string mediaType = "application/json")
        {
            var request = CreateRequest(url, method, mediaType);
            request.Headers.Add(AuthenticationWorker.API_TOKEN, testAccountGuid);
            // todo: is this needed? (CamelCasePropertyNameContractResolver)
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            request.Content = new ObjectContent<T>(content, formatter);
            return request;
        }


        protected void CallApi(string url, HttpMethod httpMethod)
        {
            using (var client = GetClient())
            using (var result = client.SendAsync(CreateRequest(url, httpMethod)).Result)
                CheckResult(url, result);
        }

        protected TReturn CallApi<TReturn>(string url, HttpMethod httpMethod)
        {
            using (var client = GetClient())
            using (var result = client.SendAsync(CreateRequest(url, httpMethod)).Result)
                return LoadReturnValue<TReturn>(url, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPost">Type represented by HTTP form collection data</typeparam>
        /// <typeparam name="TReturn">Returned object</typeparam>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="data">POST form collection data</param>
        /// <returns></returns>
        protected TReturn CallApi<TPost, TReturn>(string url, TPost data)
        {
            using (var client = GetClient())
            using (var result = client.SendAsync(CreateRequest<TPost>(url, HttpMethod.Post, data)).Result)
                return LoadReturnValue<TReturn>(url, result);
        }

        /// <summary>
        /// Call API
        /// </summary>
        /// <typeparam name="TPost">Type represented by HTTP form collection data</typeparam>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="data">POST form collection data</param>
        protected void CallApi<TPost>(string url, HttpMethod httpMethod, TPost data)
            where TPost : class
        {
            using (var client = GetClient())
            using (var result = client.SendAsync(CreateRequest<TPost>(url, httpMethod, data)).Result)
                CheckResult(url, result);
        }

        /// <summary>
        /// Call API with POST passing the data
        /// </summary>
        /// <typeparam name="TPost">Type represented by HTTP form collection data</typeparam>
        /// <param name="url"></param>
        /// <param name="data">POST form collection data</param>
        protected void CallApi<TPost>(string url, TPost data) where TPost:class
        {
            CallApi<TPost>(url, POST, data);
        }


        private static TReturn LoadReturnValue<TReturn>(string url, HttpResponseMessage result) 
            //where TReturn : class
        {
            CheckResult(url, result);
            return result.Content.ReadAsAsync<TReturn>().Result;
        }

        private static void CheckResult(string url, HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
                Assert.Fail("Server error. Url: " + url + ".\r\n" + result);
        }

        protected int RandomInt()
        {
            return random.Next();
        }
    }
}
