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
    private List<Image> backgroundButtons = new();
    
    [SerializeField] private GameObject start;

    private Image startImage;
    private Button startButton;
    private TextMeshProUGUI startText;
    
    private int buttonIndex = -1;
    
    [SerializeField] private Sprite[] darkLevelButtonSprites;
    [SerializeField] private Sprite[] lightLevelButtonSprites;
    [SerializeField] private Sprite[] startButtonSprites;
    [SerializeField] private Sprite[] lampSprites;
    
    private bool hasButtonBeenSelected = false;
    private bool[] hasBeenUnlocked = new bool[4];

    private void Start()
    {
        Time.timeScale = 1f;
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
            
            foreach (Image image in child.GetComponentsInChildren<Image>(true))
            {
                if (image.gameObject.name == "Background")
                {
                    backgroundButtons.Add(image);
                    image.gameObject.SetActive(false);
                }
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
                lamps[i].gameObject.GetComponent<SpriteRenderer>().sprite = lampSprites[PlayerPrefs.GetInt("chains" + i) == 1 ? 1 : 0];
                buttons[i].image.sprite = PlayerPrefs.GetInt("chains" + i) == 1 ? lightLevelButtonSprites[i] : darkLevelButtonSprites[i];
                PlayerPrefs.Save();
            }
            else
            {
                hasBeenUnlocked[i] = false;
                lamps[i].gameObject.GetComponentInChildren<Light2D>(true).gameObject.SetActive(false);
                buttons[i].enabled = false;
                lamps[i].gameObject.GetComponent<SpriteRenderer>().sprite = lampSprites[0];
                buttons[i].image.sprite = darkLevelButtonSprites[i];
            }
        }
    }

    public void OnlevelButtonClick(int levelIndex)
    {
        if (!hasBeenUnlocked[levelIndex])
        {
            return;
        }
        
        if (hasButtonBeenSelected && levelIndex == buttonIndex)
        {
            backgroundButtons[levelIndex].gameObject.SetActive(false);
            buttonIndex = -1;
            hasButtonBeenSelected = false;
            startImage.sprite = startButtonSprites[0];
            startText.gameObject.SetActive(false);
            startButton.enabled = true;
            
        }
        else
        {
            foreach(Image image in backgroundButtons)
            {
                image.gameObject.SetActive(false);
            }
            backgroundButtons[levelIndex].gameObject.SetActive(true);
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
