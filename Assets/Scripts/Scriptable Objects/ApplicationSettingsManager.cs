using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Application/Settings Manager")]
public class ApplicationSettingsManager : ScriptableObject
{

    [SerializeField] ApplicationSettingsDataModel applicationSettingsData;

    public void ApplyAllApplicationSettings() //todo find a better way to do this... later!
    {
        Debug.Log("Applying App Settings");
        ApplyVideoSettings();
        ApplyAudioSettings();
        ApplyGameplaySettings();
    }

    void ApplyVideoSettings()
    {
        Screen.fullScreenMode = applicationSettingsData.data.fullScreenMode;
        Debug.Log($"Screen Mode set to {applicationSettingsData.data.fullScreenMode}");
    }

    void ApplyAudioSettings()
    {

    }

    void ApplyGameplaySettings()
    {

    }

}
