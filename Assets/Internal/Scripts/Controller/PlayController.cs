using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayController : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Map", LoadSceneMode.Single);
    }
}
