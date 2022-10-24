using Clippers.EventFlow.Projections.Core.Interfaces;
using Clippers.EventFlow.Projections.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Clippers.EventFlow.Projections.Infrastructure.Cosmos
{
    public class EventObserver : IChangeFeedObserver
    {
        private readonly List<IProjection> _projections;
        private readonly IViewRepository _viewRepository;
        private readonly ILogger _logger;
        private IHubContext<NotificationHub> _notificationHub;

        public EventObserver(List<IProjection> projections, IViewRepository viewRepostory, ILogger<EventObserver> logger, IHubContext<NotificationHub> notificationHub)
        {
            _projections = projections;
            _viewRepository = viewRepostory;
            _logger = logger;
            _notificationHub = notificationHub;
        }

        public Task OpenAsync(IChangeFeedObserverContext context)
        {
            return Task.CompletedTask;
        }

        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            return Task.CompletedTask;
        }

        public async Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> documents, CancellationToken cancellationToken)
        {
            foreach (var document in documents)
            {
                var @event = DeserializeEvent(document);
                if (@event is null)
                {
                    continue;
                }
                foreach (var projection in _projections)
                {
                    if (!projection.CanHandle(@event))
                    {
                        continue;
                    }

                    var streamInfo = document.GetPropertyValue<JObject>("stream");
                    var viewName = projection.GetViewName(streamInfo["id"].Value<string>(), @event);

                    var handled = false;
                    while (!handled)
                    {
                        var view = await _viewRepository.LoadViewAsync(viewName);
                        if (view.IsNewerThanCheckpoint(context.PartitionKeyRangeId, document))
                        {
                            projection.Apply(@event, view);

                            view.UpdateCheckpoint(context.PartitionKeyRangeId, document);

                            handled = await _viewRepository.SaveViewAsync(viewName, view);
                            try
                            {
                                await _notificationHub.Clients.All.SendAsync("SendNotification", viewName);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError("Failed SignalR send.", ex);
                            }
                        }
                        else
                        {
                            // Already handled.
                            handled = true;
                        }

                        if (!handled)
                        {
                            // Oh noos! Somebody changed the view in the meantime, let's wait and try again.
                            await Task.Delay(500);
                        }
                    }
                }
            }
        }

        private static IEvent DeserializeEvent(Document document)
        {
            try
            {
                var eventType =
                    Type.GetType(
                        $"Clippers.EventFlow.Projections.Core.Events.{document.GetPropertyValue<string>("eventType")}, Clippers.EventFlow.Projections.Core");

                return (IEvent)document.GetPropertyValue<JObject>("payload").ToObject(eventType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}