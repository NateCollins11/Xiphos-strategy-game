using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuText : MonoBehaviour
{

    public float lightingAngle = 4.81f;
    public float illumination = 3;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().fontSharedMaterial.SetFloat("_LightAngle", lightingAngle);
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().fontSharedMaterial.SetFloat("_SpecularPower", illumination);

    }
}
