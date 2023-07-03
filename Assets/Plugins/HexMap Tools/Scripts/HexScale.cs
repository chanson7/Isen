using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapTools
{

    [System.Serializable]
    public class HexScale
    {
        public static readonly float InnerRadius = Mathf.Sqrt(3) * 0.25f;
        public static readonly float OuterRadius = 0.5f;

        [SerializeField, HideInInspector]
        private Vector2 scale = Vector2.one;


        public Vector2 Scale
        {
            get { return scale; }
            private set{ scale = value; }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Scale.x * 2f * InnerRadius, Scale.y);
            }
            private set
            {
                value.x /= 2f * InnerRadius;
                Scale = value;
            }
        }

        public Vector2 Distance
        {
            get
            {
                return new Vector2(Scale.x * 2f * InnerRadius, Scale.y * 1.5f * OuterRadius);
            }

            private set
            {
                value.x /= (2 * InnerRadius);
                value.y /= (3f / 2f) * OuterRadius;
                Scale = value;
            }
        }


        public HexScale()
        {
        }

        public HexScale(Vector2 scale)
        {
            Scale = scale;
        }

        public static HexScale FromHexSize(Vector2 size)
        {
            return new HexScale()
            {
                Size = size
            };
        }

        public static HexScale FromHexDistance(Vector2 distance)
        {
            return new HexScale()
            {
                Distance = distance
            };
        }

        public static HexScale FromPixels(Vector2 sizeInPixels, Vector2 overlappingPixels)
        {
            return new HexScale()
            {
                Size = new Vector2((sizeInPixels.x - overlappingPixels.x) / sizeInPixels.y,
                     ((sizeInPixels.y - (4f / 3f) * overlappingPixels.y)) / sizeInPixels.y)
            };
        }

        public static HexScale FromPixelsDist(Vector2 sizeInPixels, Vector2 distanceInPixels)
        {

            Vector2 overlappingPixels = new Vector2(sizeInPixels.x - distanceInPixels.x,
                        (3f / 4f) * sizeInPixels.y - distanceInPixels.y);

            return FromPixels(sizeInPixels, overlappingPixels);
        }
    }

}
