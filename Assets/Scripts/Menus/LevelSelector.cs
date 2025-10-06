using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private List<Button> buttons = new();
    private List<RectTransform> chains = new(){null};
    
    [SerializeField] private GameObject start;

    private Image startImage;
    private Button startButton;
    
    private int buttonIndex = -1;
    
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color startColor;
    
    private bool hasButtonBeenSelected = false;
    private bool[] hasBeenUnlocked = new bool[4];

    private void Start()
    {
        startImage = start.GetComponent<Image>();
        startButton = start.GetComponent<Button>();
        startButton.enabled = false;
        startImage.color = defaultColor;
        
        foreach (RectTransform child in transform)
        {
            foreach (RectTransform chain in child)
            {
                if (chain.name == "Chains")
                {
                    chains.Add(chain);
                }
            }
            
            foreach (Button button in child.GetComponentsInChildren<Button>())
            {
                buttons.Add(button);
            }
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnlevelButtonClick(index));
        }
        
        hasBeenUnlocked[0] = true;
        
        for (int i = 1; i < hasBeenUnlocked.Length; i++)
        {
            if (PlayerPrefs.HasKey("chains"+i))
            {
                hasBeenUnlocked[i] = PlayerPrefs.GetInt("chains"+i) == 1;
                chains[i].gameObject.SetActive(PlayerPrefs.GetInt("chains"+i) == 0);
                buttons[i].enabled = PlayerPrefs.GetInt("chains"+i) == 1;
            }
            else
            {
                hasBeenUnlocked[i] = false;
                chains[i].gameObject.SetActive(true);
                buttons[i].enabled = false;
            }
        }
    }

    public void OnlevelButtonClick(int levelIndex)
    {
        Image buttonImage = buttons[levelIndex].GetComponent<Image>();
        
        if (hasButtonBeenSelected && levelIndex == buttonIndex)
        {
            buttonImage.color = defaultColor;
            buttonIndex = -1;
            hasButtonBeenSelected = false;
            startImage.color = defaultColor;
            startButton.enabled = true;
            
        }
        else if(!hasButtonBeenSelected && hasBeenUnlocked[levelIndex])
        {
            foreach (Button button in buttons)
            {
                Image buttonImages = button.GetComponent<Image>();
                buttonImages.color = defaultColor;
            }
            
            buttonImage.color = selectedColor;
            buttonIndex = levelIndex;
            hasButtonBeenSelected = true;
            startImage.color = startColor;
            startButton.enabled = true;
        }
    }

    public void OnStartButtonClick()
    {
        if (buttonIndex != -1)
        {
            SceneManager.LoadSceneAsync("Level" + buttonIndex);
        }
    }
    
    public void UnlockLevel(int levelIndex)
    {
        hasBeenUnlocked[levelIndex] = true;
        PlayerPrefs.SetInt("chains"+levelIndex, 1);
        PlayerPrefs.Save();
        chains[levelIndex].gameObject.SetActive(false);
        buttons[levelIndex].enabled = true;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync("LevelSelector");
    }
}
