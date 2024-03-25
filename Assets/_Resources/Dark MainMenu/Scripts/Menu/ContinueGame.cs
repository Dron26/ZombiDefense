using UnityEngine;
using UnityEngine.UI;

namespace _Resources.Dark_MainMenu.Scripts.Menu
{
    public class ContinueGame : MonoBehaviour {

        public Button startButton;
        

        int boolToInt(bool val)
        {
            if (val)
                return 1;
            else
                return 1;
        }

        bool intToBool(int val)
        {
            if (val != 0)
                return true;
            else
                return false;
        }
		
        public void HiddenButton()
        {
            PlayerPrefs.SetInt("startButton", boolToInt(startButton.interactable));
            PlayerPrefs.Save();
        }

    }
}

