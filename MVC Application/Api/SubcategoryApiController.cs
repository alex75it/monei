using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.Entities;
using Monei.MvcApplication.Api.PostDataObjects;
using Monei.MvcApplication.Controllers.Api;
using Monei.MvcApplication.Controllers.Api.PostDataObjects;
using Newtonsoft.Json.Linq;

namespace Monei.MvcApplication.Api
{	
	[RoutePrefix("api/subcategory")]
	public class SubcategoryApiController : ApiControllerBase
	{
		[HttpGet, Route("ping")]
		public string Ping()
		{
			return "api/subcategory";
		}


		[HttpGet, Route("category/{categoryId}")]
        public IEnumerable<Subcategory> Search(int categoryId)
        {
			return SubcategoryRepository.List(categoryId);
        }


		[HttpGet, Route("ListByType/{typeId}")] //, Name = "LisyByType"
		public string ListByType(int typeId)
		{
			return "{ method: 'ListByType', typeId: " + typeId + "}";
		}

		[HttpPost]
		public dynamic Post([FromBody]string value)
        {
			logger.Debug("Post: " + value);

			//NameValueCollection data =  HttpUtility.ParseQueryString(value);
			
			dynamic json = JValue.Parse(value);;

			string categoryId = json.categoryId;
			string name = json.name; 
			string description = json.description;

			int categoryIdValue = int.Parse(categoryId);

			// todo: use a business object
			Subcategory entity = new Subcategory() {
				Category = new Category() { Id = categoryIdValue},
				Name = name,
				Description = description,
			};
			
			int newId = SubcategoryRepository.Create(entity);

			//dynamic result = new JObject();
			//result.categoryId = categoryId;
			//result.subcategoryId = newId;

			dynamic result = new { 
				categoryId = categoryId,
				subcategoryId = newId,
				subcategoryName = name,
			};

			return result;
        }


        // DELETE api/subcategory/5
		[HttpDelete]
        public virtual HttpResponseMessage Delete(int id)
        {
			try
			{
				// todo: check if user is the owner of subcategory?
				bool isUsed = SubcategoryManager.IsUsed(CurrentAccount.Id, id);
				
				if (isUsed)					
					return new HttpResponseMessage() {  Content= new StringContent("Subcategory is in use") };
				else
					SubcategoryRepository.Delete(id);
			}
			catch(Exception exc)
			{
				logger.Error("Fail to delete subcategory. ", exc);
				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
        }
	}
}