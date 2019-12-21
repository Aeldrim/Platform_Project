using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maskGenerator : MonoBehaviour
{
    private float scalingFactor = 20f;
    private float timeScale = 3.5f;
    private Vector3 initialScale;
    private Vector3 finalScale;

    void Start()
    {
        initialScale = transform.localScale;
        finalScale = new Vector3 (  initialScale.x + scalingFactor,
                                    initialScale.y + scalingFactor,
                                    initialScale.z);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
           StartCoroutine("LerpScale");
        }
    }

    IEnumerator LerpScale(){
        float progress = 0;

        while (progress < 1){
            transform.localScale = Vector3.Lerp(initialScale,finalScale,progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
        transform.localScale = finalScale;
    }
}
