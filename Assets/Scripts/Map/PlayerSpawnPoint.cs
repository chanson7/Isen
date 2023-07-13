using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public void Start()
    {
        GameManager.singleton.RegisterPlayerSpawnPoint(this.transform);
    }

}
