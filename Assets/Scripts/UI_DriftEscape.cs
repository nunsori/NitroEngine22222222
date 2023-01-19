using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DriftEscape : MonoBehaviour
{

    public Slider slider;

    private void Awake() {
        slider.value = 1;
    }

    public void setMaxvalue(float value){
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetGuage(float value){
        slider.value = value;
    }
}
