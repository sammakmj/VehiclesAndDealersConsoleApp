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
        private DataService _dataService;
        private DataSet _dataSet;
        private VehicleIdSet _vehicleIdSet = null;
        private List<Vehicle> _vehiclesList = new List<Vehicle>();
        private List<Dealer> _dealerList = new List<Dealer>();
        private Dictionary<int, Dealer> _dealerDict = new Dictionary<int, Dealer>();

        public VnAMain()
        {
            _dataService = new DataService();
        }

        public void Run()
        {
            //GetDataSet();
            //GetDataSetAndVehicleIds();
            //GetVehicles();
            //GetVehicleAsync(); //;
            //GetDealers();
            DoAllAndPostAnswer();
        }

        void GetDataSet()
        {
            _dataSet = _dataService.GetDataSetAsync().GetAwaiter().GetResult();
            Console.WriteLine($"dataSet Id = {_dataSet.DatasetId}");
        }

        void GetDataSetAndVehicleIds()
        {
            GetDataSet();

            _vehicleIdSet = _dataService.GetVehicleIdSetAsync(_dataSet.DatasetId).GetAwaiter().GetResult();
            foreach (var id in _vehicleIdSet.VehicleIds)
            {
                //Console.WriteLine($"\tVehicle Id = {id}");
            }
        }

        void GetVehiclesAsync()
        {
            GetDataSetAndVehicleIds();

            _vehiclesList = _dataService.GetVehicles(_dataSet.DatasetId, _vehicleIdSet.VehicleIds).GetAwaiter().GetResult().ToList();
            foreach (var vehicle in _vehiclesList)
            {
                var vehicleJson = JsonConvert.SerializeObject(vehicle, Formatting.Indented);
                //Console.WriteLine($"Vehicle Info:\n{vehicleJson}");
            }
        }

        void GetVehicles()
        {
            GetDataSetAndVehicleIds();

            foreach (var id in _vehicleIdSet.VehicleIds)
            {
                //Console.WriteLine($"\tVehicle Id = {id}");
                var vehicle = _dataService.GetVehicleAsync(_dataSet.DatasetId, id).GetAwaiter().GetResult();
                _vehiclesList.Add(vehicle);
                string vehicleJson = JsonConvert.SerializeObject(vehicle, Formatting.Indented);
                //Console.WriteLine($"Vehicle Info:\n{vehicleJson}");
            }
        }

        void GetDealers()
        {
            GetVehicles();

            foreach (var vehicle in _vehiclesList)
            {
                var dealerId = vehicle.DealerId;

                //Console.WriteLine($"\tVehicle Id = {vehicle.VehicleId}; DealerId = {dealerId}; Dealer Info: ");
                var dealer = _dataService.GetDealerAsync(_dataSet.DatasetId, dealerId).GetAwaiter().GetResult();
                string dealerJson = JsonConvert.SerializeObject(dealer, Formatting.Indented);
                //Console.WriteLine($"{dealerJson}");
                if (!_dealerDict.ContainsKey(dealerId))
                {
                    _dealerDict[dealerId] = dealer;
                }
            }
        }

        void DoAllAndPostAnswer()
        {
            GetDealers();

            var answerPost = new AnswerPost();

            foreach (var dealer in _dealerDict)
            {
                var dealerPost = new DealerPost();
                dealerPost.DealerId = dealer.Key;
                dealerPost.Name = dealer.Value.Name;
                var vehiclePostList = _vehiclesList
                    .Where(v => v.DealerId == dealer.Key)
                    .Select(v => new VehiclePost
                    {
                        VehicleId = v.VehicleId,
                        make = v.Make,
                        Model = v.Model,
                        Year = v.Year
                    }).ToList();
                dealerPost.Vehicles = vehiclePostList;
                answerPost.Dealers.Add(dealerPost);
            }

            string answerPostJson = JsonConvert.SerializeObject(answerPost, Formatting.Indented);
            Console.WriteLine($"{answerPostJson}");


            var answerResponse = _dataService.PostAnswerAsync(_dataSet.DatasetId, answerPost).GetAwaiter().GetResult();
            string answerResponseJson = JsonConvert.SerializeObject(answerResponse, Formatting.Indented);
            Console.WriteLine($"{answerResponseJson}");
        }

    }
}