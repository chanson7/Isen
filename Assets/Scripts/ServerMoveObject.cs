using UnityEngine;
using System.Collections;
using Mirror;

public class ServerMoveObject : NetworkBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float radius;

    private Vector3 originalPosition;


    public override void OnStartServer()
    {
        originalPosition = transform.position;
        StartCoroutine(RotateAroundPoint());

        base.OnStartServer();
    }

    [Server]
    IEnumerator RotateAroundPoint()
    {
        float angle = 0f;
        while (true)
        {
            angle += Time.deltaTime * speed;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            transform.position = originalPosition + new Vector3(x, 0f, z);
            yield return null;
        }


    }

}
