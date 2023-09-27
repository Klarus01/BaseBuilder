using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public int ID;
    public string displayName;
    public Sprite sprite;
    public int maxStrackSize = 8;
}
