using System;
using System.Collections;
using Characters.Humanoids.AbstractLevel;
using Services;
using Services.Audio;
using UnityEngine;

namespace Infrastructure.AIBattle
{
    public class ObjectThrower : MonoBehaviour
    {
        public float _throwForce;
        public float proc = 0.50f;
        public Action OnThrowed;
        private Vector3 targetPoint;
        private float _maxDistance = 15f;
        private bool _isThrowed;

        public void ThrowGrenade(Grenade grenade)
        {
            _isThrowed = false;
            StartCoroutine(Throw(grenade));
        }

        private IEnumerator Throw(Grenade grenade)
        {
            while (_isThrowed == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        targetPoint = hit.point;

                        float distanceToTarget = Vector3.Distance(transform.position, targetPoint);

                        if (distanceToTarget > _maxDistance)
                        {
                            distanceToTarget = _maxDistance;
                        }

                        transform.LookAt(targetPoint);
                        transform.rotation =
                            new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
                    
                        grenade.gameObject.SetActive(true);
                        Transform grenadeTransform = grenade.transform;
                        Vector3 pos = grenadeTransform.position;
                        grenadeTransform.position = new Vector3(pos.x, pos.y + 2f, pos.z);
                        grenadeTransform.rotation = new Quaternion(-45f, 0, 0, 0);
                    
                        Grenade grenadeComponent = grenade.GetComponent<Grenade>();
                        Rigidbody rb = grenade.GetComponent<Rigidbody>();
                        _throwForce = CalculateThrowForce(distanceToTarget);
                        Debug.Log("_throwForce" + _throwForce);
                        rb.AddForce(transform.forward * _throwForce, ForceMode.VelocityChange);
                        _isThrowed = true;

                        float volume = AllServices.Container.Single<IAudioManager>().GetSoundSource().volume;

                        grenadeComponent.Throw(volume);
                        grenadeTransform.parent = null;
                        OnThrowed.Invoke();
                    }
                }

                yield return null;
            }
        }

        private float CalculateThrowForce(float distance)
        {
            float[] distances = { 14f, 12f, 10f, 7.2f, 6f, 5f, 4f, 3f, 2f };
            float[] forces = { 2.3f, 2.25f, 2.2f, 2f, 1.8f, 1.6f, 1.4f, 1.15f, 1f };
            float calculatedForce = forces[forces.Length - 1]; // Значение по умолчанию

            for (int i = 0; i < distances.Length; i++)
            {
                if (distance >= distances[i])
                {
                    calculatedForce = Mathf.Pow(distance, proc) * forces[i];
                    break;
                }
            }

            _throwForce = calculatedForce;
            return _throwForce;
        }
    }
}