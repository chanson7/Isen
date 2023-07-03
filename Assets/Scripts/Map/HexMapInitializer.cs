using UnityEngine;
using Mirror;
using HexMapTools;

public class HexMapInitializer : NetworkBehaviour
{
    [SerializeField] HexMapDefinition mapDefinition;

    [Tooltip("In # of hexagons")]
    [SerializeField] int hexRadius;

    [Tooltip("Randomizes the placement of the object within the hexagon")]
    [SerializeField][Range(0.0f, 0.3f)] float randomPositionConstant;
    [SerializeField] HexGrid hexGrid;

    public override void OnStartServer()
    {
        SpawnMapObjects(hexGrid);

        base.OnStartServer();
    }

    [Server]
    void SpawnMapObjects(HexGrid hexGrid)
    {

        Vector3 hexagonSize = new Vector3(hexGrid.HexScale.Size.x, 0f, hexGrid.HexScale.Size.y);

        for (int x = -hexRadius; x <= hexRadius; x++)
        {
            for (int y = -hexRadius; y <= hexRadius; y++)
            {
                float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                HexCoordinates coordinates = HexCoordinates.FromOffsetCoordinates(x, y);

                GameObject mapObject = mapDefinition.SelectObjectByDistanceFromOrigin(hypotenuse / hexRadius);

                if (mapObject is not null)
                {
                    NetworkServer.Spawn(Instantiate(mapObject,
                                                    hexGrid.HexCalculator.HexToPosition(coordinates) + (Random.Range(-randomPositionConstant, randomPositionConstant) * hexagonSize),
                                                    Quaternion.identity));
                }

            }
        }

        Debug.Log($"..Spawned Map Objects");

    }
}
