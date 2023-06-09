using System.Linq;

namespace Ryocatusn.Janken
{
    public class JankenResult
    {
        public HandId[] winHands { get; }
        public HandId[] loseHands { get; }
        public HandId[] drawHands { get; }

        public JankenResult(HandId[] winHands = null, HandId[] loseHands = null, HandId[] drawHands = null)
        {
            this.winHands = winHands != null ? winHands : new HandId[0];
            this.loseHands = loseHands != null ? loseHands : new HandId[0];
            this.drawHands = drawHands != null ? drawHands : new HandId[0];
        }
        public bool IsDraw()
        {
            return drawHands.Length != 0;
        }
        public bool IsWin(HandId handId)
        {
            return winHands.Contains(handId);
        }
        public bool IsLose(HandId handId)
        {
            return loseHands.Contains(handId);
        }
    }
}
