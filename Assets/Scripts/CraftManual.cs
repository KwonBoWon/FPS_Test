using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft 
{
    public string craftName;
    public GameObject go_Prefab;
    public GameObject go_PreviewPrefab;
}


public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private Craft[] craft_fire;
   
    private GameObject go_Preview;
    private GameObject go_Prefab;

    [SerializeField] private Transform tf_Player;


    // Raycast
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
            Debug.Log("fire1");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;

            go_BaseUI.SetActive(false);
        }
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(go_Preview);
        }
        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;

        go_BaseUI.SetActive(false);
    }

    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }
    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }


}
