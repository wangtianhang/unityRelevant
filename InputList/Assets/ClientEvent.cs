/********************************************************************
	created:	2014/11/14
	created:	14:11:2014   16:15
	author:		wangtianhang(690879430@qq.com)
	
	purpose:	游戏中用到的事件
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ClientEventId
{
    //================   战场  ===================
		LeaderCreate=1, // 武将创建
        BuildingCreate = 2,
		LeaderDie, // 武将死亡
        LeaderReborn,
		//LeaderScreenPosition,
        BuildingDie,
		LeaderHurt, // bloodword hp
		AngryNum,
		CheckCollistion,
        BarnTaken,

		//FlyPicture,
		//SkillYellowCircle,
		//CastSpell,
		//BreakCameraTrace,
		//ClickOtherPlace,
   
		LeaderCure,
		LeaderSkillHurt,
        LeaderShield,
        LeaderAnger,

        //ShowBattleDialog,

        //ShowResultPanel,

        UpdateLeaderPos,

        HeadIconPlayBattleAnimation,
        ShowSkillUINew,
        SkillOkClick,
        SkillCancelClick,

        BattleLastTime,
        BattleResult,
    /// <summary>
    /// 创建雷达队伍三角标
    /// </summary>
        CreateTeamItem,
    /// <summary>
    /// 更新雷达队伍位置
    /// </summary>
        UpdateTeamItem,
    /// <summary>
    /// 销毁队伍三角
    /// </summary>
        DestroyTeamIteam,
    /// <summary>
    /// 创建大本营
    /// </summary>
        CreateCityItem,
    /// <summary>
    /// 销毁大本营
    /// </summary>
        DestroyCityItem,
    /// <summary>
    /// 创建相机视野框
    /// </summary>
        CreateCameraViewItem,
    /// <summary>
    /// 更新相机视野框
    /// </summary>
        UpdateCameraViewTrans,
    /// <summary>
    /// 技能ui表现，武将立绘等
    /// </summary>
        ShowCastSkillUiEffect,
    /// <summary>
    /// 技能飘字
    /// </summary>
        PopSkillLabel,
		 /// <summary>
    /// 星级
    /// </summary>
        Star,
        /// <summary>
        /// 波数
        /// </summary>
        Wave,
        BuildingDamage,

    /// <summary>
    /// 施放技能标记
    /// </summary>
        CastSkill,

        ShowCpuCostLabel,
        
        //HeadIconClick,
        //HeadIconSkillYellowCircle,
        TimePause,
        BattleModuleInitEnd,
        BattleIntroCameraEnd,
        PlayDialogEnd,
        ShowPassiveSkillLabel,
        ShowDodgeLabel,

        HpKeepRecovery,
        LevelArmyAppear,

        ShowMainEnemyLeaderAppear,

        BuffIcon,
        
        // ===========被动技能检测==========
        BeforeCalculateCommonHurt,
        AfterCalculateCommonHurt,
        BeforeCalculateSkillHurt,
        AfterCalculateSkillHurt,

        BeforeMakeCommonHurt,
        AfterMakeCommonHurt,
        BeforeMakeSkillHurt,
        AfterMakeSkillHurt,
        BeforeCastSpell,
        AfterCastSpell,
        

        BeforeBackAttackCalculateHurt,
        AfterBackAttackCalculateHurt,
        BeforeArmyFeature,
        AfterArmyFeature,
        TeamEnterAtkState,
        TeamLeaveAtkState,
        KillTarget,
        BattleAddTeam,

        ShowBackAttackLabel,

        AfterPlayMemberAction,
        BeforePlayMemberAction,
        //BattleBuildingDeath,
        // ============输入============
        OneFingerTouchDown,
        OneFingerTouchDrag,
        OneFingerTouchUp,

        TwoFingerTouchDown,
        TwoFingerTouchDrag,
        TwoFingerTouchUp,

        //===========系统===============
        //ShowLeaderList,
        //ShowLeaderGrow,
        //ShowEquipGrow,
        //ShowBuildUI,
        NetConnect,

        PlayHarvestCoinEffect,
        PlayHarvestFoodEffect,
        CityOpenDataRefresh,

        //ShowAreaMap,
        //ShowBigMap,

        //ShowEmbattleUI,
        BuildUIInitEnd,

        BuildingClick,
        PlayUpdateEffect,
        PlayUpdateEndEffect,
        RefreshSingleBuild,

        //RefreshBuildHeadText,
        ShowHeadIcon,
        SetBuildingTopScreenPos,
        SetBuildingScreenPos,

        ShowMessage,
        ShowPersistMessage,
        AnotherDayRefresh,
        CheckBuildCombine,
        EnalbeEventDispather,
        MoveBuildCamera,
        NewBeeBattleGuideUI,
        EnableBuildDragGround,
        NewBeeBattleGuideHand,
        NewBeeBattleGuidePanel,
        
        RefreshBattlePower,
        HideOrShowBuild3dScene,
        EndShowAnimationCallback,
        
        LoadTextureFaild,
        // =================datapool数据变化=======================
        MoneyDataChange,
        PersonDataChange,
        CoinDataChange,
        FoodDataChange,
        HammerDataChange,
        StrengthDataChange,
		PointDataChange,
        CampaignDataChange,
        //==================服务器回调=====================
            
        //==================IOS内购=================================
        PurchaseFailedCallback,
        PurchaseCancelCallback,
        PurchaseSuccessCallback,
        //==================IOS内购=================================

        //=========================IOS登录系统=============================
        IOSQuickRegister,
        IOSQuickLogin,
        IOSRegister,
        IOSLogin,
        //=========================IOS登录系统=============================
// 		NetMessage = 20000,
// 		PlayerLoginCallback,
// 		GetPlayerInfoCallback,
// 		HeatBeatCallback,
// 		//FirstBeatCallback,
// 		UpdateBuildingCallback,
// 		ConstructBuildingCallback,
// 		SpeedUpBuildCallback,
// 		UpdateOrConstructEndCallback,

// 
// 		PlayerResCallback_S2C,
// 
// 		ShopGetCallback,
// 		FarmGetCallback,
// 
//         AreaUnlockCallback,
//         CityAttackCallback,
// 
//         CityOpenCallback,
//         ChangeCityOwnerCallback,
//         CityGetCallback,
// 
//         ClientServerErrorCallback,
        GetPlayerInfoCallback,
        Building_CreateCallbakc,
        Building_UpdateCallbakc,
        Shop_GetCallback,
        Farm_GetCallback,
        BuildCreateOrUpgrageEndCallback,

        ChangeCityOwnerCallback,
        CityAreaUnlockCallback,

        UpgrageLeaderCallback,
        LeaderEatLeaderCallback,
        WearEquipCallback,
        UpgradeEquipCallback,
        CompositeEquipCallback,
        PveSweepCallback,
        PveAttackCallback,
        ArmyActivityCallback,
 //       FlowerConquerCallback,        
        PveGetChapterCallback,
        CityAttackCallback,
        ArenaCallBack,
        BuyStrenthCallback,
        AnnounceCallback,
        CommitTaskCallback,
        MiddleTimeGetStrengthCallback,
        ATKFormationChangedCallback,
        DefFormationChangedCallback,
        RankFormationChangedCallback,
		BuyMallCallBack,
		SwearHeroCallBack,
        Tech_Upgrage,
        Gift_Upgrage,
        RevengeBeginCallback,
		ChangeForceGuideCallBack,
		ChangeTiggerGuideCallBack,
		TiggerGuideHasBeenCall,
        CycleCityResCallBack,
		ChangeArenaRewardStatue,
		ChangeArenaGetRewardStaute,
		RefreshChanllengeTime,
		ArenaMatchPlayer,
		OtakuRechargeSuccess,
        StartRechargeCallback,
        ///<summary> 玩家升级</summary>
        playerLevelUp,
        /// <summary> 签到回调 </summary>
        SignInCallback,
        /// <summary> 聊天 </summary>
        chat,
        /// <summary> 世界消息 </summary>
        SysMsg,
        /// <summary> 系统消息 </summary>
        DelSysMsg,
        /// <summary> 删除系统消息 </summary>
        SysMsg_Swear,
        /// <summary> 祈愿消息 </summary>
        NoticeMsg,
        /// <summary> 一次性消息 </summary>    

        /// <summary> 征战 </summary>
        SysMsg_CityAttack,
        /// <summary> 武将进化 </summary>
        LeaderEvolution, 
        /// <summary> 武将升星 </summary>
        LeaderUpStar,
        /// <summary>武将属性变化</summary>
        LeaderAttributeChanged,
		CompositeLeaderCallback,

        UpdateDonjonCallback,
        /// <summary>材料合成 </summary>
        SynthesisCallback,
        /// <summary> 武将天赋升级 </summary>
        UpgradeLeaderTalentSkill,
		/// <summary>
		/// 积分商城数据初始化
		/// </summary>
		ScoreMallCallback,
		/// <summary> 积分商城兑换商品 </summary>
		BuyScoreGoodsCallBack,
		/// <summary> 积分商城刷新 </summary>
		RefreshScoreMallCallBack,
        /// <summary> 竞技场排名刷新 </summary>
        RefreshRankCallbcak,
        /// <summary> 竞技场兑换 </summary>
        ArenaExchange,
        /// <summary> 竞技场兑换 </summary>
        ArenaExchangeRefresh,
        /// <summary> 碎片转换 </summary>
        DebrisConvers,
        /// <summary> 重置兵种属性和技能</summary>
        ResetTechAndGift,
        /// <summary> 激活码兑换</summary>
        ShopExchange,
        /// <summary>精英关卡重置</summary>
        ResetHeroLevelCallback,
        /// <summary>领取月卡</summary>
        GetMonthCardRewardCallback,
        ///<summary>活动次数重置</summary>
        ResetActivityNumCallback,
        /// <summary>Vip礼包购买</summary>
        BuyVipRewardCallback,
		/// <summary>领取运营活动奖励</summary>
        GetOperatingActivityCallback,
		BuyCityAtkNumEnd,
        /// <summary>奉行升级</summary>
        RetinueUpgrade,
		/// <summary>保存奉行</summary>
		SaveRetinue,
        /// <summary>装备升星</summary>
        EquipRisingStar,
        /// <summary>装备降星</summary>
        EquipFallingStar,
		/// <summary>升级辅助建筑</summary>
		UpgradeFunctionsBuild,





    /*----------家族技能相关--------*/
    /// <summary>丰臣家族队伍改变</summary>
    FengChenTeamFamilyChange,
    /// <summary>武田家族队伍改变</summary>
    WuTianTeamFamilyChange,
    /// <summary>织田家族队伍改变</summary>
    ZhiTianTeamFamilyChange,
    /// <summary>上杉家族队伍改变</summary>
    ShangShanTeamFamilyChange,
    /// <summary>德川家族队伍技能释放</summary>
    DeChuanTeamFamilySkillChange,
    /// <summary>德川家族队伍队伍改变</summary>
    DeChuanTeamFamilyChange,
    /// <summary>点开了</summary>
    OpenFamilySkillInfo,

    /// <summary>替换辅助建筑</summary>
    ChangeFunctionsBuild,
		/// <summary>收获武魂</summary>
		HarvestWuhun,
		
}