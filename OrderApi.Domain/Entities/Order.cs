using ETickets.SharedLibrary.Data;

namespace OrderApi.Domain.Entities
{
    public class Order : IBaseEntity
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ClientId { get; set; }
        public int PurchaseSeats { get; set; }
        public DateTime OrderedDate { get; set; } = DateTime.UtcNow;
    }
}
