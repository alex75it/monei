using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Monei.Entities;
using Monei.MvcApplication.Api.PostDataObjects;

namespace Monei.MvcApplication.Api
{	
    [RoutePrefix("api/category")]
    public class CategoryApiController : ApiControllerBase
    {
        [HttpGet, Route("ping")]
        public void Ping()
        {
            //return "pong";
        }

        [HttpGet, Route("list")]
        public IEnumerable<Category> Get()
        {
            IEnumerable<Category> list = CategoryRepository.List();
            // HACK: remove the items otherwise there is a LazyInitializationException
            //foreach(var category in list)
            //{
            //    category.Subcategories = null;
            //}

            return list;
        }

        [HttpGet, Route("list")]
        public IEnumerable<Category> Get(string orderBy)
        {
            IEnumerable<Category> list = CategoryRepository.List();
            // delete subitems to avoid LazyInitializationException
            //foreach (var category in list)
            //{
            //    category.Subcategories = null;
            //}

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
    }
}