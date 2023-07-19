using UnityEngine.SceneManagement;
using UnityEngine;

public class AppInitializer : MonoBehaviour
{

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

}
