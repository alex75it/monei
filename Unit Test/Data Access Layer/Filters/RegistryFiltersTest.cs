using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Should;

namespace Monei.Test.UnitTest.DataAccessLayer.Filters
{

	[TestClass]
	public class RegistryFiltersTest
	{
		private DateTime minSqlDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
		private DateTime maxSqlDate = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

		[TestMethod]
		public void Normalize()
		{
			RegistryFilters filters = new RegistryFilters();
			filters.Normalize();

			// Verify
			filters.StartDate.ShouldBeInRange(minSqlDate, maxSqlDate);
			filters.EndDate.ShouldBeInRange(minSqlDate, maxSqlDate);
			filters.SelectedPeriod.ShouldBeNull();
		}


		[TestMethod]
		public void Normalize_SetMinDate_WhenItIsTooLow()
		{
			RegistryFilters filters = new RegistryFilters();
			filters.StartDate = new DateTime(500, 01, 01);
			filters.Normalize();

			// Verify
			filters.StartDate.ShouldBeInRange(minSqlDate, maxSqlDate);
		}

		[TestMethod]
		public void Normalize_SetMaxDate_WhenItIsTooBig()
		{
			RegistryFilters filters = new RegistryFilters();
			filters.EndDate = new DateTime(9000, 01, 01);
			filters.Normalize();

			// Verify
			filters.EndDate.ShouldBeInRange(minSqlDate, maxSqlDate);
		}

		[TestMethod]
		public void Normalize_SwitchDates_WhenThereAreInverted()
		{
			RegistryFilters filters = new RegistryFilters();
			filters.StartDate = new DateTime(9000, 01, 01);
			filters.EndDate = new DateTime(500, 01, 01);
			filters.Normalize();

			// Verify
			filters.StartDate.ShouldBeInRange(minSqlDate, maxSqlDate);
			filters.EndDate.ShouldBeInRange(minSqlDate, maxSqlDate);
			filters.StartDate.ShouldBeLessThan(filters.EndDate);
		}

		[TestMethod]
		public void SetOperationType()
		{
			RegistryFilters filters = new RegistryFilters();
			filters.SetOperationType(OperationType.Transfer, true);
			filters.SetOperationType(OperationType.Outcome, false);

			// Verify
			filters.OperationTypes.ShouldContain(OperationType.Transfer);
			filters.OperationTypes.ShouldNotContain(OperationType.Outcome);
		}

	}
}
