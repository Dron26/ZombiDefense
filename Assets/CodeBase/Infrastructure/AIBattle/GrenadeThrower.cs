using System.Collections;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

public class GrenadeThrower : MonoCache
{
    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    
    
    public GameObject grenadePrefab;
    public Transform throwPoint;
    private float _maxDistance = 15f;

    private GameObject aimingSphere;
    private Vector3 targetPoint;
    private bool aiming;
    public float minDistance=2f; 
    public float maxDistance=15f;
    public float minSpeed=4f;
    public float maxSpeed=11f;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[1]; // Создаем массив для одной частицы
    }

    private void Initialize(GranadeData data)
    {
        if (data != null)
        {
            minDistance=data.minDistance; 
            maxDistance=data.maxDistance;
            minSpeed=data.minSpeed;
            maxSpeed=data.maxSpeed;
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPoint = hit.point;
                float distanceToTarget = Vector3.Distance(transform.position, targetPoint);
            
                if (distanceToTarget >= _maxDistance)
                {
                    distanceToTarget=_maxDistance;
                }
                
                aiming = true;
            
                    // Рассчитываем силу броска пропорционально дистанции
                    particleSystem.startSpeed = CalculateThrowForce(distanceToTarget);
            
                    transform.LookAt(targetPoint);
                    transform.Rotate(-45, 0, 0);
            
                    // Создаем сферу для отображения точки броска
                    if (aimingSphere == null)
                    {
                        aimingSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        Destroy(aimingSphere.GetComponent<Collider>()); // Убираем коллайдер, чтобы не взаимодействовать с объектами
                    }
                    aimingSphere.transform.position = targetPoint;
                    aimingSphere.transform.localScale = Vector3.one * 0.5f; // Размер сферы

                    StartCoroutine(ThrowerControlling());
            }
        }
        else if (Input.GetMouseButtonDown(2))
        {
            aiming = false;
            Destroy(aimingSphere); // Удаляем сферу при отмене
        }
    }
    
    private float CalculateThrowForce(float distance)
    {     print(distance);
        float throwForce;

        float distanceRatio = (distance - minDistance) / (maxDistance - minDistance);

        // Если дистанция находится в диапазоне 30% - 80% от максимальной дистанции, добавляем 2 к максимальной силе
        
        
            // Иначе, используем обычную интерполяцию
            throwForce = Mathf.Lerp(minSpeed, maxSpeed, distanceRatio);
            
            if (distanceRatio >= 0.1f && distanceRatio <= 0.5f)
            {
                throwForce += 1f;
            }
            if (distanceRatio >= 0.5f && distanceRatio <= 0.8f)
            {
                throwForce += 0.5f;
            }
            print(throwForce);
        return throwForce;
    }

    private  IEnumerator ThrowerControlling()
    {
        particleSystem.Play();
        Vector3 particlePosition=Vector3.zero;
        // Получаем частицы из системы частиц
        

        bool isTargetSet = false;
        int numParticles = 0;
        
        while ( particleSystem.isPlaying )
        {
            
            if (numParticles > 0&&!isTargetSet)
            {
                 numParticles = particleSystem.GetParticles(particles);
                isTargetSet = true;
            }
            if (!isTargetSet)
            {
                 numParticles = particleSystem.GetParticles(particles);
            }
           

            yield return null;
        }
        
        particlePosition = particles[0].position;

        Explosion(particlePosition);
        Debug.Log("rhzr " + particlePosition);

    }

    private void Explosion(Vector3 explosionPosition )
    {
        float explosionRadius=0;
        float maxDamage=0;
        
        // Используем слой "Enemies" для поиска только вражеских объектов
        int enemyLayer = LayerMask.GetMask("Enemies");
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, enemyLayer);

        foreach (Collider collider in colliders)
        {
            // Тот же код для нанесения урона
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Рассчет урона и вызов TakeDamage
                // ...
            }
        }
    }
}