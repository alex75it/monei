using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monei.Entities
{
    public class Subcategory :EntityBase<int>
    {
        public const int NAME_MAX_LENGTH = 25;
        public const int DESCRIPTION_MAX_LENGTH = 1000;
        
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }        

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return obj is Subcategory && ((Subcategory)obj).GetHashCode() == GetHashCode();
        }
    }
}
