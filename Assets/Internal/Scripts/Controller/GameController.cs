using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private List<LevelSetting> levels = new();

    private Dictionary<int, BoxItem> boxStore = new();
    private Dictionary<int, int> starHistory = new();
    private int currentLevel = 0;


    LevelSetting previousLevel = null;

    private List<int> currentLevelMatrix = new();
    private int playLevel = 0;

    Present currentPresent = null;
    Cake currentCake = null;

    int presentPosition = -1;
    int cakePosition = -1;
    float currentPlayTime = 0f;

    private TextMeshProUGUI show_time_txt;


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
        starHistory[0] = 0;
        InitGame();
        Transform box_ui = PreferenceController.instance.box_ui_content;
        foreach (Transform child in box_ui)
        {
            Destroy(child.gameObject);
        }

        show_time_txt = PreferenceController.instance.time_show_txt;
    }
    public void InitGame()
    {
        SpawnLevel();
    }
    private void Update()
    {
        show_time_txt.text = FormatTime(currentPlayTime);
        if (currentPlayTime > 0)
        {
            currentPlayTime -= Time.deltaTime;
        }

        if (currentPlayTime == 0)
        {
            EndGame();
        }
    }
    public void SpawnLevel()
    {
        PreferenceController.instance.level_ui_container.SetActive(true);
        Transform content_ui = PreferenceController.instance.level_ui_content;
        LevelItem level_item = PreferenceController.instance.level;
        foreach (Transform child in content_ui.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < levels.Count; i++)
        {
            LevelItem level = Instantiate(level_item, content_ui.transform);
            int totalStar = currentLevel > i ? starHistory[i] : 0;
            level.LevelInit(currentLevel < i, i + 1, totalStar);
        }
    }

    public void Play(int position)
    {
        LevelSetting level = levels[position];
        LevelSettingInit(level);
        playLevel = position;
    }

    public void LevelSettingInit(LevelSetting level)
    {
        BoxItem box = PreferenceController.instance.box_item;
        Transform box_ui = PreferenceController.instance.box_ui_content;
        GridLayoutGroup grid_layout = box_ui.gameObject.GetComponent<GridLayoutGroup>();
        grid_layout.constraintCount = level.column;

        Present present = PreferenceController.instance.present;
        Cake cake = PreferenceController.instance.cake;

        List<int> levelMatrix = new();

        for (int i = 0; i < level.row; i++)
        {
            for (int j = 0; j < level.column; j++)
            {
                BoxItem tempBoxItem;
                bool isBlock = false;
                int key = i * level.row + j;
                if (boxStore.ContainsKey(key))
                {
                    tempBoxItem = boxStore[key];
                    tempBoxItem.gameObject.SetActive(true);
                    if (level.blockPosition.Contains(key + 1))
                    {
                        isBlock = true;
                    }
                }
                else
                {
                    tempBoxItem = Instantiate(box, box_ui.transform);
                    boxStore[key] = tempBoxItem;
                    if (level.blockPosition.Contains(key + 1))
                    {
                        isBlock = true;
                    }
                }
                tempBoxItem.BoxInit(isBlock);

                if (level.presentPosition - 1 == key)
                {
                    if (currentPresent == null)
                    {
                        currentPresent = Instantiate(present, tempBoxItem.transform);
                    }
                    else
                    {
                        currentPresent.transform.SetParent(tempBoxItem.transform, false);
                        currentPresent.transform.SetAsLastSibling();
                    }
                }

                if (level.cakeInPosition - 1 == key)
                {
                    if (currentCake == null)
                    {
                        currentCake = Instantiate(cake, tempBoxItem.transform);
                    }
                    else
                    {
                        currentCake.transform.SetParent(tempBoxItem.transform, false);
                        currentCake.transform.SetAsLastSibling();
                    }
                }

                levelMatrix.Add(key);
            }
        }

        if (previousLevel != null)
        {
            for (int i = 0; i < currentLevelMatrix.Count; i++)
            {
                if (!levelMatrix.Contains(currentLevelMatrix[i]))
                {
                    BoxItem tempItem = boxStore[currentLevelMatrix[i]];
                    tempItem.gameObject.SetActive(false);
                }
            }
        }
        currentLevelMatrix = levelMatrix;
        previousLevel = level;

        currentPlayTime = level.playTime;
        presentPosition = level.presentPosition - 1;
        cakePosition = level.cakeInPosition - 1;

        PreferenceController.instance.level_ui_container.SetActive(false);
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void EndGame()
    {

    }
    public void WinGame()
    {

    }
    public void ReloadGame()
    {
        LevelSettingInit(previousLevel);
    }

    public bool IsPlaying()
    {
        return previousLevel != null;
    }
    public void CheckDirection(Direction direction)
    {
        if (!IsPlaying())
        {
            return;
        }
        bool stop = true;
        switch (direction)
        {
            case Direction.Up:
                do
                {
                    int nextPosition = presentPosition - previousLevel.column;
                    if (nextPosition < 0 || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }
                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentPresent.transform.SetParent(boxItem.transform, false);
                        presentPosition = nextPosition;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);

                do
                {
                    int nextPosition = cakePosition - previousLevel.column;
                    if (nextPosition < 0 || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentCake.transform.SetParent(boxItem.transform, false);
                        cakePosition = nextPosition;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);
                break;
            case Direction.Down:
                do
                {
                    int nextPosition = presentPosition + previousLevel.column;
                    if (nextPosition > previousLevel.column * previousLevel.row || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentPresent.transform.SetParent(boxItem.transform, false);
                        presentPosition = nextPosition;
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);

                do
                {
                    int nextPosition = cakePosition + previousLevel.column;
                    if (nextPosition > previousLevel.column * previousLevel.row || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentCake.transform.SetParent(boxItem.transform, false);
                        cakePosition = nextPosition;
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);
                break;
            case Direction.Left:
                do
                {
                    int nextPosition = presentPosition - 1;
                    if ((presentPosition % previousLevel.column) == 0 || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentPresent.transform.SetParent(boxItem.transform, false);
                        presentPosition = nextPosition;
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);

                do
                {
                    int nextPosition = cakePosition - 1;
                    if ((cakePosition % previousLevel.column) == 0 || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentCake.transform.SetParent(boxItem.transform, false);
                        cakePosition = nextPosition;
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);
                break;
            case Direction.Right:
                do
                {
                    int nextPosition = presentPosition + 1;
                    if (((presentPosition + 1) % previousLevel.column) == 0 || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentPresent.transform.SetParent(boxItem.transform, false);
                        presentPosition = nextPosition;
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);

                do
                {
                    int nextPosition = cakePosition + 1;
                    if (((cakePosition + 1) % previousLevel.column) == 0 || previousLevel.blockPosition.Contains(nextPosition + 1))
                    {
                        break;
                    }

                    if (boxStore.ContainsKey(nextPosition))
                    {
                        BoxItem boxItem = boxStore[nextPosition];
                        currentCake.transform.SetParent(boxItem.transform, false);
                        cakePosition = nextPosition;
                        continue;
                    }
                    else
                    {
                        break;
                    }

                } while (stop);
                break;
        }
    }
    public void WinLevel()
    {
        Debug.Log("Win Game");
    }

    public void ReturnHome()
    {
        PreferenceController.instance.level_ui_container.SetActive(true);
        previousLevel = null;
    }
}
[System.Serializable]
public class LevelSetting
{
    public float playTime = 10f;
    public int column = 3;
    public int row = 3;
    public int presentPosition = 1;
    public int cakeInPosition = 3;
    public List<int> blockPosition = new();
}