﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsProto
{
    internal class SpanishResourceManagerFactory : IResourceManagerFactory
    {
        public System.Resources.ResourceManager CreateResourceManager()
        {
            return new System.Resources.ResourceManager("BattleshipsProto.Resources.Strings_es", 
                Assembly.GetExecutingAssembly());
        }
    }
}