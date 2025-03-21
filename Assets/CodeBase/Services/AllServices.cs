using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class AllServices
    {
        private static AllServices _instance;
        public static AllServices Container=>_instance ?? (_instance=new AllServices());

        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public void RegisterSingle<TService>(TService implementation) where TService :IService => 
            Implementation<TService>.ServiceInstance=implementation;

        public TService Single<TService>() where TService :IService =>
            Implementation<TService>.ServiceInstance;

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
        //
        // public void RegisterService<TService>(TService service) where TService : IService
        // {
        //     if (!_services.ContainsKey(typeof(TService)))
        //     {
        //         _services.Add(typeof(TService), service);
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"Service of type {typeof(TService)} already registered.");
        //     }
        // }
        //
        // public void UnregisterService<TService>() where TService : IService
        // {
        //     if (_services.ContainsKey(typeof(TService)))
        //     {
        //         _services.Remove(typeof(TService));
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"Service of type {typeof(TService)} not found for removal.");
        //     }
        // }
        //
        // public TService GetService<TService>() where TService : IService
        // {
        //     if (_services.TryGetValue(typeof(TService), out var service))
        //     {
        //         return (TService)service;
        //     }
        //     else
        //     {
        //         throw new InvalidOperationException($"Service of type {typeof(TService)} not found.");
        //     }
        // }
    }
}