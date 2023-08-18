using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public void Start()
    {
        if(GameManager.singleton is not null)
            GameManager.singleton.RegisterPlayerSpawnPoint(transform);
    }

}
