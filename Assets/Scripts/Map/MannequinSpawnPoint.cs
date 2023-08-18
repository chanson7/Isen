using UnityEngine;
using Mirror;

public class MannequinSpawnPoint : MonoBehaviour
{

    [SerializeField] GameObject mannequinPrefab;

    private void Start()
    {
        if (NetworkServer.active)
        {
            GameObject mannequin = Instantiate(mannequinPrefab, transform.position, transform.rotation);

            NetworkServer.Spawn(mannequin);
        }

        Destroy(this.gameObject);
    }

}
