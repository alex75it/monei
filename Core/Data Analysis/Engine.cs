using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Core.DataAnalysis.BusinessObjects;
using Monei.DataAccessLayer.Filters;
using Monei.DataAccessLayer.Interfaces;
using Monei.Entities;


namespace Monei.Core.DataAnalysis
{
	public class Engine
	{
		private IRegistryRepository registryRepository;

		public Engine(IRegistryRepository registryRepository)
		{
			this.registryRepository = registryRepository;
		}

		public YearData GetYearData(int accountId, int year)
		{ 
			RegistryFilters filters = new RegistryFilters(){
				AccountId = accountId,
				StartDate = new DateTime(year, 1 ,1),
				EndDate = new DateTime(year, 12, 31),
			};

			CategoriesData categoryData = new CategoriesData();

			var records = registryRepository.ListRecords(filters);


			//todo: refactoring to make it testable and to divide responsabilities

			List<Category> usedCategories = records.Select(r => r.Category).Distinct().ToList();

			YearData yearData = new YearData(year);
			yearData.Income = records.Where(r => r.OperationType == Entities.OperationType.Income).Sum(r => r.Amount);
			yearData.Outcome = records.Where(r => r.OperationType == Entities.OperationType.Outcome).Sum(r => r.Amount);
			yearData.Result = yearData.Income - yearData.Outcome;
			//yearData.Categories = ;

			var recordsForCategory = records.GroupBy( r => r.Category).ToList();
			foreach(var group in recordsForCategory)
				categoryData.AddValue(group.Key.Id, new CategoryData(group.Key, group.Sum(r => r.RealAmount)));

			var recordsForMonths = records.GroupBy(r => r.Date.Month).OrderBy(m => m.Key).ToList();

			foreach (var monthRecords in recordsForMonths)
			{
				MonthData monthData = new MonthData(monthRecords.Key);
				monthData.Income = monthRecords.Where(r => r.OperationType == OperationType.Income).Sum(r => r.Amount);
				monthData.Outcome = monthRecords.Where(r => r.OperationType == OperationType.Outcome).Sum(r => r.Amount);
				monthData.Result = monthData.Income - monthData.Outcome;
				
				foreach(var byCategory in monthRecords.GroupBy(r => r.Category).ToList())
					monthData.Categories.AddValue(byCategory.Key.Id, new CategoryData(byCategory.Key, byCategory.Sum(r => r.RealAmount)));
				monthData.Categories.AddNewCategories(usedCategories);

				yearData.Months.Add(monthData);
			}

			return yearData;
		}


	}
}
