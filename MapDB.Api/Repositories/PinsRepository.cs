using MapDB.Api.Entities;

namespace MapDB.Api.Repositories{

    /* Deprecated class from before MongoDB was implemented. */
    public class PinsRepository : IPinsRepository{

        private readonly List<Pin> Pins = new(){
            new Pin { ID = Guid.NewGuid(), Name = "Victoria University", Location = "Wellington", Category = "School", Description = "", CreationDate = DateTimeOffset.UtcNow },
            new Pin { ID = Guid.NewGuid(), Name = "Massey University", Location = "Palmerston North", Category = "School", Description = "", CreationDate = DateTimeOffset.UtcNow },
            new Pin { ID = Guid.NewGuid(), Name = "Whitireia", Location = "Porirua", Category = "School", Description = "", CreationDate = DateTimeOffset.UtcNow },
        };

        public async Task<IEnumerable<Pin>> GetPinsAsync(){
            return await Task.FromResult(Pins); // returns a completed task with the Pins in it
        }

        public async Task<Pin> GetPinAsync(Guid ID){
            var Pin = Pins.Where(Pin => Pin.ID == ID).SingleOrDefault(); 
            return await Task.FromResult(Pin);
        }

        public async Task CreatePinAsync(Pin Pin)
        {
            Pins.Add(Pin);
            await Task.CompletedTask; // there is nothing to return, so only returns a completed task
        }

        public async Task UpdatePinAsync(Pin Pin)
        {
            var index = Pins.FindIndex(existingPin => existingPin.ID == Pin.ID);
            Pins[index] = Pin;
            await Task.CompletedTask;
        }

        public async Task DeletePinAsync(Guid ID)
        {
            var index = Pins.FindIndex(existingPin => existingPin.ID == ID);
            Pins.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}