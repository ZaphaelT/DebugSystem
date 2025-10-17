using UnityEngine;
using TMPro;

public class AddMoney : MonoBehaviour
{
    public static AddMoney Instance { get; private set; }
    public TextMeshProUGUI moneyText;
    public int moneyAmount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateMoneyText();
    }

    void Update()
    {

    }

    public void AddIncome(int amount)
    {
        moneyAmount += amount;
        UpdateMoneyText();
    }

    // Próba wydania okreœlonej iloœci monet.
    // Zwraca true jeœli operacja siê powiod³a (i zaktualizowano UI), false jeœli brak œrodków.
    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;
        if (moneyAmount >= amount)
        {
            moneyAmount -= amount;
            UpdateMoneyText();
            return true;
        }
        return false;
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = "Money: " + moneyAmount.ToString();
    }
}
