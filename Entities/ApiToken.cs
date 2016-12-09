using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monei.Entities
{
    public class ApiToken
    {
        public int UserId { get; set; }

        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsExpired {
            get {
                return ExpirationDate < DateTime.UtcNow;
            }
        }
    }
}
