using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record MovieDTO(
        int Id,
        [Required] string Name,
        //[Required, Range(1, int.MaxValue)] int Seats,
        [Required, DataType(DataType.Currency)] decimal Price
        );
           
}
