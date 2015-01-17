using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Monei.Entities;

namespace Monei.MvcApplication.Models
{
	public class ContractListModel
	{
		public IEnumerable<Contract> Contracts { get; set; } 

	}

	public class ContractModel {
		public Contract Contract { get; set; }
	}



}