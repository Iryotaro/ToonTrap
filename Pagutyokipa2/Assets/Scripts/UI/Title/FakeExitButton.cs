using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ryocatusn.UI
{
    public class FakeExitButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private TextMeshProUGUI text;
        private bool start = false;
        private Quaternion direction;
        private float speed;

        public void Setup(TextMeshProUGUI text, TitleUIView titleUIView)
        {
            if (titleUIView.exitButton.Get() == null) return;

            direction = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            transform.position = titleUIView.exitButton.Get().transform.position;
            this.text.text = text.text;
            start = true;
            speed = titleUIView.exitButtonSpeed;

            Destroy(gameObject, 10);

            button.OnPointerDownAsObservable()
                .Subscribe(_ => this.text.text = "偽物だよ！");
        }
        private void Update()
        {
            if (!start) return;

            transform.Translate(direction * Vector3.up * speed * Time.deltaTime);
        }
    }
}
