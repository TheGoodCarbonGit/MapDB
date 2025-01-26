using System;
using System.ComponentModel.DataAnnotations;

namespace MapDB.Api.DTOs
{
    public record PinDTO(Guid ID, string Name, string Location, string Category, string Description, DateTimeOffset CreationDate);

    public record CreatePinDTO([Required] string Name, string Location, [Required] string Category, string Description);

    public record UpdatePinDTO([Required] string Name, string Location, [Required] string Category, string Description);
}