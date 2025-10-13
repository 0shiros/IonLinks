using UnityEngine;

public class ResetSave : MonoBehaviour
{
    public void EraseCurrentSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public void toto()
    {
        Debug.Log("dddd");
    }
}
