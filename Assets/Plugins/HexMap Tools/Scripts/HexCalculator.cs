using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapTools
{

    [System.Serializable]
    public class HexCalculator
    {
        [SerializeField, HideInInspector]
        private HexScale hexScale;

        [SerializeField, HideInInspector]
        private Transform container;



        public HexScale HexScale {
            get { return hexScale; }
            private set { hexScale = value; }
        }

        public Transform Container
        {
            get { return container; }
            private set { container = value; }
        }

        public HexCalculator(HexScale hexScale, Transform container = null)
        {
            HexScale = hexScale;
            Container = container;
        }


        public Vector3 HexToLocalPosition(HexCoordinates coords)
        {
            return new Vector3((HexScale.Distance.x * coords.X +  coords.Y * (HexScale.Distance.x/2f)) ,
                                (HexScale.Distance.y * coords.Y), 0);
        }

        public Vector3 HexToPosition(HexCoordinates coords)
        {
            if (container == null)
                return HexToLocalPosition(coords);

            return container.TransformPoint(HexToLocalPosition(coords));
        }

        public HexCoordinates HexFromLocalPosition(Vector3 pos)
        {


            pos.x /= HexScale.Scale.x;
            pos.y /= HexScale.Scale.y;

            float z = pos.y * (4f / 3f);
            float x = (2f / 3f) * (Mathf.Sqrt(3) * pos.x - pos.y);
            float y = -z - x;

            int rx = Mathf.RoundToInt(x);
            int ry = Mathf.RoundToInt(y);
            int rz = Mathf.RoundToInt(z);

            float xDiff = Mathf.Abs(rx - x);
            float yDiff = Mathf.Abs(ry - y);
            float zDiff = Mathf.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff)
                rx = -ry - rz;
            else if (zDiff > yDiff)
                rz = -rx - ry;



            //if (yDiff > xDiff && yDiff > zDiff)
            //    ry = -ry - rz;
            //else if (zDiff > xDiff)
            //    rz = -rx - ry;

            return new HexCoordinates(rx, rz);
        }

        public HexCoordinates HexFromPosition(Vector3 pos)
        {
            if (container == null)
                return HexFromLocalPosition(pos);

            return HexFromLocalPosition(container.InverseTransformPoint(pos));

        }


    }

}
