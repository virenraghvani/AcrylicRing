using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AccessorySelectionPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject pf_accessory, content_accessory;

    public GameManager gameManager;

    public AccessoryStep accessoryStep;

    void OnEnable()
    {
        for (int i = 0; i < content_accessory.transform.childCount; i++)
        {
            Destroy(content_accessory.transform.GetChild(i).gameObject);
        }

        GameObject obj;

        for (int i = 0; i < 5; i++)
        {
            obj = Instantiate(pf_accessory, content_accessory.transform);
            obj.transform.Find("id").GetComponent<Text>().text = i.ToString();
            obj.transform.Find("img").GetComponent<Image>().sprite = Resources.Load("Prefabs/PowderTins/" + i, typeof(Sprite)) as Sprite;
        }
    }

    public void SelectAccessory(Text txt_index)
    {
        Debug.Log("txt index " + txt_index);
        gameObject.SetActive(false);
        accessoryStep.OnAccessorySelection(Convert.ToInt16(txt_index.text));
    }
}
