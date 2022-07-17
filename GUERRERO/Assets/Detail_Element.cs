using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detail_Element : MonoBehaviour
{
    [SerializeField]
    private Text Name_txt;

    [SerializeField]
    private GameObject Data;

    [SerializeField]
    private Vector3 Offset;
    private void Update()
    {
        if (Data.activeSelf == true)
        {
            FollowMouse(Input.mousePosition);
        }
    }
    private void FollowMouse(Vector3 mousePos)
    {
        transform.position = mousePos+Offset;
    }
    public void Set_Detail(string name)
    {
        StartCoroutine(Show_Detail());
        InGameUI_Manager.Instance.ShowText(name, Name_txt);
    }

    public void Reset_Detail()
    {
        StopAllCoroutines();
        InGameUI_Manager.Instance.ShowText(string.Empty, Name_txt);
        Data.SetActive(false);
    }

    IEnumerator Show_Detail()
    {
        yield return new WaitForSeconds(1f);
        Data.SetActive(true);
    }
}
