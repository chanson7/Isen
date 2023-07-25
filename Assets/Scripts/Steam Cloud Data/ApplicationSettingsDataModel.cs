using UnityEngine;
using HeathenEngineering.SteamworksIntegration;
using System.Collections.Generic;
using System.Linq;
using CloudAPI = HeathenEngineering.SteamworksIntegration.API.RemoteStorage.Client;

[CreateAssetMenu(menuName = "Steam Cloud/Application Settings")]
public class ApplicationSettingsDataModel : DataModel<ApplicationSettings>
{

    const string FILE_NAME = "application";

    public void RetrieveAppSettingsFile()
    {
        Debug.Log("Retrieving User's Application Settings...");

        if (CloudAPI.IsEnabled)
        {
            this.Refresh();

            Debug.Log($"{this.availableFiles.Length} files found");

            List<RemoteStorageFile> files = this.availableFiles.ToList();

            if (files.Count < 1)
            {
                Debug.Log("No Application Settings file found, uploading one");

                if (CloudAPI.FileWrite(FILE_NAME + this.extension, this.data, System.Text.Encoding.UTF8))
                    Debug.Log("New Application Settings file uploaded");
                else
                    Debug.LogWarning("Failed to upload Application Settings file");
            }
            else if (files.Count > 1)
            {

                while (files.Count > 1)
                {
                    RemoteStorageFile removeFile = files[0].timestamp >= files[1].timestamp ? files[1] : files[0];

                    CloudAPI.FileDelete(removeFile.name);
                    files.Remove(removeFile);
                }
                Debug.Log("Too many Application Settings files found, deleting the oldest ones");
            }
            else
            {
                this.LoadFileAddress(files[0]);

                Debug.Log("Application Settings file loaded!");
            }
        }
        else
        {
            Debug.LogError("Could not retrieve App Settings file :( \n" +
                           $"User has Steam Cloud enabled? {CloudAPI.IsEnabledForAccount}\n" +
                           $"App has Steam Cloud enabled? {CloudAPI.IsEnabledForApp}");
        }
    }

    public void RevertToDefaultSettings()
    {
        if (CloudAPI.IsEnabled)
        {

        }
        else
        {
            Debug.LogError("Could not retrieve App Settings file :( \n" +
                           $"User has Steam Cloud enabled? {CloudAPI.IsEnabledForAccount}\n" +
                           $"App has Steam Cloud enabled? {CloudAPI.IsEnabledForApp}");
        }

    }
}