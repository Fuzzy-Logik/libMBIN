﻿using System.Collections.Generic;

namespace MBINCompiler.Models.Structs
{
    public class GcTeleportEndpoint : NMSTemplate
    {
        /* 0x00 */ public GcUniverseAddressData UniverseAddress;
        /* 0x20 */ public Position;
        /* 0x30 */ public Facing;
        public int TeleporterType;
        p/* 0x40 */ public string[] TeleporterTypeValues()
        {
            return new[] { "Base", "SpaceStation" };
        }
    }
}