﻿using Characters.Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Services.Audio;
using Services.GeneralFactory;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factories.FactoryWarriors.Humanoids
{
    public class HumanoidFactory : MonoCache, IServiceFactory
    {
        private AudioManager _audioManager;

        public void Create(GameObject prefab, Transform transform )
        {
            GameObject newHumanoid = Instantiate(prefab, transform);
            Humanoid humanoidComponent = newHumanoid.GetComponent<Humanoid>();
            HumanoidWeaponController humanoidWeaponController  = newHumanoid.GetComponent<HumanoidWeaponController>();
            Transform newHumanoidTransform = humanoidComponent.transform;
            newHumanoidTransform.localPosition = Vector3.zero;
            newHumanoidTransform.tag="PlayerUnit";
            //humanoidComponent.OnInitialize += OnInitialized;
            float randomAngle = Random.Range(0f, 360f);
            newHumanoidTransform.rotation = Quaternion.Euler(0f, randomAngle, 0f);
        }
    }
}