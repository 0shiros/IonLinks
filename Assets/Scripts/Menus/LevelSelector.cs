using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private List<Button> buttons = new();
    
    [SerializeField] private GameObject start;

    private Image startImage;
    private Button startButton;
    
    private int buttonIndex = -1;
    
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color startColor;
    
    private bool hasButtonBeenSelected = false;

    private void Start()
    {
        startImage = start.GetComponent<Image>();
        startButton = start.GetComponent<Button>();
        startButton.enabled = false;
        startImage.color = defaultColor;
        
        foreach (RectTransform child in transform)
        {
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
        else
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
}
