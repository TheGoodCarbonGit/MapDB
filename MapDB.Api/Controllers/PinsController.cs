using System.ComponentModel;
using MapDB.Api.DTOs;
using MapDB.Api.Entities;
using MapDB.Api.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MapDB.Api.Controllers{

    [ApiController] // marks it as an API controller, allows certain behaviours
    [Route("Pins")] // route for the browser

    public class PinsController : ControllerBase { // implements ControllerBase

        private readonly IPinsRepository repository;

        public PinsController(IPinsRepository repository){ // injects dependency
            this.repository = repository;
        }

        // GET /
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpGet]

        public async Task<IEnumerable<PinDTO>> GetPinsAsync(string name = null){
            var Pins = (await repository.GetPinsAsync()) // completes this task first
                        .Select( Pin => Pin.AsDTO());

            if(!string.IsNullOrWhiteSpace(name)){
                Pins = Pins.Where(Pin => Pin.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            return Pins;
        }
        
        // GET /Pins/{ID}
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpGet("{ID}")]
        public async Task<ActionResult<PinDTO>> GetPinAsync(Guid ID){ // allows the return of NotFound and Pin
            var Pin = await repository.GetPinAsync(ID);

            if(Pin is null){
                return NotFound();
            }

            return Pin.AsDTO();
        }

        // POST /Pins
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<ActionResult<PinDTO>> CreatePinAsync(CreatePinDTO PinDTO){
            Pin Pin = new(){
                ID = Guid.NewGuid(),
                Name = PinDTO.Name,
                Coordinates = PinDTO.Coordinates,
                Category = PinDTO.Category,
                Description = PinDTO.Description,
                CreationDate = DateTimeOffset.UtcNow
            };

            await repository.CreatePinAsync(Pin);
            return CreatedAtAction(nameof(GetPinAsync), new { id = Pin.ID}, Pin.AsDTO()); // returns a 201
        }

        // PUT /Pins/{ID}
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPut("{ID}")]
        public async Task<ActionResult> UpdatePinAsync(Guid ID, UpdatePinDTO PinDTO){ 
            var existingPin = await repository.GetPinAsync(ID);

            if(existingPin is null){
                return NotFound();
            }

            existingPin.Name = PinDTO.Name;
            existingPin.Coordinates = PinDTO.Coordinates;
            existingPin.Category = PinDTO.Category;
            existingPin.Description = PinDTO.Description;

            await repository.UpdatePinAsync(existingPin);

            return NoContent(); // conventions of put methods do not require a return
        } 

        // DELETE /Pins/{ID}
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpDelete("{ID}")]
        public async Task<ActionResult> DeletePinAsync(Guid ID){
            var existingPin = await repository.GetPinAsync(ID);

            if(existingPin is null){
                return NotFound();
            }

            await repository.DeletePinAsync(ID);

            return NoContent();
        }
    }
}