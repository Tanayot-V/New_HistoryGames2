using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TransparencyCaptureToFile:MonoBehaviour
{
    public int num = 0;
    public string name;
   
    public IEnumerator capture()
    {

        yield return new WaitForEndOfFrame();
        //After Unity4,you have to do this function after WaitForEndOfFrame in Coroutine
        //Or you will get the error:"ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame"
        zzTransparencyCapture.captureScreenshot(name + num+".png");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Push();
        }
    }

    public void Push()
    {
        num += 1;
        StartCoroutine(capture());
        Debug.Log(name + num + ".png");

    }
}