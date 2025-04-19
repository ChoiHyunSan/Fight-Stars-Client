using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button pressToStart;

    private void Start()
    {
        pressToStart.onClick.AddListener(OnPressToStart);
    }

    private void OnPressToStart()
    {
        Debug.Log("Press to Start clicked");

        AuthManager.Instance.SaveLogin();
    }
}
