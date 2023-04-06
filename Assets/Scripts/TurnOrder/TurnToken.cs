using MailSnail.Units;

namespace MailSnail.TurnOrder
{
    public class TurnToken
    {
        public bool IsDone { get; private set; }
        public Unit Unit { get; private set; }


        public TurnToken(Unit unit)
        {
            this.Unit = unit;
        }

        public void SetIsDone(bool isDone)
        {
            this.IsDone = isDone;
        }
    }
}