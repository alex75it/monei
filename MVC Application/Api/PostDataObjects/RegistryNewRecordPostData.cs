using Monei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monei.MvcApplication.Api.PostDataObjects
{
    public class RegistryNewRecordPostData :RegistryRecord
    {
        public int CategoryId { get; set; }

        public int SubcategoryId { get; set; }


    }
}