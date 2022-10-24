using Clippers.Core.EventStore;
using Clippers.Core.Haircut.Models;
using Clippers.Core.Haircut.Repository;

namespace Clippers.Infrastructure.Repositories
{
    public class HaircutRepository : IHaircutRepository
    {
        private readonly IEventStore _eventStore;

        public HaircutRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<HaircutModel> LoadHaircut(string id)
        {
            var streamId = $"haircut:{id}";

            var stream = await _eventStore.LoadStreamAsync(streamId);

            return new HaircutModel(stream.Events);
        }

        public async Task<bool> SaveHaircut(HaircutModel haircut)
        {
            if (haircut.Changes.Any())
            {
                var streamId = $"haircut:{haircut.HaircutId}";

                return await _eventStore.AppendToStreamAsync(
                    streamId,
                    haircut.Version,
                    haircut.Changes);
            }

            return true;
        }
    }
}
