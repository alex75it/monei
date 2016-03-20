using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monei.DataAccessLayer.Extensions;

namespace Monei.DataAccessLayer.Filters
{
    public class BaseFilters
    {

        public DateTime NormalizeDate(DateTime date)
        {
            return date.NormalizeForSql();
        }

    }
}
