using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OfficeCommunicator.Dal.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<ChatMessage> SentMessages { get; set; }

        public virtual ICollection<ChatMessage> ReceivedMessages { get; set; }
    }
}
