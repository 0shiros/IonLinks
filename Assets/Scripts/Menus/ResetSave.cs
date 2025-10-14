using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSave : MonoBehaviour
{
    public void EraseCurrentSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
