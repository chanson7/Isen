using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapTools
{

    


    [System.Serializable]
    public struct HexCoordinates : System.IEquatable<HexCoordinates>
    {

        

        [SerializeField, HideInInspector]
        private int x, y;


        public int X { get { return x; } private set { x = value; } }
        public int Y { get { return y; } private set { y = value; } }
        public int Z { get { return -X - Y; } }

        public int Col
        {
            get
            {
                return X + (Y - (Y&1)) / 2;
            }
        }
        public int Row
        {
            get
            {
                return Y;
            }
        }


        public static HexCoordinates Zero
        {
            get { return new HexCoordinates(0, 0); }
        }

        /// <summary>
        /// Creates from axial coordinates
        /// </summary>
        /// <param name="axialX"></param>
        /// <param name="axialY"></param>
        public HexCoordinates(int axialX, int axialY)
        {
            this.x = axialX;
            this.y = axialY;
        }

        

        /// <summary>
        /// Creates from offset coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static HexCoordinates FromOffsetCoordinates(int x, int y)
        {
            return new HexCoordinates(x - y / 2, y);
        }


        public int Length()
        {
            return (Mathf.Abs(X) + Mathf.Abs(Y) + Mathf.Abs(Z)) / 2;
        }


        public override string ToString()
        {
            return string.Format("[X: {0}; Z: {1}; Y: {2}]", X, Y, Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X + 15101) + (Y + 15013) * 30011;
            }
        }


        public override bool Equals(object obj)
        {
            if (obj is HexCoordinates)
                return this == (HexCoordinates)obj;

            return false;
        }


        public bool Equals(HexCoordinates other)
        {
            return this == other;
        }

        public static bool operator ==(HexCoordinates a, HexCoordinates b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(HexCoordinates a, HexCoordinates b)
        {
            return !(a == b);
        }

        public static HexCoordinates Add(HexCoordinates a, HexCoordinates b)
        {
            return new HexCoordinates(a.X + b.X, a.Y + b.Y);
        }

        public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
        {
            return Add(a, b);
        }

        public static HexCoordinates Subtract(HexCoordinates a, HexCoordinates b)
        {
            return new HexCoordinates(a.X - b.X, a.Y - b.Y);
        }

        public static HexCoordinates operator -(HexCoordinates a, HexCoordinates b)
        {
            return Subtract(a, b);
        }

        public static HexCoordinates Scale(HexCoordinates a, int k)
        {
            return new HexCoordinates(a.X * k, a.Y * k);
        }

        public static HexCoordinates operator*(int k, HexCoordinates a)
        {
            return Scale(a, k);
        }
        public static HexCoordinates operator *(HexCoordinates a, int k)
        {
            return Scale(a, k);
        }

    }

}
