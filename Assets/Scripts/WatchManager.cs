using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WatchManager : MonoBehaviour
{
    #region Singleton
    private static WatchManager _instance;
    public static WatchManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }
    #endregion
    #region Debug Display Methods
    private TMP_Text wristFlag;
    private Text handSideText;
    private Text manoClassText;
    private Text continuousGestureText;
    private Text leftRightText;
    #endregion

    private WristInfoGizmo wristInfoGizmo;
    private GameObject wristInformationGizmo;
    [SerializeField]
    private GameObject handSideGizmo;
    [SerializeField]
    private GameObject manoClassGizmo;
    [SerializeField]
    private GameObject continuousGestureGizmo;
    [SerializeField]
    private GameObject leftRightGizmo;
    [SerializeField]
    private GameObject smoothingSliderControler;
    [SerializeField]
    private bool _showWristInfo;
    [SerializeField]
    private bool _showGestureAnalysis;
    [SerializeField]
    private bool _gestures;
    [SerializeField]
    private bool _showSmoothingslider;
    private bool skeleton3d;
    private bool showHandSide;
    private Session manomotion_session;

    #region properties
    public bool ShowSkeleton3d
    {
        get { return skeleton3d; }
        set { skeleton3d = value; }
    }
    public bool ShowWristInfo
    {
        get{return _showWristInfo;}
        set{_showWristInfo = value;}
    }
    public bool ShowGestureAnalysis
    {
        get{return _showGestureAnalysis;}
        set{_showGestureAnalysis = value;}
    }
    public bool ShowHandSide
    {
        get{return showHandSide; }
        set{ showHandSide = value;}
    }
    public bool ShowGestures
    {
        get { return _gestures; }
        set { _gestures = value;}
    }
    public bool ShowSmoothingSlider
    {
        get { return _showSmoothingslider; }
        set {_showSmoothingslider = value;}
    }
    #endregion
    private void Awake()
    {
        ShowGestures = true;
        ShowWristInfo = true;
    }
    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        SetGestureDescriptionParts();
        SetFeaturesToCalculate();
        SetFlagDescriptionParts();
        GameObject.Find("Finger").SetActive(false);
    }
    // Updates the GestureInfo, TrackingInfo, Warning and Session every frame.
    private void Update()
    {
        GestureInfo gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        DisplayWristFlags(trackingInfo, gestureInfo);
        DisplayWristInformation();
        DisplayContinuousGestures(gestureInfo.mano_gesture_continuous);
        DisplayHandSide(gestureInfo.hand_side);
        DisplayManoclass(gestureInfo.mano_class);
        DisplayLeftRight(gestureInfo.is_right);
        DisplaySmoothingSlider();
    }
    // Sets which features that should be calculated
    private void SetFeaturesToCalculate()
    {
        ManomotionManager.Instance.ShouldCalculateGestures(ShowGestures);
        ManomotionManager.Instance.ShouldRunWristInfo(ShowWristInfo);
        ManomotionManager.Instance.ShouldCalculateSkeleton3D(ShowSkeleton3d);
    }
    // Initializes the components of the Manoclass,Continuous Gesture and Trigger Ges ture Gizmos
    private void SetGestureDescriptionParts()
    {
        manoClassText = manoClassGizmo.transform.Find("Description").GetComponent<Text>();
        handSideText = handSideGizmo.transform.Find("Description").GetComponent<Text>();
        continuousGestureText = continuousGestureGizmo.transform.Find("Description").GetComponent<Text>();
        leftRightText = leftRightGizmo.transform.Find("Description").GetComponent<Text>();
    }
    // Displays the Wrist Information from WristInfoGizmo for the detected hand if feature is on.
    private void DisplayWristInformation()
    {
        if (wristInfoGizmo == null)
        {
            wristInfoGizmo = GameObject.Find("TryOnManager").GetComponent<WristInfoGizmo>();
            wristInformationGizmo = GameObject.Find("Wrist");
        }
        wristInformationGizmo.SetActive(ShowWristInfo);
        if (ShowWristInfo)
        {
            wristInfoGizmo.ShowWristInformation();
        }
    }
    // Prints out the wrist information error codeif wrist information can´t be calculated correctly.
    private void DisplayWristFlags(TrackingInfo trackingInfo, GestureInfo gestureInfo)
    {
        if (wristFlag != null)
        {
            if (ShowWristInfo && gestureInfo.mano_class != ManoClass.NO_HAND)
            {
                switch (trackingInfo.wristInfo.wristWarning)
                {
                    case 0:
                        wristFlag.text = "";
                        break;
                    case 2000:
                        wristFlag.text = "WRIST ERROR[2000]";
                        break;
                    default:
                        wristFlag.text = "";
                        break;
                }
            }
            else
            {
                wristFlag.text = "";
            }
        }
    }
    // Initialized the text componets for displaying the finger and wrist flags.
    private void SetFlagDescriptionParts()
    {
        try
        {
            wristFlag = GameObject.Find("WristFlag").GetComponent<TMP_Text>();
        }
        catch (Exception ex)
        {
            Debug.Log("Can't find wrist flag TMP_Text.");
            Debug.Log(ex);
        }
    }
    // Displays wich hand side currently facing the camera.
    private void DisplayHandSide(HandSide handside)
    {
        handSideGizmo.SetActive(ShowGestureAnalysis);
        Debug.Log("Calculate Handside");
        if (!handSideGizmo.activeInHierarchy && ShowHandSide)
        {
            handSideGizmo.SetActive(ShowHandSide);
            Debug.Log("Show HandsideGizmo");
        }
        if (ShowGestureAnalysis || ShowHandSide)
        {
            switch (handside)
            {
                case HandSide.Palmside:
                    handSideText.text = "Handside: Palm Side";
                    Debug.Log("Plam Side");
                    break;
                case HandSide.Backside:
                    handSideText.text = "Handside: Back Side";
                    Debug.Log("Back side");
                    break;
                case HandSide.None: 
                    handSideText.text = "Handside: None";
                    break;
            }
        Debug.Log(handside+ " : end if");
        }
    }
    // Displays information regarding the detected manoclass
    private void DisplayManoclass(ManoClass manoclass)
    {
        manoClassGizmo.SetActive(ShowGestureAnalysis);
        if (ShowGestureAnalysis)
        {
            switch (manoclass)
            {
                case ManoClass.NO_HAND:
                    manoClassText.text = "Manoclass: No Hand";
                    break;
                case ManoClass.GRAB_GESTURE:
                    manoClassText.text = "Manoclass: Grab Class";
                    break;
                case ManoClass.PINCH_GESTURE:
                    manoClassText.text = "Manoclass: Pinch Class";
                    break;
                case ManoClass.POINTER_GESTURE:
                    manoClassText.text = "Manoclass: Pointer Class";
                    break;
            }
        }
    }
    //Displays information regarding the detected manoclass
    private void DisplayContinuousGestures(ManoGestureContinuous manoGestureContinuous)
    {
        continuousGestureGizmo.SetActive(ShowGestureAnalysis);
        if (ShowGestureAnalysis)
        {
            switch (manoGestureContinuous)
            {
                case ManoGestureContinuous.CLOSED_HAND_GESTURE:
                    continuousGestureText.text = "Continuous: Closed Hand";
                    break;
                case ManoGestureContinuous.OPEN_HAND_GESTURE:
                    continuousGestureText.text = "Continuous: Open Hand";
                    break;
                case ManoGestureContinuous.HOLD_GESTURE:
                    continuousGestureText.text = "Continuous: Hold";
                    break;
                case ManoGestureContinuous.OPEN_PINCH_GESTURE:
                    continuousGestureText.text = "Continuous: Open Pinch";
                    break;
                case ManoGestureContinuous.POINTER_GESTURE:
                    continuousGestureText.text = "Continuous: Pointing";
                    break;
                case ManoGestureContinuous.NO_GESTURE:
                    continuousGestureText.text = "Continuous: None";
                    break;
            }
        }
    }
    // Displays wich hand currently facing the camera.
    private void DisplayLeftRight(int isRight)
    {
        leftRightGizmo.SetActive(ShowGestureAnalysis);

        if (ShowGestureAnalysis)
        {
            switch (isRight)
            {
                case 0:
                    leftRightText.text = "Hand: Left";
                    break;
                case 1:
                    leftRightText.text = "Hand: Right";
                    break;
                default:
                    leftRightText.text = "Hand: None";
                    break;
            }
        }
    }
    //Display smooting Object
    private void DisplaySmoothingSlider()
    {
        smoothingSliderControler.SetActive(_showSmoothingslider);
    }
}
