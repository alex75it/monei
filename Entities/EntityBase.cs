using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Monei.Entities
{
    public class EntityBase<TKey>
    {
        public const int EMPTY_ID = 0;

        public EntityBase()
        {
            CreationDate = DateTime.UtcNow;
            LastChangeDate = null;
        }

        public virtual TKey Id { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime? LastChangeDate { get; set; }
        public virtual Account CreationAccount { get; set; }
        public virtual Account LastUpdateAccount { get; set; }
    }
}
