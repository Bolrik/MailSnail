using MailSnail.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MailSnail.Quests
{
    public class Quest
    {
        public ItemData Type { get; private set; }
        public int BoardID { get; private set; }
    }
}
