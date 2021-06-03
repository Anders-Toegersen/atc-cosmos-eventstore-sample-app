using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Atc.Cosmos.EventStore.Events;
using Atc.Cosmos.EventStore.Streams;
using Microsoft.Azure.Cosmos;

namespace Atc.Cosmos.EventStore.Cosmos
{
    public class CosmosMetadataReader : IStreamMetadataReader
    {
        private readonly IEventStoreContainerProvider containerProvider;
        private readonly IDateTimeProvider timeProvider;

        public CosmosMetadataReader(
            IEventStoreContainerProvider containerProvider,
            IDateTimeProvider timeProvider)
        {
            this.containerProvider = containerProvider;
            this.timeProvider = timeProvider;
        }

        public async ValueTask<IStreamMetadata> GetAsync(
            StreamId streamId,
            CancellationToken cancellationToken)
        {
            try
            {
                var metadata = await containerProvider
                    .GetStreamContainer()
                    .ReadItemAsync<StreamMetadata>(
                        StreamMetadata.StreamMetadataId,
                        new PartitionKey(streamId.Value),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                metadata.Resource.ETag = metadata.ETag;

                return metadata.Resource;
            }
            catch (CosmosException ex)
            when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return new StreamMetadata(
                    StreamMetadata.StreamMetadataId,
                    streamId.Value,
                    streamId,
                    StreamVersion.StartOfStream,
                    StreamState.New,
                    timeProvider.GetDateTime());
            }
        }
    }
}