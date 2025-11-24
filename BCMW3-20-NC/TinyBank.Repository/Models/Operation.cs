using TinyBank.Repository.Attributes;
using TinyBank.Repository.Models.Enums;

namespace TinyBank.Repository.Models
{
    [DtoTransformable]
    public class Operation
    {
        public int Id { get; set; }
        public OperationType OperationType { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public DateTime HappendAt { get; set; } = DateTime.Now;
    }
}
