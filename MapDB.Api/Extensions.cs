using MapDB.Api.DTOs;
using MapDB.Api.Entities;

namespace MapDB.Api
{
    public static class Extensions
    {
        public static PinDTO AsDTO(this Pin Pin){      
            return new PinDTO(Pin.ID, Pin.Name, Pin.Location, Pin.Category, Pin.Description, Pin.CreationDate);
        }
    }
}