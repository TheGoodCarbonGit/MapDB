using System;
using System.ComponentModel.DataAnnotations;

namespace MapDB.Api.DTOs
{
    public record PinDTO(Guid ID, string Name, double [] Coordinates, string Category, string Description, DateTimeOffset CreationDate);

    public record CreatePinDTO([Required] string Name, double[] Coordinates, [Required] string Category, string Description);

    public record UpdatePinDTO([Required] string Name, double[] Coordinates, [Required] string Category, string Description);
}