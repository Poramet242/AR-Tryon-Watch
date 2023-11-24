using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class testvector : MonoBehaviour
{
    public GameObject leftWrist;
    public GameObject rightWrist;

    public GameObject _objectPos;

    public GameObject[] HandOutline;

    private string buttonName;

    private GameObject _objButton;
    private Canvas canvas;

    private List<Button> buttonObjectlist = new List<Button>();
    private List<GameObject> selectedObjectPrefabslist = new List<GameObject>();
    private List<GameObject> _objectInstancePrefab = new List<GameObject>();

    private string _tagButtonObject = "WatchButton";

    void Start()
    {
        setObjectInstances();
        foreach (GameObject item in HandOutline)
        {
            item.SetActive(true);
        }
    }
    void Update()
    {
        foreach (Button _button in buttonObjectlist)
        {
            _button.onClick.AddListener(() =>
            {
                buttonName = _button.gameObject.name;
            });
        }
        Claculates(_objectPos);
    }
    public void Claculates(GameObject objectIns)
    {
        float centerpointWrist = 0.5f;
        Vector3 spawnPos = Vector3.Lerp(leftWrist.transform.localPosition, rightWrist.transform.localPosition, centerpointWrist);
        objectIns.transform.localPosition = spawnPos;
        objectIns.transform.LookAt(leftWrist.transform.position);
        SelectObjectInstance(objectIns);
    }
    private void SelectObjectInstance (GameObject pointWristPosition)
    {
        foreach (GameObject objectsinlist in _objectInstancePrefab)
        {
            objectsinlist.transform.position = pointWristPosition.transform.position;
            if (objectsinlist.gameObject.name == buttonName)
            {
                objectsinlist.gameObject.SetActive(true);
                Debug.Log("Can show"+ objectsinlist.name);
            }
            else
            {
                objectsinlist.gameObject.SetActive(false);
                Debug.Log("Can't show"+ objectsinlist.name);
            }
        }
    }
    public void setObjectInstances()
    {
        selectedObjectPrefabslist = Resources.LoadAll<GameObject>("Prefabs/Watchs/").ToList();
        canvas = GameObject.Find("MainCanvas/WatchButtonCnv").GetComponent<Canvas>();
        foreach (GameObject objectSelect in selectedObjectPrefabslist)
        {
            _objectInstancePrefab.Add(Instantiate<GameObject>(objectSelect, _objectPos.transform));
        }
        setObjectAddToList(_objectInstancePrefab);
    }
    public void createButton(string ObjectName)
    {
        _objButton = DefaultControls.CreateButton(new DefaultControls.Resources());
        _objButton.gameObject.name = ObjectName;
        _objButton.transform.Find("Text").GetComponent<Text>().text = ObjectName;
        _objButton.transform.Find("Text").GetComponent<Text>().fontSize = 24;
        _objButton.transform.GetComponent<Button>().tag = _tagButtonObject;
        _objButton.transform.SetParent(canvas.transform, false);
        buttonObjectlist.Add(_objButton.GetComponent<Button>());
    }
    public void setObjectAddToList(List<GameObject> _objectInlist)
    {
        foreach (GameObject item in _objectInlist)
        {
            if (item.gameObject.tag == "AppleWatch")
            {
                createButton(item.name = item.name.Replace("(Clone)", buttonName));
            }
            else if (item.gameObject.tag == "DivingWatch")
            {
                createButton(item.name = item.name.Replace("(Clone)", buttonName));
            }
        }
    }
}
