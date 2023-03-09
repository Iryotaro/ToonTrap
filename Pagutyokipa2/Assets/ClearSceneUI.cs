using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSceneUI : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
