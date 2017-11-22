using System.ComponentModel.DataAnnotations;

namespace OfficeCommunicator.Dal.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
