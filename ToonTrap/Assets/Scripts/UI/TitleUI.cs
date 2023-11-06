using UnityEngine;

namespace Ryocatusn.UI
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField]
        private Animator start;
        [SerializeField]
        private Animator quit;
        [SerializeField]
        private Transition transition;

        private InputMaster input;
        private Choice choice = Choice.Start;

        public enum Choice
        {
            Start,
            Quit
        }

        private void Awake()
        {
            input = new InputMaster();
        }
        private void OnEnable()
        {
            input.Title.Enable();
        }
        private void OnDisable()
        {
            input.Title.Disable();
        }

        private void Start()
        {
            input.Title.SelectUp.performed += ctx => ChangeChoice(Choice.Start);
            input.Title.SelectDown.performed += ctx => ChangeChoice(Choice.Quit);

            bool started = false;

            input.Title.Decide.performed += ctx =>
            {
                if (choice == Choice.Start && !started)
                {
                    started = true;
                    StartGame();
                }
                else if (choice == Choice.Quit)
                {
                    Quit();
                }
            };

            ChangeChoice(Choice.Start);
        }

        private void ChangeChoice(Choice choice)
        {
            this.choice = choice;

            if (this.choice == Choice.Start)
            {
                start.Play("Choice");
                quit.Play("Wait");
            }
            else if (this.choice == Choice.Quit)
            {
                start.Play("Wait");
                quit.Play("Choice");
            }
        }

        private void StartGame()
        {
            Transition.FullLoadScene("Title", "Game", TransitionSettings.Default());
        }
        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
