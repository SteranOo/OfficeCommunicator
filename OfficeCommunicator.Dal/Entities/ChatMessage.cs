using System;
using System.ComponentModel.DataAnnotations;

namespace OfficeCommunicator.Dal.Entities
{
    public class ChatMessage : BaseEntity
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Time { get; set; }

        public int SenderId { get; set; }

        public virtual User Sender { get; set; }

        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }
    }
}
