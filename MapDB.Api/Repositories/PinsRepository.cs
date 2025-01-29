using MapDB.Api.Entities;

namespace MapDB.Api.Repositories{

    /* Deprecated class from before MongoDB was implemented. */
    public class PinsRepository : IPinsRepository{

        private readonly List<Pin> Pins = new(){
            new Pin { ID = Guid.NewGuid(), Name = "Soilpro NZ", Coordinates = [175.0183659089638, -37.20317658868487], Category = "Good Friend", Description = "", CreationDate = DateTimeOffset.UtcNow },
            new Pin { ID = Guid.NewGuid(), Name = "Birchville Community Garden", Coordinates = [175.0991339, -41.09363516], Category = "Project", Description = "", CreationDate = DateTimeOffset.UtcNow },
            new Pin { ID = Guid.NewGuid(), Name = "Tolaga Bay Inn Charitable Trust", Coordinates = [178.2971262, -38.37198794], Category = "Carbon Farmer", Description = "", CreationDate = DateTimeOffset.UtcNow },
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