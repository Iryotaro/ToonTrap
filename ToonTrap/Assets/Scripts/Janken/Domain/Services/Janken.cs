using System.Collections.Generic;
using System.Linq;

namespace Ryocatusn.Janken
{
    public class Janken
    {
        //次の要素のほうが強く、前の要素のほうが弱い
        private List<Hand.Shape> shapes = new List<Hand.Shape>() { Hand.Shape.Paper, Hand.Shape.Scissors, Hand.Shape.Rock };

        public JankenResult DoJanken(Hand[] hands, bool reverse = false)
        {
            bool draw = IsDraw(hands.ToList());
            if (draw) return new JankenResult(drawHands: hands.Select(x => x.id).ToArray());

            List<Hand> winHands = new List<Hand>();
            List<Hand> loseHands = new List<Hand>();
            foreach (Hand hand in hands)
            {
                bool win = IsWin(hand, hands.ToList());

                if (win) winHands.Add(hand);
                else loseHands.Add(hand);
            }

            if (!reverse) return new JankenResult(winHands: winHands.Select(x => x.id).ToArray(), loseHands: loseHands.Select(x => x.id).ToArray());
            else return new JankenResult(winHands: loseHands.Select(x => x.id).ToArray(), loseHands: winHands.Select(x => x.id).ToArray());
        }

        private bool IsDraw(List<Hand> hands)
        {
            Hand.Shape baseShape = hands[0].shape;
            if (hands.Where(x => !x.shape.Equals(baseShape)).ToList().Count() == 0) return true;

            foreach (Hand.Shape shape in shapes)
                if (hands.Where(x => x.shape.Equals(shape)).Count() == 0) return false;

            return true;
        }
        private bool IsWin(Hand hand, List<Hand> hands)
        {
            Hand.Shape strongShape = GetStrongShape(hand);
            if (hands.Where(x => x.shape.Equals(strongShape)).Count() == 0) return true;
            return false;
        }

        private Hand.Shape GetStrongShape(Hand hand)
        {
            int index = shapes.IndexOf(hand.shape);
            index = (index + 1) % 3;
            return shapes[index];
        }
    }
}
