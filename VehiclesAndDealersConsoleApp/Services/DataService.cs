using Sammak.VnD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sammak.VnD.Services
{
    public class DataService : BaseDataService
    {
        public async Task<Dataset> GetDatasetAsync()
        {
            string path = @"api/datasetId";
            return await GenericGetAsync<Dataset>(path);
        }

        public async Task<VehicleIdSet> GetVehicleIdsAsync(string datasetId)
        {
            string path = $"api/{datasetId}/vehicles";
            return await GenericGetAsync<VehicleIdSet>(path);
        }

        public async Task<Vehicle> GetVehicleAsync(string datasetId, int vehicleId)
        {
            string path = $"api/{datasetId}/vehicles/{vehicleId}";
            return await GenericGetAsync<Vehicle>(path);
        }

        public async Task<Vehicle[]> GetVehiclesAsync(string datasetId, List<int> vehicleIds)
        {
            var vehiclesTasks = new List<Task<Vehicle>>();
            vehicleIds.ForEach( id =>
            {
                var path = $"api/{datasetId}/vehicles/{id}";
                var vehicleTask = GenericGetAsync<Vehicle>(path);
                vehiclesTasks.Add(vehicleTask);
            });
            return await Task.WhenAll(vehiclesTasks);
        }

        public async Task<Dealer[]> GetDealersAsync(string datasetId, List<int> dealerIds)
        {
            var dealersTasks = new List<Task<Dealer>>();
            dealerIds.ForEach(id =>
            {
                var path = $"api/{datasetId}/dealers/{id}";
                var dealerTask = GenericGetAsync<Dealer>(path);
                dealersTasks.Add(dealerTask);
            });
            return await Task.WhenAll(dealersTasks);
        }

        public async Task<Dealer> GetDealerAsync(string datasetId, int dealerId)
        {
            string path = $"api/{datasetId}/dealers/{dealerId}";
            return await GenericGetAsync<Dealer>(path);
        }

        public async Task<AnswerResponse> PostAnswerAsync(string datasetId, AnswerPost answerPost)
        {
            string path = $"api/{datasetId}/answer";
            return await GenericPostAsync<AnswerPost, AnswerResponse>(path, answerPost);
        }

   }
}
