﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models
{
    public interface IInternalDeviceState
    {
        Dictionary<string, object> GetAll();
        void SetAll(Dictionary<string, object> newState);
        bool Has(string key);
        object Get(string key);
        void Set(string key, object value);
    }

    public class InternalDeviceState : IInternalDeviceState
    {
        public const string CALC_TELEMETRY = "CalculateRandomizedTelemetry";

        private IDictionary<string, object> dictionary;

        public InternalDeviceState()
        {
            this.dictionary = new ConcurrentDictionary<string, object>();
        }

        public InternalDeviceState(DeviceModel deviceModel)
        {
            var initialState = this.SetupTelemetry(deviceModel);
            this.dictionary = new ConcurrentDictionary<string, object>(initialState);
        }

        public object Get(string key)
        {
            return this.dictionary[key];
        }

        public Dictionary<string, object> GetAll()
        {
            return new Dictionary<string, object>(this.dictionary);
        }

        public bool Has(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Sets the key with the given value, adds new value if key does not exist.
        /// Sets the changed flag to true.
        /// </summary>
        public virtual void Set(string key, object value)
        {
            if (this.dictionary.ContainsKey(key))
            {
                this.dictionary[key] = value;
            }
            else
            {
                this.dictionary.Add(key, value);
            }
        }

        public virtual void SetAll(Dictionary<string, object> newState)
        {
            this.dictionary = newState;
        }

        /// <summary>
        /// Initializes device state from the device model.
        /// </summary>
        private Dictionary<string, object> SetupTelemetry(DeviceModel deviceModel)
        {
            Dictionary<string, object> state = CloneObject(deviceModel.Simulation.InitialState);

            // Ensure the state contains the "online" key
            if (!state.ContainsKey("online"))
            {
                state["online"] = true;
            }

            // TODO: This is used to control whether telemetry is calculated in UpdateDeviceState.
            //       methods can turn telemetry off/on; e.g. setting temp high- turnoff, set low, turn on
            //       it would be better to do this at the telemetry item level - we should add this in the future
            //       https://github.com/Azure/device-simulation-dotnet/issues/174
            state.Add(CALC_TELEMETRY, true);

            return state;
        }

        /// <summary>Copy an object by value</summary>
        private static T CloneObject<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(
                JsonConvert.SerializeObject(source));
        }
    }
}