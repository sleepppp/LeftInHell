using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.GameData
{
    public static class RecordExtension
    {
        public static Color GetColor(this ColorRecord record)
        {
            return new Color
                (
                    (float)record.R / 255f,
                    (float)record.G / 255f,
                    (float)record.B / 255f,
                    (float)record.A / 255f
                );
        }
    }
}