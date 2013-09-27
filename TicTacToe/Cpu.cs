namespace TicTacToe
{
    public class Cpu : Player
    {

        public override Cell AI(GameBoard board)
        {
            var oponentMarker = Marker == Marker.O ? Marker.X : Marker.O;
            return board.CheckForTwoInARow(Marker) ??
                   board.CheckForTwoInARow(oponentMarker) ??
                   PreemptiveBlockingMove(board, oponentMarker) ??
                   RandomMove(board);
        }

        public Cell PreemptiveBlockingMove(GameBoard board, Marker otherMarker)
        {
            return board.CheckIfPlayerHasCorner(otherMarker) ??
                board.CheckIfPlayerHasSide(otherMarker) ??
                board.CheckIfPlayerHasCenter(otherMarker);
        }

        public static Cell RandomMove(GameBoard board)
        {
            return board.ChooseRandomRemainingCell();
        }

        public override void Turn(GameBoard board, Cell claimedCell)
        {
            board.SetClaimedCell(claimedCell, Marker);
        }

        public override bool IsCpu()
        {
            return true;
        }
    }
}