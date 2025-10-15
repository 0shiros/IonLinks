using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonResetLevel : MonoBehaviour
{
    public void ResetLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
