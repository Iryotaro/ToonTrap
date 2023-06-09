namespace Ryocatusn.Janken
{
    public interface IForJankenViewEditor
    {
        public Hand.Shape GetShape();
        public void UpdateView(Hand.Shape shape);
    }
}
