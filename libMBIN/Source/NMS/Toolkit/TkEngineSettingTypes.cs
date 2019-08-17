﻿namespace libMBIN.NMS.Toolkit
{
    [NMS( GUID = 0xF859EBED47A81E0A, SubGUID = 0x943946549DEC2216)]
    public class TkEngineSettingTypes : NMSTemplate {

        public enum EngineSettingEnum { FullScreen, Borderless, ResolutionWidth, ResolutionHeight, Monitor, FoVOnFoot, FoVInShip, VSync, GSync, ShadowDetail,
                                        TextureDetail, GenerationDetail, ReflectionsQuality, AntiAliasing, MotionBlurQuality, AnisotropyLevel, Brightness,
                                        AvailableMonitors, FrameRate, NumLowThreads, NumHighThreads, NumGraphicsThreads, TextureStreaming, TexturePageSizeKb,
                                        MotionBlurStrength, ShowRequirementsWarnings, AmbientOcclusion, UseLightshafts, MaxTextureMemoryMb, FixedTextureMemory,
                                        EnableTessellation, UseArbSparseTexture, TerrainQuality, UseTerrainTextureCache, AdapterIndex, UseHDR }
        public EngineSettingEnum EngineSetting;
    }
}
