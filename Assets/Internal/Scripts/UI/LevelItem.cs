using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public GameObject lock_icon;
    public TextMeshProUGUI level_txt;
    public List<Image> star = new();
    public Button playBtn;
    private int position = -1;
    private bool isLock = false;
    private void Start()
    {
        playBtn.onClick.AddListener(() =>
        {
            PlayInLevel();
        });
    }
    public void LevelInit(bool isLock, int position, int starCount)
    {
        this.isLock = isLock;
        lock_icon.SetActive(isLock);
        level_txt.text = position.ToString();
        this.position = position;
        level_txt.gameObject.SetActive(!isLock);
        for (int i = 0; i < star.Count; i++)
        {
            Sprite tempStar = PreferenceController.instance.without_star;
            if (!isLock && i < starCount)
            {
                tempStar = PreferenceController.instance.with_star;
            }
            star[i].sprite = tempStar;
        }
    }
    public void UnlockLevel(int starCount)
    {
        level_txt.gameObject.SetActive(true);
        for (int i = 0; i < star.Count; i++)
        {
            Sprite tempStar = PreferenceController.instance.without_star;
            if (i < starCount)
            {
                tempStar = PreferenceController.instance.with_star;
            }
            star[i].sprite = tempStar;
        }
    }

    public void PlayInLevel()
    {
        if (!isLock)
        {
            GameController.instance.Play(position - 1);
        }
    }
}
