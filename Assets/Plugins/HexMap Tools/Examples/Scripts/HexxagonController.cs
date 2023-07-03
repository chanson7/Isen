using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HexMapTools;



namespace HexMapToolsExamples
{

    


    [RequireComponent(typeof(HexGrid))]
    public class HexxagonController : MonoBehaviour {


        public Text blueScoreText;
        public Text redScoreText;
        public Text gameOverText;

        public Animator cameraAnimator;


        private HexCalculator hexCalculator;
        private HexContainer<Cell> cells;

        private HexCoordinates selectedCoords;
        private List<HexCoordinates> possibleMoves;
        private List<Cell> blueCells;
        private List<Cell> redCells;
        private CellColor player;
        private bool isGameOver = false;


        private void Start()
        {
            HexGrid hexGrid = GetComponent<HexGrid>();

            hexCalculator = hexGrid.HexCalculator;
            possibleMoves = new List<HexCoordinates>();
            blueCells = new List<Cell>();
            redCells = new List<Cell>();

            player = CellColor.Blue;
            cameraAnimator.SetInteger("Player", (int)player);


            cells = new HexContainer<Cell>(hexGrid);
            cells.FillWithChildren();

            
            //Count score
            foreach(var pair in cells)
            {
                Cell cell = pair.Value;

                cell.Init(pair.Key);

                if (cell.Color == CellColor.Blue)
                {
                    blueCells.Add(cell);
                }
                else if (cell.Color == CellColor.Red)
                {
                    redCells.Add(cell);
                }
            }


            //Show score
            blueScoreText.text = blueCells.Count.ToString();
            redScoreText.text = redCells.Count.ToString();

            gameOverText.gameObject.SetActive(false);

        }

        private void Update()
        {
            if (isGameOver)
                return;


            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                HexCoordinates mouseCoords = hexCalculator.HexFromPosition(mouse);


                //Move or select cell
                if(possibleMoves.Contains(mouseCoords))
                {
                    Move(mouseCoords);
                    CheckWin();

                    if(!isGameOver)
                        ChangePlayer();
                    
                }
                else
                {
                    SelectCell(mouseCoords);
                }

            }
        }

        private void Move(HexCoordinates coords)
        {

            //jump if it is too far
            if (HexUtility.Distance(coords, selectedCoords) > 1)
            {
                ChangeCellState(cells[selectedCoords], CellColor.White);
            }
            
        
            //Change color in the chosen cell
            ChangeCellState(cells[coords], player);


            //Change color in all neighbours
            var neighbours = HexUtility.GetNeighbours(coords);
            foreach(var n in neighbours)
            {
                Cell cell = cells[n];

                if(cell != null && cell.Color != CellColor.White)
                {
                    ChangeCellState(cell, player);
                }
            }


            DeselectCell();
        }

        private void CheckWin()
        {

            //Check if next player can move
            List<Cell> playerCells = null;

            if (player == CellColor.Blue)
                playerCells = redCells;
            else
                playerCells = blueCells;


            //if can move - return
            foreach(Cell cell in playerCells)
            {
                var moves = GetPossibleMoves(cell.Coords);

                if (moves.Count > 0)
                    return;
            }

            //else - game over, check and show who wins
            isGameOver = true;
            gameOverText.gameObject.SetActive(true);


            CellColor winner = CellColor.White;
            if (blueCells.Count > redCells.Count)
            {
                winner = CellColor.Blue;
                gameOverText.text = "Blue wins";
            }
            else if (redCells.Count > blueCells.Count)
            {
                winner = CellColor.Red;
                gameOverText.text = "Red wins";
            }
            else //Draw
            {
                winner = CellColor.White;
                gameOverText.text = "Draw";
            }


            cameraAnimator.SetInteger("Player", (int)winner);
            

        }


        //Change the player and the background
        private void ChangePlayer()
        {
            if (player == CellColor.Blue)
            {
                player = CellColor.Red;
            }  
            else
            {
                player = CellColor.Blue;
            }

            cameraAnimator.SetInteger("Player", (int)player);

        }


        //Change cell state and count points
        private void ChangeCellState(Cell cell, CellColor state)
        {
            if(cell != null)
            {
                if (cell.Color == state)
                    return;


                if (cell.Color == CellColor.Blue)
                    blueCells.Remove(cell);
                else if (cell.Color == CellColor.Red)
                    redCells.Remove(cell);


                cell.Color = state;

                if (state == CellColor.Blue)
                    blueCells.Add(cell);
                if (state == CellColor.Red)
                    redCells.Add(cell);


                //Update score
                blueScoreText.text = blueCells.Count.ToString();
                redScoreText.text = redCells.Count.ToString();
            }
        }


        List<HexCoordinates> GetPossibleMoves(HexCoordinates coords)
        {
            List<HexCoordinates> moves = new List<HexCoordinates>();

            var newCoords = HexUtility.GetInRange(coords, 2);

            foreach (var c in newCoords)
            {
                Cell cell = cells[c];
                if (cell != null && cell.Color == CellColor.White)
                {
                    moves.Add(c);
                }
            }

            return moves;
        }

        //Turn off all highlighted cells
        private void DeselectCell()
        {
            foreach (var move in possibleMoves)
            {
                cells[move].IsHighlighted = false; ;
            }
            possibleMoves.Clear();
        }

        //Select cell and highlight possible moves
        private void SelectCell(HexCoordinates coords)
        {
            DeselectCell();

            Cell cell = cells[coords];

            //if active player isn't owner, return
            if (cell == null || cell.Color != player)
                return;


            selectedCoords = coords;

            possibleMoves = GetPossibleMoves(coords);

            //Highlight all possible moves
            foreach(HexCoordinates move in possibleMoves)
            {
                cells[move].IsHighlighted = true;
            }
        }

    }

}
