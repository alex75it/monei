using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.Entities;
using Monei.Entities.Interfaces;
using NUnit.Framework;
using Should;

namespace Monei.Test.UnitTest.Entities
{
	[TestFixture]
	public class RegistryRecordTest
	{
		[Test]
		public void RealAmout_ShouldReturnAValueForIncomeOperation()
		{
			RegistryRecord record = new RegistryRecord();
			record.OperationType = OperationType.Income;
			record.Amount = 123.34m;

			record.RealAmount.ShouldEqual(record.Amount);
		}

		[Test]
		public void RealAmout_ShouldReturnAValueForOutcomeOperation()
		{
			RegistryRecord record = new RegistryRecord();
			record.OperationType = OperationType.Outcome;
			record.Amount = 1570.00m;

			record.RealAmount.ShouldEqual(-1*record.Amount);
		}

		[Test]
		public void RealAmout_ShouldReturnZeroForTransferOperation()
		{
			RegistryRecord record = new RegistryRecord();
			record.OperationType = OperationType.Transfer ;
			record.Amount = 250m;

			record.RealAmount.ShouldEqual(0);
		}

		[Test]
		public void CreateHeadersRow()
		{
			RegistryRecord record = new RegistryRecord();
			record.OperationType = OperationType.Transfer;
			record.Amount = 250m;

			string headerRow = ((ICsvFormattable)record).CreateHeadersRow();

			headerRow.ShouldNotBeNull();
			headerRow.ShouldNotBeEmpty();
		}

		[Test]
		public void CreateRow()
		{
			RegistryRecord record_1 = new RegistryRecord();
			record_1.Category = new Category() { Name = "Cat A"};
			record_1.Date = DateTime.Today;
			record_1.OperationType = OperationType.Transfer;
			record_1.Amount = 100m;

			RegistryRecord record_2 = new RegistryRecord();
			record_2.Category = new Category() { Name = "Cat B" };
			record_2.Date = DateTime.Today;
			record_2.OperationType = OperationType.Income;
			record_2.Amount = 200m;

			RegistryRecord record_3 = new RegistryRecord();
			record_3.Category = new Category() { Name = "Cat C" };
			record_3.Subcategory = new Subcategory() { Name="Subcat A"};
			record_3.Date = DateTime.Today;
			record_3.OperationType = OperationType.Outcome;
			record_3.Amount = 300m;

			foreach (var record in new RegistryRecord[]
			{
				record_1, record_2, record_3
			})
			{
				string row = ((ICsvFormattable)record).CreateRow();

				row.ShouldNotBeNull();
				row.ShouldNotBeEmpty();
			}

		}

	}
}
