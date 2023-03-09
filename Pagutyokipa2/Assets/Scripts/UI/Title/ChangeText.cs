using TMPro;
using Ryocatusn.Ryoseqs;

namespace Ryocatusn.UI
{
    public class ChangeText : SequenceCommand
    {
        public ChangeText(TextMeshProUGUI text, string message)
        {
            handler = new TaskHandler(_ =>
            {
                text.text = message;
            });
        }
    }
}
