using MapDB.Api.Entities;

namespace MapDB.Api.Repositories{
    public interface IPinsRepository{

            // Compiler returns a task, that will eventually return an Pin (because of async)
            Task<Pin> GetPinAsync(Guid ID);
            
            Task<IEnumerable<Pin>> GetPinsAsync();

            Task CreatePinAsync(Pin Pin);

            Task UpdatePinAsync(Pin Pin);

            Task DeletePinAsync(Guid ID);
    }
}