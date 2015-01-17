using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Monei.DataAccessLayer.Extensions;
using Monei.DataAccessLayer.Exceptions;
using System.Text.RegularExpressions;

namespace Monei.DataAccessLayer.MongoDB
{

	public class CategoryRepository : MongoDBRepositoryBase<Category>, ICategoryRepository
	{
		private const string COLLECTION_NAME = "Category";

		private const int MAX_NAME_LENGTH = 25;	// max length for name


		private static CategoryRepository instance = new CategoryRepository();

		//public static CategoryRepository Instance { get {
		//	return instance;
		//} }

	    public static new MongoCollection<Category> GetCollection() {
			var collection = GetCollection(COLLECTION_NAME);
			collection.EnsureIndex(new IndexKeysBuilder().Ascending("Name"), IndexOptions.SetUnique(true));
			return collection;
		}


		public IEnumerable<Category> List() {

			return GetCollection().FindAll().OrderBy(c => c.Name).ToList();
		}

		public IEnumerable<Category> ListWithSubcategories()
		{
			//return GetCollection().FindAll().OrderBy(c => c.Name).ToList();
			return List();
		}


		public Category Create(Category item)
		{
			item.CreationDate = DateTime.UtcNow;
			//item.CreationAccount = account;

			Validate(item);

			GetCollection().Insert(item);
			return item;
		}

		public Category Update(Category item)
		{
			Validate(item);

			Category updateItem = Read(item.Id);

			updateItem.LastChangeDate = DateTime.UtcNow;
			//updateItem.LastUpdateAccount = account;

			updateItem.Name = item.Name;
			updateItem.Description = item.Description;
			

			//Update<Category>.Set(updateItem)//

			var result = GetCollection().Save(updateItem);

			if (result.Ok)
			{
				return item;
			}
			else
				throw new Exception("Update of Category fail.\r\n + result.LastErrorMessage");

			//GetCollection().Update( Query.EQ("_id", item.Id),
			//	Update.Set()
			//	);
	
		}

		private void Validate(Category item)
		{
			if (string.IsNullOrWhiteSpace(item.Name))
				throw new CategoryEmptyNameNameException();

			if (item.Name.Length > MAX_NAME_LENGTH)
				throw new CategoryTooLongNameException(MAX_NAME_LENGTH);

			// check if name is unique
			if(Read(item.Name) != null)
				throw new CategoryNameAlreadyExistsException();
		}

		public Category Read(int id) {
			return GetCollection().FindOneById(id);
		}

		public Category Read(string name)
		{			
			IMongoQuery query = Query.Matches("Name", new BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase)));
			return GetCollection().FindOne(query);
		}

		public void Delete(int id)
		{
			GetCollection().RemoveById(id);
		}
		
		
		public void MoveSubcategory(int subcategoryId, int categoryId)
		{
			throw new NotImplementedException();
		}

	}//class
}
