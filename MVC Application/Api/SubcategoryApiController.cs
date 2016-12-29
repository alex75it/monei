using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Monei.Entities;
using Monei.MvcApplication.Api.PostDataObjects;

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
            var subcategories = SubcategoryRepository.List(categoryId);

            // HACK: remove subitems to avoid LazyInitializationException            
            foreach (var subcategory in subcategories)
                subcategory.Category = null;

            return subcategories;
        }

        [HttpPost, Route("")]
        public int Create(SubcategoryPostData data)
        {
            Subcategory entity = new Subcategory() {
                Category = new Category() { Id = data.CategoryId},
                Name = data.Name,
                Description = data.Description,
            };
            
            int newId = SubcategoryRepository.Create(entity);

            return newId;
        }

        [HttpDelete, Route("{id}")]
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