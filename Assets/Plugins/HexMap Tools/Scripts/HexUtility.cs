using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexMapTools
{

    public enum HexDirection { NE, E, SE, SW, W, NW };

    public static class HexUtility {


        private static readonly HexCoordinates[] directions = new HexCoordinates[6]
        {
            new HexCoordinates(0, 1),
            new HexCoordinates(1, 0),
            new HexCoordinates(1, -1),
            new HexCoordinates(0, -1),
            new HexCoordinates(-1, 0),
            new HexCoordinates(-1, 1)
        };


        public static int Distance(HexCoordinates a, HexCoordinates b)
        {
            return (a - b).Length();
        }

        public static HexCoordinates GetDirection(HexDirection direction)
        {
            return directions[(int)direction];
        }

        public static HexCoordinates GetNeighbour(HexCoordinates coords, HexDirection direction)
        {
            return coords + directions[(int)direction];
        }


        public static HexCoordinates[] GetNeighbours(HexCoordinates coords)
        {
            return new HexCoordinates[6]
            {
                coords + directions[0],
                coords + directions[1],
                coords + directions[2],
                coords + directions[3],
                coords + directions[4],
                coords + directions[5]
            };
        }

        public static HexDirection NeighbourToDirection(HexCoordinates coords, HexCoordinates neighbour)
        {
            HexCoordinates directionCoords = neighbour - coords;

            for (int i = 0; i < directions.Length; ++i)
            {
                if (directions[i] == directionCoords)
                    return (HexDirection)i;

            }
            throw new System.ArgumentException("Passed neighbour is not a real neighbour", "neighbour");
        }


        public static List<HexCoordinates> GetRing(HexCoordinates center, int radius)
        {
            List<HexCoordinates> ring = new List<HexCoordinates>();

            if (radius == 0)
            {
                ring.Add(center);
                return ring;
            }

            HexCoordinates coords = center + directions[(int)HexDirection.W] * radius;

            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < radius; ++j)
                {
                    coords = GetNeighbour(coords, (HexDirection)i);
                    ring.Add(coords);
                }
            }


            return ring;
        }


        public static List<HexCoordinates> GetInRange(HexCoordinates center, int radius)
        {
            List<HexCoordinates> region = new List<HexCoordinates>();

            for (int x = -radius; x <= radius; ++x)
            {
                for (int y = Mathf.Max(-radius, -x - radius); y <= Mathf.Min(radius, -x + radius); ++y)
                {
                    region.Add(center + new HexCoordinates(x, y));
                }
            }

            return region;
        }


    }


}