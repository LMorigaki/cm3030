using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates thumbnail image from 3D model in scene<br/>
/// Place 3D model as child of object "target" and Play<br/>
/// It may takes a few seconds for image to appear in asset database<br/>
/// after playing
/// </summary>
public class CreateThumbnail : MonoBehaviour
{
    public Camera cam;
    public RenderTexture renderTexture;
    public GameObject target;
    public string directory, fileName;

    // Start is called before the first frame update
    void Start()
    {
        string[] vs = target.transform.GetChild(0).name.Split(' ');
        StartCoroutine(TakePhoto(vs[0] + " img " + vs[1]));
    }

    IEnumerator TakePhoto(string filename)
    {
        yield return new WaitForEndOfFrame();

        if (cam != null && renderTexture != null)
        {
            cam.targetTexture = renderTexture;
        }
        else if (cam.targetTexture != null)
        {
            renderTexture = cam.targetTexture;
        }

        Texture2D tex = new Texture2D(512, 512);
        RenderTexture.active = renderTexture;

        tex.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        tex.Apply();

        string dir = System.IO.Path.GetDirectoryName(directory);
        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(directory + "/" + filename + ".png", bytes);
        Debug.Log(filename + " OK");
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
