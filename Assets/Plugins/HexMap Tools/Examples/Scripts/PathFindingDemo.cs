using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HexMapTools;

namespace HexMapToolsExamples
{

    [RequireComponent(typeof(HexGrid))]
    public class PathFindingDemo : MonoBehaviour {

        [Range(0, 1)]
        public float hexCostModifier = 1f;

        [Range(0, 1)]
        public float heuristicWeight = 1f;

        [Range(0, 3000)]
        public int maxIterations = 2000;

        [Range(1, 10)]
        public int blueCost = 2;

        [Range(1, 10)]
        public int redCost = 4;


        public Text shortestPath;
        public Text foundPath;
        public Text iterations;


        private HexCoordinates start;
        private HexCoordinates end;

        private List<HexCoordinates> path;
        private List<HexCoordinates> visited;

        private HexCalculator hexCalculator;
        private HexContainer<Cell> cells;


        void Start() {

            path = new List<HexCoordinates>();
            visited = new List<HexCoordinates>();

            HexGrid hexGrid = GetComponent<HexGrid>();
            hexCalculator = hexGrid.HexCalculator;
            cells = new HexContainer<Cell>(hexGrid);

            
            cells.FillWithChildren();

            var first = cells.GetEnumerator();
            first.MoveNext();

            start = first.Current.Key;
            end = start;
        }

        void Update() {

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HexCoordinates mouseCoords = hexCalculator.HexFromPosition(mouse);

            //Set start position
            if (Input.GetKeyDown(KeyCode.Mouse0) && cells[mouseCoords] != null)
            {
                cells[start].IsSelected = false;
                start = mouseCoords;
                cells[start].IsSelected = true;
                
            }
            //Set end position
            else if (Input.GetKeyDown(KeyCode.Mouse1) && cells[mouseCoords] != null)
            {
                end = mouseCoords;
            }


            UpdatePath();


            

        }

        void UpdatePath()
        {
            foreach (var hex in path)
            {
                cells[hex].IsSelected = false;
            }
            foreach (var hex in visited)
            {
                cells[hex].IsHighlighted = false;
            }


            // path with user settings
            HexPathFinder pathFinder = new HexPathFinder(HexCost, hexCostModifier, heuristicWeight, maxIterations);
            pathFinder.FindPath(start, end, out path);
            visited = pathFinder.Visited;
            foundPath.text = "Found path cost: " + CalculatePathCost(path);
            iterations.text = "Iterations: " + pathFinder.Iterations;


            // shortest path
            pathFinder = new HexPathFinder(HexCost);
            List<HexCoordinates> shortest;
            pathFinder.FindPath(start, end, out shortest);
            shortestPath.text = "Shortest path cost: " + CalculatePathCost(shortest);


            foreach (var hex in path)
            {
                cells[hex].IsSelected = true;
            }
            foreach (var hex in visited)
            {
                cells[hex].IsHighlighted = true;
            }
        }

        float HexCost(HexCoordinates a, HexCoordinates b)
        {

                Cell cell = cells[b];

                if (cell == null)
                    return float.PositiveInfinity;

                if (cell.Color == CellColor.Blue)
                    return blueCost;
                else if (cell.Color == CellColor.Red)
                    return redCost;

                return 1;
        }

        float CalculatePathCost(List<HexCoordinates> path)
        {
            float cost = 0;

            for(int i = 1; i < path.Count; ++i)
            {
                cost += HexCost(path[i-1], path[i]);
            }

            return cost;
        }
    }

}
