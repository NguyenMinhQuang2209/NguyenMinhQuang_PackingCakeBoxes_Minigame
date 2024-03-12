using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxItem : MonoBehaviour
{
    public GameObject block;
    public void BoxInit(bool isBlock)
    {
        block.SetActive(isBlock);
    }
}
