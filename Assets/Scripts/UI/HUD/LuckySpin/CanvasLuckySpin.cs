// using System.Collections;
// using Infrastructure.BaseMonoCache.Code.MonoCache;
// using Infrastructure.States;
// using Observer;
// using Service.SaveLoadService;
// using UnityEngine;
//
// namespace UI.HUD.LuckySpin
// {
//     enum GiftType
//     {
//         X1 = 0,
//         Points550 = 1,
//         AgainSpin = 2,
//         X4 = 3,
//         Coins1000 = 4,
//         X8 = 5,
//         Coins100 = 6,
//         Points1000 = 7
//     }
//
//     public class CanvasLuckySpin : MonoCache
//     {
//         [SerializeField] private ButtonDailySpin _dailySpin;
//         [SerializeField] private RectTransform _roulette;
//
//         [SerializeField] private SaveLoad _saveLoad;
//         [SerializeField] private DatabaseStatistics _databaseStatistics;
//
//         [SerializeField] private GiftBasket _giftBasket;
//         
//         private  BattleLevel _battleLevel;
//
//         private int _randomValue;
//         private int _finalAngle;
//         private float _timeInterval;
//
//         private Coroutine _coroutine;
//         
//         private int _totalMoney;
//         private int _totalPoints;
//
//         public void Initialize( BattleLevel battleLevel)
//         {
//             _battleLevel = battleLevel;
//         }
//         
//         public void CloseLuckySpin()
//         {
//             _totalMoney = _databaseStatistics.TotalMoney;
//             _totalPoints = _databaseStatistics.TotalPoints;
//             
//             if (_giftBasket.GetGiftFrames.Length != 0)
//             {
//                 foreach (GiftFrame gift in _giftBasket.GetGiftFrames)
//                 {
//                     switch (gift.NumberGift)
//                     {
//                         case (int)GiftType.X1:
//                             break;
//                         case (int)GiftType.Points550:
//                             ApplyPoints(_databaseStatistics.TotalPoints + 550);
//                             break;
//                         case (int)GiftType.AgainSpin:
//                             _saveLoad.SaveCountSpins(3);
//                             break;
//                         case (int)GiftType.X4:
//                             ApplyCoins(_databaseStatistics.TotalMoney * 4);
//                             ApplyPoints(_databaseStatistics.TotalPoints * 4);
//                             break;
//                         case (int)GiftType.Coins1000:
//                             ApplyCoins(_databaseStatistics.TotalMoney + 1000);
//                             break;
//                         case (int)GiftType.X8:
//                             ApplyCoins(_databaseStatistics.TotalMoney * 8);
//                             ApplyPoints(_databaseStatistics.TotalPoints * 8);
//                             break;
//                         case (int)GiftType.Coins100:
//                             ApplyCoins(_databaseStatistics.TotalMoney + 100);
//                             break;
//                         case (int)GiftType.Points1000:
//                             ApplyPoints(_databaseStatistics.TotalPoints + 1000);
//                             break;
//                     }
//                 }
//             }
//             
//             _saveLoad.ApplyMoney(_totalMoney);
//             _saveLoad.ApplyTotalPoints(_totalPoints);
//
//             print("здесь переход на сцену меню");
//             _battleLevel.EnterMenuLevel();
//         }
//
//         private void ApplyCoins(int money) => 
//             _totalMoney += money;
//
//         private void ApplyPoints(int points) => 
//             _totalPoints += points;
//
//         public void Spin()
//         {
//             if (_dailySpin.CanSpin())
//                 Twist();
//         }
//
//         private void Twist()
//         {
//             if (_coroutine != null)
//             {
//                 StopCoroutine(_coroutine);
//                 _coroutine = null;
//             }
//
//             Time.timeScale = 1;
//
//             _coroutine = StartCoroutine(RotationRoulette());
//         }
//
//         private IEnumerator RotationRoulette()
//         {
//             _randomValue = Random.Range(20, 30);
//             _timeInterval = .01f;
//
//             float zAngle = _roulette.rotation.z;
//
//             for (int i = 0; i < _randomValue; i++)
//             {
//                 _roulette.Rotate(0, 0, Mathf.Lerp(zAngle, 22.5f, .8f));
//
//                 if (i > Mathf.RoundToInt(_randomValue * .55f))
//                     _timeInterval *= .15f;
//
//                 yield return new WaitForSeconds(_timeInterval);
//             }
//
//             _finalAngle = Mathf.RoundToInt(_roulette.eulerAngles.z / 45);
//
//             switch (_finalAngle)
//             {
//                 case 0:
//                     _giftBasket.AddGiftFrame((int)GiftType.X1);
//                     break;
//                 case 1:
//                     _giftBasket.AddGiftFrame((int)GiftType.Points550);
//                     break;
//                 case 2:
//                     _giftBasket.AddGiftFrame((int)GiftType.AgainSpin);
//                     break;
//                 case 3:
//                     _giftBasket.AddGiftFrame((int)GiftType.X4);
//                     break;
//                 case 4:
//                     _giftBasket.AddGiftFrame((int)GiftType.Coins1000);
//                     break;
//                 case 5:
//                     _giftBasket.AddGiftFrame((int)GiftType.X8);
//                     break;
//                 case 6:
//                     _giftBasket.AddGiftFrame((int)GiftType.Coins100);
//                     break;
//                 case 7:
//                     _giftBasket.AddGiftFrame((int)GiftType.Points1000);
//                     break;
//             }
//         }
//     }
// }