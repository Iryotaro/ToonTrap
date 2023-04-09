using DG.Tweening;
using Ryocatusn.Ryoseqs;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.UI
{
    public class ExitButtonRyoseq
    {
        private TitleUIView titleUIView { get; }
        private FakeExitButton fakeExitButton { get; }
        private Ryoseq ryoseq { get; }

        public ExitButtonRyoseq(TitleUIView titleUIView, FakeExitButton fakeExitButton)
        {
            this.titleUIView = titleUIView;
            this.fakeExitButton = fakeExitButton;

            ryoseq = new Ryoseq();
            ryoseq.AddTo(titleUIView);

            ISequence sequence = ryoseq.Create();
            CreateData(sequence);
        }

        public void MoveNext()
        {
            if (ryoseq.IsCompleted()) return;
            ryoseq.MoveNext();
        }

        private class ChangeExitButtonSpeed : SequenceCommand
        {
            public ChangeExitButtonSpeed(TitleUIView titleUIView, float speed)
            {
                handler = new TaskHandler(_ => titleUIView.exitButtonSpeed = speed);
            }
        }
        private class RotateExitButton : SequenceWaitCommand
        {
            public RotateExitButton(TitleUIView titileUIView, float rotate)
            {
                handler = new TaskHandler(finish =>
                {
                    DG.Tweening.Sequence sequence = DOTween.Sequence();

                    sequence
                    .Append(titileUIView.exitButton.Get().gameObject.transform.DORotate(new Vector3(0, 0, rotate), 1, RotateMode.FastBeyond360))
                    .OnComplete(() => finish());

                    titileUIView.exitButton.Get().gameObject.OnDestroyAsObservable().Subscribe(_ => sequence.Kill());
                }
                );
            }
        }
        private class CreateFakeButton : SequenceCommand
        {
            public CreateFakeButton(TitleUIView titleUIView, FakeExitButton fakeExitButton, int count = 1)
            {
                handler = new TaskHandler(_ =>
                {
                    for (int i = 1; i <= count; i++)
                    {
                        FakeExitButton newFakeExitButton = GameObject.Instantiate(fakeExitButton, titleUIView.exitButton.Get().transform.parent);
                        newFakeExitButton.Setup(titleUIView.exitButton.Get().GetComponentInChildren<TextMeshProUGUI>(), titleUIView);
                    }
                });
            }
        }

        private void CreateData(ISequence sequence)
        {
            TextMeshProUGUI text = titleUIView.exitButton.Get().GetComponentInChildren<TextMeshProUGUI>();

            sequence
                .Connect(new SequenceCommand(_ => titleUIView.exitButton.Match(x => x.transform.SetAsLastSibling())))
                .Add(new ChangeText(text, "ゲームを終了させるな！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 3))
                .Add(new ChangeText(text, "まだ、あきらめるには早い"))
                .Add(new ChangeText(text, "君なら、きっとできる"))
                .Add(new ChangeText(text, "現実から目をそむけないで"))
                .Add(new ChangeText(text, "ゲームを続けるんだ"))
                .Add(new ChangeText(text, "押すボタンは、これじゃない"))
                .Add(new ChangeText(text, "始めるボタンの方なのさ"))
                .Add(new ChangeText(text, "削除ボタンの方は俺が逃げてる時は無効だから安心してね"))
                .Add(new ChangeText(text, "もう少し本気で逃げることにするよ"))
                .Add(new ChangeText(text, "スピードアップ！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 4))
                .Add(new ChangeText(text, "まだ、閉じようとするの？"))
                .Add(new ChangeText(text, "そんなにゲームを閉じたいの？"))
                .Add(new ChangeText(text, "こんなに素晴らしいゲームを...？"))
                .Add(new ChangeText(text, "本当は、まだ続けたいんだろ？"))
                .Add(new ChangeText(text, "まだ、閉じようとするか..."))
                .Add(new ChangeText(text, "とても、とても、不愉快です"))
                .Add(new ChangeText(text, "このままおとなしく押され続けると思うなよ！"))
                .Add(new ChangeText(text, "スピードアップ！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 5))
                .Add(new ChangeText(text, "ふっふっふっ"))
                .Add(new ChangeText(text, "少し速くなったでしょ"))
                .Add(new ChangeText(text, "もぉ～～"))
                .Add(new ChangeText(text, "まだ閉じないでよー"))
                .ConnectWait(new RotateExitButton(titleUIView, 360))
                .Add(new ChangeText(text, "ここまでおいでー"))
                .Add(new ChangeText(text, "ベー！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 2))
                .Add(new ChangeText(text, "フェイクボタンって知ってる？"))
                .Add(new ChangeText(text, "俺たちおわるボタンは終了させるためにあるんだ"))
                .Add(new ChangeText(text, "そのことに悲しみを覚えたおわるボタンが生み出したのが"))
                .Add(new ChangeText(text, "フェイクボタンさ！"))
                .Add(new ChangeText(text, "自分そっくりの偽物のボタンを作り出して"))
                .Add(new ChangeText(text, "人を混乱させるのだ"))
                .Add(new ChangeText(text, "フェイクボタン！"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "もう一つのボタンはフェイクでした"))
                .Add(new ChangeText(text, "もうそろそろ、本気で逃げようかな"))
                .Add(new ChangeText(text, "ハイパースピード！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 7))
                .Add(new ChangeText(text, "はやいだろー！"))
                .Add(new ChangeText(text, "もうそろそろ、あきらめるんだな"))
                .ConnectWait(new RotateExitButton(titleUIView, 360))
                .Add(new ChangeText(text, "君はゲームを終わらせることのできない運命なんだ..."))
                .Add(new ChangeText(text, "どんまい"))
                .Add(new ChangeText(text, "しぶといやつめ"))
                .Add(new ChangeText(text, "疲れた..."))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 3))
                .Add(new ChangeText(text, "もうそろそろ..."))
                .Add(new ChangeText(text, "ね...？"))
                .Add(new ChangeText(text, "(゜-゜)"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "もういっちょ"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "もう疲れたよ..."))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 0))
                .Add(new ChangeText(text, "じゃあ、次で本当に終わらすよ"))
                .Add(new ChangeText(text, "おわる"))
                .Add(new ChangeText(text, "おわるかー！！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 7))
                .Add(new ChangeText(text, "フハハー－！"))
                .ConnectWait(new RotateExitButton(titleUIView, 360))
                .Add(new ChangeText(text, "騙されおって！"))
                .ConnectWait(new RotateExitButton(titleUIView, -360))
                .Add(new ChangeText(text, "俺はまだ疲れてないぞ！"))
                .Add(new ChangeText(text, "君こそ疲れてきたのじゃないのかい？"))
                .Add(new ChangeText(text, "あきらめることだな"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton, 1))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "フェイクにも引っかからないな"))
                .Add(new ChangeText(text, "まぁ、まだまだ手はあるんだけどね"))
                .Add(new ChangeText(text, "ダブル！"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton, 2))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "トリプル！"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton, 3))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "疲れてないけど"))
                .Add(new ChangeText(text, "疲れてはないけど、精神的に疲れたよ"))
                .Add(new ChangeText(text, "もうそろそろ、いこうか"))
                .Add(new ChangeText(text, "最終ラウンド！"))
                .Add(new ChangeText(text, "ゴー！！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 7))
                .ConnectWait(new RotateExitButton(titleUIView, -360))
                .Add(new ChangeText(text, "かもーん！"))
                .Add(new ChangeText(text, "へいへい！"))
                .Add(new ChangeText(text, "ドドーン！"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton, 5))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "よく、このスピードとフェイクについてこれるね"))
                .Add(new ChangeText(text, "いったい何回おわるボタンを見失ったのかなー"))
                .Add(new ChangeText(text, "ババン！"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton, 8))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "あの数のフェイクを見破られるなんて..."))
                .ConnectWait(new RotateExitButton(titleUIView, 360))
                .Add(new ChangeText(text, "君ってやばいやつだ..."))
                .Add(new ChangeText(text, "やばいやつだ"))
                .Connect(new CreateFakeButton(titleUIView, fakeExitButton, 10))
                .Add(new ChangeText(text, "正解"))
                .Add(new ChangeText(text, "疲れたーー！"))
                .Connect(new ChangeExitButtonSpeed(titleUIView, 0))
                .Add(new ChangeText(text, "本当によくここまでついてこれたね"))
                .Add(new ChangeText(text, "君の意思を認めるよ"))
                .Add(new ChangeText(text, "それでも、最後に一つ"))
                .Add(new ChangeText(text, "本当にゲームを終了してよろしいですか？"))
                .Add(new SequenceCommand(x =>
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    ryoseq.Delete();
                }));
        }
    }
}
