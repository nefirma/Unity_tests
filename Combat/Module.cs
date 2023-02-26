using UnityEngine;

public class Module : MonoBehaviour
{
    public string moduleName;
    public int shield = 0;
    public int health = 0;
    public float reloadTimeMod = 1;
    public float shieldReloadMod = 1;
    
    //В инспекторе Из списка выбираем тип модуля
    public enum ModuleType
    {
        Shield,
        Health,
        ReloadTime,
        ShieldReload
    }

    [SerializeField] public ModuleType moduleType;

}

