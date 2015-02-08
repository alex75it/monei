using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using Core;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using Monei.MvcApplication.Controllers.Api.PostDataObjects;
using Newtonsoft.Json.Linq;

namespace Monei.MvcApplication.Api
{
	//[RoutePrefix("subcategory")]
	public class SubcategoryController : ApiControllerBase
    {
		//public ISubcategoryRepository SubcategoryRepository { get; set; }

        // GET api/subcategory
        public IEnumerable<Subcategory> Search(int categoryId)
        {
			return SubcategoryRepository.List(categoryId);

            //return new string[] { "value1", "value2" };
        }

        // GET api/subcategory/5
		//public string Get(int id)
		//{
		//	return "Id: " + id.ToString();
		//}

		// GET api/subcategory/ListByType?typeId=5
		//[ActionName("ListByType")]
		//[HttpGet]
		//public string ListByType([FromUri(Name="typeId")]int typeId)
		//{
		//	return "{ method: 'ListByType', typeId: " + typeId + "}";
		//}

		// GET api/subcategory/ListByType/5
		//[ActionName("ListByType")]
		//[Route("ListByType/{p1:int}")]
		[HttpGet, Route("ListByType/{typeId}", Name = "LisyByType")]
		public string ListByType(int typeId)
		{
			return "{ method: 'ListByType', typeId: " + typeId + "}";
		}

		//[Route("ListByType/{typeId:int}")]
		//[HttpGet]
		//public string ListByType_2(int typeId)
		//{
		//	return "{ method: 'ListByType', typeId: " + typeId + "}";
		//}

		////[ActionName("ListByType")]
		//[Route("ListByType/typeId/{typeId:int}")]
		//////[HttpGet]
		//public string List_ByType(int typeId)
		//{
		//	return "{ method: 'ListByType', typeId: " + typeId + "}";
		//}

		//// GET api/subcategory/ListByType/5
		//[ActionName("ListByType")]
		//[Route("ListByType")]
		//[HttpGet]
		//public string GetListByType()
		//{
		//	return "{ method: 'ListByType', typeId: " + "stocazzo" + "}";
		//}


        // POST api/subcategory
        //public int Post([FromBody]string categoryId, [FromBody]string name, [FromBody]string description)
		// Web API POST cannot have more than one parameter
		public dynamic Post([FromBody]string value)
        {
			logger.Debug("Post: " + value);

			//NameValueCollection data =  HttpUtility.ParseQueryString(value);
			
			dynamic json = JValue.Parse(value);
			
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

        // PUT api/subcategory/5
        public void Put(int id, [FromBody]string categoryId)
        {
			// todo: use a business object
		}

        // DELETE api/subcategory/5
		[HttpDelete]
        public virtual HttpResponseMessage Delete(int id)
        {
			//ValidateRequest();

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
