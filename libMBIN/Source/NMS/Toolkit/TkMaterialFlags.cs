﻿using libMBIN.NMS.Toolkit;
using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Toolkit
{
	[NMS(GUID = 0x9E223CF16E9CD222)]
    public class TkMaterialFlags : ToolkitComponent {

		public enum MaterialFlagEnum { _F01_DIFFUSEMAP, _F02_SKINNED, _F03_NORMALMAP, _F04_ENVMAP, _F05_, _F06_, _F07_UNLIT, _F08_, _F09_TRANSPARENT, _F10_NORECEIVESHADOW, _F11_ALPHACUTOUT, _F12_BATCHED_BILLBOARD, _F13_UVANIMATION, _F14_UVSCROLL, _F15_WIND, _F16_DIFFUSE2MAP, _F17_MULTIPLYDIFFUSE2MAP, _F18_UVTILES, _F19_BILLBOARD, _F20_PARALLAXMAP, _F21_VERTEXCOLOUR, _F22_TRANSPARENT_SCALAR, _F23_CAMERA_RELATIVE, _F24_AOMAP, _F25_ROUGHNESS_MASK, _F26_STRETCHY_PARTICLE, _F27_VBTANGENT, _F28_VBSKINNED, _F29_VBCOLOUR, _F30_REFRACTION_MAP, _F31_DISPLACEMENT, _F32_LEAF, _F33_GRASS, _F34_GLOW, _F35_GLOW_MASK, _F36_DOUBLESIDED, _F37_RECOLOUR, _F38_NO_DEFORM, _F39_METALLIC_MASK, _F40_SUBSURFACE_MASK, _F41_DETAIL_DIFFUSE, _F42_DETAIL_NORMAL, _F43_NORMAL_TILING, _F44_IMPOSTER, _F45_SCANABLE, _F46_BILLBOARD_AT, _F47_WRITE_LOG_Z, _F48_WARPED_DIFFUSE_LIGHTING, _F49_DISABLE_AMBIENT, _F50_DISABLE_POSTPROCESS, _F51_DECAL_DIFFUSE, _F52_DECAL_NORMAL, _F53_COLOURISABLE, _F54_COLOURMASK, _F55_MULTITEXTURE, _F56_MATCH_GROUND, _F57_DETAIL_OVERLAY, _F58_USE_CENTRAL_NORMAL, _F59_SCREENSPACE_FADE, _F60_ACUTE_ANGLE_FADE, _F61_CLAMP_AMBIENT, _F62_DETAIL_ALPHACUTOUT, _F63_DISSOLVE, _F64_ }
		public MaterialFlagEnum MaterialFlag;
    }
}
