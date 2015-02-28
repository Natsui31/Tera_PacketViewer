using System;
using System.Collections.Generic;
using System.Linq;

namespace Network
{
    public class OpCodes
    {
        public static Dictionary<ushort, String> Recv = new Dictionary<ushort, String>();
        public static Dictionary<ushort, String> Send = new Dictionary<ushort, String>();

        public static Dictionary<short, string> SendNames = new Dictionary<short, string>();

        public static int Version = 1725;

        public static void Init()
        {
            #region Client packets
            // OPCODE For Client EU Revision 282514
            // AUTH
            Recv.Add((ushort)0x4DBC, "CM_CHECK_VERSION");  // OK
            Recv.Add((ushort)0xB114, "CM_ACCOUNT_AUTH"); 
            Recv.Add((ushort)0xBFA4, "CM_HARDWARE_INFO"); 

            // CHARACTER
            Recv.Add((ushort)0x96F1, "CM_CHARACTER_LIST"); 
            Recv.Add((ushort)0xFBC3, "CM_CHARACTER_CREATE_ALLOWED"); 
            Recv.Add((ushort)0x7F90, "CM_CHARACTER_CREATE_NAME_PATTERN_CHECK"); 
            Recv.Add((ushort)0xAE1B, "CM_CHARACTER_CREATE_NAME_USED_CHECK"); 
            Recv.Add((ushort)0xD932, "CM_CHARACTER_CREATE"); 
            Recv.Add((ushort)0xAD7C, "CM_CHARACTER_DELETE"); 
            Recv.Add((ushort)0xDA2D, "CM_CHARACTER_RESTORE"); 

            // ENTER WORLD
            Recv.Add((ushort)0xA5B7, "CM_ENTER_WORLD"); 
            Recv.Add((ushort)0xDE8B, "CM_TRADEBROKER_HIGHEST_ITEM_LEVEL"); 
            Recv.Add((ushort)0x910B, "CM_LOAD_TOPO_FIN");
            Recv.Add((ushort)0x96A5, "CM_UPDATE_CONTENTS_PLAYTIME");
            Recv.Add((ushort)0xB135, "CM_SIMPLE_TIP_REPEATED_CHECK"); 
            Recv.Add((ushort)0xD99E, "CM_PLAYER_CLIMB_START");
            Recv.Add((ushort)0x7957, "CM_USER_SETTINGS_SAVE");
            Recv.Add((ushort)0x74F3, "CM_MOVIE_END");

            // REQUEST
            Recv.Add((ushort)0xE952, "CM_REQUEST_CONTRACT"); // Not Sure
            Recv.Add((ushort)0xD870, "CM_REQUEST_ANSWER"); 
            Recv.Add((ushort)0xC09A, "CM_REQUEST_GAMESTAT_PING");

            // MOUNT
            Recv.Add((ushort)0xB118, "CM_PLAYER_UNMOUNT");

            // OPTIONS
            Recv.Add((ushort)0xAD59, "CM_OPTION_SHOW_MASK");
            Recv.Add((ushort)0x6CCD, "CM_OPTION_SET_VISIBILITY_DISTANCE"); 

            // CHAT
            Recv.Add((ushort)0xF5B1, "CM_CHAT"); 
            Recv.Add((ushort)0x5F0E, "CM_CHAT_INFO"); 
            Recv.Add((ushort)0x5042, "CM_LOOKING_FOR_GROUP_CHAT_INFO"); 
            Recv.Add((ushort)0x5294, "CM_WHISP"); // OK

            // DIALOG
            Recv.Add((ushort)0x94BE, "CM_NPC_CONTACT"); 
            Recv.Add((ushort)0x9A93, "CM_DIALOG_EVENT"); 
            Recv.Add((ushort)0xA308, "CM_DIALOG"); 

            // ALLIANCE
            Recv.Add((ushort)0x7E06, "CM_ALLIANCE_INFO"); 

            // GAMEOBJECT
            Recv.Add((ushort)0x89F9, "CM_GAMEOBJECT_REMOVE");

            // SKILL
            Recv.Add((ushort)0xF8DD, "CM_SKILL_START"); 
            Recv.Add((ushort)0x70AB, "CM_SKILL_INSTANCE_START"); 
            Recv.Add((ushort)0x76A8, "CM_SKILL_CANCEL");
            Recv.Add((ushort)0x738A, "CM_GLYPH_REINIT");

            // PLAYER
            Recv.Add((ushort)0x8147, "CM_PLAYER_MOVE"); // OK
            Recv.Add((ushort)0x6797, "CM_PLAYER_ZONE_CHANGE"); 
            Recv.Add((ushort)0xC6D1, "CM_LOOKING_FOR_BATTLEGROUND_WINDOW_OPEN"); 
            Recv.Add((ushort)0xC590, "CM_LOOKING_FOR_INSTANCE_WINDOW_OPEN"); 
            Recv.Add((ushort)0xE8A2, "CM_PLAYER_REPORT");
            Recv.Add((ushort)0x8DA7, "CM_PLAYER_COMPARE_ACHIEVEMENTS");
            Recv.Add((ushort)0xADCF, "CM_PLAYER_INSPECT");
            Recv.Add((ushort)0x9520, "CM_PLAYER_SELECT_CREATURE"); 
            Recv.Add((ushort)0xE195, "CM_PLAYER_DONJON_CLEAR_COUNT_LIST");

            // GATHER
            Recv.Add((ushort)0x885F, "CM_GATHER_START");

            // PROFIL
            Recv.Add((ushort)0x96A4, "CM_PLAYER_SET_TITLE");
            Recv.Add((ushort)0x7653, "CM_PLAYER_DESCRIPTION");
            Recv.Add((ushort)0x6DF5, "CM_PLAYER_REINIT_INSTANCES");
            Recv.Add((ushort)0xE9BC, "CM_PLAYER_DONJON_STATS_PVP");

            // INVENTORY
            Recv.Add((ushort)0xF9FD, "CM_INVENTORY_SHOW"); 
            Recv.Add((ushort)0x7EEF, "CM_ITEM_MOVE"); 
            Recv.Add((ushort)0xB139, "CM_ITEM_USE"); 
            Recv.Add((ushort)0xD1F6, "CM_ITEM_SIMPLE_INFO");
            Recv.Add((ushort)0x83A1, "CM_INVENTORY_ORDER"); 
            Recv.Add((ushort)0xBE5B, "CM_ITEM_UNEQUIP"); 
            Recv.Add((ushort)0xF8D3, "CM_ITEM_EQUIP");
            Recv.Add((ushort)0xF772, "CM_PLAYER_EQUIPEMENT_ITEM_INFO"); 
            Recv.Add((ushort)0xACA1, "CM_PLAYER_DUNGEON_COOLTIME_LIST");
            Recv.Add((ushort)0xB106, "CM_PLAYER_ITEM_TRASH");
            Recv.Add((ushort)0x9DE4, "CM_PLAYER_DROP_ITEM_PICKUP"); 

            // EXCHANGE
            Recv.Add((ushort)0xFCEB, "CM_EXCHANGE_ITEM_ADD_BUY"); 
            Recv.Add((ushort)0xF1C5, "CM_EXCHANGE_ITEM_REMOVE_BUY");
            Recv.Add((ushort)0xF624, "CM_EXCHANGE_ITEM_ADD_SELL");
            Recv.Add((ushort)0xAE84, "CM_EXCHANGE_ITEM_REMOVE_SELL");
            Recv.Add((ushort)0xFBCD, "CM_EXCHANGE_COMPLETE");
            Recv.Add((ushort)0x8D7A, "CM_EXCHANGE_CANCEL");

            // MAP
            Recv.Add((ushort)0x8D02, "CM_MAP_SHOW");

            // ACTIVITIES
            Recv.Add((ushort)0x88B2, "CM_PLAYER_EMOTE"); 
            Recv.Add((ushort)0x5761, "CM_ENCHANT_WINDOW_OPEN");
            Recv.Add((ushort)0xB5E3, "CM_INSTANCERANK_WINDOW_OPEN");
            Recv.Add((ushort)0x62EB, "CM_BATTLEGROUND_WINDOW_OPEN");

            // STOCK EXCHANGE ITEM
            Recv.Add((ushort)0xCE53, "CM_STOCK_EXCHANGE_ITEM_UNIQUE_LIST"); 
            Recv.Add((ushort)0xD7AD, "CM_STOCK_EXCHANGE_ITEM_UNIQUE_REQUEST"); 
            Recv.Add((ushort)0x70E7, "CM_STOCK_EXCHANGE_ITEM_ACCOUNT_LIST"); 
            Recv.Add((ushort)0x6913, "CM_STOCK_EXCHANGE_ITEM_ACCOUNT_REQUEST"); 
            Recv.Add((ushort)0xD960, "CM_STOCK_EXCHANGE_ITEM_INFO"); 
            Recv.Add((ushort)0x518F, "CM_STOCK_EXCHANGE_ITEM_UNK"); 

            // GUILD
            Recv.Add((ushort)0xCCE7, "CM_GUILD_MEMBER_LIST"); 
            Recv.Add((ushort)0xFF6A, "CM_GUILD_INFO"); 
            Recv.Add((ushort)0xBEB7, "CM_GUILD_APPLICATION"); 
            Recv.Add((ushort)0xD387, "CM_GUILD_VERSUS_STATUS"); 
            Recv.Add((ushort)0xC409, "CM_GUILD_LEAVE");
            Recv.Add((ushort)0xD2DE, "CM_GUILD_SERVER_LIST"); 
            Recv.Add((ushort)0xCF05, "CM_GUILD_LOGO"); 

            // SOCIAL
            Recv.Add((ushort)0x9577, "CM_PLAYER_FRIEND_LIST"); 
            Recv.Add((ushort)0xDA73, "CM_PLAYER_FRIEND_ADD"); 
            Recv.Add((ushort)0xDC58, "CM_PLAYER_FRIEND_REMOVE");
            Recv.Add((ushort)0x63AE, "CM_PLAYER_FRIEND_NOTE");
            Recv.Add((ushort)0xB1BA, "CM_PLAYER_BLOCK_ADD");
            Recv.Add((ushort)0x50A3, "CM_PLAYER_BLOCK_REMOVE");
            Recv.Add((ushort)0x63B0, "CM_PLAYER_BLOCK_NOTE");
            Recv.Add((ushort)0xE726, "CM_REIGN_INFO");
            Recv.Add((ushort)0x8D10, "CM_GUARD_PK_POLICY"); 

            // GROUP
            Recv.Add((ushort)0xBD26, "CM_GROUP_LEAVE");
            Recv.Add((ushort)0xFDFB, "CM_GROUP_KICK");
            Recv.Add((ushort)0xA5E6, "CM_GROUP_CONFIRM_KICK");
            Recv.Add((ushort)0xA995, "CM_GROUP_CONFIRM_LEADER_CHANGE"); 
            Recv.Add((ushort)0xA8BB, "CM_GROUP_DESTROY");

            Recv.Add((ushort)0x8D48, "CM_PLAYER_TRADE_HISTORY");

            // TERA SHOP
            Recv.Add((ushort)0xEEDF, "CM_SHOP_WINDOW_OPEN"); 

            // SYSTEM
            Recv.Add((ushort)0x74DF, "CM_WELCOME_MESSAGE"); 
            Recv.Add((ushort)0xCDBD, "CM_QUIT_TO_CHARACTER_LIST"); 
            Recv.Add((ushort)0xA765, "CM_QUIT_TO_CHARACTER_LIST_CANCEL"); 
            Recv.Add((ushort)0xD250, "CM_QUIT_GAME"); 
            Recv.Add((ushort)0xBEE0, "CM_QUIT_GAME_CANCEL"); 

            // PEGASUS
            Recv.Add((ushort)0x8122, "CM_PEGASUS_START"); 

            // CHANNEL
            Recv.Add((ushort)0xFB75, "CM_CHANNEL_LIST"); 

            #endregion

            #region Server packets
            // OPCODE For Server EU Revision 282543
            // AUTH
            Send.Add((ushort)0x4DBD, "SM_CHECK_VERSION");  // OK
            Send.Add((ushort)0xDC28, "SM_LOADING_SCREEN_CONTROL_INFO"); 
            Send.Add((ushort)0x521E, "SM_REMAIN_PLAY_TIME"); 
            Send.Add((ushort)0xE9DE, "SM_LOGIN_ARBITER"); 
            Send.Add((ushort)0xACC6, "SM_LOGIN_ACCOUNT_INFO"); 
            Send.Add((ushort)0x8093, "SM_ACCOUNT_PACKAGE_LIST"); 
            Send.Add((ushort)0xC8A8, "SM_SYSTEM_INFO"); 

            // CHARACTER
            Send.Add((ushort)0xFE7B, "SM_CHARACTER_LIST"); // OK
            Send.Add((ushort)0x726F, "SM_CHARACTER_CREATE_ALLOWED"); 
            Send.Add((ushort)0xB743, "SM_CHARACTER_CREATE_NAME_PATTERN_CHECK"); 
            Send.Add((ushort)0xB5C4, "SM_CHARACTER_CREATE_NAME_USED_CHECK"); 
            Send.Add((ushort)0x89C6, "SM_CHARACTER_CREATE"); 
            Send.Add((ushort)0xF9E8, "SM_CHARACTER_DELETE"); 
            Send.Add((ushort)0xCCE0, "SM_CHARACTER_RESTORE"); 

            // ENTER WORLD
            Send.Add((ushort)0x5CCF, "SM_ENTER_WORLD"); 
            Send.Add((ushort)0xD61A, "SM_LOGIN"); 
            Send.Add((ushort)0xDD66, "SM_CURRENT_ELECTION_STATE"); 
            Send.Add((ushort)0x9274, "SM_AVAILABLE_SOCIAL_LIST"); 
            Send.Add((ushort)0x969C, "SM_NPC_GUILD_LIST"); 
            Send.Add((ushort)0xA33A, "SM_VIRTUAL_LATENCY"); 
            Send.Add((ushort)0x539D, "SM_MOVE_DISTANCE_DELTA"); 
            Send.Add((ushort)0xAC2B, "SM_F2P_PREMIUM_USER_PERMISSION"); 
            Send.Add((ushort)0x86B7, "SM_PLAYER_EQUIP_ITEM_CHANGER");
            Send.Add((ushort)0xD85D, "SM_FESTIVAL_LIST"); 
            Send.Add((ushort)0xF1AD, "SM_MASSTIGE_STATUS"); 
            Send.Add((ushort)0x792B, "SM_LOAD_TOPO"); 
            Send.Add((ushort)0xA953, "SM_LOAD_HINT"); 
            Send.Add((ushort)0xA2D8, "SM_SIMPLE_TIP_REPEATED_CHECK"); 
            Send.Add((ushort)0x8F16, "SM_USER_SETTINGS_LOAD"); 
            Send.Add((ushort)0xA7DE, "SM_MOVIE_PLAY");
            Send.Add((ushort)0x96BD, "SM_VISITED_SECTION_LIST");
            Send.Add((ushort)0x5EB0, "SM_TRADEBROKER_HIGHEST_ITEM_LEVEL"); 
            Send.Add((ushort)0x8A2B, "SM_ACHIEVEMENT_PROGRESS_UPDATE"); 

            // PEGASUS
            Send.Add((ushort)0xCAB4, "SM_PEGASUS_START"); 
            Send.Add((ushort)0xC0E7, "SM_PEGASUS_UPDATE"); 
            Send.Add((ushort)0xCA3E, "SM_PEGASUS_END"); 
            Send.Add((ushort)0xDB85, "SM_PEGASUS_MAP_SHOW"); 

            // MOUNT
            Send.Add((ushort)0xBF48, "SM_PLAYER_MOUNT"); 
            Send.Add((ushort)0xAEB8, "SM_PLAYER_UNMOUNT"); 

            // GUILD
            Send.Add((ushort)0xE489, "SM_GUILD_MEMBER_LIST"); 
            Send.Add((ushort)0xA942, "SM_GUILD_VERSUS_STATUS"); 
            Send.Add((ushort)0x5606, "SM_GUILD_SERVER_LIST");

            // PET
            Send.Add((ushort)0xDD08, "SM_PET_INCUBATOR_INFO_CHANGE"); 
            Send.Add((ushort)0x8908, "SM_PET_INFO_CLEAR"); 

            // ALLIANCE
            Send.Add((ushort)0xEB1B, "SM_ALLIANCE_INFO"); 

            // ATTACK
            Send.Add((ushort)0xCF42, "SM_ACTION_STAGE"); 
            Send.Add((ushort)0xD2E1, "SM_ACTION_END"); 
            Send.Add((ushort)0xFF1F, "SM_CREATURE_INSTANCE_ARROW"); 
            Send.Add((ushort)0xA71F, "SM_PLAYER_EXPERIENCE_UPDATE"); 

            // OPTIONS
            Send.Add((ushort)0xB4C9, "SM_OPTION_SHOW_MASK");

            // SKILL
            Send.Add((ushort)0x80CE, "SM_SKILL_START_COOLTIME"); 
            Send.Add((ushort)0x0001, "SM_SKILL_PERIOD");
            Send.Add((ushort)0x9F0C, "SM_SKILL_RESULTS"); 
            Send.Add((ushort)0xDEC3, "SM_PLAYER_SKILL_LIST"); 

            // CHAT
            Send.Add((ushort)0x76B0, "SM_CHAT"); // OK
            Send.Add((ushort)0x8CD3, "SM_CHAT_LOOKING_FOR_GROUP"); 
            Send.Add((ushort)0xA856, "SM_CHAT_LOOKING_FOR_GROUP_INFO"); 
            Send.Add((ushort)0x8A9F, "SM_CHAT_INFO");
            Send.Add((ushort)0xF2BC, "SM_WHISP"); // OK

            // PLAYER
            Send.Add((ushort)0xAD6B, "SM_PLAYER_FRIEND_LIST"); // OK
            Send.Add((ushort)0x840C, "SM_OWN_PLAYER_SPAWN"); 
            Send.Add((ushort)0xCD87, "SM_PLAYER_STATS_UPDATE"); 
            Send.Add((ushort)0xE3F9, "SM_PLAYER_MOVE"); 
            Send.Add((ushort)0xD371, "SM_PLAYER_ZONE_CHANGE");
            Send.Add((ushort)0xB8C0, "SM_PLAYER_SELECT_CREATURE"); 
            Send.Add((ushort)0xBEFE, "SM_PLAYER_STATE"); 
            Send.Add((ushort)0xA3C0, "SM_RESPONSE_GAMESTAT_PONG");
            Send.Add((ushort)0x6022, "SM_PLAYER_DONJON_CLEAR_COUNT_LIST");
            Send.Add((ushort)0x5390, "SM_PLAYER_SPAWN"); 
            Send.Add((ushort)0x8668, "SM_PLAYER_DESPAWN"); 
            Send.Add((ushort)0xC7A3, "SM_PLAYER_CLIMB_START");
            Send.Add((ushort)0xDFDC, "SM_PLAYER_DESCRIPTION"); 
            Send.Add((ushort)0xF5B4, "SM_PLAYER_UNK1"); 
            Send.Add((ushort)0xF169, "SM_PLAYER_UNK2"); 
            Send.Add((ushort)0xA3C6, "SM_PLAYER_DEATH");
            Send.Add((ushort)0x8CB7, "SM_PLAYER_REVIVE");
            Send.Add((ushort)0x862C, "SM_PLAYER_DEATH_WINDOW");
            Send.Add((ushort)0xC99E, "SM_PLAYER_GATHER_STATS"); 

            // OBJECT
            Send.Add((ushort)0x5DB2, "SM_GAMEOBJECT_SPAWN");
            Send.Add((ushort)0xC7A2, "SM_GAMEOBJECT_DESPAWN");

            // CRAFT
            Send.Add((ushort)0x97F1, "SM_CRAFT_STATS"); 
            Send.Add((ushort)0xA3DF, "SM_CRAFT_RECIPE_LIST"); 

            // ABNORMALITY
            Send.Add((ushort)0xDFA9, "SM_ABNORMALITY_BEGIN"); 
            Send.Add((ushort)0x9E24, "SM_ABNORMALITY_END"); 

            // CREATURE
            Send.Add((ushort)0xBD20, "SM_CREATURE_HP_UPDATE");
            Send.Add((ushort)0xDD71, "SM_CREATURE_UNK"); 
            Send.Add((ushort)0xC8B0, "SM_CREATURE_MP_UPDATE");
            Send.Add((ushort)0x68C2, "SM_CREATURE_SPAWN"); 
            Send.Add((ushort)0xEA0B, "SM_CREATURE_DESPAWN"); 
            Send.Add((ushort)0xAA64, "SM_CREATURE_MOVE");
            Send.Add((ushort)0xEC17, "SM_CREATURE_ROTATE");
            Send.Add((ushort)0xC2EA, "SM_CREATURE_TARGET_PLAYER");
            Send.Add((ushort)0x95B4, "SM_CREATURE_SHOW_HP");

            // DROP
            Send.Add((ushort)0x95CC, "SM_DROP_ITEM_LOOT"); 
            Send.Add((ushort)0xD68B, "SM_DROP_ITEM_DESPAWN"); 
            Send.Add((ushort)0xEB8B, "SM_DROP_ITEM_SPAWN"); 

            // DIALOG
            Send.Add((ushort)0x5A8A, "SM_DIALOG"); 
            Send.Add((ushort)0xEFBC, "SM_DIALOG_CLOSE"); 
            Send.Add((ushort)0xFD90, "SM_DIALOG_EVENT"); 
            Send.Add((ushort)0x819E, "SM_DIALOG_MENU_SELECT"); 
            Send.Add((ushort)0x9E7A, "SM_DIALOG_TRADELIST_SHOW"); 

            // CAMPFIRE
            Send.Add((ushort)0xCCE4, "SM_CAMPFIRE_SPAWN");
            Send.Add((ushort)0xB5EF, "SM_CAMPFIRE_DESPAWN");

            // GROUP
            Send.Add((ushort)0x93B2, "SM_GROUP_MEMBER_LIST"); // OK
            Send.Add((ushort)0xED19, "SM_GROUP_QUEST_SHARE"); 
            Send.Add((ushort)0xBB1C, "SM_GROUP_MEMBER_STATS"); 
            Send.Add((ushort)0xE82F, "SM_GROUP_ABNORMALS");
            Send.Add((ushort)0xB314, "SM_GROUP_UNK"); 
            Send.Add((ushort)0x4FD6, "SM_GROUP_MEMBER_HP_UPDATE"); 
            Send.Add((ushort)0x6DBE, "SM_GROUP_MEMBER_MP_UPDATE");
            Send.Add((ushort)0xEF84, "SM_GROUP_MEMBER_MOVE"); 
            Send.Add((ushort)0x8378, "SM_GROUP_LEAVE"); 
            Send.Add((ushort)0xCD5C, "SM_GROUP_LEADER_CHANGED"); 
            Send.Add((ushort)0xF1E5, "SM_GROUP_CONFIRM_KICK_WINDOW_SHOW");

            // PROFIL
            Send.Add((ushort)0xF8E2, "SM_PLAYER_SET_TITLE");
            Send.Add((ushort)0xB8C4, "SM_PLAYER_DONJON_STATS_PVP");

            // QUEST
            Send.Add((ushort)0xE3AE, "SM_QUEST_CLEAR_INFO"); 
            Send.Add((ushort)0xCCA7, "SM_QUEST_INFO"); 
            Send.Add((ushort)0xC7D9, "SM_QUEST_DAILY_COMPLETE_COUNT"); 
            Send.Add((ushort)0x95BF, "SM_MISSION_COMPLETE_INFO");
            Send.Add((ushort)0xF929, "SM_QUEST_BALLOON");
            Send.Add((ushort)0x8F45, "SM_QUEST_VILLAGER_INFO");
            Send.Add((ushort)0xFB81, "SM_QUEST_WORLD_VILLAGER_INFO");
            Send.Add((ushort)0x5714, "SM_QUEST_WORLD_VILLAGER_INFO_CLEAR");
            Send.Add((ushort)0xB433, "SM_QUEST_UPDATE");

            // INVENTORY
            Send.Add((ushort)0x8034, "SM_INVENTORY"); 
            Send.Add((ushort)0xC6A9, "SM_ITEM_INFO"); 
            Send.Add((ushort)0xD3D7, "SM_ITEM_SIMPLE_INFO");
            Send.Add((ushort)0xE040, "SM_PLAYER_INVENTORY_SLOT_CHANGED"); 
            Send.Add((ushort)0x8890, "SM_PLAYER_APPEARANCE_CHANGE"); 
            Send.Add((ushort)0x5601, "SM_ITEM_START_COOLTIME");
            Send.Add((ushort)0xE62F, "SM_EXCHANGE_WINDOW_UPDATE");

            // STOCK EXCHANGE ITEM
            Send.Add((ushort)0xACBE, "SM_STOCK_EXCHANGE_ITEM_HINT"); 

            Send.Add((ushort)0xB28D, "SM_STOCK_EXCHANGE_ITEM_UNIQUE_LIST"); 
            Send.Add((ushort)0xF3A8, "SM_STOCK_EXCHANGE_ITEM_UNIQUE_ADD"); 

            Send.Add((ushort)0xFD9D, "SM_STOCK_EXCHANGE_ITEM_ACCOUNT_LIST"); 
            Send.Add((ushort)0xBD6E, "SM_STOCK_EXCHANGE_ITEM_ACCOUNT_ADD"); 
            Send.Add((ushort)0x6B70, "SM_STOCK_EXCHANGE_ITEM_INFO"); 
            Send.Add((ushort)0xDC8F, "SM_STOCK_EXCHANGE_ITEM_UNK"); 

            // TRADE
            Send.Add((ushort)0xCCD6, "SM_TRADE_WINDOW_SHOW");

            // MAP
            Send.Add((ushort)0x9860, "SM_MAP");

            // SOCIAL
            Send.Add((ushort)0x6B3F, "SM_SOCIAL"); 
            Send.Add((ushort)0xC18B, "SM_PLAYER_FRIEND_LIST");
            Send.Add((ushort)0x9547, "SM_PLAYER_FRIEND_ADD_SUCCESS");
            Send.Add((ushort)0x9946, "SM_PLAYER_FRIEND_REMOVE_SUCCESS");
            Send.Add((ushort)0xC156, "SM_REIGN_INFO");
            Send.Add((ushort)0x6425, "SM_GUARD_PK_POLICY"); 

            // SHOP
            Send.Add((ushort)0xE6AF, "SM_SHOP_WINDOW_OPEN");

            // SYSTEM
            Send.Add((ushort)0xDD29, "SM_SYSTEM_MESSAGE"); 
            Send.Add((ushort)0xAB23, "SM_WELCOME_MESSAGE"); 
            Send.Add((ushort)0xC54E, "SM_QUIT_TO_CHARACTER_LIST_WINDOW"); 
            Send.Add((ushort)0xF348, "SM_QUIT_TO_CHARACTER_LIST_CANCEL"); 
            Send.Add((ushort)0x8BA9, "SM_QUIT_TO_CHARACTER_LIST"); 
            Send.Add((ushort)0xB5E4, "SM_QUIT_GAME_WINDOW"); 
            Send.Add((ushort)0xDD59, "SM_QUIT_GAME_CANCEL"); 
            Send.Add((ushort)0xA594, "SM_QUIT_GAME"); 

            // REQUEST
            Send.Add((ushort)0xAF1A, "SM_REQUEST_CONTRACT"); 
            Send.Add((ushort)0xAF0C, "SM_REQUEST_CONTRACT_REPLY"); 
            Send.Add((ushort)0xA249, "SM_REQUEST_CONTRACT_CANCEL");
            Send.Add((ushort)0xBA1A, "SM_REQUEST_COMPLETE"); 
            Send.Add((ushort)0xCECD, "SM_REQUEST_WINDOW_SHOW"); 
            Send.Add((ushort)0x55B9, "SM_REQUEST_WINDOW_HIDE");

            // CHANNEL
            Send.Add((ushort)0xF32B, "SM_PLAYER_ENTER_CHANNEL");
            Send.Add((ushort)0x907B, "SM_PLAYER_CHANNEL_INFO"); 
            Send.Add((ushort)0xF419, "SM_PLAYER_CHANNEL_LIST"); 

            // GATHER
            Send.Add((ushort)0xC775, "SM_GATHER_START"); 
            Send.Add((ushort)0x949B, "SM_GATHER_PROGRESS"); 
            Send.Add((ushort)0xD0F5, "SM_GATHER_END"); 
            Send.Add((ushort)0xDB23, "SM_GATHER_SPAWN"); 
            Send.Add((ushort)0xC86C, "SM_GATHER_DESPAWN"); 
            Send.Add((ushort)0x6AB5, "SM_GATHERCRAFT_POINT"); 

            // UNK
            Send.Add((ushort)0x670A, "SM_PLAYER_UNK"); 


            #endregion

            //SendNames = Send.ToDictionary(p => p.Value, p => p.Key.Name);
        }
    }
}