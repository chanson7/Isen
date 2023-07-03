using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HexMapDefinition", menuName = "ScriptableObjects/HexMapDefinition", order = 1)]
public class HexMapDefinition : ScriptableObject
{

    [SerializeField] List<GameObject> environmentObjects = new List<GameObject>();

    public GameObject SelectObjectByDistanceFromOrigin(float percentageOfMapRadius)
    {

        switch (percentageOfMapRadius)
        {

            case < .15f:
                return null;

            case < 0.35f:
                if (Random.value < 0.1f)
                    return environmentObjects[1];
                else
                    return null;

            case < 0.55f:
                if (Random.value < 0.35f)
                    return environmentObjects[1];
                else
                    return null;

            case < 1f:
                if (Random.value < 0.6f)
                    return environmentObjects[0];
                else
                    return null;

            default:
                return null;
        }

    }

}
