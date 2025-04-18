using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] WeaponController weaponController;

    private UIDocument document;
    private Label ammoLabel;

    //[UxmlAttribute,CreateProperty]
    //public string ammotext;

    private void Start()
    {
        document = GetComponent<UIDocument>();
        ammoLabel = document.rootVisualElement.Q("AmmoLabel") as Label;
        ammoLabel.dataSource = weaponController;
    }
}
