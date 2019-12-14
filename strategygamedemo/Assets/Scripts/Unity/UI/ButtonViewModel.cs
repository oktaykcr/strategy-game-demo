using UnityEngine;
using UnityEngine.UI;

public class ButtonViewModel : MonoBehaviour, IButton
{
    
    void Start()
    {
        var button = transform.GetComponent<Button>();
        button.onClick.AddListener(delegate () { this.ButtonClicked(); });
    }

    public void ButtonClicked()
    {
        if (!GameBoardViewModel.Instance.IsProductSelected())
        {
            var buttonName = gameObject.name;
            GameObject selectedProduct = null;
            IProduct product;
            if (buttonName.Contains("Barrack"))
            {
                product = new Barrack("Barrack", ProductType.Building, true, 0.5f);
                selectedProduct = ViewFactory.Create<IProduct, BuildingViewModel>(product, Resources.Load<GameObject>("Prefabs/Barrack"), null);
                selectedProduct.name = "Barrack_Template";
            }

            if (buttonName.Contains("PowerPlant"))
            {
                product = new PowerPlant("Power Plant", ProductType.Building, true, 0.5f);
                selectedProduct = ViewFactory.Create<IProduct, BuildingViewModel>(product, Resources.Load<GameObject>("Prefabs/PowerPlant"), null);
                selectedProduct.name = "PowerPlant_Template";
            }

            GameBoardViewModel.Instance.SelectProduct(selectedProduct);
        }
    }
}
