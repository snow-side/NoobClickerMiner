using UnityEngine;
using UnityEngine.UI;

public class IconAvailable : MonoBehaviour
{
    [SerializeField]
    Image Available;

    [SerializeField]
    EntityUpdateType Type;

    void Awake() => GameEvents.EntityUpdated.AddListener(_Update);

    void _Update(EntityUpdateType type, bool val)
    {
        if (Type != type)
            return;

        if (val)
            Available.color = Color.green;
        else
            Available.color = Color.red;
    }
}
