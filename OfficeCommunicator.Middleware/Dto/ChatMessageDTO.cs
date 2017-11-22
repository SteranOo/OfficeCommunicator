using System;
using System.Runtime.Serialization;

namespace OfficeCommunicator.Middleware.Dto
{
    [DataContract]
    public class ChatMessageDto
    {
        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Sender { get; set; }

        [DataMember]
        public string Recipient { get; set; }

        [DataMember]
        public DateTime Time { get; set; }
    }
}
