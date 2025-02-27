using System;
using System.Collections.Generic;
using System.Linq;
using Agartha;
using HarmonyLib;
using Hazel;
using SuperNewRoles.CustomObject;
using SuperNewRoles.Helpers;
using SuperNewRoles.Mode;
using SuperNewRoles.Mode.SuperHostRoles;
using SuperNewRoles.Patches;
using SuperNewRoles.Roles;
using UnityEngine;

namespace SuperNewRoles.Buttons
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    static class HudManagerStartPatch
    {
        public static CustomButton SheriffKillButton;
        public static CustomButton ClergymanLightOutButton;
        public static CustomButton SpeedBoosterBoostButton;
        public static CustomButton EvilSpeedBoosterBoostButton;
        public static CustomButton LighterLightOnButton;
        public static CustomButton MovingSetButton;
        public static CustomButton MovingTpButton;
        public static CustomButton TeleporterButton;
        public static CustomButton DoorrDoorButton;
        public static CustomButton SelfBomberButton;
        public static CustomButton DoctorVitalsButton;
        public static CustomButton CountChangerButton;
        public static CustomButton ScientistButton;
        public static CustomButton HawkHawkEyeButton;
        public static CustomButton JackalKillButton;
        public static CustomButton JackalSidekickButton;
        public static CustomButton JackalSeerSidekickButton;
        public static CustomButton MagazinerAddButton;
        public static CustomButton MagazinerGetButton;
        public static CustomButton trueloverLoveButton;
        public static CustomButton ImpostorSidekickButton;
        public static CustomButton SideKillerSidekickButton;
        public static CustomButton FalseChargesFalseChargeButton;
        public static CustomButton MadMakerSidekickButton;
        public static CustomButton DemonButton;
        public static CustomButton ArsonistDouseButton;
        public static CustomButton ArsonistIgniteButton;
        public static CustomButton SpeederButton;
        public static CustomButton ChiefSidekickButton;
        public static CustomButton VultureButton;
        public static CustomButton ShielderButton;
        public static CustomButton CleanerButton;
        public static CustomButton MadCleanerButton;
        public static CustomButton FreezerButton;
        public static CustomButton SamuraiButton;
        public static CustomButton VentMakerButton;
        public static CustomButton GhostMechanicRepairButton;
        public static CustomButton EvilHackerButton;
        public static CustomButton EvilHackerMadmateSetting;
        public static CustomButton PositionSwapperButton;
        public static CustomButton KunoichiKunaiButton;
        public static CustomButton KunoichiHideButton;
        public static CustomButton SecretlyKillerMainButton;
        public static CustomButton SecretlyKillerSecretlyKillButton;
        public static CustomButton ClairvoyantButton;
        public static CustomButton DoubleKillerMainKillButton;
        public static CustomButton DoubleKillerSubKillButton;
        public static CustomButton SuicideWisherSuicideButton;
        public static CustomButton FastMakerButton;
        public static CustomButton ToiletFanButton;
        public static CustomButton ButtonerButton;
        public static CustomButton RevolutionistButton;
        public static CustomButton SuicidalIdeationButton;
        public static CustomButton HitmanKillButton;
        public static CustomButton MatryoshkaButton;
        public static CustomButton NunButton;
        public static CustomButton PsychometristButton;
        public static CustomButton PartTimerButton;
        public static CustomButton PainterButton;
        public static CustomButton PhotographerButton;
        public static CustomButton StefinderKillButton;
        public static CustomButton SluggerButton;
        public static CustomButton CrackerButton;
        public static CustomButton WaveCannonButton;
        public static CustomButton DoppelgangerButton;
        public static CustomButton PavlovsownerCreatedogButton;
        public static CustomButton PavlovsdogKillButton;
        public static CustomButton CamouflagerButton;

        public static TMPro.TMP_Text sheriffNumShotsText;
        public static TMPro.TMP_Text PavlovsdogKillSelfText;
        public static TMPro.TMP_Text GhostMechanicNumRepairText;
        public static TMPro.TMP_Text PositionSwapperNumText;
        public static TMPro.TMP_Text SecretlyKillNumText;

        public static void SetCustomButtonCooldowns()
        {
            Sheriff.ResetKillCoolDown();
            Clergyman.ResetCoolDown();
            Teleporter.ResetCoolDown();
            Jackal.ResetCoolDown();
            //クールダウンリセット
        }

        public static PlayerControl SetTarget(List<PlayerControl> untarget = null, bool Crewmateonly = false)
        {
            return PlayerControlFixedUpdatePatch.SetTarget(untargetablePlayers: untarget, onlyCrewmates: Crewmateonly);
        }

        public static void Postfix(HudManager __instance)
        {
            PavlovsdogKillButton = new(
                () =>
                {
                    PlayerControl target = Roles.Neutral.Pavlovsdogs.SetTarget(false);
                    ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, target);
                    PavlovsdogKillButton.MaxTimer = RoleClass.Pavlovsdogs.IsOwnerDead ? CustomOptions.PavlovsdogRunAwayKillCoolTime.GetFloat() : CustomOptions.PavlovsdogKillCoolTime.GetFloat();
                    PavlovsdogKillButton.Timer = PavlovsdogKillButton.MaxTimer;
                    RoleClass.Pavlovsdogs.DeathTime = CustomOptions.PavlovsdogRunAwayDeathTime.GetFloat();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Pavlovsdogs; },
                () =>
                {
                    if (RoleClass.Pavlovsdogs.IsOwnerDead && CachedPlayer.LocalPlayer.IsAlive())
                    {
                        RoleClass.Pavlovsdogs.DeathTime -= Time.deltaTime;
                        PavlovsdogKillSelfText.text = RoleClass.Pavlovsdogs.DeathTime > 0 ? string.Format(ModTranslation.GetString("SerialKillerSuicideText"), ((int)RoleClass.Pavlovsdogs.DeathTime) + 1) : "";
                        if (RoleClass.Pavlovsdogs.DeathTime <= 0)
                        {
                            PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                        }
                    }
                    return Roles.Neutral.Pavlovsdogs.SetTarget(false) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (CustomOptions.PavlovsdogRunAwayDeathTimeIsMeetingReset.GetBool()) RoleClass.Pavlovsdogs.DeathTime = CustomOptions.PavlovsdogRunAwayDeathTime.GetFloat();
                    PavlovsdogKillButton.MaxTimer = RoleClass.Pavlovsdogs.IsOwnerDead ? CustomOptions.PavlovsdogRunAwayKillCoolTime.GetFloat() : CustomOptions.PavlovsdogKillCoolTime.GetFloat();
                    PavlovsdogKillButton.Timer = PavlovsdogKillButton.MaxTimer;
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () => { return RoleClass.IsMeeting; }
            )
            {
                buttonText = FastDestroyableSingleton<HudManager>.Instance.KillButton.buttonLabelText.text,
                showButtonText = true
            };

            PavlovsdogKillSelfText = GameObject.Instantiate(PavlovsdogKillButton.actionButton.cooldownTimerText, PavlovsdogKillButton.actionButton.cooldownTimerText.transform.parent);
            PavlovsdogKillSelfText.text = "";
            PavlovsdogKillSelfText.enableWordWrapping = false;
            PavlovsdogKillSelfText.transform.localScale = Vector3.one * 0.5f;
            PavlovsdogKillSelfText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            PavlovsownerCreatedogButton = new(
                () =>
                {
                    PlayerControl target = Roles.Neutral.Pavlovsdogs.SetTarget();
                    RoleClass.Pavlovsowner.CreateLimit--;
                    bool IsSelfDeath = target.IsImpostor() && CustomOptions.PavlovsownerIsTargetImpostorDeath.GetBool();
                    MessageWriter writer = RPCHelper.StartRPC(CustomRPC.PavlovsOwnerCreateDog);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write(target.PlayerId);
                    writer.Write(IsSelfDeath);
                    writer.EndRPC();
                    RPCProcedure.PavlovsOwnerCreateDog(CachedPlayer.LocalPlayer.PlayerId, target.PlayerId, IsSelfDeath);
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Pavlovsowner && RoleClass.Pavlovsowner.CanCreateDog; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && Roles.Neutral.Pavlovsdogs.SetTarget();
                },
                () =>
                {
                    PavlovsownerCreatedogButton.MaxTimer = CustomOptions.PavlovsownerCreateCoolTime.GetFloat();
                    PavlovsownerCreatedogButton.Timer = PavlovsownerCreatedogButton.MaxTimer;
                },
                RoleClass.Pavlovsowner.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("PavlovsownerCreatedogButtonName"),
                showButtonText = true
            };

            WaveCannonButton = new(
                () =>
                {
                    var pos = CachedPlayer.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
                    MessageWriter writer = RPCHelper.StartRPC(CustomRPC.WaveCannon);
                    writer.Write((byte)WaveCannonObject.RpcType.Spawn);
                    writer.Write((byte)0);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerPhysics.FlipX);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.WriteBytesAndSize(buff);
                    writer.EndRPC();
                    RPCProcedure.WaveCannon((byte)WaveCannonObject.RpcType.Spawn, 0, CachedPlayer.LocalPlayer.PlayerPhysics.FlipX, CachedPlayer.LocalPlayer.PlayerId, buff);
                },
                (bool isAlive, RoleId role) => { return isAlive && role is RoleId.WaveCannon or RoleId.WaveCannonJackal; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    WaveCannonButton.MaxTimer = PlayerControl.LocalPlayer.IsRole(RoleId.WaveCannon) ? CustomOptions.WaveCannonCoolTime.GetFloat() : CustomOptions.WaveCannonJackalCoolTime.GetFloat();
                    WaveCannonButton.Timer = WaveCannonButton.MaxTimer;
                    WaveCannonButton.effectCancellable = false;
                    WaveCannonButton.EffectDuration = PlayerControl.LocalPlayer.IsRole(RoleId.WaveCannon) ? CustomOptions.WaveCannonChargeTime.GetFloat() : CustomOptions.WaveCannonJackalChargeTime.GetFloat();
                    WaveCannonButton.HasEffect = true;
                },
                RoleClass.WaveCannon.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; },
                true,
                5f,
                () =>
                {
                    WaveCannonObject obj = WaveCannonObject.Objects.FirstOrDefault(x => x.Owner != null && x.Owner.PlayerId == CachedPlayer.LocalPlayer.PlayerId && x.Id == WaveCannonObject.Ids[CachedPlayer.LocalPlayer.PlayerId] - 1);
                    if (obj == null)
                    {
                        Logger.Info("nullなのでreturnしました", "WaveCannonButton");
                        return;
                    }
                    var pos = CachedPlayer.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
                    MessageWriter writer = RPCHelper.StartRPC(CustomRPC.WaveCannon);
                    writer.Write((byte)WaveCannonObject.RpcType.Shoot);
                    writer.Write((byte)obj.Id);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerPhysics.FlipX);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.WriteBytesAndSize(buff);
                    writer.EndRPC();
                    RPCProcedure.WaveCannon((byte)WaveCannonObject.RpcType.Shoot, (byte)obj.Id, CachedPlayer.LocalPlayer.PlayerPhysics.FlipX, CachedPlayer.LocalPlayer.PlayerId, buff);
                }
            )
            {
                buttonText = ModTranslation.GetString("WaveCannonButtonName"),
                showButtonText = true
            };

            SluggerButton = new(
                () =>
                {
                    var anim = PlayerAnimation.GetPlayerAnimation(CachedPlayer.LocalPlayer.PlayerId);
                    anim.RpcAnimation(RpcAnimationType.SluggerCharge);
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Slugger; },
                () =>
                {
                    if (SluggerButton.isEffectActive && !PlayerControl.LocalPlayer.CanMove)
                    {
                        var anim = PlayerAnimation.GetPlayerAnimation(CachedPlayer.LocalPlayer.PlayerId);
                        SluggerButton.isEffectActive = false;
                        anim.RpcAnimation(RpcAnimationType.Stop);
                    }
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    SluggerButton.MaxTimer = CustomOptions.SluggerCoolTime.GetFloat();
                    SluggerButton.Timer = SluggerButton.MaxTimer;
                    SluggerButton.effectCancellable = false;
                    SluggerButton.EffectDuration = CustomOptions.SluggerChargeTime.GetFloat();
                    SluggerButton.HasEffect = true;
                },
                RoleClass.Slugger.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; },
                true,
                5f,
                () =>
                {
                    List<PlayerControl> Targets = new();
                    //一気にキルできるか。後に設定で変更可に
                    if (CustomOptions.SluggerIsMultiKill.GetBool())
                    {
                        Targets = Roles.Impostor.Slugger.SetTarget();
                    }
                    else
                    {
                        if (FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget != null) Targets.Add(FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget);
                    }
                    RpcAnimationType AnimationType = RpcAnimationType.SluggerMurder;
                    //空振り判定
                    if (Targets.Count <= 0)
                    {
                        AnimationType = RpcAnimationType.SluggerMurder;
                    }
                    var anim = PlayerAnimation.GetPlayerAnimation(CachedPlayer.LocalPlayer.PlayerId);
                    anim.RpcAnimation(AnimationType);
                    MessageWriter RPCWriter = RPCHelper.StartRPC((RpcCalls)CustomRPC.SluggerExile);
                    RPCWriter.Write(CachedPlayer.LocalPlayer.PlayerId);
                    RPCWriter.Write((byte)Targets.Count);
                    foreach (PlayerControl Target in Targets)
                    {
                        RPCWriter.Write(Target.PlayerId);
                    }
                    RPCWriter.EndRPC();
                    List<byte> TargetsId = new();
                    foreach (PlayerControl Target in Targets)
                    {
                        TargetsId.Add(Target.PlayerId);
                        Target.RpcSetFinalStatus(FinalStatus.SluggerHarisen);
                    }
                    RPCProcedure.SluggerExile(CachedPlayer.LocalPlayer.PlayerId, TargetsId);
                    SluggerButton.MaxTimer = CustomOptions.SluggerCoolTime.GetFloat();
                    SluggerButton.Timer = SluggerButton.MaxTimer;
                    if (CustomOptions.SluggerIsKillCoolSync.GetBool())
                    {
                        PlayerControl.LocalPlayer.killTimer = RoleHelpers.GetCoolTime(CachedPlayer.LocalPlayer);
                    }
                }
            )
            {
                buttonText = ModTranslation.GetString("SluggerButtonName"),
                showButtonText = true
            };

            PhotographerButton = new(
                () =>
                {
                    List<byte> Targets = Roles.Neutral.Photographer.SetTarget();
                    RoleClass.Photographer.PhotedPlayerIds.AddRange(Targets);
                    PhotographerButton.Timer = RoleClass.Photographer.BonusCount > 0 && Targets.Count >= RoleClass.Photographer.BonusCount ? CustomOptions.PhotographerBonusCoolTime.GetFloat() : PhotographerButton.MaxTimer;
                    if (CustomOptions.PhotographerIsNotification.GetBool())
                    {
                        RPCHelper.StartRPC(CustomRPC.SharePhotograph).EndRPC();
                        RPCProcedure.SharePhotograph();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Photographer; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && Roles.Neutral.Photographer.SetTarget().Count > 0;
                },
                () =>
                {
                    PhotographerButton.MaxTimer = CustomOptions.PhotographerCoolTime.GetFloat();
                    PhotographerButton.Timer = PhotographerButton.MaxTimer;
                },
                RoleClass.Photographer.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
                )
            {
                buttonText = ModTranslation.GetString("PhotographerButtonName"),
                showButtonText = true
            };

            KunoichiKunaiButton = new(
                () => { RoleClass.Kunoichi.Kunai.kunai.SetActive(!RoleClass.Kunoichi.Kunai.kunai.active); },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Kunoichi; },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () =>
                {
                    KunoichiKunaiButton.MaxTimer = 0f;
                    KunoichiKunaiButton.Timer = 0f;
                },
                RoleClass.Kunoichi.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("KunoichiKunai"),
                showButtonText = true
            };
            KunoichiHideButton = new CustomButton(
                () =>
                {
                    /*  Kunoichi.cs Update() にある、
                        「透明化に必要な待機時間の取得と処理 (ボタン動作の時)」コメント以降のif文の中で透明化の処理を行っている。*/
                    RoleClass.Kunoichi.IsHideButton = true;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Kunoichi && RoleClass.Kunoichi.IsWaitAndPressTheButtonToHide; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { Kunoichi.HideOff(); },
                RoleClass.Kunoichi.GetHideButtonSprite(),
                new Vector3(-2.7f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.L,
                50,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ScientistButtonName"),
                showButtonText = true
            };

            CrackerButton = new(
                () =>
                {
                    byte TargetId = SetTarget(RoleClass.Cracker.CurrentCrackedPlayerControls).PlayerId;
                    RoleClass.Cracker.currentCrackedPlayers.Add(TargetId);
                    RPCHelper.SendSinglePlayerRpc(CustomRPC.CrackerCrack, TargetId);
                    RPCProcedure.CrackerCrack(TargetId);
                    RoleClass.Cracker.TurnCount--;
                    RoleClass.Cracker.MaxTurnCount--;
                    CrackerButton.Timer = CrackerButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Cracker && RoleClass.Cracker.TurnCount > 0 && RoleClass.Cracker.MaxTurnCount > 0; },
                () =>
                {
                    return SetTarget(RoleClass.Cracker.CurrentCrackedPlayerControls) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    CrackerButton.MaxTimer = CustomOptions.CrackerCoolTime.GetFloat();
                    CrackerButton.Timer = CrackerButton.MaxTimer;
                },
                ModHelpers.LoadSpriteFromResources("SuperNewRoles.Resources.CrackerButton.png", 115f),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("CrackerButtonName"),
                showButtonText = true
            };

            FalseChargesFalseChargeButton = new(
                () =>
                {
                    PlayerControl Target = SetTarget();
                    if (Target && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove)
                    {
                        if (ModeHandler.IsMode(ModeId.SuperHostRoles))
                        {
                            PlayerControl.LocalPlayer.CmdCheckMurder(Target);
                        }
                        else
                        {
                            RoleClass.FalseCharges.FalseChargePlayer = Target.PlayerId;
                            ModHelpers.UncheckedMurderPlayer(Target, PlayerControl.LocalPlayer);
                            PlayerControl.LocalPlayer.RpcSetFinalStatus(FinalStatus.FalseChargesFalseCharge);
                            RoleClass.FalseCharges.Turns = RoleClass.FalseCharges.DefaultTurn;
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.FalseCharges; },
                () =>
                {
                    return SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    FalseChargesFalseChargeButton.MaxTimer = RoleClass.FalseCharges.CoolTime;
                    FalseChargesFalseChargeButton.Timer = RoleClass.FalseCharges.CoolTime;
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("FalseChargesButtonTitle"),
                showButtonText = true
            };

            trueloverLoveButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove && !RoleClass.Truelover.IsCreate && !PlayerControl.LocalPlayer.IsLovers())
                    {
                        var target = SetTarget();
                        if (target == null || target.IsLovers()) return;
                        RoleClass.Truelover.IsCreate = true;
                        RoleHelpers.SetLovers(PlayerControl.LocalPlayer, target);
                        RoleHelpers.SetLoversRPC(PlayerControl.LocalPlayer, target);
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.truelover && !RoleClass.Truelover.IsCreate; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && SetTarget();
                },
                () => { trueloverLoveButton.Timer = 0f; trueloverLoveButton.MaxTimer = 0f; },
                RoleClass.Truelover.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("trueloverloveButtonName"),
                showButtonText = true
            };

            MagazinerGetButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove && RoleClass.Magaziner.MyPlayerCount >= 1 && FastDestroyableSingleton<HudManager>.Instance.KillButton.isCoolingDown && RoleClass.Magaziner.IsOKSet)
                    {
                        PlayerControl.LocalPlayer.SetKillTimerUnchecked(RoleClass.Magaziner.SetTime);
                        RoleClass.Magaziner.MyPlayerCount--;
                        if (RoleClass.Magaziner.SetTime != 0)
                        {
                            RoleClass.Magaziner.IsOKSet = false;
                            new LateTask(() =>
                            {
                                RoleClass.Magaziner.IsOKSet = true;
                            }, 1f, "IsOkSetSet");
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Magaziner; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && FastDestroyableSingleton<HudManager>.Instance.KillButton.isCoolingDown && RoleClass.Magaziner.MyPlayerCount >= 1;
                },
                () => { MagazinerGetButton.Timer = 0f; MagazinerGetButton.MaxTimer = 0f; },
                RoleClass.Magaziner.GetGetButtonSprite(),
                new Vector3(-2.7f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("MagazinerGetButtonName"),
                showButtonText = true
            };

            MagazinerAddButton = new(
                () =>
                {
                    if (!FastDestroyableSingleton<HudManager>.Instance.KillButton.isCoolingDown && PlayerControl.LocalPlayer.CanMove)
                    {
                        PlayerControl.LocalPlayer.SetKillTimerUnchecked(PlayerControl.GameOptions.KillCooldown);
                        RoleClass.Magaziner.MyPlayerCount++;
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Magaziner; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && !FastDestroyableSingleton<HudManager>.Instance.KillButton.isCoolingDown;
                },
                () => { MagazinerAddButton.Timer = 0f; MagazinerAddButton.MaxTimer = 0f; },
                RoleClass.Magaziner.GetAddButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("MagazinerAddButtonName"),
                showButtonText = true
            };

            ScientistButton = new Buttons.CustomButton(
                () =>
                {
                    if (!PlayerControl.LocalPlayer.CanMove) return;
                    RoleClass.NiceScientist.ButtonTimer = DateTime.Now;
                    ScientistButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    Scientist.Start();
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.NiceScientist || role == RoleId.EvilScientist); },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { Scientist.EndMeeting(); },
                RoleClass.NiceScientist.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ScientistButtonName"),
                showButtonText = true
            };

            HawkHawkEyeButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove)
                    {
                        if (PlayerControl.LocalPlayer.IsRole(RoleId.Hawk))
                        {
                            RoleClass.Hawk.Timer = RoleClass.Hawk.DurationTime;
                            RoleClass.Hawk.ButtonTimer = DateTime.Now;
                            HawkHawkEyeButton.MaxTimer = RoleClass.Hawk.CoolTime;
                            HawkHawkEyeButton.Timer = RoleClass.Hawk.CoolTime;
                        }
                        if (PlayerControl.LocalPlayer.IsRole(RoleId.NiceHawk))
                        {
                            RoleClass.NiceHawk.Timer = RoleClass.NiceHawk.DurationTime;
                            RoleClass.NiceHawk.ButtonTimer = DateTime.Now;
                            HawkHawkEyeButton.MaxTimer = RoleClass.NiceHawk.CoolTime;
                            HawkHawkEyeButton.Timer = RoleClass.NiceHawk.CoolTime;
                            RoleClass.NiceHawk.Postion = CachedPlayer.LocalPlayer.transform.localPosition;
                            RoleClass.NiceHawk.timer1 = 10;
                            RoleClass.NiceHawk.Timer2 = DateTime.Now;
                        }
                        if (PlayerControl.LocalPlayer.IsRole(RoleId.MadHawk))
                        {
                            RoleClass.MadHawk.Timer = RoleClass.MadHawk.DurationTime;
                            RoleClass.MadHawk.ButtonTimer = DateTime.Now;
                            HawkHawkEyeButton.MaxTimer = RoleClass.MadHawk.CoolTime;
                            HawkHawkEyeButton.Timer = RoleClass.MadHawk.CoolTime;
                            RoleClass.MadHawk.Postion = CachedPlayer.LocalPlayer.transform.localPosition;
                            RoleClass.MadHawk.timer1 = 10;
                            RoleClass.MadHawk.Timer2 = DateTime.Now;
                        }
                        RoleClass.Hawk.IsHawkOn = true;
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.Hawk || role == RoleId.NiceHawk || role == RoleId.MadHawk); },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.Hawk))
                    {
                        HawkHawkEyeButton.MaxTimer = RoleClass.Hawk.CoolTime;
                        HawkHawkEyeButton.Timer = RoleClass.Hawk.CoolTime;
                    }
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.NiceHawk))
                    {
                        HawkHawkEyeButton.MaxTimer = RoleClass.NiceHawk.CoolTime;
                        HawkHawkEyeButton.Timer = RoleClass.NiceHawk.CoolTime;
                    }
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.MadHawk))
                    {
                        HawkHawkEyeButton.MaxTimer = RoleClass.MadHawk.CoolTime;
                        HawkHawkEyeButton.Timer = RoleClass.MadHawk.CoolTime;
                    }
                    RoleClass.Hawk.IsHawkOn = false;
                },
                RoleClass.Hawk.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("HawkButtonName"),
                showButtonText = true
            };

            CountChangerButton = new(
                () =>
                {
                    if (RoleClass.CountChanger.Count >= 1 && SetTarget() && PlayerControl.LocalPlayer.CanMove)
                    {
                        RoleClass.CountChanger.IsSet = true;
                        RoleClass.CountChanger.Count--;
                        var Target = PlayerControlFixedUpdatePatch.SetTarget(onlyCrewmates: true);
                        var TargetID = Target.PlayerId;
                        var LocalID = CachedPlayer.LocalPlayer.PlayerId;

                        RPCProcedure.CountChangerSetRPC(LocalID, TargetID);
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CountChangerSetRPC, SendOption.Reliable, -1);
                        killWriter.Write(LocalID);
                        killWriter.Write(TargetID);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.CountChanger && !RoleClass.CountChanger.IsSet && RoleClass.CountChanger.Count >= 1; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && PlayerControlFixedUpdatePatch.SetTarget(onlyCrewmates: true);
                },
                () =>
                {
                    CountChangerButton.MaxTimer = PlayerControl.GameOptions.KillCooldown;
                    CountChangerButton.Timer = PlayerControl.GameOptions.KillCooldown;
                },
                RoleClass.CountChanger.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("CountChangerButtonName"),
                showButtonText = true
            };

            DoctorVitalsButton = new(
                () =>
                {
                    if (RoleClass.Doctor.Vital == null)
                    {
                        var moto = PlayerControl.LocalPlayer.Data.Role.Role;
                        DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.Scientist);
                        CachedPlayer.LocalPlayer.Data.Role.TryCast<ScientistRole>().UseAbility();
                        DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, moto);
                        RoleClass.Doctor.Vital = GameObject.FindObjectOfType<VitalsMinigame>();
                    }
                    RoleClass.Doctor.MyPanelFlag = true;
                },
                (bool isAlive, RoleId role) => { return role == RoleId.Doctor && isAlive; },
                () =>
                {
                    if (RoleClass.Doctor.IsChargingNow)
                    {
                        DoctorVitalsButton.MaxTimer = 10f;
                        Logger.Info(RoleClass.Doctor.Battery.ToString());
                        DoctorVitalsButton.Timer = RoleClass.Doctor.Battery <= 0 ? 10f : RoleClass.Doctor.Battery / 10f;
                    }
                    else if (RoleClass.Doctor.Battery > 0)
                    {
                        DoctorVitalsButton.MaxTimer = 0f;
                        DoctorVitalsButton.Timer = 0f;
                    }
                    return (PlayerControl.LocalPlayer.CanMove && RoleClass.Doctor.Battery > 0) || RoleClass.Doctor.IsChargingNow;
                },
                () => { },
                RoleClass.Doctor.GetVitalsSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("DoctorVitalName"),
                showButtonText = true
            };

            JackalSidekickButton = new(
                () =>
                {
                    var target = PlayerControlFixedUpdatePatch.JackalSetTarget();
                    if (target && PlayerControl.LocalPlayer.CanMove && RoleClass.Jackal.CanCreateSidekick)
                    {
                        if (RoleClass.Jackal.CanCreateFriend)
                        {
                            target.ResetAndSetRole(RoleId.JackalFriends); //クルーにして フレンズにする
                        }
                        else
                        {
                            bool IsFakeSidekick = EvilEraser.IsBlockAndTryUse(EvilEraser.BlockTypes.JackalSidekick, target);
                            MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CreateSidekick, SendOption.Reliable, -1);
                            killWriter.Write(target.PlayerId);
                            killWriter.Write(IsFakeSidekick);
                            AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                            RPCProcedure.CreateSidekick(target.PlayerId, IsFakeSidekick);
                        }
                        RoleClass.Jackal.CanCreateSidekick = false;
                        Jackal.ResetCoolDown();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Jackal && ModeHandler.IsMode(ModeId.Default) && RoleClass.Jackal.CanCreateSidekick && CustomOptions.JackalCreateSidekick.GetBool(); },
                () =>
                {
                    return PlayerControlFixedUpdatePatch.JackalSetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.Jackal)) { Jackal.EndMeeting(); }
                },
                RoleClass.Jackal.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("JackalCreateSidekickButtonName"),
                showButtonText = true
            };

            JackalSeerSidekickButton = new(
                () =>
                {
                    var target_JS = PlayerControlFixedUpdatePatch.JackalSetTarget();
                    if (target_JS && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove && RoleClass.JackalSeer.CanCreateSidekick)
                    {
                        bool IsFakeSidekickSeer = EvilEraser.IsBlockAndTryUse(EvilEraser.BlockTypes.JackalSeerSidekick, target_JS);
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CreateSidekickSeer, SendOption.Reliable, -1);
                        killWriter.Write(target_JS.PlayerId);
                        killWriter.Write(IsFakeSidekickSeer);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.CreateSidekickSeer(target_JS.PlayerId, IsFakeSidekickSeer);
                        RoleClass.JackalSeer.CanCreateSidekick = false;
                        JackalSeer.ResetCoolDown();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.JackalSeer && ModeHandler.IsMode(ModeId.Default) && RoleClass.JackalSeer.CanCreateSidekick && CustomOptions.JackalSeerCreateSidekick.GetBool(); },
                () =>
                {
                    return PlayerControlFixedUpdatePatch.JackalSetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.JackalSeer)) { JackalSeer.EndMeeting(); }
                },
                RoleClass.Jackal.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("JackalCreateSidekickButtonName"),
                showButtonText = true
            };

            JackalKillButton = new(
                () =>
                {
                    if (PlayerControlFixedUpdatePatch.JackalSetTarget() && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove)
                    {
                        ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, PlayerControlFixedUpdatePatch.JackalSetTarget());
                        switch (PlayerControl.LocalPlayer.GetRole())
                        {
                            case RoleId.Jackal:
                                Jackal.ResetCoolDown();
                                break;
                            case RoleId.JackalSeer:
                                JackalSeer.ResetCoolDown();
                                break;
                            case RoleId.TeleportingJackal:
                                TeleportingJackal.ResetCoolDowns();
                                break;
                            case RoleId.WaveCannonJackal:
                                Roles.Neutral.WaveCannonJackal.ResetCoolDowns();
                                break;
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.Jackal || role == RoleId.TeleportingJackal || role == RoleId.JackalSeer || role == RoleId.WaveCannonJackal) && ModeHandler.IsMode(ModeId.Default); },
                () =>
                {
                    return PlayerControlFixedUpdatePatch.JackalSetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.Jackal)) { Jackal.EndMeeting(); }
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.JackalSeer)) { JackalSeer.EndMeeting(); }
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.WaveCannonJackal)) { Roles.Neutral.WaveCannonJackal.ResetCoolDowns(); }
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = FastDestroyableSingleton<HudManager>.Instance.KillButton.buttonLabelText.text,
                showButtonText = true
            };

            SelfBomberButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove)
                    {
                        SelfBomber.SelfBomb();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && ModeHandler.IsMode(ModeId.Default) && PlayerControl.LocalPlayer.IsRole(RoleId.SelfBomber); },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { SelfBomber.ResetCoolDown(); },
                RoleClass.SelfBomber.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SelfBomberButtonName"),
                showButtonText = true
            };

            DoorrDoorButton = new(
                () =>
                {
                    Doorr.DoorrBtn();
                    Doorr.ResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && Doorr.IsDoorr(PlayerControl.LocalPlayer); },
                () =>
                {
                    return Doorr.CheckTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { Doorr.EndMeeting(); },
                RoleClass.Doorr.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("DoorrButtonText"),
                showButtonText = true
            };

            TeleporterButton = new(
                () =>
                {
                    if (!PlayerControl.LocalPlayer.CanMove) return;
                    RoleClass.Clergyman.ButtonTimer = DateTime.Now;
                    TeleporterButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    Teleporter.TeleportStart();
                    Teleporter.ResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.Teleporter || role == RoleId.TeleportingJackal || role == RoleId.NiceTeleporter || (role == RoleId.Levelinger && RoleClass.Levelinger.IsPower(RoleClass.Levelinger.LevelPowerTypes.Teleporter))); },
                () =>
                {
                    return true && PlayerControl.LocalPlayer.CanMove;
                },
                () => { Teleporter.EndMeeting(); },
                RoleClass.Teleporter.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("TeleporterTeleportButton"),
                showButtonText = true
            };

            MovingSetButton = new(
                () =>
                {
                    if (!PlayerControl.LocalPlayer.CanMove) return;
                    if (!Moving.IsSetPostion())
                    {
                        Moving.SetPostion();
                    }
                    Moving.ResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.Moving || role == RoleId.EvilMoving || RoleClass.Levelinger.IsPower(RoleClass.Levelinger.LevelPowerTypes.Moving)) && !Moving.IsSetPostion(); },
                () =>
                {
                    return true && PlayerControl.LocalPlayer.CanMove;
                },
                () => { Moving.EndMeeting(); },
                RoleClass.Moving.GetNoSetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("MovingButtonSetName"),
                showButtonText = true
            };

            MovingTpButton = new Buttons.CustomButton(
                () =>
                {
                    if (!PlayerControl.LocalPlayer.CanMove) return;
                    if (Moving.IsSetPostion())
                    {
                        Moving.TP();
                    }
                    Moving.ResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.Moving || role == RoleId.EvilMoving || RoleClass.Levelinger.IsPower(RoleClass.Levelinger.LevelPowerTypes.Moving)) && Moving.IsSetPostion(); },
                () =>
                {
                    return true && PlayerControl.LocalPlayer.CanMove;
                },
                () => { Moving.EndMeeting(); },
                RoleClass.Moving.GetSetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("MovingButtonTpName"),
                showButtonText = true
            };

            SheriffKillButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.RemoteSheriff))
                    {
                        DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.Shapeshifter);
                        foreach (CachedPlayer p in CachedPlayer.AllPlayers)
                        {
                            p.Data.Role.NameColor = Color.white;

                            CachedPlayer.LocalPlayer.Data.Role.TryCast<ShapeshifterRole>().UseAbility();

                            if (p.PlayerControl.IsImpostor())
                            {
                                p.Data.Role.NameColor = RoleClass.ImpostorRed;
                            }
                        }
                        DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.Crewmate);
                    }
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.Sheriff))
                    {
                        if (RoleClass.Sheriff.KillMaxCount > 0 && SetTarget())
                        {
                            var Target = PlayerControlFixedUpdatePatch.SetTarget();
                            var LocalID = CachedPlayer.LocalPlayer.PlayerId;
                            var misfire = !Sheriff.IsSheriffKill(Target);
                            if (RoleClass.Chief.SheriffPlayer.Contains(LocalID))
                            {
                                misfire = !Sheriff.IsChiefSheriffKill(Target);
                            }
                            var TargetID = Target.PlayerId;

                            RPCProcedure.SheriffKill(LocalID, TargetID, misfire);
                            MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SheriffKill, SendOption.Reliable, -1);
                            killWriter.Write(LocalID);
                            killWriter.Write(TargetID);
                            killWriter.Write(misfire);
                            AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                            FinalStatusClass.RpcSetFinalStatus(misfire ? CachedPlayer.LocalPlayer : Target, misfire ? FinalStatus.SheriffMisFire : (Target.IsRole(RoleId.HauntedWolf) ? FinalStatus.SheriffHauntedWolfKill : FinalStatus.SheriffKill));
                            Sheriff.ResetKillCoolDown();
                            RoleClass.Sheriff.KillMaxCount--;
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && ModeHandler.IsMode(ModeId.Default) && Sheriff.IsSheriffButton(PlayerControl.LocalPlayer); },
                () =>
                {
                    float killcount = 0f;
                    bool flag = false;
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.RemoteSheriff))
                    {
                        killcount = RoleClass.RemoteSheriff.KillMaxCount;
                        flag = true;
                    }
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.Sheriff))
                    {
                        killcount = RoleClass.Sheriff.KillMaxCount;
                        flag = PlayerControlFixedUpdatePatch.SetTarget() && PlayerControl.LocalPlayer.CanMove;
                    }
                    sheriffNumShotsText.text = killcount > 0 ? string.Format(ModTranslation.GetString("SheriffNumTextName"), killcount) : "";
                    return flag;
                },
                () => { Sheriff.EndMeeting(); },
                RoleClass.Sheriff.GetButtonSprite(),
                new Vector3(0f, 1f, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () => { return false; }
            );
            sheriffNumShotsText = GameObject.Instantiate(SheriffKillButton.actionButton.cooldownTimerText, SheriffKillButton.actionButton.cooldownTimerText.transform.parent);
            sheriffNumShotsText.text = "";
            sheriffNumShotsText.enableWordWrapping = false;
            sheriffNumShotsText.transform.localScale = Vector3.one * 0.5f;
            sheriffNumShotsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);
            SheriffKillButton.buttonText = ModTranslation.GetString("SheriffKillButtonName");
            SheriffKillButton.showButtonText = true;

            ClergymanLightOutButton = new(
                () =>
                {
                    if (ClergymanLightOutButton.isEffectActive)
                    {
                        ClergymanLightOutButton.MaxTimer = RoleClass.Clergyman.CoolTime;
                        ClergymanLightOutButton.Timer = ClergymanLightOutButton.MaxTimer;
                        MessageWriter RPCWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RPCClergymanLightOut, SendOption.Reliable, -1);
                        RPCWriter.Write(false);
                        AmongUsClient.Instance.FinishRpcImmediately(RPCWriter);
                        RPCProcedure.RPCClergymanLightOut(false);
                        ClergymanLightOutButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    }
                    else
                    {
                        Clergyman.LightOutStart();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Clergyman; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { Clergyman.EndMeeting(); },
                RoleClass.Clergyman.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; },
                true,
                5f,
                () =>
                {
                    MessageWriter RPCWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RPCClergymanLightOut, SendOption.Reliable, -1);
                    RPCWriter.Write(false);
                    AmongUsClient.Instance.FinishRpcImmediately(RPCWriter);
                    RPCProcedure.RPCClergymanLightOut(false);
                    ClergymanLightOutButton.MaxTimer = RoleClass.Clergyman.CoolTime;
                    ClergymanLightOutButton.Timer = ClergymanLightOutButton.MaxTimer;
                }
            )
            {
                buttonText = ModTranslation.GetString("ClergymanLightOutButtonName"),
                showButtonText = true
            };

            SpeedBoosterBoostButton = new Buttons.CustomButton(
                () =>
                {
                    RoleClass.SpeedBooster.ButtonTimer = DateTime.Now;
                    SpeedBoosterBoostButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    SpeedBooster.BoostStart();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.SpeedBooster; },
                () =>
                {
                    return SpeedBoosterBoostButton.Timer <= 0;
                },
                () => { SpeedBooster.EndMeeting(); },
                RoleClass.SpeedBooster.GetSpeedBoostButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SpeedBoosterBoostButtonName"),
                showButtonText = true,
                HasEffect = true
            };

            EvilSpeedBoosterBoostButton = new Buttons.CustomButton(
                () =>
                {
                    RoleClass.EvilSpeedBooster.ButtonTimer = DateTime.Now;
                    EvilSpeedBoosterBoostButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    EvilSpeedBooster.BoostStart();
                },
                (bool isAlive, RoleId role) => { return isAlive && (role == RoleId.EvilSpeedBooster || RoleClass.Levelinger.IsPower(RoleClass.Levelinger.LevelPowerTypes.SpeedBooster)); },
                () =>
                {
                    return EvilSpeedBoosterBoostButton.Timer <= 0;
                },
                () => { EvilSpeedBooster.EndMeeting(); },
                RoleClass.SpeedBooster.GetSpeedBoostButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("EvilSpeedBoosterBoostButtonName"),
                showButtonText = true
            };

            LighterLightOnButton = new Buttons.CustomButton(
                () =>
                {
                    RoleClass.Lighter.IsLightOn = true;
                    RoleClass.Lighter.ButtonTimer = DateTime.Now;
                    LighterLightOnButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    Lighter.LightOnStart();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Lighter; },
                () =>
                {
                    return LighterLightOnButton.Timer <= 0;
                },
                () => { Lighter.EndMeeting(); },
                RoleClass.Lighter.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("LighterButtonName"),
                showButtonText = true
            };

            ImpostorSidekickButton = new(
                () =>
                {
                    var target = SetTarget(Crewmateonly: true);
                    if (target && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove && !RoleClass.Levelinger.IsCreateMadmate)
                    {
                        target.SetRoleRPC(RoleId.MadMate);
                        RoleClass.Levelinger.IsCreateMadmate = true;
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Levelinger && RoleClass.Levelinger.IsPower(RoleClass.Levelinger.LevelPowerTypes.Sidekick) && !RoleClass.Levelinger.IsCreateMadmate; },
                () =>
                {
                    return SetTarget(Crewmateonly: true) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    ImpostorSidekickButton.MaxTimer = PlayerControl.GameOptions.KillCooldown;
                    ImpostorSidekickButton.Timer = PlayerControl.GameOptions.KillCooldown;
                },
                RoleClass.Jackal.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SidekickName"),
                showButtonText = true
            };

            SideKillerSidekickButton = new(
                () =>
                {
                    var target = SetTarget(Crewmateonly: true);
                    if (target && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove && !RoleClass.SideKiller.IsCreateMadKiller)
                    {
                        MessageWriter writer = RPCHelper.StartRPC(CustomRPC.SetMadKiller);
                        writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                        writer.Write(target.PlayerId);
                        writer.EndRPC();
                        RPCProcedure.SetMadKiller(CachedPlayer.LocalPlayer.PlayerId, target.PlayerId);
                        RoleClass.SideKiller.IsCreateMadKiller = true;
                        PlayerControl.LocalPlayer.killTimer = RoleClass.SideKiller.KillCoolTime;
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.SideKiller && !RoleClass.SideKiller.IsCreateMadKiller; },
                () =>
                {
                    return SetTarget(Crewmateonly: true) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    SideKillerSidekickButton.MaxTimer = RoleClass.SideKiller.KillCoolTime;
                    SideKillerSidekickButton.Timer = RoleClass.SideKiller.KillCoolTime;
                },
                RoleClass.Jackal.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SidekickName"),
                showButtonText = true
            };

            MadMakerSidekickButton = new(
                () =>
                {
                    var target = SetTarget();
                    if (!target.Data.Role.IsImpostor && target && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove && !RoleClass.MadMaker.IsCreateMadmate)
                    {
                        Madmate.CreateMadMate(target);
                        RoleClass.MadMaker.IsCreateMadmate = true;
                    }
                    else if (target.Data.Role.IsImpostor)
                    {
                        if (ModeHandler.IsMode(ModeId.Default))
                        {
                            if (PlayerControl.LocalPlayer.IsRole(RoleId.MadMaker))
                            {
                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RPCMurderPlayer, SendOption.Reliable, -1);
                                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                                writer.Write(byte.MaxValue);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.RPCMurderPlayer(CachedPlayer.LocalPlayer.PlayerId, CachedPlayer.LocalPlayer.PlayerId, byte.MaxValue);
                                PlayerControl.LocalPlayer.RpcSetFinalStatus(FinalStatus.MadmakerMisSet);
                            }
                        }
                        else if (ModeHandler.IsMode(ModeId.SuperHostRoles))
                        {
                            PlayerControl.LocalPlayer.CmdCheckMurder(target);
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.MadMaker && ModeHandler.IsMode(ModeId.Default) && !RoleClass.MadMaker.IsCreateMadmate; },
                () =>
                {
                    return SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { },
                RoleClass.Jackal.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SidekickName"),
                showButtonText = true
            };

            RoleClass.SerialKiller.SuicideKillText = GameObject.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText.transform.parent);
            RoleClass.SerialKiller.SuicideKillText.text = "";
            RoleClass.SerialKiller.SuicideKillText.enableWordWrapping = false;
            RoleClass.SerialKiller.SuicideKillText.transform.localScale = Vector3.one * 0.5f;
            RoleClass.SerialKiller.SuicideKillText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            DemonButton = new(
                () =>
                {
                    Demon.DemonCurse(SetTarget(untarget: Demon.GetUntarget()));
                    DemonButton.Timer = DemonButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return Demon.IsButton(); },
                () =>
                {
                    return SetTarget(untarget: Demon.GetUntarget()) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    DemonButton.MaxTimer = RoleClass.Demon.CoolTime;
                    DemonButton.Timer = RoleClass.Demon.CoolTime;
                },
                RoleClass.Demon.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("DemonButtonName"),
                showButtonText = true
            };

            ArsonistDouseButton = new(
                () =>
                {
                    var Target = SetTarget(untarget: Arsonist.GetUntarget());
                    RoleClass.Arsonist.DouseTarget = Target;
                    ArsonistDouseButton.MaxTimer = RoleClass.Arsonist.DurationTime;
                    ArsonistDouseButton.Timer = ArsonistDouseButton.MaxTimer;
                    ArsonistDouseButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    RoleClass.Arsonist.IsDouse = true;
                    //SuperNewRolesPlugin.Logger.LogInfo("アーソニストが塗るボタンを押した");
                },
                (bool isAlive, RoleId role) => { return Arsonist.IsButton(); },
                () =>
                {
                    return SetTarget(untarget: Arsonist.GetUntarget()) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    ArsonistDouseButton.MaxTimer = RoleClass.Arsonist.CoolTime;
                    ArsonistDouseButton.Timer = RoleClass.Arsonist.CoolTime;
                },
                RoleClass.Arsonist.GetDouseButtonSprite(),
                new Vector3(0f, 1f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.Q,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ArsonistDouseButtonName"),
                showButtonText = true
            };

            ArsonistIgniteButton = new(
                () =>
                {
                    Arsonist.SetWinArsonist();
                    RoleClass.Arsonist.TriggerArsonistWin = true;
                    AdditionalTempData.winCondition = WinCondition.ArsonistWin;
                    RPCProcedure.ShareWinner(CachedPlayer.LocalPlayer.PlayerId);
                    MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareWinner, SendOption.Reliable, -1);
                    Writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(Writer);

                    Writer = RPCHelper.StartRPC(CustomRPC.SetWinCond);
                    Writer.Write((byte)CustomGameOverReason.ArsonistWin);
                    Writer.EndRPC();
                    RPCProcedure.SetWinCond((byte)CustomGameOverReason.ArsonistWin);
                    //SuperNewRolesPlugin.Logger.LogInfo("CheckAndEndGame");
                    var reason = (GameOverReason)CustomGameOverReason.ArsonistWin;
                    if (ModeHandler.IsMode(ModeId.SuperHostRoles)) reason = GameOverReason.ImpostorByKill;
                    if (AmongUsClient.Instance.AmHost)
                    {
                        CheckGameEndPatch.CustomEndGame(reason, false);
                    }
                    else
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomEndGame, SendOption.Reliable, -1);
                        writer.Write((byte)reason);
                        writer.Write(false);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }
                },
                (bool isAlive, RoleId role) => { return Arsonist.IseveryButton(); },
                () =>
                {
                    return Arsonist.IsWin(PlayerControl.LocalPlayer);
                },
                () =>
                {
                    ArsonistIgniteButton.MaxTimer = 0;
                    ArsonistIgniteButton.Timer = 0;
                },
                RoleClass.Arsonist.GetIgniteButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ArsonistIgniteButtonName"),
                showButtonText = true
            };

            SpeederButton = new(
                () =>
                {
                    Speeder.DownStart();
                    SpeederButton.MaxTimer = RoleClass.Speeder.CoolTime;
                    SpeederButton.Timer = SpeederButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Speeder; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { Speeder.EndMeeting(); },
                RoleClass.Speeder.GetButtonSprite(),

                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; },
                true,
                5f,
                () =>
                {
                    Speeder.SpeedDownEnd();
                }
            )
            {
                buttonText = ModTranslation.GetString("SpeederButtonName"),
                showButtonText = true,
                HasEffect = true
            };

            ChiefSidekickButton = new(
                () =>
                {
                    var target = SetTarget();
                    if (target && !RoleClass.Chief.IsCreateSheriff)
                    {
                        if (!target.IsImpostor())
                        {
                            MessageWriter writer = RPCHelper.StartRPC(CustomRPC.ChiefSidekick);
                            writer.Write(target.PlayerId);
                            writer.Write(target.IsClearTask());
                            RPCHelper.EndRPC(writer);
                            RPCProcedure.ChiefSidekick(target.PlayerId, target.IsClearTask());
                            RoleClass.Chief.IsCreateSheriff = true;
                        }
                        else
                        {
                            PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                            PlayerControl.LocalPlayer.RpcSetFinalStatus(FinalStatus.ChiefMisSet);
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Chief && ModeHandler.IsMode(ModeId.Default) && !RoleClass.Chief.IsCreateSheriff; },
                () =>
                {
                    return SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { },
                RoleClass.Chief.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SidekickName"),
                showButtonText = true
            };

            VultureButton = new(
                () =>
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported)
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                                    writer.Write(playerInfo.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId);
                                    RoleClass.Vulture.DeadBodyCount--;
                                    SuperNewRolesPlugin.Logger.LogInfo("DeadBodyCount:" + RoleClass.Vulture.DeadBodyCount);
                                    VultureButton.Timer = VultureButton.MaxTimer;
                                    break;
                                }
                            }
                        }
                    }
                    if (RoleClass.Vulture.DeadBodyCount < 0)
                    {
                        RPCProcedure.ShareWinner(CachedPlayer.LocalPlayer.PlayerId);
                        MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareWinner, SendOption.Reliable, -1);
                        Writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(Writer);
                        if (AmongUsClient.Instance.AmHost)
                        {
                            CheckGameEndPatch.CustomEndGame((GameOverReason)CustomGameOverReason.VultureWin, false);
                        }
                        else
                        {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomEndGame, SendOption.Reliable, -1);
                            writer.Write((byte)CustomGameOverReason.VultureWin);
                            writer.Write(false);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Vulture; },
                () =>
                {
                    return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    VultureButton.MaxTimer = RoleClass.Vulture.CoolTime;
                    VultureButton.Timer = RoleClass.Vulture.CoolTime;
                },
                RoleClass.Vulture.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("VultureButtonName"),
                showButtonText = true
            };

            ShielderButton = new(
                () =>
                {
                    MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetShielder, SendOption.Reliable, -1);
                    Writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    Writer.Write(true);
                    AmongUsClient.Instance.FinishRpcImmediately(Writer);
                    RPCProcedure.SetShielder(CachedPlayer.LocalPlayer.PlayerId, true);
                    ShielderButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    ShielderButton.MaxTimer = RoleClass.Shielder.DurationTime;
                    ShielderButton.Timer = ShielderButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Shielder; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    ShielderButton.MaxTimer = RoleClass.Shielder.CoolTime;
                    ShielderButton.Timer = RoleClass.Shielder.CoolTime;
                },
                RoleClass.Shielder.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ShielderButtonName"),
                showButtonText = true
            };

            CleanerButton = new(
                () =>
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported)
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                                    writer.Write(playerInfo.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId);
                                    CleanerButton.Timer = CleanerButton.MaxTimer;
                                    RoleClass.Cleaner.CleanMaxCount--;
                                    SuperNewRolesPlugin.Logger.LogInfo("DeadBodyCount:" + RoleClass.Cleaner.CleanMaxCount);
                                    CleanerButton.Timer = CleanerButton.MaxTimer;

                                    RoleClass.Cleaner.CoolTime = CleanerButton.Timer = CleanerButton.MaxTimer;
                                    PlayerControl.LocalPlayer.killTimer = RoleClass.Cleaner.CoolTime;
                                    break;
                                }
                            }
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Cleaner; },
                () =>
                {
                    return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    CleanerButton.MaxTimer = RoleClass.Cleaner.CoolTime;
                    CleanerButton.Timer = RoleClass.Cleaner.CoolTime;
                },
                RoleClass.Cleaner.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("CleanerButtonName"),
                showButtonText = true
            };

            MadCleanerButton = new(
                () =>
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported)
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                                    writer.Write(playerInfo.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.CleanBody(playerInfo.PlayerId);
                                    break;
                                }
                            }
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.MadCleaner; },
                () =>
                {
                    return __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    MadCleanerButton.MaxTimer = RoleClass.MadCleaner.CoolTime;
                    MadCleanerButton.Timer = RoleClass.MadCleaner.CoolTime;
                },
                RoleClass.MadCleaner.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("CleanerButtonName"),
                showButtonText = true
            };

            FreezerButton = new(
                () =>
                {
                    Freezer.DownStart();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Freezer; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { Freezer.EndMeeting(); },
                RoleClass.Freezer.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; },
                true,
                5f,
                () =>
                {
                    Freezer.SpeedDownEnd();
                }
            )
            {
                buttonText = ModTranslation.GetString("FreezerButtonName"),
                showButtonText = true,
            };

            SamuraiButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove)
                    {
                        Samurai.SamuraiKill();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Samurai && ModeHandler.IsMode(ModeId.Default) && !RoleClass.Samurai.Sword; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { Samurai.ResetCoolDown(); },
                RoleClass.Samurai.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SamuraiButtonName"),
                showButtonText = true
            };

            VentMakerButton = new(
                () =>
                {
                    RoleClass.VentMaker.VentCount++;
                    MessageWriter writer = RPCHelper.StartRPC(CustomRPC.MakeVent);
                    writer.Write(CachedPlayer.LocalPlayer.transform.position.x);
                    writer.Write(CachedPlayer.LocalPlayer.transform.position.y);
                    writer.Write(CachedPlayer.LocalPlayer.transform.position.z);
                    writer.EndRPC();
                    RPCProcedure.MakeVent(CachedPlayer.LocalPlayer.transform.position.x, CachedPlayer.LocalPlayer.transform.position.y, CachedPlayer.LocalPlayer.transform.position.z);
                    GameObject Vent = GameObject.Find("VentMakerVent" + MapUtilities.CachedShipStatus.AllVents.Select(x => x.Id).Max().ToString());

                    RoleClass.VentMaker.Vent = Vent.GetComponent<Vent>();
                    if (RoleClass.VentMaker.VentCount == 2) RoleClass.VentMaker.IsMakeVent = false;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.VentMaker && RoleClass.VentMaker.IsMakeVent; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () => { },
                RoleClass.VentMaker.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("VentMakerButtonName"),
                showButtonText = true
            };

            GhostMechanicRepairButton = new(
                () =>
                {
                    RoleClass.GhostMechanic.LimitCount--;

                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
                    {
                        if (task.TaskType == TaskTypes.FixLights)
                        {
                            RPCHelper.StartRPC(CustomRPC.FixLights).EndRPC();
                            RPCProcedure.FixLights();
                        }
                        else if (task.TaskType == TaskTypes.RestoreOxy)
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                        }
                        else if (task.TaskType == TaskTypes.ResetReactor)
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 16);
                        }
                        else if (task.TaskType == TaskTypes.ResetSeismic)
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Laboratory, 16);
                        }
                        else if (task.TaskType == TaskTypes.FixComms)
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                        }
                        else if (task.TaskType == TaskTypes.StopCharles)
                        {
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                            MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                        }
                    }
                    if (RoleClass.GhostMechanic.LimitCount <= 0)
                    {
                        GhostMechanicNumRepairText.text = "";
                    }
                },
                (bool isAlive, RoleId role) => { return !isAlive && PlayerControl.LocalPlayer.IsGhostRole(RoleId.GhostMechanic) && RoleClass.GhostMechanic.LimitCount > 0; },
                () =>
                {
                    bool sabotageActive = false;
                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
                        if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                            || (SubmergedCompatibility.isSubmerged() && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask))
                        {
                            sabotageActive = true;
                            break;
                        }
                    GhostMechanicNumRepairText.text = string.Format(ModTranslation.GetString("GhostMechanicCountText"), RoleClass.GhostMechanic.LimitCount);
                    return sabotageActive && PlayerControl.LocalPlayer.CanMove;
                },
                () => { GhostMechanicRepairButton.MaxTimer = 0f; GhostMechanicRepairButton.Timer = 0f; },
                RoleClass.GhostMechanic.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            );
            GhostMechanicNumRepairText = GameObject.Instantiate(GhostMechanicRepairButton.actionButton.cooldownTimerText, GhostMechanicRepairButton.actionButton.cooldownTimerText.transform.parent);
            GhostMechanicNumRepairText.text = "";
            GhostMechanicNumRepairText.enableWordWrapping = false;
            GhostMechanicNumRepairText.transform.localScale = Vector3.one * 0.5f;
            GhostMechanicNumRepairText.transform.localPosition += new Vector3(0f, 0.7f, 0);
            GhostMechanicRepairButton.buttonText = ModTranslation.GetString("GhostMechanicButtonName");
            GhostMechanicRepairButton.showButtonText = true;

            EvilHackerButton = new(
                () =>
                {
                    CachedPlayer.LocalPlayer.NetTransform.Halt();
                    Action<MapBehaviour> tmpAction = (MapBehaviour m) => { m.ShowCountOverlay(); };
                    FastDestroyableSingleton<HudManager>.Instance.ShowMap(tmpAction);
                    RoleClass.EvilHacker.IsMyAdmin = true;
                },
                (bool isAlive, RoleId role) => { return role == RoleId.EvilHacker; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    EvilHackerButton.MaxTimer = 0f;
                    EvilHackerButton.Timer = 0f;
                    RoleClass.EvilHacker.IsMyAdmin = false;
                },
                RoleClass.EvilHacker.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ADMINButton"),
                showButtonText = true
            };

            EvilHackerMadmateSetting = new(
                () =>
                {
                    var target = SetTarget();
                    if (!target.Data.Role.IsImpostor && target && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove && RoleClass.EvilHacker.IsCreateMadmate)
                    {
                        Madmate.CreateMadMate(target);
                        RoleClass.EvilHacker.IsCreateMadmate = false;
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.EvilHacker && ModeHandler.IsMode(ModeId.Default) && RoleClass.EvilHacker.IsCreateMadmate; },
                () =>
                {
                    return SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { },
                RoleClass.Jackal.GetButtonSprite(),
                new Vector3(-2.7f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                null,
                0,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SidekickName"),
                showButtonText = true
            };

            PositionSwapperButton = new(
                () =>
                {
                    RoleClass.PositionSwapper.SwapCount--;

                    PositionSwapperButton.actionButton.cooldownTimerText.color = new Color(255F, 255F, 255F);
                    PositionSwapper.SwapStart();
                    PositionSwapper.ResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.PositionSwapper; },
                () =>
                {
                    float swapcount = RoleClass.PositionSwapper.SwapCount;
                    PositionSwapperNumText.text = swapcount > 0
                        ? string.Format(ModTranslation.GetString("PositionSwapperNumTextName"), swapcount)
                        : string.Format(ModTranslation.GetString("PositionSwapperNumTextName"), "0");
                    return PlayerControl.LocalPlayer.CanMove
&& RoleClass.PositionSwapper.SwapCount > 0 && true && PlayerControl.LocalPlayer.CanMove;
                },
                () => { PositionSwapper.EndMeeting(); },
                RoleClass.PositionSwapper.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            );
            {
                PositionSwapperNumText = GameObject.Instantiate(PositionSwapperButton.actionButton.cooldownTimerText, PositionSwapperButton.actionButton.cooldownTimerText.transform.parent);
                PositionSwapperNumText.text = "";
                PositionSwapperNumText.enableWordWrapping = false;
                PositionSwapperNumText.transform.localScale = Vector3.one * 0.5f;
                PositionSwapperNumText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);
                PositionSwapperButton.buttonText = ModTranslation.GetString("PositionSwapperButtonName");
                PositionSwapperButton.showButtonText = true;
            };

            SecretlyKillerMainButton = new(
                () =>
                {
                    ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, RoleClass.SecretlyKiller.target);
                    SecretlyKiller.MainResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.SecretlyKiller; },
                () =>
                {
                    //クールでブロック
                    RoleClass.SecretlyKiller.MainCool = HudManagerStartPatch.SecretlyKillerMainButton.Timer;
                    RoleClass.SecretlyKiller.SecretlyCool = HudManagerStartPatch.SecretlyKillerSecretlyKillButton.Timer;
                    if (RoleClass.SecretlyKiller.SecretlyCool > 0f && RoleClass.SecretlyKiller.IsKillCoolChange) return false;
                    if (RoleClass.SecretlyKiller.MainCool > 0f) return false;

                    RoleClass.SecretlyKiller.target = SetTarget();
                    return RoleClass.SecretlyKiller.target != null
&& !RoleClass.SecretlyKiller.target.IsImpostor() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { SecretlyKiller.EndMeeting(); },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                49,
                () =>
                {
                    return !PlayerControl.LocalPlayer.CanMove;
                }
            )
            {
                buttonText = ModTranslation.GetString("FinalStatusKill"),
                showButtonText = true
            };

            SecretlyKillerSecretlyKillButton = new(
                () =>
                {
                    RoleClass.SecretlyKiller.SecretlyKillLimit--;
                    SecretlyKiller.SecretlyKill();
                    SecretlyKiller.SecretlyResetCoolDown();
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.SecretlyKiller; },
                () =>
                {
                    //テキストぉ
                    float SecretKillLimit = RoleClass.SecretlyKiller.SecretlyKillLimit;
                    SecretlyKillNumText.text = SecretKillLimit > 0
                        ? string.Format(ModTranslation.GetString("PositionSwapperNumTextName"), SecretKillLimit)
                        : string.Format(ModTranslation.GetString("PositionSwapperNumTextName"), "0");

                    if (RoleClass.SecretlyKiller.MainCool > 0f && RoleClass.SecretlyKiller.IsKillCoolChange) return false;
                    if (RoleClass.SecretlyKiller.SecretlyKillLimit < 1 || RoleClass.SecretlyKiller.SecretlyCool > 0f) return false;
                    //メイン
                    RoleClass.SecretlyKiller.target = SetTarget();
                    return RoleClass.SecretlyKiller.target != null
&& !RoleClass.SecretlyKiller.target.IsImpostor() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { SecretlyKiller.EndMeeting(); },
                __instance.KillButton.graphic.sprite,
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.KillButton,
                KeyCode.F,
                49,
                () =>
                {
                    var ma = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
                    return (ma == null || ma.IsActive) && (!RoleClass.SecretlyKiller.IsBlackOutKillCharge || !PlayerControl.LocalPlayer.CanMove);
                }
            );
            {
                SecretlyKillNumText = GameObject.Instantiate(SecretlyKillerSecretlyKillButton.actionButton.cooldownTimerText, SecretlyKillerSecretlyKillButton.actionButton.cooldownTimerText.transform.parent);
                SecretlyKillNumText.text = "";
                SecretlyKillNumText.enableWordWrapping = false;
                SecretlyKillNumText.transform.localScale = Vector3.one * 0.5f;
                SecretlyKillNumText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);
                SecretlyKillerSecretlyKillButton.buttonText = ModTranslation.GetString("SecretlyKillButtonName");
                SecretlyKillerSecretlyKillButton.showButtonText = true;
            };

            ClairvoyantButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.CanMove)
                    {
                        MapOptions.MapOption.Timer = MapOptions.MapOption.DurationTime;
                        MapOptions.MapOption.ButtonTimer = DateTime.Now;
                        ClairvoyantButton.MaxTimer = MapOptions.MapOption.CoolTime;
                        ClairvoyantButton.Timer = MapOptions.MapOption.CoolTime;
                        MapOptions.MapOption.IsZoomOn = true;
                    }
                },
                (bool isAlive, RoleId role) => { return !PlayerControl.LocalPlayer.IsAlive() && MapOptions.MapOption.ClairvoyantZoom && ModeHandler.IsMode(ModeId.Default); },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    ClairvoyantButton.MaxTimer = MapOptions.MapOption.CoolTime;
                    ClairvoyantButton.Timer = MapOptions.MapOption.CoolTime;
                    MapOptions.MapOption.IsZoomOn = false;
                },
                RoleClass.Hawk.GetButtonSprite(),
                new Vector3(-2.7f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ClairvoyantButtonName"),
                showButtonText = true
            };

            DoubleKillerMainKillButton = new(
                () =>
                {
                    if (PlayerControlFixedUpdatePatch.SetTarget() && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove)
                    {
                        ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, PlayerControlFixedUpdatePatch.SetTarget());
                        switch (PlayerControl.LocalPlayer.GetRole())
                        {
                            case RoleId.DoubleKiller:
                                DoubleKiller.ResetMainCoolDown();
                                break;
                            case RoleId.Smasher:
                                Smasher.ResetCoolDown();
                                break;
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return (isAlive && (role == RoleId.DoubleKiller) && ModeHandler.IsMode(ModeId.Default)) || (isAlive && (role == RoleId.Smasher) && ModeHandler.IsMode(ModeId.Default)); },
                () =>
                {
                    return PlayerControlFixedUpdatePatch.SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.DoubleKiller)) { DoubleKiller.EndMeeting(); }
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = FastDestroyableSingleton<HudManager>.Instance.KillButton.buttonLabelText.text,
                showButtonText = true
            };

            DoubleKillerSubKillButton = new(
                () =>
                {
                    if (PlayerControlFixedUpdatePatch.SetTarget() && RoleHelpers.IsAlive(PlayerControl.LocalPlayer) && PlayerControl.LocalPlayer.CanMove)
                    {
                        ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, PlayerControlFixedUpdatePatch.SetTarget());
                        switch (PlayerControl.LocalPlayer.GetRole())
                        {
                            case RoleId.DoubleKiller:
                                DoubleKiller.ResetSubCoolDown();
                                break;
                            case RoleId.Smasher:
                                Smasher.ResetSmashCoolDown();
                                break;
                        }
                    }
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.Smasher))
                    {
                        RoleClass.Smasher.SmashOn = true;
                    }
                },
                (bool isAlive, RoleId role) => { return (isAlive && (role == RoleId.DoubleKiller) && ModeHandler.IsMode(ModeId.Default)) || (isAlive && (role == RoleId.Smasher) && ModeHandler.IsMode(ModeId.Default) && !RoleClass.Smasher.SmashOn); },
                () =>
                {
                    return PlayerControlFixedUpdatePatch.SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.DoubleKiller)) { DoubleKiller.EndMeeting(); }
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.KillButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = FastDestroyableSingleton<HudManager>.Instance.KillButton.buttonLabelText.text,
                showButtonText = true
            };
            SuicideWisherSuicideButton = new(
                () =>
                {
                    //自殺
                    PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer);
                    PlayerControl.LocalPlayer.RpcSetFinalStatus(FinalStatus.SuicideWisherSelfDeath);
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.SuicideWisher && ModeHandler.IsMode(ModeId.Default); },
                () =>
                {
                    return true;
                },
                () => { },
                RoleClass.SuicideWisher.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("SuicideName"),
                showButtonText = true
            };

            FastMakerButton = new(
                () =>
                {
                    var target = SetTarget();
                    //マッド作ってないなら
                    if (target && PlayerControl.LocalPlayer.CanMove && !RoleClass.FastMaker.IsCreatedMadMate)
                    {
                        PlayerControl.LocalPlayer.RpcShowGuardEffect(target); // 守護エフェクトの表示
                        Madmate.CreateMadMate(target);//くるぅにして、マッドにする
                        RoleClass.FastMaker.IsCreatedMadMate = true;//作ったことに
                        SuperNewRolesPlugin.Logger.LogInfo("[FastMakerButton]マッドを作ったから普通のキルボタンに戻すよ!");
                    }
                },
                //マッドを作った後はカスタムキルボタンを消去する
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.FastMaker && !RoleClass.FastMaker.IsCreatedMadMate && ModeHandler.IsMode(ModeId.Default); },
                () =>
                {
                    return SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () => { },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                //マッドを作る前はキルボタンに擬態する
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("KillName"),
                showButtonText = true
            };

            ToiletFanButton = new(
                () => { RPCHelper.RpcOpenToilet(); },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.ToiletFan; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    ToiletFanButton.MaxTimer = RoleClass.ToiletFan.ToiletCool;
                    ToiletFanButton.Timer = RoleClass.ToiletFan.ToiletCool;
                },
                RoleClass.ToiletFan.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("ToiletName"),
                showButtonText = true
            };

            ButtonerButton = new(
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.EvilButtoner))
                        RoleClass.EvilButtoner.SkillCount--;
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.NiceButtoner))
                        RoleClass.NiceButtoner.SkillCount--;
                    else
                        return;
                    EvilButtoner.EvilButtonerStartMeeting(PlayerControl.LocalPlayer);
                },
                (bool isAlive, RoleId role) => { return isAlive && role is RoleId.EvilButtoner or RoleId.NiceButtoner; },
                () =>
                {
                    if (!PlayerControl.LocalPlayer.CanMove) return false;
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.NiceButtoner)) return RoleClass.NiceButtoner.SkillCount != 0;
                    else if (PlayerControl.LocalPlayer.IsRole(RoleId.EvilButtoner)) return RoleClass.EvilButtoner.SkillCount != 0;
                    return false;
                },
                () =>
                {
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.NiceButtoner) && RoleClass.NiceButtoner.SkillCount == 0) return;
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.EvilButtoner) && RoleClass.EvilButtoner.SkillCount == 0) return;
                    //イビルボタナーなら
                    if (PlayerControl.LocalPlayer.IsRole(RoleId.EvilButtoner))
                    {
                        ButtonerButton.MaxTimer = RoleClass.EvilButtoner.CoolTime;
                        ButtonerButton.Timer = RoleClass.EvilButtoner.CoolTime;
                    }
                    //ナイスボタナーなら
                    else
                    {
                        ButtonerButton.MaxTimer = RoleClass.NiceButtoner.CoolTime;
                        ButtonerButton.Timer = RoleClass.NiceButtoner.CoolTime;
                    }
                },
                RoleClass.EvilButtoner.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }

            )
            {
                buttonText = ModTranslation.GetString("ButtonerButtonName"),
                showButtonText = true
            };

            RevolutionistButton = new(
                () =>
                {
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Revolutionist; },
                () =>
                {
                    return true;
                },
                () =>
                {
                    RevolutionistButton.MaxTimer = RoleClass.Revolutionist.CoolTime;
                    RevolutionistButton.Timer = RoleClass.Revolutionist.CoolTime;
                },
                RoleClass.Moving.GetNoSetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("RevolutionistButtonName"),
                showButtonText = true,
                color = Color.yellow
            };

            SuicidalIdeationButton = new CustomButton(
                () => { },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.SuicidalIdeation; },
                () =>
                {
                    return true;
                },
                () =>
                {
                    SuicidalIdeationButton.MaxTimer = CustomOptions.SuicidalIdeationTimeLeft.GetFloat();
                    SuicidalIdeationButton.Timer = SuicidalIdeationButton.MaxTimer;
                },
                RoleClass.SuicidalIdeation.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () =>
                {
                    return RoleClass.IsMeeting;
                }
            )
            {
                buttonText = ModTranslation.GetString("SuicidalIdeationButtonName"),
                showButtonText = true
            };

            HitmanKillButton = new(
                () =>
                {
                    PlayerControl target = SetTarget();
                    if (ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, target) == ModHelpers.MurderAttemptResult.PerformKill)
                    {
                    }
                    if (RoleClass.Hitman.Target.PlayerId != target.PlayerId)
                    {
                        Roles.Neutral.Hitman.LimitDown();
                    }
                    else
                    {
                        Roles.Neutral.Hitman.KillSuc();
                    }
                    RoleClass.Hitman.UpdateTime = CustomOptions.HitmanChangeTargetTime.GetFloat();
                    RoleClass.Hitman.ArrowUpdateTime = 0;
                    Roles.Neutral.Hitman.SetTarget();
                    HitmanKillButton.Timer = HitmanKillButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Hitman; },
                () =>
                {
                    return SetTarget() && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    Roles.Neutral.Hitman.EndMeeting();
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () => { return false; }
            )
            {
                buttonText = FastDestroyableSingleton<HudManager>.Instance.KillButton.buttonLabelText.text,
                showButtonText = true
            };

            MatryoshkaButton = new(
                () =>
                {
                    if (MatryoshkaButton.isEffectActive)
                    {
                        RoleClass.Matryoshka.WearLimit--;
                        Roles.Impostor.Matryoshka.RpcSet(null, false);
                        MatryoshkaButton.MaxTimer = CustomOptions.MatryoshkaCoolTime.GetFloat();
                        MatryoshkaButton.Timer = MatryoshkaButton.MaxTimer;
                        return;
                    }
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                            Vector2 truePosition2 = component.TruePosition;
                            Logger.Info((!component.Reported).ToString() + $"{truePosition2} : {truePosition} : {Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance} : {Vector2.Distance(truePosition2, truePosition)} : {PlayerControl.LocalPlayer.MaxReportDistance}");
                            if (!component.Reported)
                            {
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    if (RoleClass.Matryoshka.Data.Values.All(data =>
                                    {
                                        return data == null || data.ParentId != component.ParentId;
                                    }))
                                    {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);
                                        Roles.Impostor.Matryoshka.RpcSet(playerInfo.Object, true);
                                        RoleClass.Matryoshka.MyKillCoolTime += CustomOptions.MatryoshkaAddKillCoolTime.GetFloat();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Matryoshka && RoleClass.Matryoshka.WearLimit > 0; },
                () =>
                {
                    if (RoleClass.Matryoshka.IsLocalOn)
                    {
                        MatryoshkaButton.Sprite = RoleClass.Matryoshka.TakeOffButtonSprite;
                        MatryoshkaButton.buttonText = ModTranslation.GetString("MatryoshkaTakeOffButtonName");
                    }
                    else
                    {
                        MatryoshkaButton.Sprite = RoleClass.Matryoshka.PutOnButtonSprite;
                        MatryoshkaButton.buttonText = ModTranslation.GetString("MatryoshkaPutOnButtonName");
                    }
                    MatryoshkaButton.HasEffect = __instance.ReportButton.graphic.color == Palette.EnabledColor;
                    return (__instance.ReportButton.graphic.color == Palette.EnabledColor || RoleClass.Matryoshka.IsLocalOn) && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    MatryoshkaButton.MaxTimer = CustomOptions.MatryoshkaCoolTime.GetFloat();
                    MatryoshkaButton.Timer = MatryoshkaButton.MaxTimer;
                    MatryoshkaButton.effectCancellable = true;
                    MatryoshkaButton.EffectDuration = CustomOptions.MatryoshkaWearTime.GetFloat();
                    if (RoleClass.Matryoshka.IsLocalOn)
                    {
                        RoleClass.Matryoshka.WearLimit--;
                    }
                    Roles.Impostor.Matryoshka.RpcSet(null, false);
                },
                RoleClass.Matryoshka.PutOnButtonSprite,
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () =>
                {
                    return false;
                },
                true,
                5f,
                () =>
                {
                    RoleClass.Matryoshka.WearLimit--;
                    Roles.Impostor.Matryoshka.RpcSet(null, false);
                    MatryoshkaButton.MaxTimer = CustomOptions.MatryoshkaCoolTime.GetFloat();
                    MatryoshkaButton.Timer = MatryoshkaButton.MaxTimer;
                }
                )
            {
                buttonText = ModTranslation.GetString("MatryoshkaPutOnButtonName"),
                showButtonText = true
            };

            NunButton = new(
                () =>
                {
                    MessageWriter writer = RPCHelper.StartRPC(CustomRPC.UncheckedUsePlatform);
                    writer.Write((byte)255);
                    writer.Write(false);
                    writer.EndRPC();
                    RPCProcedure.UncheckedUsePlatform((byte)255, false);
                    NunButton.Timer = NunButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Nun; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    NunButton.MaxTimer = CustomOptions.NunCoolTime.GetFloat();
                    NunButton.Timer = NunButton.MaxTimer;
                },
                RoleClass.Nun.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () =>
                {
                    return false;
                }
            )
            {
                buttonText = ModTranslation.GetString("NunButtonName"),
                showButtonText = true
            };

            PsychometristButton = new(
                () =>
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported)
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2 - new Vector2(0.15f, 0.2f), truePosition) <= RoleClass.Psychometrist.Distance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    RoleClass.Psychometrist.CurrentTarget = component;
                                }
                            }
                        }
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Psychometrist; },
                () =>
                {
                    if (PsychometristButton.isEffectActive)
                    {
                        if (RoleClass.Psychometrist.CurrentTarget == null || __instance.ReportButton.graphic.color != Palette.EnabledColor || Vector2.Distance(RoleClass.Psychometrist.CurrentTarget.TruePosition - new Vector2(0.15f, 0.2f), CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition()) > RoleClass.Psychometrist.Distance)
                        {
                            RoleClass.Psychometrist.CurrentTarget = null;
                            PsychometristButton.Timer = 0f;
                            PsychometristButton.isEffectActive = false;
                        }
                    }
                    bool Is = __instance.ReportButton.graphic.color == Palette.EnabledColor && PlayerControl.LocalPlayer.CanMove;
                    if (!Is) return false;
                    Is = false;
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported)
                            {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2 - new Vector2(0.15f, 0.2f), truePosition) <= RoleClass.Psychometrist.Distance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                                {
                                    Is = true;
                                    break;
                                }
                            }
                        }
                    }
                    return Is;
                },
                () =>
                {
                    PsychometristButton.MaxTimer = CustomOptions.PsychometristCoolTime.GetFloat();
                    PsychometristButton.Timer = PsychometristButton.MaxTimer;
                    PsychometristButton.effectCancellable = false;
                    PsychometristButton.EffectDuration = CustomOptions.PsychometristReadTime.GetFloat();
                    PsychometristButton.isEffectActive = false;
                },
                RoleClass.Psychometrist.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () =>
                {
                    return false;
                },
                true,
                CustomOptions.PsychometristReadTime.GetFloat(),
                () =>
                {
                    if (RoleClass.IsMeeting) return;
                    Roles.CrewMate.Psychometrist.ClickButton();
                }
            )
            {
                buttonText = ModTranslation.GetString("PsychometristButtonName"),
                showButtonText = true
            };

            PartTimerButton = new(
                () =>
                {
                    MessageWriter writer = RPCHelper.StartRPC(CustomRPC.PartTimerSet);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write(SetTarget().PlayerId);
                    writer.EndRPC();
                    RPCProcedure.PartTimerSet(CachedPlayer.LocalPlayer.PlayerId, SetTarget().PlayerId);
                    PartTimerButton.Timer = PartTimerButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.PartTimer && !RoleClass.PartTimer.IsLocalOn; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && SetTarget();
                },
                () =>
                {
                    PartTimerButton.MaxTimer = CustomOptions.PartTimerCoolTime.GetFloat();
                    PartTimerButton.Timer = PartTimerButton.MaxTimer;
                },
                RoleClass.PartTimer.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () =>
                {
                    return false;
                }
            )
            {
                buttonText = ModTranslation.GetString("PartTimerButtonName"),
                showButtonText = true
            };

            PainterButton = new(
                () =>
                {
                    Roles.CrewMate.Painter.SetTarget(SetTarget());
                    PainterButton.Timer = PainterButton.MaxTimer;
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Painter && RoleClass.Painter.CurrentTarget == null; },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove && SetTarget();
                },
                () =>
                {
                    PainterButton.MaxTimer = CustomOptions.PainterCoolTime.GetFloat();
                    PainterButton.Timer = PainterButton.MaxTimer;
                },
                RoleClass.Painter.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () =>
                {
                    return false;
                }
            )
            {
                buttonText = ModTranslation.GetString("PainterButtonName"),
                showButtonText = true
            };

            StefinderKillButton = new(
                () =>
                {
                    MessageWriter Writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.StefinderIsKilled, SendOption.Reliable, -1);
                    Writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(Writer);

                    RPCProcedure.StefinderIsKilled(PlayerControl.LocalPlayer.PlayerId);
                    RoleClass.Stefinder.IsKill = true;
                    ModHelpers.CheckMuderAttemptAndKill(PlayerControl.LocalPlayer, RoleClass.Stefinder.target);
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Stefinder && !RoleClass.Stefinder.IsKill; },
                () =>
                {
                    RoleClass.Stefinder.target = SetTarget();
                    return RoleClass.Stefinder.target != null && PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    StefinderKillButton.MaxTimer = CustomOptions.StefinderKillCoolDown.GetFloat();
                    StefinderKillButton.Timer = StefinderKillButton.MaxTimer;
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1, 0),
                __instance,
                __instance.KillButton,
                KeyCode.Q,
                8,
                () =>
                {
                    return !PlayerControl.LocalPlayer.CanMove;
                }
            )
            {
                buttonText = ModTranslation.GetString("FinalStatusKill"),
                showButtonText = true
            };

            DoppelgangerButton = new(
                () => { Roles.Impostor.Doppelganger.DoppelgangerShape(); },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Doppelganger && ModeHandler.IsMode(ModeId.Default); },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () =>
                {
                    DoppelgangerButton.MaxTimer = RoleClass.Doppelganger.CoolTime;
                    DoppelgangerButton.Timer = RoleClass.Doppelganger.CoolTime;
                },
                RoleClass.Doppelganger.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; }
            )
            {
                buttonText = ModTranslation.GetString("DoppelgangerButtonName"),
                showButtonText = true
            };
            RoleClass.Doppelganger.DoppelgangerDurationText = GameObject.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText.transform.parent);
            RoleClass.Doppelganger.DoppelgangerDurationText.text = "";
            RoleClass.Doppelganger.DoppelgangerDurationText.enableWordWrapping = false;
            RoleClass.Doppelganger.DoppelgangerDurationText.transform.localScale = Vector3.one * 0.5f;
            RoleClass.Doppelganger.DoppelgangerDurationText.transform.localPosition += new Vector3(-2.575f, -0.95f, 0);

            Roles.Impostor.Conjurer.SetupCustomButtons(__instance);

            Roles.Neutral.GM.CreateButton(__instance);

            CamouflagerButton = new(
                () =>
                {
                    if (CamouflagerButton.isEffectActive)
                    {
                        Roles.Impostor.Camouflager.RpcResetCamouflage();
                        CamouflagerButton.MaxTimer = RoleClass.Camouflager.CoolTime;
                        CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                    }
                    else
                    {
                        Roles.Impostor.Camouflager.RpcCamouflage();
                    }
                },
                (bool isAlive, RoleId role) => { return isAlive && role == RoleId.Camouflager && ModeHandler.IsMode(ModeId.Default); },
                () => { return PlayerControl.LocalPlayer.CanMove; },
                () =>
                {
                    CamouflagerButton.MaxTimer = RoleClass.Camouflager.CoolTime;
                    CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                    CamouflagerButton.effectCancellable = false;
                    CamouflagerButton.EffectDuration = CustomOptions.CamouflagerDurationTime.GetFloat();
                    CamouflagerButton.HasEffect = true;
                },
                RoleClass.Camouflager.GetButtonSprite(),
                new Vector3(-1.8f, -0.06f, 0),
                __instance,
                __instance.AbilityButton,
                KeyCode.F,
                49,
                () => { return false; },
                true,
                5f,
                () =>
                {
                    Logger.Info("効果終了のお知らせ");
                    Roles.Impostor.Camouflager.RpcResetCamouflage();
                    CamouflagerButton.MaxTimer = RoleClass.Camouflager.CoolTime;
                    CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                }
            )
            {
                buttonText = ModTranslation.GetString("CamouflagerButtonName"),
                showButtonText = true
            };

            SetCustomButtonCooldowns();
        }
    }
}