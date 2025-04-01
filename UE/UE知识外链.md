# 引擎入门

[为Unity开发者准备的虚幻引擎指南](https://dev.epicgames.com/documentation/en-us/unreal-engine/unreal-engine-for-unity-developers)



# 黑神话悟空解包

## 官网

[FModel](https://fmodel.app/)

仓库：https://github.com/4sval/FModel/releases

## 教程

[《黑神话：悟空》MOD制作：模型替换教程 - 哔哩哔哩 (bilibili.com)](https://www.bilibili.com/read/cv37491469/)

[使用FModel提取《黑神话:悟空》的资产 - paw5zx - 博客园 (cnblogs.com)](https://www.cnblogs.com/paw5zx/p/18372743)

[使用FModel 提取黑神话悟空模型_黑神话悟空模型提取-CSDN博客](https://blog.csdn.net/aerodancing1984/article/details/141617497)

[使用FModel提取UE4/5游戏资产 - paw5zx - 博客园 (cnblogs.com)](https://www.cnblogs.com/paw5zx/p/18304354)

## 黑神话悟空相关备注

### 主要知识点

- Fmodel更新（非必需）要开梯子,可在打开Fmodel之前先开启猎豹加速器，领好每日免费加速
- 模型文件多为`SK_`或`SM_`开头
- 3D Viewer视角控制类似Unity，WASDQE+鼠标左键
- wem格式的音效提取wav，可以用foobar2000播放器，安装插件[foobar2000: Components Repository - vgmstream decoder](https://www.foobar2000.org/components/view/foo_input_vgmstream)，如导出MP3需选择内置lame.exe
- b1/Content/00Main/Design/Units 里面包括了所有角色的数据信息，可以据此找到它们的模型（可以在文本里搜SK_）和其他信息（如材质贴图等）

### 资产查找记录

| 游戏内模型   | 关键词或路径                                                 |
| ------------ | ------------------------------------------------------------ |
| 音效         | b1/Content/00Main/Audio/SoundBank/Media                      |
| 大头         | b1/Content/00MainHZ/Characters/Enemy/GYCY/GYCY_YanJianXi_01/Meshes/SK_GYCY_YanJIanXi_01_base<br />b1/Content/00MainHZ/Characters/HFS/GYCY_YanKanXi/Materials/MF_GYCY_YanKanXi_01_shenti_01 |
| 千手观音     | b1/Content/00MainHZ/Environment/Buildings/Meshs/LYS/Test/Dazu_01/QSGY_FIX |
| 绿蛙boss     | b1/Content/00MainHZ/Characters/Enemy/GYCY/GYCY_Wa_01/Meshes/SK_GYCY_Wa_01 |
| 熊Boss及毛发 | b1/Content/00MainHZ/Characters/Enemy/GYCY/GYCY_Xiong_02      |
| 鼠狙击手带弩 | b1/Content/00MainHZ/Characters/Enemy/HFM/HFM_ShuNuShou_01b/Mesh/SK_HFM_ShuNuShou_01b |
| 主角相关     | b1/Content/00MainHZ/Characters/Wukong                        |
| 武器         | b1/Content/00MainHZ/Characters/Weapons_Mesh                  |
| 道具         | b1/Content/00MainHZ/Item                                     |

### 角色用词参考备注（摘抄自mod代码）

```lua
--第一章
['101'] = "/Game/00Main/Design/Units/GYCY/TAMER_wxs_baiyi_01.TAMER_wxs_baiyi_01_C", --白衣秀士
['102'] = "/Game/00Main/Design/Units/GYCY/TAMER_gycy_yanjianxi_01a.TAMER_gycy_yanjianxi_01a_C", --金池长老
['103'] = "/Game/00Main/Design/Units/GYCY/TAMER_gycy_hfdw.TAMER_gycy_hfdw_C",     --黑风大王（武僧形态）
['104'] = "/Game/00Main/Design/Units/GYCY/TAMER_gycy_xiong_02.TAMER_gycy_xiong_02_C", --黑熊精

--第二章
['201'] = "/Game/00Main/Design/Units/HFM/TAMER_HFM_Suoyang_01a.TAMER_HFM_Suoyang_01a_C", --地狼
['202'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_hou_01a.TAMER_hfm_hou_01a_C", --百目真人
['203'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_hu_01.TAMER_hfm_hu_01_C", --黄金国虎先锋
['205'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_shawuliang_01a.TAMER_hfm_shawuliang_01a_C", --虎伥
['207'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_hfds_01a.TAMER_hfm_hfds_01a_C", --黄风大圣
['208'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_chuilong_01a.TAMER_lys_chuilong_01a_C", --小郦龙

--第三章
['301'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_wudulong_03a.TAMER_lys_wudulong_03a_C", --青背龙
['302'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_xuehou.TAMER_lys_xuehou_C", --赤尻马猴
['303'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_kjldragon.TAMER_lys_kjldragon_C", --亢金龙
['304'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_kjlwoman.TAMER_lys_kjlwoman_C", --亢金星君
['305'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_mo1.TAMER_lys_mo1_C", --魔将·妙音
['307'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_hu_wind_01.TAMER_hfm_hu_wind_01_C", --寅虎

--第四章
['402'] = "/Game/00Main/Design/Units/PSD/TAMER_psd_zhizhujing_02.TAMER_psd_zhizhujing_02_C", --二姐
['403'] = "/Game/00Main/Design/Units/PSD/TAMER_wxs_wulong.TAMER_wxs_wulong_C", --黑手道人
['404'] = "/Game/00Main/Design/Units/PSD/TAMER_psd_jiachongzongbing_02.TAMER_psd_jiachongzongbing_02_C", --虫总兵
['405'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_dage.TAMER_lys_dage_C", --小黄龙

--第五章

--第六章
['601'] = "/Game/00Main/Design/Units/MGD/TAMER_mgd_yangjian_01.TAMER_mgd_yangjian_01_C", --二郎显圣真君
['602'] = "/Game/00Main/Design/Units/MGD/TAMER_mgd_erlangshen_01.TAMER_mgd_erlangshen_01_C", --二郎神（青狮形态法天象地）
['604'] = "/Game/00Main/Design/Units/MGD/TAMER_mgd_jsds.TAMER_mgd_jsds_C", --大圣残躯
['605'] = "/Game/00Main/Design/Units/MGD/TAMER_mgd_jsds_p2.TAMER_mgd_jsds_p2_C", --大圣残躯P2

-- ['000'] = "/Game/00Main/Design/Units/NPC/TAMER_NPC_ZhuBaJie_01B_Follow.TAMER_NPC_ZhuBaJie_01B_Follow_C", --猪八戒（友军）
-- ['001'] = "/Game/00Main/Design/Units/NPC/TAMER_npc_zhubajie_02a.TAMER_npc_zhubajie_02a_C", --猪八戒巨型黑猪形态（友军）

-- ['105'] = "/Game/00Main/Design/Units/LYS/TAMER_lys_wudulong_02a.TAMER_lys_wudulong_02a_C", --赤冉龙
-- ['106'] = "/Game/00Main/Design/Units/LSH/TAMER_lsh_yanjianxi_online.TAMER_lsh_yanjianxi_online_C", --幽魂
-- ['204'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_bashanhu_01.TAMER_hfm_bashanhu_01_C", --疯虎
-- ['206'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_shuwang_01a.TAMER_hfm_shuwang_01a_C", --鼠大郎
-- ['306'] = "/Game/00Main/Design/Units/LYS/TAMER_LYS_LaoSeng_01.TAMER_LYS_LaoSeng_01_C", --不空
-- ['603'] = "/Game/00Main/Design/Units/MGD/TAMER_mgd_yuan.TAMER_mgd_yuan_C", --大圣石猿
-- ['406'] = "/Game/00Main/Design/Units/PSD/TAMER_psd_xiezijing_og.TAMER_psd_xiezijing_og_C", --毒敌大王
-- ['501'] = "/Game/00Main/Design/Units/HFM/TAMER_hfm_mamian_01a.TAMER_hfm_mamian_01a_C", --马哥（火焰山土地召唤出的黑泥形态，不是boss）
-- ['401'] = "/Game/00Main/Design/Units/PSD/TAMER_psd_youyan_02.TAMER_psd_youyan_02_C", --百足虫

--另一段代码，仅关键词值得参考
['101'] = {
    Name = "广谋_袖蛇",
    Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_she_01.MC_10_gycy_she_01",
    SkillID = 11311,
    RecoverID = 10199
  },
['102'] = {
        Name = "波波浪浪_巧舌",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_wa_01.MC_10_gycy_wa_01",
        SkillID = 11212,
        RecoverID = 10199
    },
['103'] = {
        Name = "幽魂_禅击",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_yanjianxi.MC_10_gycy_yanjianxi",
        SkillID = 11313,
        RecoverID = 10199
    },
['104'] = {
        Name = "鼠司空_奉祭",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_20_hfm_bxa.MC_20_hfm_bxa",
        SkillID = 11314,
        RecoverID = 10199
    },
['105'] = {
        Name = "百目真人_沸血",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_20_hfm_hou_01.MC_20_hfm_hou_01",
        SkillID = 11315,
        RecoverID = 10199
    },
['106'] = {
        Name = "虎伥_快刀",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_20_hfm_shawuliang.MC_20_hfm_shawuliang",
        SkillID = 11317,
        RecoverID = 10199
    },
['107'] = {
        Name = "不空_瞬身",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_30_lys_laoseng.MC_30_lys_laoseng",
        SkillID = 11320,
        RecoverID = 10199
    },
['108'] = {
        Name = "无量蝠_霜刀",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_30_lys_mo3.MC_30_lys_mo3",
        SkillID = 11322,
        RecoverID = 10199
    },
['109'] = {
        Name = "不净_持戒",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_30_lys_seng_04.MC_30_lys_seng_04",
        SkillID = 10124,
        RecoverID = 10199
    },
['201'] = {
        Name = "不白_硬骨",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_30_lys_sengmian.MC_30_lys_sengmian",
        SkillID = 11325,
        RecoverID = 10199
    },
['202'] = {
        Name = "不能_老拳",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_30_lys_tuiseng_01.MC_30_lys_tuiseng_01",
        SkillID = 10126,
        RecoverID = 10199
    },
['203'] = {
        Name = "虫总兵_屠暴",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_40_psd_jiachongzongbing_02.MC_40_psd_jiachongzongbing_02",
        SkillID = 10127,
        RecoverID = 10199
    },
['204'] = {
        Name = "儡蜱士_鸣虫",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_40_psd_leibishi_01.MC_40_psd_leibishi_01",
        SkillID = 11328,
        RecoverID = 10199
    },
['206'] = {
        Name = "蝎太子_倒马毒",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_40_psd_xiezijing_01a.MC_40_psd_xiezijing_01a",
        SkillID = 10130,
        RecoverID = 10199
    },
['207'] = {
        Name = "百足虫_蜷身",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_40_psd_youyan_02.MC_40_psd_youyan_02",
        SkillID = 11331,
        RecoverID = 10199
    },
['209'] = {
        Name = "石父_夯力",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_50_hys_hms.MC_50_hys_hms",
        SkillID = 10133,
        RecoverID = 10199
    },
['301'] = {
        Name = "地罗刹_重盾",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_50_hys_huijingrenou_03.MC_50_hys_huijingrenou_03",
        SkillID = 11334,
        RecoverID = 10199
    },
['302'] = {
        Name = "鳖宝_冲天",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_50_hys_niaozui.MC_50_hys_niaozui",
        SkillID = 10135,
        RecoverID = 10199
    },
['304'] = {
        Name = "燧先锋_烽燧",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_50_hys_shixianfeng_02.MC_50_hys_shixianfeng_02",
        SkillID = 11337,
        RecoverID = 10199
    },
['305'] = {
        Name = "琴螂仙_仙秽",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_70_baijiangcan_02.MC_70_baijiangcan_02",
        SkillID = 11338,
        RecoverID = 10199
    },
['306'] = {
        Name = "火灵元母_虫后",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_70_huolingsha_02.MC_70_huolingsha_02",
        SkillID = 11339,
        RecoverID = 10199
    },
['307'] = {
        Name = "老人参精_根生",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_70_renshenjing_02.MC_70_renshenjing_02",
        SkillID = 11340,
        RecoverID = 10199
    },
['308'] = {
        Name = "蘑女_飘蓬",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_70_zhusun_03.MC_70_zhusun_03",
        SkillID = 10141,
        RecoverID = 10199
    },
['309'] = {
        Name = "菇男_转团",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_70_zhusun_01.MC_70_zhusun_01",
        SkillID = 11342,
        RecoverID = 10199
    },
['401'] = {
        Name = "狼刺客_追魂",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_10_gycy_lang_02.MC_10_gycy_lang_02",
        SkillID = 10161,
        RecoverID = 10199
    },
['402'] = {
        Name = "疯虎_拔山",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_bashanhu_01.MC_20_hfm_bashanhu_01",
        SkillID = 11362,
        RecoverID = 10199
    },
['403'] = {
        Name = "沙二郎_敲山",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_fuzishu.MC_20_hfm_fuzishu",
        SkillID = 11363,
        RecoverID = 10199
    },
['404'] = {
        Name = "鼠禁卫_剐肉",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_hongpaoshu.MC_20_hfm_hongpaoshu",
        SkillID = 10164,
        RecoverID = 10199
    },
['405'] = {
        Name = "骨悚然_退鬼",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_magu_01a.MC_20_hfm_magu_01a",
        SkillID = 11365,
        RecoverID = 10199
    },
['406'] = {
        Name = "狐侍长_剖心",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_maoyou_02a.MC_20_hfm_maoyou_02a",
        SkillID = 10166,
        RecoverID = 10199
    },
['407'] = {
        Name = "疾蝠_抖翅",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_roubianfu.MC_20_hfm_roubianfu",
        SkillID = 11367,
        RecoverID = 10199
    },
['408'] = {
        Name = "石双双_窝心脚",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_shanzhen_03.MC_20_hfm_shanzhen_03",
        SkillID = 10168,
        RecoverID = 10199
    },
['409'] = {
        Name = "鼠弩手_强弩",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_shunushou_01b.MC_20_hfm_shunushou_01b",
        SkillID = 11369,
        RecoverID = 10199
    },
['501'] = {
        Name = "地狼_快意",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_20_hfm_suoyang_01a.MC_20_hfm_suoyang_01a",
        SkillID = 10170,
        RecoverID = 10199
    },
['502'] = {
        Name = "隼居士_扇阴风",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_baitouweng.MC_30_lys_baitouweng",
        SkillID = 11371,
        RecoverID = 10199
    },
['503'] = {
        Name = "赤发鬼_阴箭",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_chifagui.MC_30_lys_chifagui",
        SkillID = 11372,
        RecoverID = 10199
    },
['504'] = {
        Name = "戒刀僧_开刀",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_jiedaoseng.MC_30_lys_jiedaoseng",
        SkillID = 10173,
        RecoverID = 10199
    },
['505'] = {
        Name = "泥塑金刚_劫火",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_numujingang.MC_30_lys_numujingang",
        SkillID = 10174,
        RecoverID = 10199
    },
['506'] = {
        Name = "夜叉奴_摧伤",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_shibing_01.MC_30_lys_shibing_01",
        SkillID = 10175,
        RecoverID = 10199
    },
['507'] = {
        Name = "巡山鬼_迅跃",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_xunshangui_01a.MC_30_lys_xunshangui_01a",
        SkillID = 10176,
        RecoverID = 10199
    },
['508'] = {
        Name = "鸦香客_寒香",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_30_lys_yaxiangke.MC_30_lys_yaxiangke",
        SkillID = 11377,
        RecoverID = 10199
    },
['509'] = {
        Name = "虫校尉_飞蛰",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_40_psd_chong_01.MC_40_psd_chong_01",
        SkillID = 10178,
        RecoverID = 10199
    },
['601'] = {
        Name = "蜻蜓精_乱箭",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_40_psd_chong_03.MC_40_psd_chong_03",
        SkillID = 11379,
        RecoverID = 10199
    },
['602'] = {
        Name = "傀蛛士_毒搔",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_40_psd_cunmin_05a.MC_40_psd_cunmin_05a",
        SkillID = 10181,
        RecoverID = 10199
    },
['603'] = {
        Name = "蛇捕头_虎啸",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_40_psd_hutoushe_01.MC_40_psd_hutoushe_01",
        SkillID = 10183,
        RecoverID = 10199
    },
['604'] = {
        Name = "蛇司药_洒药",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_40_psd_she_03.MC_40_psd_she_03",
        SkillID = 11384,
        RecoverID = 10199
    },
['605'] = {
        Name = "幽灯鬼_点鬼火",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_40_psd_yexungui_01.MC_40_psd_yexungui_01",
        SkillID = 11385,
        RecoverID = 10199
    },
['606'] = {
        Name = "黑脸鬼_炼刃",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_50_hys_huijin_b.MC_50_hys_huijin_b",
        SkillID = 10186,
        RecoverID = 10199
    },
['607'] = {
        Name = "牯都督_犇犇",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_50_hys_niu_02.MC_50_hys_niu_02",
        SkillID = 10187,
        RecoverID = 10199
    },
['608'] = {
        Name = "雾里云·云里雾_举火",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_50_hys_wuliyun.MC_50_hys_wuliyun",
        SkillID = 10188,
        RecoverID = 10199
    },
['609'] = {
        Name = "九叶灵芝精_群生",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S2/MC_70_lingzhijing_03a.MC_70_lingzhijing_03a",
        SkillID = 11392,
        RecoverID = 10199
    },
['701'] = {
        Name = "冰蛙",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_wa_95_bing.MC_10_gycy_wa_95_bing",
        SkillID = 11395,
        RecoverID = 10199
    },
['702'] = {
        Name = "火蛙",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_wa_96_huo.MC_10_gycy_wa_96_huo",
        SkillID = 11396,
        RecoverID = 10199
    },
['703'] = {
        Name = "毒蛙",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_wa_97_du.MC_10_gycy_wa_97_du",
        SkillID = 11397,
        RecoverID = 10199
    },
['704'] = {
        Name = "雷蛙",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_wa_98_lei.MC_10_gycy_wa_98_lei",
        SkillID = 11398,
        RecoverID = 10199
    },
['705'] = {
        Name = "石蛙",
        Path = "/Game/00MainHZ/Characters/Transform/VigorSkill/S1/MC_10_gycy_wa_99_shi.MC_10_gycy_wa_99_shi",
        SkillID = 11399,
        RecoverID = 10199
    }
```



# 黑神话悟空游戏MOD

## N网

[Mods at Black Myth: Wukong Nexus - Mods and community (nexusmods.com)](https://www.nexusmods.com/blackmythwukong/mods/)

其中优质mod如下：
[BossRush at Black Myth: Wukong Nexus - Mods and community (nexusmods.com)](https://www.nexusmods.com/blackmythwukong/mods/147?tab=description)
[MonkeyFurPokemon at Black Myth: Wukong Nexus - Mods and community (nexusmods.com)](https://www.nexusmods.com/blackmythwukong/mods/191?tab=description)
[72Bian at Black Myth: Wukong Nexus - Mods and community (nexusmods.com)](https://www.nexusmods.com/blackmythwukong/mods/269)

## 开发环境

推荐VSCode，lua插件推荐Lua Language Server coded by Lua

[UE4SS官网文档](https://docs.ue4ss.com/dev/index.html)

[UE4SS工具](https://www.nexusmods.com/blackmythwukong/mods/19)放到游戏里，自己写的mod要放到Mod文件夹下的一个子文件夹下，结构如图：

```
|——BlackMythWukong/b1/Binaries/Win64/ue4ss/Mods
|		|————Mod名字
|				|——————enabled.txt
|				|——————Scripts
|						|——————main.lua
|——UE4SS-settings.ini
|——UE4SS.log
|——UE4SS-ObjectDump.txt
```

如果找不到enabled.txt,该mod则不会启动

UE4SS-settings.ini里可如下设置

```ini
[Debug]
; Whether to enable the external UE4SS debug console.
ConsoleEnabled = 1;0
GuiConsoleEnabled = 1;0
GuiConsoleVisible = 1;0

; Multiplier for Font Size within the Debug Gui
; Default: 1
GuiConsoleFontScaling = 2;1

...

[Memory]
; The maximum memory usage (in percentage, see Task Manager %) allowed before asset loading (when LoadAllAssetsBefore* is 1) cannot happen.
; Once this percentage is reached, the asset loader will stop loading and whatever operation was in progress (object dump, or cxx generator) will continue.
; Default: 85
MaxMemoryUsageDuringAssetLoading = 80;85

```

UE4SS.log是日志输出，每次重新运行游戏，日志会清空重新写入。

运行游戏后会同时打开GUI控制台，里面常用两种功能：dump源代码到UE4SS-ObjectDump.txt，以及运行时重新加载lua代码

## 示例代码

main.lua,可用于黑神话悟空游戏。 功能为获取主角附近的模型资源、隐藏指定物体、穿墙、传送等

```lua
local tag = nil
local radius = 2000 --扫描半径
local wukong = nil
local recordLocation = nil --记忆位置
local recordLocation2 = nil --穿墙之前的位置
local getWukong = function()
    if wukong == nil then
        wukong = FindFirstOf("Unit_Player_Wukong_C")
    end
    return wukong
end

function FVectorToString(fvector)
    return string.format(" X=%s,Y=%s,Z=%s", fvector.X, fvector.Y, fvector.Z)
end

function IsFVectorNearby(vectorA, vectorB)
    -- 计算每个轴向上的距离差  
    local dx = math.abs(vectorA.X - vectorB.X)  
    local dy = math.abs(vectorA.Y - vectorB.Y)  
    local dz = math.abs(vectorA.Z - vectorB.Z)  
      
    -- 判断是否都小于等于阈值  
    if dx <= radius and dy <= radius and dz <= radius then  
        return true  
    else  
        return false  
    end  
end

function IsUsefulMeshName(meshName)
    if string.find(meshName, "/Game/00MainHZ/Environment/Speedtree/") or--排除植物
    string.find(meshName, "/Game/00MainHZ/Environment/Trees") or--排除植物
    string.find(meshName, "/Engine/") or--排除UE引擎内置资源
    string.find(meshName, "Wukong") or--排除主角
    string.find(meshName, "VFX") or--排除特效
    string.find(meshName, "/Game/3rd/SuperGrid/") or
    string.find(meshName, "/Megascans/")--排除地编通用资源
    then
        return false
    else
        return true
    end
end

RegisterKeyBind(Key.J, function()
    print("Press J\n")
    local wukong = getWukong()
    local pos_wukong = wukong:K2_GetActorLocation()
    --print("Wukong pos:"..FVectorToString(pos_wukong) .. "\n")
    local objs = FindAllOf("StaticMeshComponent")--静态网格体组件
    local dic = {}
    for index, value in ipairs(objs) do
        if value.StaticMesh ~= nil and value.bVisible then
            local name = value.StaticMesh:GetFullName()
            if name ~= nil and dic[name] ~= 1 and IsUsefulMeshName(name) then
                local pos = value:K2_GetComponentLocation()
                if IsFVectorNearby(pos, pos_wukong) then
                    print(name 
                    -- .. FVectorToString(pos) 
                    .. "\n")
                    dic[name] = 1
                end
            end
        end
    end
end)

RegisterKeyBind(Key.H, function()
    --print("Press H\n")
    local wukong = getWukong()
    local pos_wukong = wukong:K2_GetActorLocation()
    local objs = FindAllOf("StaticMeshComponent")--静态网格体组件
    for index, value in ipairs(objs) do
        if value.StaticMesh ~= nil then
            local name = value.StaticMesh:GetFullName()
            local str = "Dazu_01/QSGY_FIX"--隐藏千手观音
            if tag ~= nil then
                str = tag
            end
            if name ~= nil and string.find(name, str) and IsUsefulMeshName(name) then
                local pos = value:K2_GetComponentLocation()
                if IsFVectorNearby(pos, pos_wukong) then
                    value:ToggleVisibility(false);
                end
            end
        end
    end
end)

RegisterConsoleCommandHandler("tag", function(FullCommand, Parameters, Output)
    if #Parameters < 1 then
        tag = nil
        return false 
    end
    tag = Parameters[1]
    return true
end)

RegisterConsoleCommandHandler("more", function(FullCommand, Parameters, Output)
    radius = 6000
end)

RegisterConsoleCommandHandler("less", function(FullCommand, Parameters, Output)
    radius = 2000
end)
RegisterKeyBind(Key.U, function() --穿墙术
    local wukong = getWukong()
    local pos = wukong:K2_GetActorLocation()
    recordLocation2 = pos
    local vector3 = wukong:GetActorForwardVector()
    vector3.X = pos.X + vector3.X * 300
    vector3.Y = pos.Y + vector3.Y * 300
    vector3.Z = pos.Z + vector3.Z * 300
    wukong:K2_SetActorLocation(vector3 ,false,{},false)
end)

RegisterKeyBind(Key.K,function () --记录当前位置
    local wukong = getWukong()
    recordLocation = wukong:K2_GetActorLocation()
end)

RegisterKeyBind(Key.L,function () --回到记录位置
    if recordLocation ~= nil then
        local wukong = getWukong()
        wukong:K2_SetActorLocation(recordLocation,false,{},false)
    elseif recordLocation2 ~= nil then
        local wukong = getWukong()
        wukong:K2_SetActorLocation(recordLocation2,false,{},false)
    end
end)
```



# C++代码检查工具——Clang-Tidy

[C++排名第一的代码质量检测工具](https://mp.weixin.qq.com/s/y7TIxULPPgcJas7CRSl0gw)



# 怪猎崛起资源解包并导入UE5

[UE5动画-战斗教程-导出《怪物猎人崛起》资源并导入到UE5中使用_哔哩哔哩_bilibili](https://www.bilibili.com/video/BV1RwZvYrEHe)
