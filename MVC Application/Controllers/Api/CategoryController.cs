using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.Entities;
using Monei.MvcApplication.Controllers.Api.PostDataObjects;

namespace Monei.MvcApplication.Controllers.Api
{
	public class CategoryController : ApiControllerBase
    {
		//[HttpGet, Route("Tree", Name = "Tree")]
		//public IList<dynamic> Tree()
		//{
		//	//return "{ method: 'ListByType', typeId: " + typeId + "}";
		//	//, subcategories=c.Subcategories.ToList()
		//	IList<dynamic> list = CategoryRepository.List().Select(c =>
		//		new { id = c.Id, name = c.Name, subcategories = c.Subcategories.Select( s => new { id=s.Id, name=s.Name }) })
		//		.ToList<dynamic>();
		//	return list;
		//}

		[HttpGet /*Route("action/Category/Tree", Name = "Tree")*/]
		public IList<dynamic> Tree()
		{			
			IList<dynamic> list = CategoryRepository.ListWithSubcategories().Select(c =>
				new { id = c.Id, name = c.Name, subcategories = c.Subcategories.Select(s => new { id = s.Id, name = s.Name }).ToList() })
				.ToList<dynamic>();
			return list;
		}


		[HttpPost]
		//public bool MoveSubcategory([FromBody]int subcategoryId, [FromBody]int categoryId)
		public string MoveSubcategory(MoveSubcategoryPostData data)
		{
			try
			{
				CategoryRepository.MoveSubcategory(data.SubcategoryId, data.CategoryId);
				return "OK";
			}
			catch (Exception exc)
			{
				Guid errorId = Guid.NewGuid();
				logger.ErrorFormat("Fail to move Subcategory. Subcategory ID: {0}, Category ID: {1}. Error ID: {2}. {3}.", data.SubcategoryId, data.CategoryId, errorId, exc);
				return string.Format("Fail to move Subcategory. Error ID: {0}.", errorId);
			}
		}

		//// GET api/category
		//public IEnumerable<string> Get()
		//{
		//	return new string[] { "value1", "value2" };
		//}

		//// GET api/category/5
		//public string Get(int id)
		//{
		//	return "value";
		//}

        // POST api/category
        public void Post([FromBody]string value)
        {
        }

        // PUT api/category/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/category/5
        public void Delete(int id)
        {
        }
    }
}
