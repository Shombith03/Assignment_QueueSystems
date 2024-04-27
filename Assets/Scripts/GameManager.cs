using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _currencyAmount = 0;
    private const string CURRENCY_KEY = "CURRENCY";
    [SerializeField]
    private Text _currencyText, _roomUnlockedText;

    [SerializeField]
    private RoomData[] _roomData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GetCurrency();
        ShowCurrency();
    }

    private void GetCurrency()
    {
        if (PlayerPrefs.HasKey(CURRENCY_KEY))
        {
            _currencyAmount = PlayerPrefs.GetInt(CURRENCY_KEY);
        }
        else
        {
            PlayerPrefs.SetInt(CURRENCY_KEY, _currencyAmount);
        }
    }

    public void IncrementCurrency(int amount)
    {
        _currencyAmount += amount;
        StartCoroutine(CheckIfRoomUnlocked());
        SaveCurrency();
        ShowCurrency();
    }

    public int GetCurrencyAmount()
    {
        return _currencyAmount;
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CURRENCY_KEY, _currencyAmount);
    }

    private void ShowCurrency()
    {
        _currencyText.text = _currencyAmount.ToString(); 
    }

    private IEnumerator CheckIfRoomUnlocked()
    {
        if(_currencyAmount >= 90)
        {
            _roomUnlockedText.gameObject.SetActive(true);
            _roomUnlockedText.text = "ROOM 2 is now unlocked";
            yield return new WaitForSeconds(1f);
            _roomUnlockedText.gameObject.SetActive(false);
            foreach (var room in _roomData)
            {
                if(room.isLocked)
                {
                    room.UnlockRoom();
                }
            }
        }
        else
        {
            yield return null;
        }
    }

}
