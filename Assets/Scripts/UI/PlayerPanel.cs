using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using TopDownPlayer;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] WeaponController weaponController;
    [SerializeField] Player player;

    private UIDocument document;
    private Label ammoLabel;
    private ProgressBar healthBar;

    //[UxmlAttribute,CreateProperty]
    //public string ammotext;

    private void Start()
    {
        document = GetComponent<UIDocument>();
        ammoLabel = document.rootVisualElement.Q("AmmoLabel") as Label;
        healthBar = document.rootVisualElement.Q("HealthBar") as ProgressBar;
        healthBar.dataSource = player;
        ammoLabel.dataSource = weaponController;
    }
}
