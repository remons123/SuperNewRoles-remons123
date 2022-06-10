using System.Collections;
using System;
using System.Text;
using UnityEngine;
using System.Reflection;
using HarmonyLib;

namespace SuperNewRoles.Patch
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    public class ResetDeviceTimer
    {
        public static void Prefix(ExileController __instance, [HarmonyArgument(0)] ref GameData.PlayerInfo exiled, [HarmonyArgument(1)] bool tie)
        {
            if (MapOptions.MapOption.RestrictDevicesOption.getBool())
                SetTimers.resetDeviceTimes();
        }
    }
    public class SetTimers
    {
        public static void resetDeviceTimes()
        {
            AdminPatch.RestrictAdminTime = AdminPatch.RestrictAdminTimeMax;
            CameraPatch.RestrictCameraTime = CameraPatch.RestrictCameraTimeMax;
            VitalsPatch.RestrictVitalsTime = VitalsPatch.RestrictVitalsTimeMax;
                            SuperNewRolesPlugin.Logger.LogInfo("リセット");
        }
    }
}
