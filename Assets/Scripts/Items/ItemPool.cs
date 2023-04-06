using MailSnail.Items;
using UnityEngine;

namespace MailSnail.Items
{
    [CreateAssetMenu(fileName = "Item Pool", menuName = "Pool/Item/new Item Pool")]
    public class ItemPool : ScriptableObject
    {
        [field: SerializeField] public ItemData Package { get; private set; }
        [field: SerializeField] public ItemData Mail { get; private set; }
    }


}