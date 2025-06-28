using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDTO
    (
        int Id,
        [Required, Range(1, int.MaxValue)] int MovieId,
        [Required, Range(1, int.MaxValue)] int ClientId,
        //[Required, Range(1, int.MaxValue)] int PurchaseSeats,
        DateTime OrderedDate

    );
}
