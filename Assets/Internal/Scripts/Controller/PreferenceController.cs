using System.Collections;
using System.Collections.Generic;
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
    }

    public void ReturnHome()
    {
        level_ui_container.gameObject.SetActive(true);
    }
}
