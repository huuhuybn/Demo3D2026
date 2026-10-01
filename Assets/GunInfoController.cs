using TMPro;
using UnityEngine;

public class GunInfoController : MonoBehaviour
{
    
    public TextMeshProUGUI GunInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateGunInfo(string dam,string hp)
    {
        GunInfo.text = "Damage :  " + dam;
        GunInfo.text += "\nHP :  " + hp;
    }
}
