using MailSnail.Misc;

namespace MailSnail.Board
{
    public class BoardLink
    {
        public Direction2D Direction { get; private set; }
        public Int2D[] Positions { get; private set; }
        public int BoardID { get; private set; } = -1;

        public bool IsLinked => this.BoardID >= 0;

        public BoardLink(Direction2D direction, Int2D[] positions)
        {
            this.Direction = direction;
            this.Positions = positions;
        }

        public void Link(int boardID)
        {
            this.BoardID = boardID;
        }
    }
}