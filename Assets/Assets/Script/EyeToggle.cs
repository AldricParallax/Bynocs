using System.Collections;
using UnityEngine;

public class EyeToggle : MonoBehaviour
{
    public static EyeToggle instance;
    Material mat;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }



    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    public void UpdateEye(int bLeftEyeEnable)
    {
        if(mat == null)
            mat = GetComponent<MeshRenderer>().material;

        switch (bLeftEyeEnable)
        {
            case 1:
                mat.SetColor("_LeftEyeColor", new Color(0, 0, 0, 1));
                mat.SetColor("_RightEyeColor", new Color(0, 0, 0, 0));
                break;
            case 0:
                mat.SetColor("_LeftEyeColor", new Color(0, 0, 0, 0));
                mat.SetColor("_RightEyeColor", new Color(0, 0, 0, 1));
                break;
            default:
                mat.SetColor("_LeftEyeColor", new Color(0, 0, 0, 0));
                mat.SetColor("_RightEyeColor", new Color(0, 0, 0, 0));
                break;
        }
    }
    // New method to fade in/out eyes
    public void StartEyeFade(int bLeftEyeEnable, float targetAlpha, float duration)
    {
        string eyeProperty = (bLeftEyeEnable == 1) ? "_LeftEyeColor" : "_RightEyeColor";
        StartCoroutine(FadeEye(eyeProperty, targetAlpha, duration));
    }

    private IEnumerator FadeEye(string property, float targetAlpha, float duration)
    {
        if (mat == null || !mat.HasProperty(property))
        {
            Debug.LogError($"Shader property {property} not found!");
            yield break;
        }

        Color startColor = mat.GetColor(property);
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetAlpha, elapsedTime / duration);
            mat.SetColor(property, new Color(startColor.r, startColor.g, startColor.b, alpha));
            yield return null;
        }

        mat.SetColor(property, targetColor);
    }
    public void StartEyeFadeLoop(int bLeftEyeEnable, float minAlpha, float maxAlpha, float duration)
    {
        string eyeProperty = (bLeftEyeEnable == 1) ? "_LeftEyeColor" : "_RightEyeColor";
        StartCoroutine(FadeEyeLoop(eyeProperty, minAlpha, maxAlpha, duration));
    }

    private IEnumerator FadeEyeLoop(string property, float minAlpha, float maxAlpha, float duration)
    {
        if (mat == null || !mat.HasProperty(property))
        {
            Debug.LogError($"Shader property {property} not found!");
            yield break;
        }

        
        
        yield return StartCoroutine(FadeEye(property, maxAlpha, duration)); // Fade in
        yield return new WaitForSeconds(0.5f); // Wait for a brief period
        yield return StartCoroutine(FadeEye(property, minAlpha, duration)); // Fade out
        
    }
}
