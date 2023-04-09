using DG.Tweening;
using Ryocatusn.Ryoseqs;
using TMPro;
using UnityEngine;

namespace Ryocatusn.UI
{
    public class DeleteButtonRyoseq
    {
        private TitleUIView titleUIView;
        private Ryoseq ryoseq { get; }
        private ISequence sequence { get; }

        public DeleteButtonRyoseq(TitleUIView titleUIView)
        {
            this.titleUIView = titleUIView;

            ryoseq = new Ryoseq();
            ryoseq.AddTo(titleUIView);

            sequence = ryoseq.Create();
            CreateData(sequence);
        }

        public void MoveNext()
        {
            if (ryoseq.IsCompleted()) return;
            ryoseq.MoveNext();
        }
        public void Reset()
        {
            if (ryoseq.IsCompleted()) return;
            ryoseq.Reset(sequence);
        }

        private void CreateData(ISequence sequence)
        {
            TextMeshProUGUI text = titleUIView.deleteButton.GetComponentInChildren<TextMeshProUGUI>();

            sequence
                .Connect(new SequenceCommand(_ => titleUIView.deleteButton.transform.SetAsLastSibling()))
                .Add(new ChangeText(text, "消しちゃうの..."))
                .Add(new ChangeText(text, "本当に...？"))
                .Add(new ChangeText(text, "今まで何のために...頑張ってきたのか..."))
                .Add(new ChangeText(text, "."))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new ChangeText(text, ".."))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new ChangeText(text, "..."))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new ChangeText(text, "...."))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new ChangeText(text, "....."))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new ChangeText(text, "......"))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new ChangeText(text, "......！"))
                .Add(new ChangeText(text, "あ〜"))
                .Add(new ChangeText(text, "ひらめいた"))
                .Add(new ChangeText(text, "\"もう一度このゲームを遊びたい\"ということか..."))
                .Add(new ChangeText(text, "しかたないなぁー！"))
                .Add(new ChangeText(text, "ならば消してあげるよ！"))
                .Add(new ChangeText(text, "本当に消す！！"))
                .Connect(new SequenceCommand(_ =>
                {
                    PlayerPrefs.DeleteKey("SaveStage");
                }))
                .Connect(new ChangeText(text, "削除したよ！"))
                .Add(new ChangeText(text, "削除してるよ"))
                .Add(new ChangeText(text, "削除してる..."))
                .Add(new ChangeText(text, "？"))
                .Add(new ChangeText(text, "本当は消したくなかったの...？"))
                .Add(new ChangeText(text, "（やばい...もう完全に消しちゃった...）"))
                .Add(new ChangeText(text, "あ、ちょっとトイレ行きたい..."))
                .Add(new ChangeText(text, "ま...またね..."))
                .Connect(new SequenceCommand(_ => titleUIView.deleteButton.transform.DOMoveX(15, 3).SetEase(Ease.Linear).SetLink(titleUIView.deleteButton.gameObject)))
                .OnCompleted(() =>
                {
                    ryoseq.Delete();
                });
        }
    }
}
