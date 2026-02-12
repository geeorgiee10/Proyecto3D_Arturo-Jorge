using UnityEngine;

[CreateAssetMenu(menuName = "QTE/QTEPattern")]
public class QTEPattern : ScriptableObject
{
    public QTEInput[] sequence;
}

[System.Serializable]
public class QTEInput
{
    public QTEKey key;
    public QTEType type;
}
