using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
public class ShowDebugTryonManager : MonoBehaviour
{
    [SerializeField]
    private WatchManager watchManager;
    [SerializeField]
    private GameObject tutorialObj;
    [SerializeField]
    private GameObject ObjectStart;
    
    public Button showDebug;
    public Button closeDebug;
    public Button selected;
    public Button unSelect;


    [SerializeField]
    private bool _showtutorial;
    public bool Showtutorial
    {
        get { return _showtutorial; }
        set { _showtutorial = value; }
    }
    private void Start()
    {
        watchManager = GameObject.Find("WatchManager").GetComponent<WatchManager>();
    }
    private void Update()
    {
        showDebug.onClick.AddListener(() => 
        { 
            watchManager.ShowGestureAnalysis = true;
            watchManager.ShowSmoothingSlider = true;
            showDebug.gameObject.SetActive(false);
            closeDebug.gameObject.SetActive(true);
        });
        closeDebug.onClick.AddListener(() => 
        { 
            watchManager.ShowGestureAnalysis = false;
            watchManager.ShowSmoothingSlider = false;
            showDebug.gameObject.SetActive(true);
            closeDebug.gameObject.SetActive(false);
        });
        selected.onClick.AddListener(() => 
        {
            ObjectStart.SetActive(false);
            selected.gameObject.SetActive(false);
            unSelect.gameObject.SetActive(true);
        });
        unSelect.onClick.AddListener(() => 
        {
            ObjectStart.SetActive(true);
            selected.gameObject.SetActive(true);
            unSelect.gameObject.SetActive(false);
        });
        if (Showtutorial == true)
        {
            tutorialObj.SetActive(true);
            ObjectStart.SetActive(false);
            Debug.Log("Show tutorial");
        }
        else if(Showtutorial == false)
        {
            tutorialObj.SetActive(false);
            Debug.Log("Can't show tutorial");
        }
    }
}
