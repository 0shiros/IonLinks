using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private List<Button> buttons = new();
    private List<Transform> lamps = new(){null};
    
    [SerializeField] private GameObject start;

    private Image startImage;
    private Button startButton;
    private TextMeshProUGUI startText;
    
    private int buttonIndex = -1;
    
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Sprite[] startButtonSprites;
    [SerializeField] private Sprite[] lampSprites;
    
    private bool hasButtonBeenSelected = false;
    private bool[] hasBeenUnlocked = new bool[4];

    private void Start()
    {
        startImage = start.GetComponent<Image>();
        startButton = start.GetComponent<Button>();
        startText = start.GetComponentInChildren<TextMeshProUGUI>(true);
        startButton.enabled = false;
        startImage.sprite = startButtonSprites[0];
        startText.gameObject.SetActive(false);
        
        foreach (RectTransform child in transform)
        {
            foreach (Transform lamp in child)
            {
                if (lamp.name == "Lamp")
                {
                    lamps.Add(lamp);
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
                lamps[i].gameObject.GetComponentInChildren<Light2D>(true).gameObject.SetActive(PlayerPrefs.GetInt("chains"+i) == 1);
                buttons[i].enabled = PlayerPrefs.GetInt("chains"+i) == 1;
                lamps[i].gameObject.GetComponent<SpriteRenderer>().sprite = lampSprites[1];
                PlayerPrefs.Save();
            }
            else
            {
                hasBeenUnlocked[i] = false;
                lamps[i].gameObject.GetComponentInChildren<Light2D>(true).gameObject.SetActive(false);
                buttons[i].enabled = false;
                lamps[i].gameObject.GetComponent<SpriteRenderer>().sprite = lampSprites[0];
            }
        }
    }

    public void OnlevelButtonClick(int levelIndex)
    {
        if (!hasBeenUnlocked[levelIndex])
        {
            return;
        }
        
        Image buttonImage = buttons[levelIndex].GetComponent<Image>();
        
        if (hasButtonBeenSelected && levelIndex == buttonIndex)
        {
            buttonImage.color = defaultColor;
            buttonIndex = -1;
            hasButtonBeenSelected = false;
            startImage.sprite = startButtonSprites[0];
            startText.gameObject.SetActive(false);
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
            startImage.sprite = startButtonSprites[1];
            startText.gameObject.SetActive(true);
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
