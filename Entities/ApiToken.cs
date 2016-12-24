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
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired {
            get {
                return DateTime.UtcNow > ExpiryDate;
            }
        }

        public static ApiToken Create(int accountId, TimeSpan lifeLength)
        {
            return new ApiToken() {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.Add(lifeLength)
            };
        }
    }
}
