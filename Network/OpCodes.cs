using System;
using System.Collections.Generic;
using System.Linq;

namespace Network
{
    public class OpCodes
    {
        public static Dictionary<ushort, String> Recv = new Dictionary<ushort, String>(); // Client Opcode
        public static Dictionary<ushort, String> Send = new Dictionary<ushort, String>(); // Server Opcode

        //public static Dictionary<short, string> SendNames = new Dictionary<short, string>();

        public static int Version = 1725;

        public static void Init()
        {
            #region Client OpCode Recv
            // OpCode For Client EU Revision 282514
            // AUTH
            Recv.Add((ushort)0x4DBC, "CM_CHECK_VERSION");  // OK
            Recv.Add((ushort)0xBFA4, "CM_HARDWARE_INFO");
            Recv.Add((ushort)0x837D, "CM_MOVIE_END"); // Not Sure
            Recv.Add((ushort)0xE952, "CM_REQUEST_CONTRACT"); // Not Sure
            Send.Add((ushort)0xE8FF, "SM_SEND_ACCOUNT_SETTINGS");

            // CHARACTER SELECTION SCREEN
            Recv.Add((ushort)0xFB01, "CM_CHARACTER_CREATE_ALLOWED"); // OK
            Recv.Add((ushort)0x8E1C, "CM_CHARACTER_CREATE_NAME_USED_CHECK"); // OK
            Recv.Add((ushort)0x562C, "CM_CHARACTER_CREATE"); // OK
            Recv.Add((ushort)0xADDD, "CM_CHARACTER_DELETE"); // OK
            Recv.Add((ushort)0xF8FD, "CM_CHARACTER_UNK03");

            // MODALFORM
            Recv.Add((ushort)0xF07C, "CM_MODALFORM_GET_ITEM");
            Recv.Add((ushort)0x7C96, "CM_MODALFORM_INFO_ITEM");
            Recv.Add((ushort)0xFD4A, "CM_MODALFORM_POPUP_SHOW");
            Recv.Add((ushort)0xC143, "CM_MODALFORM_POPUP_ACCEPT"); // Or GameObject_Spawn_CampFire
            Recv.Add((ushort)0x6F14, "CM_MODALFORM_POPUP_CANCEL");
            Recv.Add((ushort)0x5FB0, "CM_MODALFORM_POPUP_HIDE");

            // UI
            Recv.Add((ushort)0x5294, "CM_WHISP"); // OK
            Recv.Add((ushort)0xAE54, "CM_STRING_PATTERN_CHECK"); // OK
            Recv.Add((ushort)0x87AC, "CM_UI_ACCESS_VANGUARD_REQUESTS");
            Recv.Add((ushort)0xBCF9, "CM_UI_UPDATE_MAP"); // 2nd 8-byte is Map Index
            Recv.Add((ushort)0x5350, "CM_UI_UPDATE_ALLIANCE");
            Recv.Add((ushort)0xE53B, "CM_UI_UPDATE_ACHIEVEMENTS");
            Recv.Add((ushort)0x9DBE, "CM_UI_UPDATE_CHANNEL");
            Recv.Add((ushort)0x94C7, "CM_UI_CHANGE_CHANNEL");            

            // BATTLEGROUND MATCHING
            Recv.Add((ushort)0xB207, "CM_BATTLEGROUND_MATCHING_UPDATE");
            Recv.Add((ushort)0x77C0, "CM_BATTLEGROUND_MATCHING_JOIN");

            // INSTANCE MATCHING
            Recv.Add((ushort)0x8851, "CM_INSTANCE_MATCHING_UPDATE");

            // GAMEOBJECT
            //Recv.Add((ushort)0xFFFF, "CM_GAMEOBJECT_CAMPFIRE_SPAWN");

            // CASH SHOP
            Recv.Add((ushort)0x6F60, "CM_CASH_SHOP_OPEN");
            Recv.Add((ushort)0xD4EC, "CM_CASH_SHOP_ACCESS");

            // PEGASE
            Recv.Add((ushort)0xC3B9, "CM_PEGASE_START");
            Recv.Add((ushort)0x9CC6, "CM_PEGASE_END"); // FALSE

            // INVENTORY
            Recv.Add((ushort)0xB08D, "CM_INVENTORY_UPDATE");
            Recv.Add((ushort)0xFFAA, "CM_INVENTORY_ITEM_MOVE");

            // PROFIL
            Recv.Add((ushort)0xDAA7, "CM_PROFIL_UPDATE");
            Recv.Add((ushort)0xBF91, "CM_PROFIL_ITEM_EQUIP");
            Recv.Add((ushort)0x57A0, "CM_PROFIL_ITEM_UNEQUIP");

            // ENCHANTEMENTS
            Recv.Add((ushort)0xBB69, "CM_ENCHANTS_UI_OPEN"); // Not That.
            Recv.Add((ushort)0xBE1F, "CM_ENCHANTS_UPDATE_2");
            Recv.Add((ushort)0xD42D, "CM_ENCHANTS_UPDATE_3");

            // PLAYER (SELF)
            Recv.Add((ushort)0x90AA, "CM_PLAYER_ENTER_WORLD"); // OK
            Recv.Add((ushort)0x8147, "CM_PLAYER_MOVE"); // OK
            Recv.Add((ushort)0x82F5, "CM_PLAYER_CAST_SPELL");
            Recv.Add((ushort)0xE916, "CM_PLAYER_NPC_INTERACT");
            Recv.Add((ushort)0x6809, "CM_PLAYER_NPC_DIALOG");

            // GLYPHS
            Recv.Add((ushort)0xF808, "CM_GLYPHS_UPDATE");
            Recv.Add((ushort)0xEEF1, "CM_GLYPHS_SET");

            // EXCHANGE
            Recv.Add((ushort)0x9F16, "CM_EXCHANGE_ITEM_ADD_BUY");
            Recv.Add((ushort)0xEB04, "CM_EXCHANGE_ITEM_REMOVE_BUY");
            Recv.Add((ushort)0xA12E, "CM_EXCHANGE_ITEM_ADD_SELL");
            Recv.Add((ushort)0x6EB6, "CM_EXCHANGE_ITEM_REMOVE_SELL");
            Recv.Add((ushort)0xC996, "CM_EXCHANGE_COMPLETE");
            //Recv.Add((ushort)0xFFFF, "CM_EXCHANGE_CANCEL");

            // TRADE
            Recv.Add((ushort)0xB055, "CM_TRADE_LOGS_UPDATE");

            // FRIENDS_LIST
            Recv.Add((ushort)0x7CA3, "CM_FRIENDS_LIST_UPDATE");
            Recv.Add((ushort)0xD763, "CM_FRIENDS_LIST_FRIENDS_NOTE");
            Recv.Add((ushort)0x5C4C, "CM_FRIENDS_LIST_FRIENDS_ADD");
            Recv.Add((ushort)0xA972, "CM_FRIENDS_LIST_FRIENDS_DEL");
            Recv.Add((ushort)0xA355, "CM_FRIENDS_LIST_BLOCKED_ADD");
            Recv.Add((ushort)0x5F47, "CM_FRIENDS_LIST_BLOCKED_DEL");
            Recv.Add((ushort)0xBF3B, "CM_FRIENDS_LIST_BLOCKED_NOTE");

            // SETTINGS
            Recv.Add((ushort)0xFD24, "CM_SETTINGS_VIDEO_UPDATE");
            Recv.Add((ushort)0xE852, "CM_SETTINGS_PLAYER_UPDATE");

            // LFG
            Recv.Add((ushort)0x6745, "CM_SYSTEM_LFG_CREATE_ANNOUNCE");
            Recv.Add((ushort)0x716C, "CM_SYSTEM_LFG_DELETE_ANNOUNCE");
            Recv.Add((ushort)0x98AD, "CM_SYSTEM_LFG_UPDATE_ANNOUNCE");
            Recv.Add((ushort)0xC238, "CM_SYSTEM_LFG_PARTY_GET_LINK");
            Recv.Add((ushort)0xCB14, "CM_SYSTEM_LFG_PARTY_JOIN");
            Recv.Add((ushort)0xB559, "CM_SYSTEM_LFG_OPEN_UI");
            Recv.Add((ushort)0xACEA, "CM_SYSTEM_LFG_CLOSE_UI");
            Recv.Add((ushort)0xBF51, "CM_SYSTEM_LFG_UNK01");

            // GUILD
            Recv.Add((ushort)0xD6DD, "CM_GUILD_LOGO"); // Not Sure
            Recv.Add((ushort)0xD115, "CM_GUILD_MEMBERS_UPDATE"); // OK
            Recv.Add((ushort)0xA12C, "CM_GUILD_INFOS"); // OK
            Recv.Add((ushort)0xFEE9, "CM_GUILD_MEMBERS_APPLYING"); // OK
            Recv.Add((ushort)0x93A7, "CM_GUILD_CRUSADES_INFOS"); // OK
            Recv.Add((ushort)0xD1A2, "CM_GUILD_ACCEPT_APPLYING");

            // Others
            Recv.Add((ushort)0x878F, "CM_PLAYER_WORLD_UPDATE");
            //Recv.Add((ushort)0x6809, "CM_PLAYER_INTERACT_UNKNOWN"); // FALSE
            Recv.Add((ushort)0xC3C3, "CM_ITEM_BOX_ROLL");


            Recv.Add((ushort)0xB395, "CM_UI_NPC_OPEN_MODAL");
            //Recv.Add((ushort)0xB395, "CM_BANK_UI_OPEN");
            //Recv.Add((ushort)0xB395, "CM_MAILBOX_UI_OPEN");
            //Recv.Add((ushort)0xB395, "CM_AUCTION_HOUSE_UI_OPEN");

            // AUCTION HOUSE
            Recv.Add((ushort)0x8DE7, "CM_AUCTION_HOUSE_UI_CLOSE");
            Recv.Add((ushort)0xFB70, "CM_AUCTION_HOUSE_SEARCH");
            Recv.Add((ushort)0xF015, "CM_AUCTION_HOUSE_SEARCH_HISTORY");
            Recv.Add((ushort)0xA1FA, "CM_AUCTION_HOUSE_NEXT");
            Recv.Add((ushort)0x56C1, "CM_AUCTION_HOUSE_NEXT_HISTORY");
            Recv.Add((ushort)0xF4BA, "CM_AUCTION_HOUSE_BUY1");
            Recv.Add((ushort)0xB1D4, "CM_AUCTION_HOUSE_PURCHASES");
            Recv.Add((ushort)0xDBD2, "CM_AUCTION_HOUSE_SALES");
            Recv.Add((ushort)0x5C80, "CM_AUCTION_HOUSE_ITEMS_ON_SALE");
            Recv.Add((ushort)0x7CC1, "CM_AUCTION_HOUSE_GET_ITEMS");

            // MAILBOX
            Recv.Add((ushort)0xA28A, "CM_MAILBOX_UPDATE");
            Recv.Add((ushort)0x4F06, "CM_MAILBOX_GET_ITEMS");
            Recv.Add((ushort)0xB02D, "CM_MAILBOX_MAIL_DELETE1");
            Recv.Add((ushort)0xD2B8, "CM_MAILBOX_MAIL_DELETE2");
            Recv.Add((ushort)0xB70D, "CM_MAILBOX_MAIL_CLOSE");

            #endregion

            #region Server OpCode Send
            // OpCode For Server EU Revision 282543
            // AUTH
            Send.Add((ushort)0x4DBD, "SM_CHECK_VERSION");  // OK
            
            // ACCOUNT
            Send.Add((ushort)0x91F6, "SM_ACCOUNT_SETTINGS1"); // Not That but Fine
            Send.Add((ushort)0xC1A8, "SM_ACCOUNT_SETTINGS2"); // Not That but Fine

            // CHARACTER SELECTION SCREEN
            Send.Add((ushort)0xFE7B, "SM_CHARACTER_LIST"); // OK
            Send.Add((ushort)0x726F, "SM_CHARACTER_CREATE_ALLOWED");
            Send.Add((ushort)0xC7AB, "SM_CHARACTER_CREATE_NAME_PATTERN_CHECK"); // OK
            Send.Add((ushort)0xBCE6, "SM_CHARACTER_CREATE_NAME_USED_CHECK");  // OK
            Send.Add((ushort)0x98CD, "SM_CHARACTER_CREATE"); // OK
            Send.Add((ushort)0xF394, "SM_CHARACTER_DELETE"); // OK
            Send.Add((ushort)0xAA10, "SM_CHARACTER_RESTORE"); // We can't get this on live server.

            Send.Add((ushort)0x85F2, "SM_CHARACTER_BOOL_FUNC_02");
            Send.Add((ushort)0x5145, "SM_CHARACTER_PLANET_DB");
            Send.Add((ushort)0xDC6F, "SM_CHARACTER_UNK_B1");
            Send.Add((ushort)0x790C, "SM_CHARACTER_UNK_B2");

            // CHAT
            Send.Add((ushort)0x76B0, "SM_CHAT"); // OK

            // PROFILE
            Send.Add((ushort)0xC684, "SM_PROFIL_UPDATE");

            // ENCHANT
            Send.Add((ushort)0x8E5A, "SM_ENCHANTS_UI_OPEN"); 
            Send.Add((ushort)0xB5E7, "SM_ENCHANTS_UPDATE_2");
            Send.Add((ushort)0xA6A9, "SM_ENCHANTS_UPDATE_3");

            // DATA
            Send.Add((ushort)0x4FA6, "SM_SEND_DATA_01");
            Send.Add((ushort)0x9F3F, "SM_SEND_DATA_02");
            Send.Add((ushort)0xDA4F, "SM_SEND_DATA_03");
            Send.Add((ushort)0xC989, "SM_SEND_DATA_04");
            Send.Add((ushort)0xF329, "SM_SEND_DATA_05");
            Send.Add((ushort)0x58EB, "SM_SEND_DATA_06");

            Send.Add((ushort)0x6D7F, "SM_SEND_STRUCT_WHEN_ENTER_WORLD_01");
            Send.Add((ushort)0x9FFB, "SM_SEND_DATA_WHEN_ENTER_WORLD_02");

            // FRIENDS_LIST
            Send.Add((ushort)0xAD6B, "SM_FRIENDS_LIST_UPDATE");

            // PLAYER (ALL)
            Send.Add((ushort)0xD819, "SM_PLAYER_TARGETING_SMTHG");
            Send.Add((ushort)0xB363, "SM_PLAYER_MOVE_UNK01");
            Send.Add((ushort)0xBD9C, "SM_PLAYER_UNK01");

            // GUILD


            // LFG
            Send.Add((ushort)0xF1B2, "SM_SYSTEM_LFG_SEND_ANNOUNCE");

            // OTHER
            Send.Add((ushort)0x9549, "SM_QUIT_GAME"); 

            #endregion

            //SendNames = Send.ToDictionary(p => p.Value, p => p.Key.Name);
        }
    }
}