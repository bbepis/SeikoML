using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RoR2.UI
{
    class patch_ItemIcon : ItemIcon
    {
        // Redefine access level
        public ItemIndex ItemIndex
        {
            get
            {
                return itemIndex;
            }
        }
    }
}
