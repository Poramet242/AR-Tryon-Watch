using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Object = UnityEngine.Object;
using System;

public class SelectColor : MonoBehaviour
{
    public Material Watchmaterial;
    public GameObject thisObject;

    public Sprite buttonSprites;
    private GameObject _buttonColor;
    public Color _color;

    private Canvas _canvas;
    private Canvas _canvasAppleWatch;
    private Canvas _canvasDivingWatch;

    private string _colorName;
    private string _tagButtonObject = "WatchButton";
    public bool _enabled;
    private bool _SelectedColorsAppleWatch;
    private bool _SelectedColorsDivingWatch;
    private List<Button> selectColorButtonList = new List<Button>();
    [SerializeField]
    private List<GameObject> selectColorAppleWatchtList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> selectColorDivingWatchList = new List<GameObject>();

    public bool showSelectColorAppleWatch
    {
        get { return _SelectedColorsAppleWatch; }
        set { _SelectedColorsAppleWatch = value;}
    }
    public bool showSelectColorDivingWatch
    {
        get { return _SelectedColorsDivingWatch; }
        set {_SelectedColorsDivingWatch = value;}
    }

    private void Start()
    {
        thisObject.GetComponent<MeshRenderer>().material = Watchmaterial;
        _canvas = GameObject.Find("MainCanvas/ColorButtonCnv").GetComponent<Canvas>();

        foreach (GameObject item in selectColorAppleWatchtList)
        {
            _colorName = item.name;
            setButtonSelectColor(_colorName, _canvasAppleWatch);
        }
        foreach (GameObject item in selectColorDivingWatchList)
        {
            _colorName = item.name;
            setButtonSelectColor(_colorName, _canvasDivingWatch);
        }
        _canvas.gameObject.SetActive(false);
        _canvasAppleWatch.gameObject.SetActive(false);
        _canvasDivingWatch.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_enabled)
        {
            _canvas.gameObject.SetActive(true);
            if (showSelectColorAppleWatch)
            {
                _canvasAppleWatch.gameObject.SetActive(true);
            }
        }
        else
        {
            _canvas.gameObject.SetActive(false);
        }
    }
    public void setButtonSelectColor(string nameColor , Canvas _canvas)
    {
        _buttonColor = DefaultControls.CreateButton(new DefaultControls.Resources());
        _buttonColor.transform.SetParent(_canvas.transform, false);
        _buttonColor.name = nameColor;
        _buttonColor.transform.Find("Text").GetComponent<Text>().text = null;
        _buttonColor.transform.GetComponent<Button>().image.sprite = buttonSprites;
        _buttonColor.transform.GetComponent<Button>().image.type = Image.Type.Simple;
        _buttonColor.transform.GetComponent<Button>().image.preserveAspect = true;
        selectColorButtonList.Add(_buttonColor.GetComponent<Button>());
    }
}
