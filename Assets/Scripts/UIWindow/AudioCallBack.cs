using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCallBack : MonoBehaviour {

    public void PlayItemAudio()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.FBItem);
    }

    public void PlayWinAduio()
    {
        AudioSvc.Instance.PlayUIAudio(Constants.FBWin);
    }
}
