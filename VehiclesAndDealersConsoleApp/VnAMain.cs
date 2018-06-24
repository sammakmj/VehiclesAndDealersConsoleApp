using Newtonsoft.Json;
using Sammak.VnD.Models;
using Sammak.VnD.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sammak.VnD
{
    internal class VnAMain
    {
        private DataService _dataService = new DataService();

        public void Run()
        {
            //GetDataset();
            //GetDatasetAndVehicleIds();
            //GetVehicles();
            //GetVehicleAsync(); //;
            //GetDealers();
            DoAllAndPostAnswer();
        }

        string GetDataset()
        {
            var dataset = _dataService.GetDatasetAsync().GetAwaiter().GetResult();
            Console.WriteLine($"dataset Id = {dataset.DatasetId}");
            return dataset.DatasetId;
        }

        List<int> GetVehicleIds(string datasetId)
        {
            var vehicleIds = _dataService.GetVehicleIdsAsync(datasetId).GetAwaiter().GetResult();

            //foreach (var id in vehicleIdSet.VehicleIds)
            //{
            //    //Console.WriteLine($"\tVehicle Id = {id}");
            //}
            return vehicleIds.VehicleIds;
        }

        List<Vehicle> GetVehicles(string datasetId)
        {
            var vehicleIds = GetVehicleIds(datasetId);
            var vehiclesList = _dataService.GetVehiclesAsync(datasetId, vehicleIds).GetAwaiter().GetResult().ToList();
            //foreach (var vehicle in _vehiclesList)
            //{
            //    var vehicleJson = JsonConvert.SerializeObject(vehicle, Formatting.Indented);
            //    //Console.WriteLine($"Vehicle Info:\n{vehicleJson}");
            //}
            return vehiclesList;
        }

        List<Dealer> GetDealers(string datasetId, List<Vehicle> vehiclesList = null)
        {
            if (vehiclesList == null)
            {
                // if the list of vehicles has not been supplied by the caller, this method will
                // make a call the to data service to get it.
                vehiclesList = GetVehicles(datasetId);
            }
            var dealerIds = new List<int>();

            foreach (var vehicle in vehiclesList)
            {
                var dealerId = vehicle.DealerId;
                if (!dealerIds.Exists(d => d == dealerId))
                {
                    dealerIds.Add(dealerId);
                }
            }

            var dealersList = _dataService.GetDealersAsync(datasetId, dealerIds).GetAwaiter().GetResult().ToList();

            //Console.WriteLine($"\tVehicle Id = {vehicle.VehicleId}; DealerId = {dealerId}; Dealer Info: ");
            //var dealer = _dataService.GetDealerAsync(_dataset.DatasetId, dealerId).GetAwaiter().GetResult();
            //    string dealerJson = JsonConvert.SerializeObject(dealer, Formatting.Indented);
            //    //Console.WriteLine($"{dealerJson}");
            //    if (!_dealerDict.ContainsKey(dealerId))
            //    {
            //        _dealerDict[dealerId] = dealer;
            //    }
            return dealersList;
        }

        void DoAllAndPostAnswer()
        {
            var datasetId = GetDataset();
            var vehiclesList = GetVehicles(datasetId);
            var dealersList = GetDealers(datasetId, vehiclesList);

            var answerPost = new AnswerPost();

            dealersList.ForEach(dealer =>
            {
                var dealerPost = new DealerPost
                {
                    DealerId = dealer.DealerId,
                    Name = dealer.Name
                };
                var vehiclePostList = vehiclesList
                    .Where(v => v.DealerId == dealer.DealerId)
                    .Select(v => new VehiclePost
                    {
                        VehicleId = v.VehicleId,
                        make = v.Make,
                        Model = v.Model,
                        Year = v.Year
                    }).ToList();
                dealerPost.Vehicles = vehiclePostList;
                answerPost.Dealers.Add(dealerPost);
            });

            //string answerPostJson = JsonConvert.SerializeObject(answerPost, Formatting.Indented);
            //Console.WriteLine($"{answerPostJson}");

            var answerResponse = _dataService.PostAnswerAsync(datasetId, answerPost).GetAwaiter().GetResult();
            string answerResponseJson = JsonConvert.SerializeObject(answerResponse, Formatting.Indented);
            Console.WriteLine($"{answerResponseJson}");
        }

    }
}