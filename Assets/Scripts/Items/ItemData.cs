using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MailSnail.Items
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Data/Item/new Item Data")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    //    None,
    //    Package,
    //    Mail
    }
}
