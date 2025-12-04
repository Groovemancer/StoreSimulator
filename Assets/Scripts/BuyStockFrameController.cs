using TMPro;
using UnityEngine;

public class BuyStockFrameController : MonoBehaviour
{
    public StockInfo info;

    public TMP_Text nameText, priceText, amountInBoxText, boxPriceText, buttonText;

    public StockBoxController boxToSpawn;

    private float boxCost = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateFrameInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFrameInfo()
    {
        info = StockInfoController.instance.GetInfo(info.name);

        nameText.text = info.name;

        float priceCoverted = CurrencyManager.Instance.ConvertMoney(info.price);

        priceText.text = CurrencyManager.Instance.GetCurrencySymbol() + priceCoverted.ToString("F2");

        int boxAmount = boxToSpawn.GetStockAmount(info.typeOfStock);
        amountInBoxText.text = boxAmount.ToString() + " per box";

        boxCost = boxAmount * info.price;
        float boxCostDisplay = boxAmount * priceCoverted;
        boxPriceText.text = "Box: " + CurrencyManager.Instance.GetCurrencySymbol() + boxCostDisplay.ToString("F2");

        buttonText.text = "PAY: " + CurrencyManager.Instance.GetCurrencySymbol() + boxCostDisplay.ToString("F2");
    }

    public void BuyBox()
    {
        if (StoreController.instance.CheckMoneyAvailable(boxCost) == true)
        {
            StoreController.instance.SpendMoney(boxCost);

            Instantiate(boxToSpawn, StoreController.instance.stockSpawnPoint.position, Quaternion.identity).SetupBox(info);

        }
    }
}
