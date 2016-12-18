using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Entities
{
    public class ApiToken
    { 
        public Guid Id { get; set; }

        public int AccountId { get; set; }
        
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsExpired {
            get {
                return DateTime.UtcNow > ExpireDate;
            }
        }
    }
}
