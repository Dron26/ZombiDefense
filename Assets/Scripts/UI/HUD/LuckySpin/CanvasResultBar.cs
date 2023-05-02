using System.Collections.Generic;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.States;
using Observer;
using Service.SaveLoadService;
using TMPro;
using UnityEngine;

namespace UI.HUD.LuckySpin
{
    public class CanvasResultBar : MonoCache
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private DatabaseStatistics _databaseStatistics;

        [SerializeField] private CanvasLuckySpin _canvasLucky;

        [SerializeField] private SaveLoad _saveLoad;

        [SerializeField] private Transform _container;
        [SerializeField] private MemberFrame _prefabMemberFrame;

        [SerializeField] private TMP_Text _resultBattle;

        [SerializeField] private TMP_Text _totalMoneyView;
        [SerializeField] private TMP_Text _totalPointView;
        
        private BattleLevel _battleLevel;

        public void Initialize(BattleLevel battleLevel )
        {
            _battleLevel = battleLevel;
        }
        
        public void CloseResultBar()
        {
            _saveLoad.ApplyMoney(_databaseStatistics.TotalMoney);
            _saveLoad.ApplyTotalPoints(_databaseStatistics.TotalPoints);
            
            print("здесь переход на сцену меню");
            _battleLevel.EnterMenuLevel();
        }
        
        public void OnSpinCanvas()
        {
            _canvasLucky.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        
        public void DrawResult(string result)
        {
            print("Нужны переменные для локализатора");
            _resultBattle.text = result;
        }

        public void CalculateBonus(HumanoidFactory humanoidFactory)
        {
            _databaseStatistics.SetDataBase(humanoidFactory);

            foreach (var memberBattle in _databaseStatistics.GetMembersBattle())
            {
                MemberFrame memberFrame = Instantiate(_prefabMemberFrame, _container);

                memberFrame.Init
                (null,
                    GetFraction(memberBattle.Key),
                    GetNameMember(memberBattle.Key),
                    GetLevelMember(memberBattle.Key),
                    GetCountSurvival(humanoidFactory, memberBattle.Key),
                    GetCountGetDamage(memberBattle),
                    CountTakeDamage(memberBattle));
            }

            ViewTotal();

            _camera.gameObject.SetActive(true);
        }

        private void ViewTotal()
        {
            print("Нужно пробрасывать переменные тоталов, для локализации");
            
            _totalMoneyView.text = $"TOTAL MONEY: {_databaseStatistics.TotalMoney.ToString()}";
            _totalPointView.text = $"TOTAL POINTS: {_databaseStatistics.TotalPoints.ToString()}";
        }

        private string CountTakeDamage(KeyValuePair<int, InfoMemberBattle> memberBattle)
        {
            print("Нужно пробрасывать переменные для нанесенного дамага, для локализации");
            
            return $"Take damage {memberBattle.Value.DamageDone.ToString()}";
        }

        private string GetCountGetDamage(KeyValuePair<int, InfoMemberBattle> memberBattle)
        {
            print("Нужно пробрасывать переменные для полученного дамага, для локализации");
            
            return $"Get damage: {memberBattle.Value.DamageReceived.ToString()}";
        }

        private string GetCountSurvival(HumanoidFactory humanoidFactory, int level)
        {
            int countHumanoids = 0;
            int countSurvivals = 0;
            
            foreach (var humanoid in humanoidFactory.GetAllHumanoids().Where(humanoid => humanoid.GetLevel() == level))
            {
                countHumanoids++;

                if (humanoid.IsLife()) 
                    countSurvivals++;
            }

            print("Нужно пробрасывать переменные для названия Survivals, для локализации");
            
            return $"Survivals: {countSurvivals} / {countHumanoids}";
        }

        private string GetLevelMember(int memberBattleKey)
        {
            print("Нужно пробрасывать переменные для левела, для локализации");
            
            return $"Lv: {memberBattleKey}";
        }

        private string GetNameMember(int level)
        {
            print("Нужно пробрасывать переменные для имен, для локализации");
            
            return level switch
            {
                (int)Level.Soldier => "Soldier",
                (int)Level.Archer => "Archer",
                (int)Level.Knight => "Knight",
                (int)Level.King => "King",
                (int)Level.CyberSoldier => "CyberSoldier",
                (int)Level.CyberArcher => "CyberArcher",
                (int)Level.CyberKnight => "CyberKnight",
                (int)Level.CyberKing => "CyberKing",
                (int)Level.CrazyTractor => "CrazyTractor",
                (int)Level.CyberZombie => "CyberZombie",
                (int)Level.GunGrandmother => "GunGrandmother",
                (int)Level.Virus => "Virus",
                _ => ""
            };
        }

        private string GetFraction(int level)
        {
            print("Нужно пробрасывать переменные для фракции, для локализации");
            
            return level switch
            {
                > 0 and <= 4 => "People",
                > 4 and <= 8 => "Cyber",
                > 8 and <= 12 => "Hero",
                _ => ""
            };
        }
    }
}