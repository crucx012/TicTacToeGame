using System;
using System.Linq;

namespace TicTacToe
{
    public class GameBoard
    {
        private static Cell[] _cells = new Cell[9];
        private readonly int[] _sides = {1,3,5,7};
        private readonly int[,] _adjacentSides = {{1,3}, 
                                                  {1,5}, 
                                                  {3,7}, 
                                                  {5,7}};
        private readonly int[] _corners = {0,2,6,8};
        private readonly int[] _reverseCorners = {8,6,2,0};
        private readonly int[,] _oppositeCorners = {{2,6}, 
                                                    {0,8},
                                                    {0,8},
                                                    {2,6}};
        private readonly int[,] _winningCombinations = {{0,1,2}, 
                                                        {3,4,5}, 
                                                        {6,7,8}, 
                                                        {0,3,6}, 
                                                        {1,4,7}, 
                                                        {2,5,8}, 
                                                        {0,4,8}, 
                                                        {2,4,6}};

        public GameBoard(Cell[] cells)
        {
            _cells = cells;
        }

        public Marker CheckBoardForWinner()
        {
            for (var i = 0; i < 8; i++)
                if (_cells[_winningCombinations[i, 0]].CurrentMarker == _cells[_winningCombinations[i, 1]].CurrentMarker && 
                    _cells[_winningCombinations[i, 0]].CurrentMarker == _cells[_winningCombinations[i, 2]].CurrentMarker &&
                    _cells[_winningCombinations[i,0]].CurrentMarker != Marker.N)
                        return _cells[_winningCombinations[i, 0]].CurrentMarker;

            return Marker.N;
        }

        public Cell CheckForTwoInARow(Marker personMarker)
        {
            Cell thirdCellInRow = null;
            for (var i = 0; i < 8; i++)
            {
                var match = 0;
                var empty = 0;
                for (var j = 0; j < 3; j++)
                {
                    if (_cells[_winningCombinations[i, j]].CurrentMarker == personMarker)
                        match++;
                    else if (_cells[_winningCombinations[i, j]].CurrentMarker == Marker.N)
                    {
                        empty++;
                        thirdCellInRow = _cells[_winningCombinations[i, j]];
                    }
                    if (match == 2 && empty == 1)
                        return thirdCellInRow;
                }
            }
            return null;
        }

        public Cell CheckIfPlayerHasCorner(Marker personMarker)
        {
            if (_corners.Where(corner => _cells[corner].CurrentMarker == personMarker).Any())
                if (_cells[4].CurrentMarker == Marker.N)
                    return _cells[4];

            return CheckIfPlayerHasCornerAndSide(personMarker) ??
                   CheckIfPlayerHasOppositeCorners(personMarker);
        }

        private Cell CheckIfPlayerHasOppositeCorners(Marker personMarker)
        {
            for (var i = 0; i < 2; i++)
                if (Array.TrueForAll(new[] { _cells[_oppositeCorners[i, 0]].CurrentMarker, _cells[_oppositeCorners[i, 1]].CurrentMarker }, value => (personMarker == value)))
                {
                    var randomSide = _cells[_sides.Where(s => _cells[s].CurrentMarker == Marker.N).OrderBy(g => Guid.NewGuid()).FirstOrDefault()];
                    return randomSide;
                }

            return null;
        }

        public Cell CheckIfPlayerHasSide(Marker personMarker)
        {

            if (_sides.Where(side => _cells[side].CurrentMarker == personMarker).Any())
                if (_cells[4].CurrentMarker == Marker.N)
                    return _cells[4];

            return CheckIfPlayerHasCornerAndSide(personMarker) ??
                   CheckIfPlayerHasAdjacentSides(personMarker);
        }

        private Cell CheckIfPlayerHasAdjacentSides(Marker personMarker)
        {
            for (var i = 0; i < 4; i++)
                if (Array.TrueForAll(new[] { _cells[_adjacentSides[i, 0]].CurrentMarker, _cells[_adjacentSides[i, 1]].CurrentMarker }, value => (personMarker == value)))
                {
                    var claimedCell = _cells[_corners[i]];
                    if (claimedCell.CurrentMarker == Marker.N)
                        return claimedCell;
                }

            return null;
        }

        private Cell CheckIfPlayerHasCornerAndSide(Marker personMarker)
        {
            for (var i = 0; i < 4; i++)
                for (var j = 0; j < 2; j++)
                    if (Array.TrueForAll(new[] { _cells[_reverseCorners[i]].CurrentMarker, _cells[_adjacentSides[i, j]].CurrentMarker }, value => (personMarker == value)))
                        if (_cells[_oppositeCorners[i, j]].CurrentMarker == Marker.N)
                            return _cells[_oppositeCorners[i, j]];

            return null;
        }

        public Cell CheckIfPlayerHasCenter(Marker personMarker)
        {
            if (_cells[4].CurrentMarker == personMarker)
            {
                var randomCorner = _cells[_corners.Where(c => _cells[c].CurrentMarker == Marker.N).OrderBy(g => Guid.NewGuid()).FirstOrDefault()];
                return randomCorner.CurrentMarker == Marker.N ? randomCorner : CheckIfPlayerHasCenterAndCorner(personMarker);
            }
            return null;
        }

        private Cell CheckIfPlayerHasCenterAndCorner(Marker personMarker)
        {
            var random = new Random();
            for (var i = 0; i < 4; i++)
                if (_cells[_corners[i]].CurrentMarker == personMarker)
                {
                    var randomAdjacentCorner = _cells[_oppositeCorners[i, random.Next(0, 1)]];
                    if (randomAdjacentCorner.CurrentMarker == Marker.N)
                        return randomAdjacentCorner;
                }

            return null;
        }

        public Cell ChooseRandomRemainingCell()
        {
            var random = new Random();
            Cell randomCell;
            if (IsBoardEmpty())
                randomCell = random.Next(0, 4) == 0 ? _cells[4] : _cells[_corners.OrderBy(g => Guid.NewGuid()).FirstOrDefault()];
            else
                randomCell = _cells.Where(c => c.CurrentMarker == Marker.N).OrderBy(g => Guid.NewGuid()).FirstOrDefault();
            return randomCell;
        }

        public bool IsBoardEmpty()
        {
            return _cells.All(c => c.CurrentMarker == Marker.N);
        }

        public void SetClaimedCell(Cell claimedCell, Marker marker)
        {
            if (claimedCell.CurrentMarker == Marker.N)
                claimedCell.CurrentMarker = marker;
        }
    }
}
