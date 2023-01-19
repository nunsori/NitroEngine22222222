using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circle_gauge_contorller : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform circle_gauge_transform;

    public New_Kart_Controller new_Kart_Controller;

    public Image booster_gauge_image;

    public Text booster_text;

    float booster_gauge = 0f;
    private bool booster_slider_plus_cor = false;

    public int booster = 0;

    void Start()
    {
        gameObject.transform.position = circle_gauge_transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        booster_gauge_image.fillAmount = 0.3f + booster_gauge;

        if(booster_gauge_image.fillAmount >= 0.7f && new_Kart_Controller.isdrift == false)
        {
            booster_gauge = 0;
            booster_gauge_image.fillAmount = 0.3f;
            if (booster < 2)
            {
                booster++;
            }
            
        }

        booster_text.text = booster.ToString();
    }

    private void FixedUpdate()
    {
        if (new_Kart_Controller.isdrift)
        {
            if (!booster_slider_plus_cor)
            {
                StartCoroutine("booster_slider_plus");
            }
        }
    }

    private IEnumerator booster_slider_plus()
    {
        Debug.Log("on");
        booster_slider_plus_cor = true;
        yield return new WaitForSeconds(0.01f);
        //booster_gauge += Time.deltaTime;
        if (booster_gauge_image.fillAmount <= 0.7f)
        {
            booster_gauge += Time.deltaTime;
        }
        StopCoroutine("booster_slider_plus");
        booster_slider_plus_cor = false;
    }

}
