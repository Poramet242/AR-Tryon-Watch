using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInfoManager : MonoBehaviour
{
    [SerializeField]
    private FingerInfoGizmo fingerInfoGizmo;
    private GameObject fingerInformationGizmo;
    public GameObject debugRing;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        fingerInformationGizmo = GameObject.Find("Finger");
        if (fingerInfoGizmo == null)
        {
            try
            {
                fingerInfoGizmo = GameObject.Find("TryOnManager").GetComponent<FingerInfoGizmo>();
            }
            catch
            {
                messageDebugRing("ShowFingerDisplay");
            }
        }
    }
    private void Update()
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class
           == ManoClass.GRAB_GESTURE)
        {
            ringTryOn();
        }
        else
        {
            messageDebugRing("Can't Show Ring");
        }
    }
    public void ringTryOn()
    {
        fingerInfoGizmo.ShowFingerInformation();
        float centerPositon = 0.5f;
        Vector3 ringPlancement = Vector3.Lerp(fingerInfoGizmo.LeftFingerPoint3DPosition,
                                              fingerInfoGizmo.RightFingerPoint3DPosition,centerPositon);
        fingerInformationGizmo.SetActive(true);
        fingerInformationGizmo.transform.position = ringPlancement;

        Debug.Log(ringPlancement);
        messageDebugRing("ShowRing");
    }
    public void messageDebugRing(string message)
    {
        debugRing.GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }
}
