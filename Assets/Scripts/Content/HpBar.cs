using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image fillImage;
    private Transform _camera;
    public TMP_Text _nameText;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        // 카메라를 향하게 (Billboard 처리)
        transform.forward = _camera.forward;

        this.GetComponent<Canvas>().sortingOrder = 2;
    }

    public void SetRatio(float ratio)
    {
        fillImage.fillAmount = Mathf.Clamp01(ratio);
    }

    public void SetNickname(string nickname)
    {
        _nameText.text = nickname;
    }
}
