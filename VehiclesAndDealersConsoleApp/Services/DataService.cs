using Sammak.VnD.Models;
using Sammak.VnD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sammak.VnD.Services
{
    public class DataService : BaseDataService
    {
        public async Task<DataSet> GetDataSetAsync()
        {
            string path = @"api/datasetId";
            return await GenericGetAsync<DataSet>(path);
        }

        public async Task<VehicleIdSet> GetVehicleIdSetAsync(string datasetId)
        {
            string path = $"api/{datasetId}/vehicles";
            return await GenericGetAsync<VehicleIdSet>(path);
        }

        public async Task<Vehicle> GetVehicleAsync(string datasetId, int vehicleId)
        {
            string path = $"api/{datasetId}/vehicles/{vehicleId}";
            return await GenericGetAsync<Vehicle>(path);
        }

        public  Task<Vehicle[]> GetVehicles(string datasetId, List<int> vehicleIds)
        {
            var vehicles = new List<Task<Vehicle>>();
            vehicleIds.ForEach( id =>
            {
                var path = $"api/{datasetId}/vehicles/{id}";
                var vehicle = GenericGetAsync<Vehicle>(path);
                vehicles.Add(vehicle);
            });
            var x =  Task.WhenAll(vehicles.ToList());
            return x;
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
