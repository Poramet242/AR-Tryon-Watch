using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class CalculateWatchTryOn : MonoBehaviour
{
    [SerializeField]
    private GameObject _watchPos;
    [SerializeField]
    private WristInfoGizmo wristInfoGizmo;
    [SerializeField]
    private ShowDebugTryonManager showDebugTryonManager;
    [SerializeField]
    private List<Button> buttonList = new List<Button>();
    [SerializeField]
    private List<GameObject> selectedObjectPrefabslist = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _objectInstancePrefab = new List<GameObject>();
    private Canvas canvas;
    private GameObject _objButton;
    private float centerplacment = 0.5f;
    private string buttonName;
    private string objectName;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        showDebugTryonManager.Showtutorial = true;
        setObjectInstances();
    }
    private void Update()
    {
        foreach (Button _button in buttonList)
        {
            _button.onClick.AddListener(() => 
            { 
                buttonName = _button.gameObject.name;
            });
        }
        CheckGesture();
    }
    public void CheckGesture()
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.NO_HAND)
        {
            Debug.Log("Can't Calculate gesturing");
            foreach (GameObject item in _objectInstancePrefab)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.GRAB_GESTURE)
            {
                showDebugTryonManager.Showtutorial = false;
                CalculateWristWatch(_watchPos);
                Debug.Log("Grab Gesturing");
            }
        }
    }
    public void CalculateWristWatch(GameObject objectPos)
    {
        wristInfoGizmo.ShowWristInformation();
        Vector3 wristPlacement = Vector3.Lerp(wristInfoGizmo.LeftWristPoint3DPosition, wristInfoGizmo.RightWristPoint3DPosition, centerplacment);
        objectPos.transform.LookAt(wristInfoGizmo.LeftWristPoint3DPosition,-wristPlacement);
        objectPos.transform.localPosition = wristPlacement;
        objectPos.transform.localScale = new Vector3(wristInfoGizmo.WidthBetweenWristPoints,
                                                 wristInfoGizmo.WidthBetweenWristPoints, 
                                                 wristInfoGizmo.WidthBetweenWristPoints)/1.5f;
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side == HandSide.Palmside)
        {
            objectPos.transform.localScale = new Vector3(-objectPos.transform.localScale.x, -objectPos.transform.localScale.y, -objectPos.transform.localScale.z);
        }
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.PINCH_GESTURE)
        {

        }
        SelectObjectInstance(objectPos);
    }
    private void SelectObjectInstance(GameObject pointWristPosition)
    {
        foreach (GameObject item in _objectInstancePrefab)
        {
            item.transform.position = pointWristPosition.transform.position;
            item.name = item.name.Replace("(Clone)", buttonName);
            if (item.gameObject.name == buttonName)
            {
                item.gameObject.SetActive(true);
                Debug.Log("Can show" + item.name);
            }
            else
            {
                item.gameObject.SetActive(false);
                Debug.Log("Can't show" + item.name);
            }
        }
    }
    public void setObjectInstances()
    {
        canvas = GameObject.Find("MainCanvas/WatchObjectButton").GetComponent<Canvas>();
        showDebugTryonManager = GameObject.Find("MainCanvas").GetComponent<ShowDebugTryonManager>();
        if (wristInfoGizmo == null)
        {
            try
            {
                wristInfoGizmo = GameObject.Find("TryOnManager").GetComponent<WristInfoGizmo>();
            }
            catch
            { 
                Debug.Log("Can find 'TryOnManager' GameOject");
            }
        }
        setLoadGameObject();
    }
    public void setLoadGameObject()
    {
        selectedObjectPrefabslist = Resources.LoadAll<GameObject>("Prefabs/Watchs/").ToList();
        foreach (GameObject objectSelect in selectedObjectPrefabslist)
        {
            objectName = objectSelect.name;
            _objectInstancePrefab.Add(Instantiate<GameObject>(objectSelect, _watchPos.transform));
            createButton(objectName);
        }
    }
    public void createButton(string _buttonName)
    {
        _objButton = DefaultControls.CreateButton(new DefaultControls.Resources());
        _objButton.gameObject.name = _buttonName;
        _objButton.transform.Find("Text").GetComponent<Text>().text = _buttonName;
        _objButton.transform.Find("Text").GetComponent<Text>().fontSize = 12;
        _objButton.transform.Find("Text").GetComponent<Text>().fontStyle = FontStyle.Bold;
        _objButton.transform.SetParent(canvas.transform, false);
        buttonList.Add(_objButton.GetComponent<Button>());
    }
}
