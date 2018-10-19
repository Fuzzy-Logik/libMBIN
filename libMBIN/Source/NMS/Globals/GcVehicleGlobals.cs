using libMBIN.NMS.GameComponents;

namespace libMBIN.NMS.Globals {

    [NMS( GUID = 0x422A6825233203FC )]
    public class GcVehicleGlobals : GlobalDataStruct {

        [NMS(Size = 0x10)]
		/* 0x000 */ public string VehicleStrongLaser;
		/* 0x010 */ public bool   ShowAllCheckpoints;
		/* 0x014 */ public float  TestFrictionStat;
		/* 0x018 */ public float  TestSkidFrictionStat;
		/* 0x01C */ public bool   TestAnims;
		/* 0x020 */ public float  TestAnimThrust;
		/* 0x024 */ public float  TestAnimBoost;
		/* 0x028 */ public float  TestAnimTurn;
		/* 0x030 */ public Colour DefaultBoosterColour;
		/* 0x040 */ public float  WheelDustColourLightFactor;
		/* 0x044 */ public float  VehicleBoostSpeedMultiplierPercent;
		/* 0x048 */ public float  VehicleGarageHologramMinFadeRange;
		/* 0x04C */ public float  VehicleGarageHologramFadeRange;
		/* 0x050 */ public float  CheckpointPlacementOffset;
		/* 0x054 */ public float  CheckpointPlacementRayLength;
		/* 0x058 */ public float  CheckpointDeleteDistance;
		/* 0x05C */ public float  CheckpointDeleteAngle;
		/* 0x060 */ public float  BoostPadTime;
		/* 0x064 */ public float  BoostPadStrength;
		/* 0x068 */ public float  DisablePhysicsRange;
		/* 0x06C */ public float  RaceMultipleStartOffset;
		/* 0x070 */ public float  RaceMultipleStartCaptureRange;
		/* 0x074 */ public float  MiningLaserDrainSpeed;
		/* 0x078 */ public float  ProjectileDrainPerShot;
		/* 0x07C */ public float  RaceStartSpawnUpOffset;
		/* 0x080 */ public float  RaceInteractRespawnOffset;
		/* 0x084 */ public float  RaceInteractRespawnUpOffset;
		/* 0x088 */ public float  StickTurnReducer;
		/* 0x08C */ public float  TravelSpeedReportReducer;
		/* 0x090 */ public float  HornScareRadius;
		/* 0x094 */ public float  HornScareFleeRadius;
		/* 0x098 */ public float  HornScareTime;
		/* 0x09C */ public float  AttractAmount;
		/* 0x0A0 */ public float  AttractMaxSpeed;
		/* 0x0A4 */ public float  AttractAlign;
		/* 0x0A8 */ public float  AttractDirectionBrakeThresholdSq;
		/* 0x0AC */ public float  ExitStopTime;
		/* 0x0B0 */ public float  ExitStopForce;
		/* 0x0B4 */ public float  GunFireRate;
		/* 0x0B8 */ public float  MinTurretAngle;
		/* 0x0BC */ public float  SummoningRange;
		/* 0x0C0 */ public float  CheckpointRadius;
		/* 0x0C4 */ public float  RaceCooldown;
		/* 0x0C8 */ public float  RaceResetFlashDuration;
		/* 0x0CC */ public float  RaceResetFlashIntensity;
		/* 0x0D0 */ public float  CheckpointFlashDuration;
		/* 0x0D4 */ public float  CheckpointFlashIntensity;
		/* 0x0E0 */ public Colour CheckpointBeamColourActive;
        /* 0x0F0 */ public Colour CheckpointBeamColourNormal;
        [NMS(Size = 0x10)]
        /* 0x100 */ public string DefaultBuggyLoadout;
        [NMS(Size = 0x2)]
        /* 0x110 */ public NMSString0x10[] DefaultBikeLoadout;
        [NMS(Size = 0x2)]
        /* 0x130 */ public NMSString0x10[] DefaultTruckLoadout;
        /* 0x150 */ public float  CheckpointBeamSizeNormal;
		/* 0x154 */ public float  CheckpointBeamSizeActive;
		/* 0x158 */ public float  CheckpointBeamOffset;
		/* 0x15C */ public float  SpawnRotation;
		/* 0x160 */ public float  MiningLaserRadius;
		/* 0x164 */ public int    MiningLaserMiningDamage;
		/* 0x168 */ public int    MiningLaserDamage;
		/* 0x16C */ public float  MiningLaserSpeed;
		/* 0x170 */ public int    GunBaseDamage;
		/* 0x174 */ public int    GunBaseMiningDamage;
		/* 0x178 */ public float  ResourceCollectOffset;
		/* 0x17C */ public float  VehicleWheelNoise;
		/* 0x180 */ public float  VehicleWheelNoiseScale;
		/* 0x184 */ public float  WheelSideVerticalFactor;
		/* 0x188 */ public float  SuspensionDamping;
		/* 0x18C */ public float  SuspensionDampingAngularFactor;
		/* 0x190 */ public bool   ShowTempVehicleMesh;
		/* 0x191 */ public bool   ShowVehicleDebugging;
		/* 0x192 */ public bool   ShowVehicleWheelGuards;
		/* 0x193 */ public bool   ShowVehicleText;
		/* 0x194 */ public float  VehicleTextSize;
		/* 0x198 */ public bool   ShowVehicleParticleDebug;
		/* 0x199 */ public bool   VehicleDrawAudioDebug;
		/* 0x19C */ public float  VehicleSuspensionAudioSpacing;
		/* 0x1A0 */ public float  VehicleSuspensionAudioDelay;
		/* 0x1A4 */ public float  VehicleSuspensionAudioScale;
		/* 0x1A8 */ public float  VehicleSuspensionAudioTrigger;
		/* 0x1AC */ public float  VehicleFadeTime;
		/* 0x1B0 */ public float  VehicleMinSummonDistance;
		/* 0x1B4 */ public float  VehicleDeactivateRange;
		/* 0x1B8 */ public float  VehicleBoostFuelRate;
		/* 0x1BC */ public float  VehicleBoostFuelRateSurvival;
		/* 0x1C0 */ public float  VehicleFuelRate;
		/* 0x1C4 */ public float  VehicleFuelRateSurvival;
		/* 0x1C8 */ public float  VehicleFuelRateTruckMultiplier;
		/* 0x1CC */ public float  VehicleJumpTimeMin;
		/* 0x1D0 */ public float  VehicleJumpTimeMax;
		/* 0x1D4 */ public float  VehicleJumpCooldown;
		/* 0x1D8 */ public float  WheelForceHalflife;
		/* 0x1DC */ public bool   ThrottleButtonCamRelative;
		/* 0x1E0 */ public float  StickReverseTurnThreshold;
		/* 0x1E4 */ public float  StickReverseTurnStiffness;
        /* 0x1E8 */ public GcScanData VehicleScan;
        /* 0x1FC */ public GcScanData VehicleLocalScan;
        [NMS(Size = 0x5, EnumValue = new[] { "Bike", "Buggy", "Truck", "WheeledBike", "Hovercraft" })]
        /* 0x210 */ public GcVehicleData[] VehicleDataTable;

    }

}
