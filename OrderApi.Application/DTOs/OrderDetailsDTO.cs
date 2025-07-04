﻿using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO(
        [Required] int OrderId,
        [Required] int TicketId,
        [Required] int ClientId,
        [Required] string Name,
        [Required, EmailAddress] string Email,
        [Required, EmailAddress] string Address,
        [Required] string TelephoneNumber,
        [Required] string MovieName,
        [Required] int PurchaseSeats,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderedDate
        );
    
}
