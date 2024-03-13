using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxItem : MonoBehaviour
{
    public GameObject block;
    public GameObject iframe;
    public bool isDisable = false;
    public void BoxInit(bool isBlock, bool isDisable)
    {
        block.SetActive(isBlock);
        this.isDisable = isDisable;
        iframe.SetActive(!isDisable);
    }
}
