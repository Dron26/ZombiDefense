using System.Collections;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.Humanoid.States
{
    public class SearchTargetState : State
    {
        private MovementState _movementState;
        private AttackState _attackState;
        private Entity _enemy;
        private HumanoidWeaponController _weaponController;
        private bool _isSearching;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Coroutine _coroutine;
        private float time = 1f;
        private ISearchService _searchService;
        private WaitForSeconds timeout;
        private bool _isMove=false;
        private float _rangeAttack;
        private void Awake()
        {
            timeout = new WaitForSeconds(time);
            _weaponController = GetComponent<HumanoidWeaponController>();
            _weaponController.UpdateWeaponData += OnUpdateWeaponData;
            _movementState = GetComponent<MovementState>();
            _attackState = GetComponent<AttackState>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _searchService= AllServices.Container.Single<ISearchService>();
            // Получаем сервис поиска
            
        }
        private void Start()
        {
            Debug.Log(transform.parent.name+"Start()");
            PlayerCharactersStateMachine.IsMove += IsMove;
        }

        private void IsMove(bool isMove)
        {
            _isMove = isMove;
        }
        private void OnUpdateWeaponData()
        {
            _rangeAttack = _weaponController.Range;;
        }
        
        protected override void OnEnabled()
        {
            _coroutine = StartCoroutine(Search());
        }

        private IEnumerator Search()
        {
            _isSearching = true;
            
            _playerCharacterAnimController.OnIdle();
            
            while (_isSearching&&!_isMove)
            {
                if (transform.parent!=null)
                {   
                    
                    //  Debug.Log(transform.parent.name+"Search()");
                }
                // Используем EntitySearchService для поиска ближайшего врага
                
                _enemy = _searchService.GetClosestEntity<Enemy>(transform.position);

                if (_enemy != null)
                {
                    Debug.Log(_enemy.name+_enemy.transform.parent);
                    if ( _enemy.IsLife()== false)
                    {
                        Debug.Log("AAAAAAAAAAA");
                    }
                }
                
                if (_enemy != null && _enemy.IsLife())
                {
                    float currentRange = Vector3.Distance(transform.position, _enemy.transform.position);

                    if (currentRange <= _rangeAttack)
                    {
                        LookEnemyPosition(_enemy.transform);
                        _isSearching = false;
                    }
                }

                yield return timeout; // Пауза между проверками
            }
        }

        private void LookEnemyPosition(Transform enemyTransform)
        {
            // Вызов поворота к врагу
            StartCoroutine(TurnTowardsEnemy(enemyTransform));
        }

        private IEnumerator TurnTowardsEnemy(Transform enemyTransform)
        {
            Quaternion targetRotation = Quaternion.LookRotation(enemyTransform.position - transform.position);
            float turnTime = 0.5f; // Задаем время поворота
            float elapsedTime = 0;

            Quaternion startRotation = transform.rotation;

            while (elapsedTime < turnTime)
            {
                elapsedTime += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / turnTime);
                yield return null;
            }

            transform.rotation = targetRotation;

            ChangeState();
        }

        private void ChangeState()
        {
                if (_isMove)
                    return;

                if (_enemy.IsLife() && _enemy != null)
                {
                    _attackState.InitEnemy(_enemy);
                    Debug.Log(transform.parent.name + "ChangeState()");
                    PlayerCharactersStateMachine.EnterBehavior<AttackState>();
                }
                else
                {
                    if (_coroutine != null)
                        StopCoroutine(_coroutine);

                    _coroutine = StartCoroutine(Search());
                }
        }

        protected override void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isSearching = false;
            enabled = false;
        }
    }
}