using System.Collections.Generic;
using Hazel;

using SuperNewRoles.Helpers;
using SuperNewRoles.Mode.SuperHostRoles;
using UnityEngine;

namespace SuperNewRoles.Mode.Detective
{
    class Main
    {
        public static bool IsNotDetectiveWin;
        public static bool IsNotDetectiveVote;
        public static bool IsDetectiveNotTask;
        public static bool IsNotDetectiveMeetingButton;
        public static PlayerControl DetectivePlayer;
        public static Color32 DetectiveColor = new(255, 0, 255, byte.MaxValue);
        public static void ClearAndReload()
        {
            IsNotDetectiveWin = DetectiveOptions.IsWinNotCheckDetective.GetBool();
            IsNotDetectiveVote = DetectiveOptions.IsNotDetectiveVote.GetBool();
            IsDetectiveNotTask = DetectiveOptions.DetectiveIsNotTask.GetBool();
            IsNotDetectiveMeetingButton = DetectiveOptions.IsNotDetectiveMeetingButton.GetBool();
        }
        public static void RoleSelect()
        {
            DetectivePlayer = PlayerControl.LocalPlayer;
            List<PlayerControl> selectplayers = new();
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.IsCrew())
                {
                    selectplayers.Add(p);
                }
            }
            var random = ModHelpers.GetRandom(selectplayers);
            MessageWriter writer = RPCHelper.StartRPC(CustomRPC.SetDetective);
            writer.Write(random.PlayerId);
            writer.EndRPC();
            RPCProcedure.SetDetective(random.PlayerId);
            DetectivePlayer.RpcSetName(ModHelpers.Cs(DetectiveColor, DetectivePlayer.GetDefaultName()));
            DetectivePlayer.SetName(ModHelpers.Cs(DetectiveColor, DetectivePlayer.GetDefaultName()));
        }
    }
}