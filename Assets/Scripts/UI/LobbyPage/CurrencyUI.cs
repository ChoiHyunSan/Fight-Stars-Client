using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    private TMP_Text goldText;
    private TMP_Text gemText;

    void Start()
    {
        goldText = transform.Find("Gold/Text_Value")?.GetComponent<TMP_Text>();
        gemText = transform.Find("Gem/Text_Value")?.GetComponent<TMP_Text>();

#if UNITY_EDITOR
        if (goldText == null || gemText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector.");
            return;
        }
        
        if (UserDataManager.Instance == null || UserDataManager.Instance._userInfo == null)
        {
            Debug.LogError("UserDataManager or UserInfo is null");
            return;
        }
#endif
        UserDataManager.Instance._userInfo.OnUserInfoChanged += RefreshUI;
        RefreshUI();
    }

    private void RefreshUI()
    {
        goldText.text = $"{UserDataManager.Instance._userInfo.currency.gold}";  
        gemText.text = $"{UserDataManager.Instance._userInfo.currency.gems}";
    }
}
