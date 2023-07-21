// using Infrastructure.BaseMonoCache.Code.MonoCache;
// using UnityEngine;
//
// namespace UI.HUD.LuckySpin
// {
//     public class GiftBasket : MonoCache
//     {
//         [SerializeField] private GiftFrame _prefabFrame;
//         [SerializeField] private Transform _container;
//
//         [SerializeField] private CanvasRouletteEffect _canvasRouletteEffect;
//
//         private readonly GiftFrame[] _giftFrames = new GiftFrame[3];
//         private int _counter = 0;
//
//         public GiftFrame[] GetGiftFrames =>
//             _giftFrames;
//         
//         public void AddGiftFrame(int indexGift)
//         {
//             if (_counter <= 2)
//             {
//                 SetGiftFrame(indexGift);
//                 return;
//             }
//
//             if (_counter >= 3)
//             {
//                 _counter = 0;
//                 
//                 for (int i = 0; i < _giftFrames.Length; i++) 
//                      Destroy(_giftFrames[i].gameObject);
//
//                 SetGiftFrame(indexGift);
//             }
//         }
//
//         private void SetGiftFrame(int indexGift)
//         {
//             _canvasRouletteEffect.ShowResult(indexGift);
//             _giftFrames[_counter] = Instantiate(_prefabFrame, _container);
//             _giftFrames[_counter].InitDescription(GetNameGift(indexGift), GetDescriptionGift(indexGift), indexGift);
//
//             _counter++;
//         }
//
//         private string GetNameGift(int indexGift)
//         {
//             print("Здесь переменные для локализатора");
//             
//             return indexGift switch
//             {
//                 (int)GiftType.X1 => "GENERAL BONUS",
//                 (int)GiftType.Points550 => "LEADERBOARD POINTS",
//                 (int)GiftType.AgainSpin => "ADDITIONAL ROTATIONS",
//                 (int)GiftType.X4 => "GENERAL BONUS",
//                 (int)GiftType.Coins1000 => "BONUS COINS",
//                 (int)GiftType.X8 => "GENERAL BONUS",
//                 (int)GiftType.Coins100 => "BONUS COINS",
//                 (int)GiftType.Points1000 => "LEADERBOARD POINTS",
//                 _ => ""
//             };
//         }
//
//         private string GetDescriptionGift(int indexGift)
//         {
//             print("Здесь переменные для локализатора");
//
//             return indexGift switch
//             {
//                 (int)GiftType.X1 => "X1",
//                 (int)GiftType.Points550 => "550",
//                 (int)GiftType.AgainSpin => "1",
//                 (int)GiftType.X4 => "X4",
//                 (int)GiftType.Coins1000 => "1000",
//                 (int)GiftType.X8 => "X8",
//                 (int)GiftType.Coins100 => "100",
//                 (int)GiftType.Points1000 => "1000",
//                 _ => ""
//             };
//         }
//     }
// }