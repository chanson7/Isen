using UnityEngine;
using TMPro;

public class SettingsUIController : MonoBehaviour
{

    TMP_Dropdown fullScreenMode;

    public void UpdateFullScreenMode()
    {
        Screen.fullScreenMode = (FullScreenMode)fullScreenMode.value;
    }

}