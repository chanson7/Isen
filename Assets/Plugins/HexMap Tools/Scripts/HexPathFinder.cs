using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapTools
{

    public class HexPathFinder {

        private struct Node
        {
            public Node(HexCoordinates coords, float cost)
            {
                this.coords = coords;
                this.cost = cost;
            }

            public HexCoordinates coords;
            public float cost;
        }

        public class Result
        {
            public bool success = false;
            public List<HexCoordinates> path = new List<HexCoordinates>();
        }


        private Dictionary<HexCoordinates, HexCoordinates> cameFrom;
        private PriorityQueue<Node> frontier;
        private Dictionary<HexCoordinates, float> costSoFar;

        private float hexCostModifier;
        private float heuristicWeight;

        /// <summary>
        /// Returns edge cost, cost from the first hex to the second hex
        /// returns infinity if the second hex is inaccessible
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public delegate float HexCostFunc(HexCoordinates start, HexCoordinates target);

        /// <summary>
        /// Returns estimated cost between hexes
        /// </summary>
        /// <param name="start"></param>
        /// <param name="pathDestination"></param>
        /// <returns></returns>
        public delegate float HeuristicFunc(HexCoordinates start, HexCoordinates pathDestination);


        /// <summary>
        /// Returns edge cost, cost from the first hex to the second hex
        /// returns infinity if the second hex is inaccessible
        /// </summary>
        public HexCostFunc HexCost { get; set; }

        /// <summary>
        /// Returns estimated cost between hexes
        /// </summary>
        public HeuristicFunc Heuristic { get; set; }

        /// <summary>
        /// Modifies terrain cost
        /// 0 - all hexes cost 1 ( or are inaccessible if cost is infinity)
        /// 1 - normal costs are used
        /// </summary>
        public float HexCostModifier {
            get { return hexCostModifier; }
            set
            {
                if (value < 0 || value > 1)
                    throw new System.ArgumentOutOfRangeException("HexCostModifier");

                hexCostModifier = value;
            }
        }

        /// <summary>
        /// Modifies the weight of heuristic
        /// 0 - heuristic is ignored
        /// 1 - terrain cost and path length is ignored
        /// </summary>
        public float HeuristicWeight {
            get { return heuristicWeight; }
            set
            {
                if (value < 0 || value > 1)
                    throw new System.ArgumentOutOfRangeException("HeuristicWeight");

                heuristicWeight = value;
            }
        }

        /// <summary>
        /// Defines how many iterations(roughly equals to amount of checked hexes) 
        /// will be performed until algorithm decides that the path wasn’t found.
        /// </summary>
        public int MaxIterations { get; set; }

        


        public List<HexCoordinates> Visited
        {
            get
            {
                return new List<HexCoordinates>(cameFrom.Keys);
            }
        }

        public int Iterations
        {
            get; private set;
        }




        



        /// <param name="hexCostFunc">Returns edge cost, cost from the first hex to the second hex, returns infinity if the second hex is inaccessible</param>
        /// <param name="hexCostModifier">Modifies terrain cost, 0 - all hexes cost 1 ( or are inaccessible if cost is infinity), 1 - normal costs are used</param>
        /// <param name="heuristicWeight">Modifies the weight of heuristic, 0 - heuristic is ignored, 1 - terrain cost and path length is ignored</param>
        /// <param name="heuristicFunc">Returns estimated cost between hexes</param>
        public HexPathFinder(HexCostFunc hexCostFunc, float hexCostModifier = 1f, float heuristicWeight = 0.5f, int maxIterations = 10000, HeuristicFunc heuristicFunc = null)
        {
            if (hexCostFunc == null)
                throw new System.ArgumentNullException("hexCostFunc");


            HexCost = hexCostFunc;

            if (heuristicFunc == null)
                Heuristic = (a, b) => { return HexUtility.Distance(a, b) * 1.00001f; };
            else
                Heuristic = heuristicFunc;

            HexCostModifier = hexCostModifier;
            HeuristicWeight = heuristicWeight;
            MaxIterations = maxIterations;

            cameFrom = new Dictionary<HexCoordinates, HexCoordinates>();
            frontier = new PriorityQueue<Node>((a, b) => { return a.cost.CompareTo(b.cost); });
            costSoFar = new Dictionary<HexCoordinates, float>();
        }

        /// <summary>
        /// Finds path between given coordinates.
        /// Uses A* algorithm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>If path isn't found returns path to closest coords</returns>
        public bool FindPath(HexCoordinates start, HexCoordinates end, out List<HexCoordinates> path)
        {

            cameFrom.Clear();
            cameFrom.Add(start, HexCoordinates.Zero);

            if (start == end)
            {
                path = new List<HexCoordinates>();
                return true;
            }

            bool success = false;

            frontier.Clear();
            frontier.Enqueue(new Node(start, 0));

            costSoFar.Clear();
            costSoFar.Add(start, 0);

            HexCoordinates closest = start;
            int minDistance = HexUtility.Distance(start, end);

            Iterations = 0;
            while (frontier.Count > 0 && success == false)
            {
                if (Iterations > MaxIterations)
                    break;
                Iterations++;

                HexCoordinates current = frontier.Dequeue().coords;
                float currentCost = costSoFar[current];

                int distance = HexUtility.Distance(current, end);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    closest = current;
                }


                var neighbours = HexUtility.GetNeighbours(current);

                foreach (var n in neighbours)
                {
                    float hexCost = HexCost(current, n);

                    if (hexCost == float.PositiveInfinity)
                        continue;

                    hexCost = 1 + HexCostModifier * (hexCost - 1);
                    hexCost = (1 - HeuristicWeight) * hexCost;
                    

                    float newCost = currentCost + hexCost;

                    if (!cameFrom.ContainsKey(n) || costSoFar[n] > newCost)
                    {
                        cameFrom[n] = current;
                        costSoFar[n] = newCost;

                        frontier.Enqueue(new Node(n, newCost + HeuristicWeight * Heuristic(n, end)));

                        if (n == end)
                            success = true;
                    }
                }

                
            }

            if (success == false)
                end = closest;


            path = GetPath(start, end);


            return success;
        }


        private List<HexCoordinates> GetPath(HexCoordinates start, HexCoordinates end)
        {
            List<HexCoordinates> path = new List<HexCoordinates>();

            HexCoordinates current = end;

            while (current != start)
            {
                path.Add(current);

                current = cameFrom[current];
            }

            path.Reverse();

            return path;
        }

    }
}
