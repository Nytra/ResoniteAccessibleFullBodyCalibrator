using HarmonyLib;
using ResoniteModLoader;
using FrooxEngine;
using System.Reflection;
using Elements.Core;
using System;

namespace AccessibleFullBodyCalibrator
{
	public class AccessibleFullBodyCalibrator : ResoniteMod
	{
		public override string Name => "AccessibleFullBodyCalibrator";
		public override string Author => "Nytra";
		public override string Version => "1.0.0";
		public override string Link => "https://github.com/Nytra/ResoniteAccessibleFullBodyCalibrator";
		public override void OnEngineInit()
		{
			Harmony harmony = new Harmony("owo.Nytra.AccessibleFullBodyCalibrator");
			harmony.PatchAll();
		}

		private static Type trackerType = AccessTools.TypeByName("FrooxEngine.FullBodyCalibrator+Tracker");

		[HarmonyPatch(typeof(FullBodyCalibratorDialog), "OnStartCalibration")]
		class AccessibleFullBodyCalibratorPatch
		{
			public static void Postfix(FullBodyCalibratorDialog __instance, SyncRef<FullBodyCalibrator> ____calibrator)
			{
				Slot s = __instance.Slot.AddSlot("Button");
				LegacyButton b = s.AttachComponent<LegacyButton>();
				b.LocalPressed += (btn, data) =>
				{
					typeof(FullBodyCalibrator).GetMethod("set_CalibratingPose", BindingFlags.Public | BindingFlags.Instance).Invoke(____calibrator.Target, new object[] { false });
					typeof(FullBodyCalibrator).GetMethod("MapTrackers", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(____calibrator.Target, new object[] { });
					//__instance.RunSynchronously(() => 
					//{
					//	var trackers = (ISyncList)____calibrator.Target.GetSyncMember("_trackers");
					//	if (trackers == null)
					//	{
					//		Debug("trackers is null!");
					//		return;
					//	}
					//	Debug($"trackers count: {trackers.Count}");
					//	foreach (var tracker in trackers.Elements)
					//	{
					//		var slotRef = (SyncRef<Slot>)((Worker)tracker).GetSyncMember("CalibrationReference");
					//		Slot calibrationReferenceSlot = slotRef?.Target;
					//		if (calibrationReferenceSlot == null) continue;
					//		Debug($"CalibrationReference Slot: {calibrationReferenceSlot.Name}");
					//		var trackedDevicePositionerRef = (SyncRef<TrackedDevicePositioner>)((Worker)tracker).GetSyncMember("TrackedDevice");
					//		ITrackedDevice trackedDevice = trackedDevicePositionerRef?.Target?.TrackedDevice;
					//		if (trackedDevice == null) continue;
					//		Debug($"Tracked device name: {trackedDevice.Name}");
					//		if (trackedDevice is OffsetableTrackedObject offsetableTrackedObject)
					//		{
					//			if (offsetableTrackedObject.IsMapped)
					//			{
					//				Debug($"Position before: {calibrationReferenceSlot.LocalPosition}");
					//				Debug($"Rotation before: {calibrationReferenceSlot.LocalRotation}");
					//				calibrationReferenceSlot.LocalPosition = offsetableTrackedObject.BodyNodePositionOffset;
					//				calibrationReferenceSlot.LocalRotation = offsetableTrackedObject.BodyNodeRotationOffset;
					//				Debug($"Position after: {calibrationReferenceSlot.LocalPosition}");
					//				Debug($"Rotation after: {calibrationReferenceSlot.LocalRotation}");
					//				Debug("Updated position and rotation of the slot.");
					//			}
					//		}
					//	}
					//});
					s.Destroy();
				};
				b.LabelText = "Map Trackers";
				float avgScale = MathX.AvgComponent(__instance.Slot.LocalScale);
				s.LocalPosition = s.LocalPosition.SetY(-0.25f / avgScale);
				s.LocalPosition = s.LocalPosition.SetZ(-0.025f / avgScale);
				s.LocalRotation = floatQ.Identity;
				s.GlobalScale = float3.One * 1.25f;
			}
		}
	}
}