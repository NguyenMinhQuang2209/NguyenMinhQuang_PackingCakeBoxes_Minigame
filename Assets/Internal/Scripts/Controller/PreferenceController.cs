using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreferenceController : MonoBehaviour
{
    public static PreferenceController instance;

    [Header("World Object")]
    public GameObject level_ui_container;
    public Transform level_ui_content;
    public Transform box_ui_content;
    public Button home_btn;
    public TextMeshProUGUI time_show_txt;
    public Button refresh_btn;

    public GameObject fail_ui;
    public GameObject win_ui;

    public List<Image> win_star;

    [Space(10)]
    [Header("Local object")]
    public LevelItem level;
    public BoxItem box_item;
    public Present present;
    public Cake cake;


    [Space(10)]
    [Header("Another")]
    public Sprite without_star;
    public Sprite with_star;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        home_btn.onClick.AddListener(() =>
        {
            ReturnHome();
        });
        refresh_btn.onClick.AddListener(() =>
        {
            RefreshGame();
        });

        fail_ui.SetActive(false);
        win_ui.SetActive(false);
    }

    public void ReturnHome()
    {
        fail_ui.SetActive(false);
        win_ui.SetActive(false);
        GameController.instance.ReturnHome();
    }
    public void RefreshGame()
    {
        fail_ui.SetActive(false);
        win_ui.SetActive(false);
        GameController.instance.ReloadGame();
    }
    public void NextLevel()
    {
        fail_ui.SetActive(false);
        win_ui.SetActive(false);
    }
}
