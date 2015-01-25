using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;

namespace Monei.Test.IntegrationTest.DataAccessLayer.SqlServer
{

	public class TestHelper
	{

		public readonly string TEST_USERNAME = "TEST name";
		public readonly string DEMO_USERNAME = "DEMO name";

		public ICurrencyRepository CurrencyRepository { get; set; }
		public IAccountRepository AccountRepository { get; set; }
		public ICategoryRepository CategoryRepository { get; set; }
		public IRegistryRepository RegistryRepository { get; set; }
		


		private Random random = new Random(DateTime.Now.Millisecond);



		/// <summary>
		/// Compare DateTime witout check miliseconds (these is lose when date is stored in datetime field of SQL Server).
		/// Also Kind can change when stored in SQL Server (a Kind.Local date become Kind.Unspecified).
		/// Other difference on 
		/// </summary>
		/// <param name="date_A"></param>
		/// <param name="date_B"></param>
		/// <returns></returns>
		//public int AreEqualDate(DateTime date_A, DateTime date_B)
		//{
		//	//date_A.Ticks-date_B.Tick;

		//}


		public Account GetTestAccount()
		{
			string username = TEST_USERNAME;
			string password = "Test";
			Currency currency = CurrencyRepository.Read(Currency.EUR_CODE);
			Account.AccountRole role = Account.AccountRole.User;

			Account account = AccountRepository.Read(username);

			if (account == null)
				account = AccountRepository.Create(username, password, role, currency);

			return account;
		}

		public Account GetDemoAccount()
		{
			string username = DEMO_USERNAME;
			Account account = AccountRepository.Read(username);
			if (account == null)
				throw new Exception("Cannot load demo account");
			return account;
		}

		public Category GetRandomCategory()
		{
			Category category = CategoryRepository.List().FirstOrDefault();

			if (category == null)
			{
				category = new Category()
				{
					Name = "Test Category",
					Description = "Test category escription",
					ImageName = null,
				};

				Account currentAccount = GetTestAccount();
				category.CreationAccount = currentAccount;
				category = CategoryRepository.Create(category);
				//Assert.Inconclusive("Cannot get a random Category");
			}

			return category;
		}

		internal Currency GetEuroCurrency()
		{
			Currency currency = CurrencyRepository.Read(Currency.EUR_CODE);
			return currency;
		}

		internal bool RemoveTestAccount()
		{
			try
			{
				Account account = AccountRepository.Read(TEST_USERNAME);
				if (account != null)
				{
					// todo Create a business object, delete all records of Account in a single operation

					foreach (var record in RegistryRepository.ListRecords(new Monei.DataAccessLayer.Filters.RegistryFilters() { AccountId = account.Id }))
					{
						RegistryRepository.DeleteRecord(record.Id);
					}

					// remove Account
					AccountRepository.Delete(account.Id);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("Fail to delete test Account. " + exc.Message);
				throw;
			}

			return true;
		}

		internal RegistryRecord CreateRecord(DateTime date, decimal amount, string note, bool isTaxDeductible, bool isSpecialEvent, Account account, Category category)
		{
			RegistryRecord record = new RegistryRecord()
			{
				Date = date,
				Amount = amount,
				Category = category,
				Subcategory = null,
				Account = account,
				Note = note,
				CreationAccount = account,
				CreationDate = DateTime.Now,
				OperationType = Monei.Entities.OperationType.Outcome,
				LastChangeDate = null,
				LastUpdateAccount = null,
			};
			return record;
		}

		internal Subcategory CreateRandomSubcategory()
		{
			string name = "Name " + random.Next();
			string description = name + " description";
			Account account = GetTestAccount();
			Category category = GetRandomCategory();

			Subcategory subcategory = new Subcategory()
			{
				Name = name,
				Description = description,
				Category = category,
				CreationAccount = account,
			};

			return subcategory;
		}

	}
}
