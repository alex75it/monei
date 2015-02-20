using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Should;

using Monei.Test.UnitTest;

namespace Monei.Test.UnitTest.DataAccessLayer.Filters
{

	[TestClass]
	public class RegistryFiltersTest :TestBase
	{

	[TestMethod]
	public void Normalize_WhenDAtesAreNotSet()
	{
		RegistryFilters filters = new RegistryFilters();
		filters.Normalize();

		// Verify
		VerifyDates(filters);
		filters.SelectedPeriod.ShouldBeNull();
	}

	[TestMethod]
	public void Normalize_WhenStartDateIsMinDate()
	{
		// Arrange
		RegistryFilters filters = new RegistryFilters();
		filters.StartDate = DateTime.MinValue;

		// Act
		filters.Normalize();

		// Assert
		VerifyDates(filters);
	}
		

	[TestMethod]
	public void Normalize_WhenStartDateIsTooLow()
	{
		RegistryFilters filters = new RegistryFilters();
		filters.StartDate = new DateTime(500, 01, 01);
		filters.Normalize();

		// Verify
		VerifyDates(filters);
	}

	[TestMethod]
	public void Normalize_WhenEndDateIsToLow()
	{
		// Arrange
		RegistryFilters filters = new RegistryFilters();
		filters.EndDate = DateTime.MinValue;

		// Act
		filters.Normalize();

		// Assert
		VerifyDates(filters);
	}

	[TestMethod]
	public void Normalize_WhenEndDateIsTooBig()
	{
		RegistryFilters filters = new RegistryFilters();
		filters.EndDate = new DateTime(9000, 01, 01);
		filters.Normalize();

		// Verify
		VerifyDates(filters);
	}

	[TestMethod]
	public void Normalize_SwitchDates_WhenThereAreInverted()
	{
		RegistryFilters filters = new RegistryFilters();
		filters.StartDate = new DateTime(9000, 01, 01);
		filters.EndDate = new DateTime(500, 01, 01);
		filters.Normalize();

		// Verify
		filters.StartDate.ShouldBeLessThanOrEqualTo(filters.EndDate);
		VerifyDates(filters);
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

	#region private
	private static void VerifyDates(RegistryFilters filters)
	{
		filters.StartDate.ShouldBeValidSqlDate();
		filters.EndDate.ShouldBeValidSqlDate();
		filters.StartDate.ShouldBeLessThanOrEqualTo(filters.EndDate);
	}

	#endregion

	}
}
