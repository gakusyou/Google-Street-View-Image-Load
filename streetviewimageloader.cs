using UnityEngine;
using System.Collections;

public class StreetViewImageLoader : MonoBehaviour
{
    public Material material;
    private double heading = 0.0;
    private double pitch = 0.0;

    private int width = 2048;
    private int height = 2048;

    private double longitude = 140;
    private double latitude = 36;
    private Texture2D frontTex, leftTex, rightTex, backTex, upTex, downTex;
    private string key = "YOUR_KEY";

    // Use this for initialization
    void Start()
    {
        StreetView(latitude, longitude, pitch, heading);
    }

    public void StreetView(double latitude, double longitude, double pitch, double heading)
    {
        StartCoroutine(GetStreetViewImage(latitude, longitude, 0, pitch));
        StartCoroutine(GetStreetViewImage(latitude, longitude, 90, pitch));
        StartCoroutine(GetStreetViewImage(latitude, longitude, 180, pitch));
        StartCoroutine(GetStreetViewImage(latitude, longitude, 270, pitch));
        StartCoroutine(GetStreetViewImage(latitude, longitude, heading, 90));
        StartCoroutine(GetStreetViewImage(latitude, longitude, heading, -90));
        StartCoroutine(WaitTime());
    }

    private Material SetMaterial()
    {
        material.SetTexture("_FrontTex", frontTex);
        material.SetTexture("_BackTex", backTex);
        material.SetTexture("_LeftTex", leftTex);
        material.SetTexture("_RightTex", rightTex);
        material.SetTexture("_UpTex", upTex);
        material.SetTexture("_DownTex", downTex);

        return material;
    }
    private IEnumerator WaitTime()
    {
        yield return null;
        Material material = SetMaterial();
        SetSkyboxMaterial(material);
    }

    private IEnumerator GetStreetViewImage(double latitude, double longitude, double heading, double pitch)
    {
        string url = "http://maps.googleapis.com/maps/api/streetview?" + "size=" + width + "x" + height + "&key=" + key + "&location=" + latitude + "," + longitude + "&heading=" + heading + "&pitch=" + pitch + "&sensor=false";
        WWW www = new WWW(url);
        yield return www;

        switch ((int)heading)
        {
            case 0:
                switch ((int)pitch)
                {
                    case 0:
                        frontTex = GetTheTexture(www.texture);
                        break;
                    case 90:
                        upTex = GetTheTexture(www.texture);
                        break;
                    case -90:
                        downTex = GetTheTexture(www.texture);
                        break;
                }
                break;
            case 90:
                leftTex = GetTheTexture(www.texture);
                break;
            case 180:
                backTex = GetTheTexture(www.texture);
                break;
            case 270:
                rightTex = GetTheTexture(www.texture);
                break;
        }
    }

    private Texture2D GetTheTexture(Texture2D _streetViewTex)
    {
        var _skyBoxTex = _streetViewTex;
        _skyBoxTex.wrapMode = TextureWrapMode.Clamp;
        return _skyBoxTex;
    }

    private void SetSkyboxMaterial(Material _material)
    {
        RenderSettings.skybox = _material;
    }
}