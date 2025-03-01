﻿using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace Astrum.AstralCore.Types
{
    public class Player
    {
        public static readonly Type Type;

        private static readonly PropertyInfo m_APIUser;
        private static readonly PropertyInfo m_VRCPlayerApi;

        static Player()
        {
            Type = Hooks.Hooks.AssemblyCSharp.GetExportedTypes()
                .Where(f => f.Namespace == "VRC")
                .OrderByDescending(f => f.GetProperties().Count(f1 => f1.PropertyType == typeof(bool)))
                .FirstOrDefault();

            m_APIUser = Type.GetProperties().FirstOrDefault(f => f.PropertyType == typeof(APIUser));
            m_VRCPlayerApi = Type.GetProperties().FirstOrDefault(f => f.PropertyType == typeof(VRCPlayerApi));
        }

        public readonly MonoBehaviour Inner;
        public readonly APIUser APIUser;
        public readonly VRCPlayerApi VRCPlayerApi;

        public Player(MonoBehaviour inner)
        {
            Inner = inner;
            APIUser = m_APIUser?.GetValue(inner) as APIUser;
            VRCPlayerApi = m_VRCPlayerApi?.GetValue(inner) as VRCPlayerApi;
        }

        // someone should probably test these
        public override int GetHashCode() => this?.Inner.GetHashCode() ?? 0;
        public override bool Equals(object obj) => this?.Inner.Equals(obj) ?? obj is null;
        public static bool operator == (Player self, Player other) => ReferenceEquals(self?.Inner, other?.Inner);
        public static bool operator != (Player self, Player other) => !(self == other);
    }
}
