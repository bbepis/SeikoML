using System;
using System.Collections.Generic;
using System.Text;
using MonoMod;
using RoR2;
using SeikoML;

namespace RoR2
{
    [MonoModAdded]
    class ModDummy
    {
        [SystemInitializer(new Type[]
        {

        })]
        private static void Init()
        {
            ModLoader.Begin();
        }
    }
}
