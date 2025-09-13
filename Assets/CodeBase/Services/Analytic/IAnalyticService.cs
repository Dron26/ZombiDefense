using System.Collections.Generic;

namespace Services.Analytic
{
    public interface IAnalyticService: IService
    {
        void StartGame();
        void EndGame();
        void WinGame();
        void LoseGame();
        void PauseGame();
        void ResumeGame();
        void RestartGame();
        void ExitGame();

        void StartLevel();
        void EndLevel();
        void WinLevel();
        void LoseLevel();
        void PauseLevel();
        void ResumeLevel();
        void RestartLevel();
        void ExitLevel();

        void ClickButton(string buttonName);
        void BuyCharacter(string characterName);
        void BuyItem(string itemName);
        void BuyUpgrade(string upgradeName);
        void BuySkin(string skinName);

        void ApplicationQuit();
    }
}