using System.Linq;
using Ryocatusn.Util;

namespace Ryocatusn.Janken
{
    public class HandApplicationService
    {
        private IHandRepository handRepository { get; }

        public HandApplicationService(IHandRepository handRepository)
        {
            this.handRepository = handRepository;
        }
        public HandId Create(Hand.Shape shape)
        {
            Hand hand = new Hand(shape);
            handRepository.Save(hand);

            return hand.id;
        }
        public HandData Get(HandId id)
        {
            Option<Hand> hand = handRepository.Find(id);
            hand.Match(None: () => throw new JankenException("Handが見つかりません"));

            return new HandData(hand.Get().shape);
        }
        public HandEvents GetEvents(HandId id)
        {
            Option<Hand> hand = handRepository.Find(id);
            hand.Match(None: () => throw new JankenException("Handが見つかりません"));

            return new HandEvents(hand.Get().ChangeShapeEvent, Hand.JankenReverseEvent);
        }
        public JankenResult DoJanken(HandId[] ids)
        {
            Hand[] hands = ids.Select(id =>
            {
                Option<Hand> hand = handRepository.Find(id);
                if (hand.Get() == null) throw new JankenException("Handが見つかりません");
                return hand.Get();
            }).ToArray();

            Janken janken = new Janken();
            JankenResult result = janken.DoJanken(hands, Hand.reverse);

            return result;
        }
        public void ChangeShape(HandId id, Hand.Shape shape)
        {
            Option<Hand> hand = handRepository.Find(id);

            hand.Match
                (
                Some: x => x.ChangeShape(shape),
                None: () => throw new JankenException("Handが見つかりません")
                );
        }
        public void Delete(HandId id)
        {
            handRepository.Find(id).Match
                (
                Some: x => 
                {
                    x.Delete();
                    handRepository.Delete(id);
                },
                None: () => throw new JankenException("Handが見つかりません")
                );
        }
    }
}
