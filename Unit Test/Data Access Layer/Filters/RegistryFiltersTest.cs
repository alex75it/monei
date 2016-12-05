using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Filters;
using Monei.Entities;
using Should;

using Monei.Test.UnitTest;
using NUnit.Framework;

namespace Monei.Test.UnitTest.DataAccessLayer.Filters
{

    [TestFixture,Category("Data Access Layer")]
    public class RegistryFiltersTest : TestBase
    {

        [Test]
        public void Normalize_when_DatesAreNotSet_should_SetValidDates()
        {
            RegistryFilters filters = new RegistryFilters();
            filters.Normalize();

            // Verify
            VerifyDates(filters);
        }

        [Test]
        public void Normalize_when_StartDateIsMinDate_should_SetValidDates()
        {
            // Arrange
            RegistryFilters filters = new RegistryFilters();
            filters.StartDate = DateTime.MinValue;

            // Act
            filters.Normalize();

            // Assert
            VerifyDates(filters);
        }


        [Test]
        public void Normalize_when_StartDateIsTooLow_should_SetValidDates()
        {
            RegistryFilters filters = new RegistryFilters();
            filters.StartDate = new DateTime(500, 01, 01);
            filters.Normalize();

            // Verify
            VerifyDates(filters);
        }

        [Test]
        public void Normalize_when_EndDateIsToLow_should_SetValidDates()
        {
            // Arrange
            RegistryFilters filters = new RegistryFilters();
            filters.EndDate = DateTime.MinValue;

            // Act
            filters.Normalize();

            // Assert
            VerifyDates(filters);
        }

        [Test]
        public void Normalize_when_EndDateIsTooBig_should_SetValidDates()
        {
            RegistryFilters filters = new RegistryFilters();
            filters.EndDate = new DateTime(9000, 01, 01);
            filters.Normalize();

            // Verify
            VerifyDates(filters);
        }

        [Test]
        public void Normalize_when_GivenDatesAreInverted_should_SwitchDates()
        {
            RegistryFilters filters = new RegistryFilters();
            filters.StartDate = new DateTime(9000, 01, 01);
            filters.EndDate = new DateTime(500, 01, 01);
            filters.Normalize();

            // Verify
            filters.StartDate.ShouldBeLessThanOrEqualTo(filters.EndDate);
            VerifyDates(filters);
        }

        [Test]
        public void ShowOnlytaxDeductible_should_SetThePropertyCorrectly()
        {
            RegistryFilters filters = new RegistryFilters();
            filters.ShowOnlyTaxDeductible = true;

            filters.ShowOnlyTaxDeductible.ShouldBeTrue();
        }

        [Test]
        public void SetTextToSearch()
        {
            RegistryFilters filters = new RegistryFilters();
            string textToSearch = "txst";
            filters.TextToSearch = textToSearch;

            filters.TextToSearch.ShouldEqual(textToSearch);
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
