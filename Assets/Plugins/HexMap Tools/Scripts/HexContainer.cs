using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HexMapTools
{


    public class HexContainer<T> : IEnumerable<KeyValuePair<HexCoordinates, T>>, IEnumerable where T : Component
    {
        private HexGrid hexGrid;
        private HexCalculator hexCalculator;

        private Dictionary<HexCoordinates, T> hexes;

        private readonly bool autoSetCorrectPosition;
        private readonly bool autoDestroyGameObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexGrid"></param>
        /// <param name="autoSetCorrectPosition">Sets correct position when inserting object</param>
        /// <param name="autoDestroyGameObject">Destroys GameObject when removing from the container</param>
        public HexContainer(HexGrid hexGrid, bool autoSetCorrectPosition = true, bool autoDestroyGameObject = false)
        {
            this.hexGrid = hexGrid;
            hexCalculator = hexGrid.HexCalculator;

            this.autoSetCorrectPosition = autoSetCorrectPosition;
            this.autoDestroyGameObject = autoDestroyGameObject;

            hexes = new Dictionary<HexCoordinates, T>();
        }

        /// <summary>
        /// Fills with all existing children of HexGrid
        /// </summary>
        public void FillWithChildren()
        {
            foreach(Transform t in hexGrid.transform)
            {
                HexCoordinates coords = hexCalculator.HexFromLocalPosition(t.localPosition);

                T component = t.GetComponent<T>();

                if(component != null)
                    Insert(coords, component);
            }
        }


        /// <summary>
        /// Returns object at a given HexCoordinates
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public T At(HexCoordinates coords)
        {
            T hex;
            hexes.TryGetValue(coords, out hex);

            return hex;
        }

        /// <summary>
        /// Inserts object to a given HexCoordinates
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="newHex"></param>
        public void Insert(HexCoordinates coords, T newHex)
        {
            if(autoDestroyGameObject)
            {
                T prevHex;
                hexes.TryGetValue(coords, out prevHex);

                if(prevHex != null)
                {
                    Object.Destroy(prevHex.gameObject);
                }
            }

            hexes[coords] = newHex;

            if(autoSetCorrectPosition)
                newHex.transform.localPosition = hexCalculator.HexToLocalPosition(coords);
        }

        /// <summary>
        /// Removes object from the container
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="destroyGameObject"></param>
        public void Remove(HexCoordinates coords)
        {
            
            if (autoDestroyGameObject)
            {
                T prevHex;
                hexes.TryGetValue(coords, out prevHex);

                if (prevHex != null)
                {
                    Object.Destroy(prevHex.gameObject);
                }
            }

            hexes.Remove(coords);

        }

        public T this[HexCoordinates coords]
        {
            get
            {
                return At(coords);
            }

            set
            {
                Insert(coords, value);
            }
        }

        public IEnumerable<T> GetCells()
        {
            foreach(var hex in hexes)
            {
                yield return hex.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public IEnumerator<KeyValuePair<HexCoordinates, T>> GetEnumerator()
        {
            return hexes.GetEnumerator();
        }
    }

}
