using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button backButton;

    [Header("Shop Categories Button")]
    public Button characterButton;
    public Button skinButton;
    public Button goldButton;
    public Button zemButton;
    public GameObject highlight;

    [Header("Shop Categories ScrollRect")]
    public GameObject characterScrollRect;
    public GameObject skinScrollRect;
    public GameObject goldScrollRect;   
    public GameObject zemScrollRect;

    private GameObject highlightRect;
    private GameObject highlightButton;

    [Header("Popup")]
    public GameObject buyPopup;
    public GameObject noticePopup;

    public static ShopUIController Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        characterButton.onClick.AddListener(OnCharacterButtonClicked);
        skinButton.onClick.AddListener(OnSkinButtonClicked);
        goldButton.onClick.AddListener(OnGoldButtonClicked);
        zemButton.onClick.AddListener(OnZemButtonClicked);

        Highlight(characterButton, characterScrollRect);
    }

    private void Highlight(Button button, GameObject rect)
    {
        if(highlightButton != null)
        {
            highlightButton.gameObject.GetComponent<TMP_Text>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        if(highlightRect != null)
        {
            highlightRect.SetActive(false);
        }

        button.gameObject.SetActive(true);
        rect.SetActive(true);
            
        highlight.transform.position = new Vector3(button.transform.position.x, highlight.transform.position.y, highlight.transform.position.z);
        button.gameObject.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f);

        highlightButton = button.gameObject;
        highlightRect = rect;
    }

    private void OnZemButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Zem button clicked");
#endif
        Highlight(zemButton, zemScrollRect);
    }

    private void OnGoldButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Gold button clicked");
#endif
        Highlight(goldButton, goldScrollRect);
    }

    private void OnSkinButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Skin button clicked");
#endif
        Highlight(skinButton, skinScrollRect);
    }

    private void OnCharacterButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Character button clicked");
#endif
        Highlight(characterButton, characterScrollRect);
    }

    private void OnBackButtonClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Back button clicked");
#endif
        GameSceneManager.Instance.LoadScene(SceneType.Lobby);
    }

    public void ShowBuyPopup(IEnumerator enumerator)
    {
        buyPopup.SetActive(true);
        buyPopup.GetComponent<ShopBuyUI>().SetOkEnumerator(enumerator);
    }

    public void ShowNoticePopup(string text)
    {
        noticePopup.SetActive(true);
        noticePopup.GetComponent<ShopNoticeUI>().SetText(text);
    }
    public void HideNoticePopup()
    {
        noticePopup.SetActive(false);
    }

    public void HideBuyPopup()
    {
        buyPopup.SetActive(false);
    }
}


