using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleCosmosCore2App.Core
{
    public class Persistence
    {
        private string _databaseId;
        private Uri _endpointUri;
        private string _primaryKey;
        private DocumentClient _client;

        public Persistence(Uri endpointUri, string primaryKey)
        {
            _databaseId = "ContactsDB";
            _endpointUri = endpointUri;
            _primaryKey = primaryKey;
        }

        public async Task EnsureSetupAsync()
        {
            if (_client == null)
            {
                _client = new DocumentClient(_endpointUri, _primaryKey);
            }

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseId);

            // Samples
            await _client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = "SamplesCollection" });
        }

        public async Task SaveSampleAsync(Contact contact)
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "SamplesCollection");
            await _client.UpsertDocumentAsync(documentCollectionUri, contact);
        }

        public async Task<Contact> GetSampleAsync(string Id)
        {
            await EnsureSetupAsync();

            var documentUri = UriFactory.CreateDocumentUri(_databaseId, "SamplesCollection", Id);
            var result = await _client.ReadDocumentAsync<Contact>(documentUri);
            return result.Document;
        }

        // try to delete this thing
        private async Task DeleteFamilyDocument(string databaseName, string collectionName, string documentName)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
        }

        public async Task DeleteItemAsync(string id)
        {
            var documentUri = UriFactory.CreateDocumentUri(_databaseId, "SamplesCollection", id);
            await _client.DeleteDocumentAsync(documentUri);
        }

        public async Task<List<Contact>> GetSamplesAsync()
        {
            await EnsureSetupAsync();

            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, "SamplesCollection");

            // build the query
            var feedOptions = new FeedOptions() { MaxItemCount = -1 };
            var query = _client.CreateDocumentQuery<Contact>(documentCollectionUri, "SELECT * FROM Sample", feedOptions);
            var queryAll = query.AsDocumentQuery();

            // combine the results
            var results = new List<Contact>();
            while (queryAll.HasMoreResults)
            {
                results.AddRange(await queryAll.ExecuteNextAsync<Contact>());
            }

            return results;
        }
    }
}
