using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PowderSelectionPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject pf_powder, content_powder;

    public GameManager gameManager;

    void OnEnable()
    {
        for (int i = 0; i < content_powder.transform.childCount; i++)
        {
            Destroy(content_powder.transform.GetChild(i).gameObject);
        }

        GameObject obj;

        for (int i = 0; i < 5; i++)
        {
            obj = Instantiate(pf_powder, content_powder.transform);
            obj.transform.Find("id").GetComponent<Text>().text = i.ToString();
            obj.transform.Find("img").GetComponent<Image>().sprite = Resources.Load("Prefabs/PowderTins/" + i, typeof(Sprite)) as Sprite;
        }
    }

   public void SelectPowder(Text txt_index) {

        Debug.Log("txt index "+txt_index);
        gameObject.SetActive(false);
        gameManager.OnPowderSelection(Convert.ToInt16(txt_index.text));
    }

}
