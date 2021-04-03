/**
 * ______________________________________________________
 * This file is part of ko-administration-tool project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2017)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System;
using System.Runtime.InteropServices;
using KAI.Common.Declarations;
using DWORD = System.UInt32;
using ULONGLONG = System.UInt64;

// ReSharper disable All


namespace KAI.Declarations
{

    public class OperatorInfo
    {
        public UInt64 SessionID { get; set; }
        public string UserID { get; set; }
        public byte Authority { get; set; }
        public DateTime AuthDate { get; set; }
        public string AuthorizedBy { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public enum SMIOpcodes
    {
        /* WEEZ. */
        WIZ_MOVE = 0x06,
        WIZ_CHAT = 0x10,
        WIZ_DEAD = 0x11,
        WIZ_HP_CHANGE = 0x17,
        WIZ_MSP_CHANGE = 0x18,
        WIZ_EXP_CHANGE = 0x1A,
        WIZ_LEVEL_CHANGE = 0x1B,

        WIZ_WARP = 0x1E,
        WIZ_ITEM_MOVE = 0x1F,
        WIZ_TARGET_HP = 0x22,
        WIZ_POINT_CHANGE = 0x28,
        WIZ_STATE_CHANGE = 0x29,
        WIZ_LOYALTY_CHANGE = 0x2A,
        WIZ_SKILLPT_CHANGE = 0x32,
        WIZ_ITEM_COUNT_CHANGE = 0x3D,
        WIZ_ITEM_REMOVE = 0x3F,
        WIZ_WAREHOUSE = 0x45,
        WIZ_FRIEND_PROCESS = 0x49,
        WIZ_GOLD_CHANGE = 0x4A,
        WIZ_HOME = 0x48,
        WIZ_STEALTH = 0x60,
        WIZ_PREMIUM = 0x71,
        WIZ_REGENE = 0x12,
        WIZ_PARTY = 0x2F,
        WIZ_ITEM_GET = 0x26,
        WIZ_ITEM_TRADE = 0x21,

        KAI_OPCODE_AUTH = 0xA0,

        SMI_UNKNOWN = 0xD2,

        SMI_REQUEST_ALL_INFO = 0xD3,
        SMI_ONLINE_COUNTS = 0xD4,
        SMI_ONLINE_USERS = 0xD5,
        SMI_TECHNICAL_INFO = 0xD6,
        SMI_TECHNICAL_INFO_UPDATE = 0xD7,
        SMI_AI_INFO = 0xD8,
        SMI_AUTH_INFO = 0xD9,

        SMI_CHAR_SET_CURRENT = 0xDA,
        SMI_CHAR_ALL_INFO = 0xDB,
        SMI_CHAR_ALL_STATUS = 0xDC,
        SMI_NETWORK_INTERFACE_INFORMATION = 0xDD,
        SMI_NETWORK_TCP_STATISTICS = 0xDE,
        SMI_NETWORK_UDP_STATISTICS = 0xDF,
        SMI_NETWORK_STATS_UPDATE = 0xE0,


        SMI_CHARACTER_LOGIN = 0xE1,
        SMI_CHARACTER_LOGOUT = 0xE2,
        SMI_CHARACTER_ZONECHANGE = 0xE3,

        SMI_CHAR_UNSET_TRACKING = 0xE4,
        SMI_OPERATOR = 0xE5,
        SMI_ALTER_VALUE = 0xE6,
        SMI_ZONE_CHANGE = 0xE7,
        SMI_PRIVATE_CHAT = 0xE8,
        SMI_CHAR_STAT_ALTERATION = 0xE9,
        SMI_CHAR_SKILL_ALTERATION = 0xEA,
        SMI_CHAR_ITEMMOVE = 0xEB,
        SMI_CHAR_SAVEINVENALL = 0xEC,

        SMI_MANAGER_ALLINFO = 0xF0,
        SMI_MANAGER_ONLINELIST = 0xF1,
        SMI_MANAGER_LOGGED_IN = 0xF2,
        SMI_MANAGER_LOGGED_OUT = 0xF3,
        SMI_MANAGER_AUTHORIZE_NEW = 0xF4,
        SMI_MANAGER_MYINFO = 0xF5,
        SMI_MANAGER_PENALTY = 0xF6,

        KAI_OPCODE_PARTY = 0xF7,
        KAI_OPCODE_PREMIUM = 0xF8,
        KAI_OPCODE_OPERATOR_CHAT = 0xF9,
        KAI_OPCODE_OPERATOR_INOUT = 0xFA,

        SMI_TEST = 0xFE


    };

    public enum KAIOperatorInoutSubopcodes
    {
        KAI_OPERATOR_INOUT_LIST = 0x01,
        KAI_OPERATOR_INOUT_IN = 0x02,
        KAI_OPERATOR_INOUT_OUT = 0x03
    };

    public enum KAIChatSubopcodes
    {
        KAI_CHAT_MESSAGE = 0x01,
        KAI_CHAT_TYPING = 0x02
    };

    public class AccountPenalty
    {

      
        public enum NameType
        {
            NAME_TYPE_ACCOUNT = 0x00,
            NAME_TYPE_CHARACTER = 0x01
        }
        public enum OperationType
        {
            OPERATION_TYPE_SET = 0x01,
            OPERATION_TYPE_LIFT = 0x02
        };

        public enum AccountPenaltyType
        {
            ACCOUNT_PENALTY_NONE = 0x00,
            ACCOUNT_PENALTY_BAN = 0x01,
            ACCOUNT_PENALTY_MUTE = 0x02,
            ACCOUNT_PENALTY_TRADE = 0x03,
            ACCOUNT_PENALTY_MERCHANT = 0x04,
            ACCOUNT_PENALTY_ATTACK = 0x05,
            ACCOUNT_PENALTY_ZONE_CHANGE = 0x06,
            ACCOUNT_PENALTY_LETTER = 0x07,
            ACCOUNT_PENALTY_PRISON = 0x08
        }

        public NameType TypeOfName { get; set; }
        public OperationType TypeOfOperation { get; set; }
        public AccountPenaltyType TypeOfPenalty { get; set; }

        public string UserID { get; set; }
        public string ManagerID { get; set; }
        public string Reason { get; set; }
       public UInt64 StartDate { get; set; }
        public UInt64 EndDate { get; set; }
        public bool Permanent { get; set; }

    }

    public enum KAIResultCodes
    {
        KAI_RESULT_FAILURE = 0x00,
        KAI_RESULT_SUCCESS = 0x01,
        KAI_RESULT_NO_AUTHORITY = 0x02,
        KAI_RESULT_ANOTHER_OPERATOR = 0x03
    };
    public enum PartyTrackSub
    {
        PARTY_TRACK_SET = 0x1,
        PARTY_TRACK_CLEAR = 0x02
    }
    public class PartyInfo
    {

        public ushort PartyID { get; set; }
        public short LeaderID { get; set; }
        public byte MemberCount { get; set; }
        public byte Zone { get; set; }
        public Int16[] MemberIDs = new Int16[8];
    }

  



    public enum KAIAuthSubopcodes
    {
        KAI_AUTH_HANDSHAKE = 0x01,
        KAI_AUTH_LOGIN   = 0x02
    }

    public enum KAIPartySubopcodes
    {
        KAI_PARTY_TRACK = 0x01,
        KAI_PARTY_ALLINFO = 0x02,
        KAI_PARTY_CREATED = 0x03,
        KAI_PARTY_DISBANDED = 0x04,
        KAI_PARTY_MEMBERCOUNT_CHANGE = 0x05,
        KAI_PARTY_MEMBERUPDATE = 0x06,
        KAI_PARTY_CHAT = 0x07,
        KAI_PARTY_OPERATOR = 0x08
    }
    public enum KAIPartyOperationSub
    {
        KAI_PARTY_OP_DISBAND = 0x01,

    };

    public enum OperatorRequests
    {
        BAN = 0x01,
        MUTE = 0x02,
        UNMUTE = 0x03,
        ATTACKDISABLE = 0x04,
        ATTACKENABLE = 0x05,
        DISCONNECT = 0x06,
        MAKEGM = 0x07
    }

    public enum Kind
    {
        Any = -1,
        Reward = 0,
        Dagger = 11,
        OneHandedSword = 21,
        TwoHandedSword = 22,
        OneHandedAxe = 31,
        TwoHandedAxe = 32,
        Club = 41,
        TwoHandedClub = 42,
        Spear = 51,
        LongSpear = 52,
        Shield = 60,
        Bow = 70,
        Crossbow = 71,
        Earring = 91,
        Necklace = 92,
        Ring = 93,
        Belt = 94,
        QuestItem = 95,
        LuneItem = 97,
        UpgradeScroll = 98,
        MonstersStone = 101,
        Staff = 110,
        Arrow = 120,
        Javelin = 130,
        FamiliarEgg = 150,
        Familiar = 151,
        CypherRing = 160,
        Autoloot = 170,
        ImageChangeScroll = 171,
        FamiliarAttack = 172,
        FamiliarDefense = 173,
        FamiliarLoyalty = 174,
        FamiliarSpecialityFood = 175,
        FamiliarFood = 176,
        PriestMace = 181,
        WarriorArmor = 210,
        RogueArmor = 220,
        MageArmor = 230,
        PriestArmor = 240,
        SealScroll = 250,
        ChaosSkillItem = 254,
        PowerUpItem = 255
    }

    public enum ItemType
    {
        Any = -1,
        NormalItem = 0,
        MagicItem = 1,
        RareItem = 2,
        CraftItem = 3,
        UniqueItem = 4,
        UpgradeItem = 5,
        EventItem = 6,
        CospreItem = 8,
        ReverseItem = 11,
        ReverseUniqueItem = 12

    }

    public enum ItemFlag
    {
        ITEM_FLAG_NONE = 0,
        ITEM_FLAG_RENTED = 1,
        ITEM_FLAG_DUPLICATE = 3,
        ITEM_FLAG_SEALED = 4,
        ITEM_FLAG_NOT_BOUND = 7,
        ITEM_FLAG_BOUND = 8
    };

    [Serializable]
    public class ItemInformation
    {

       
        public int Num { get; set; }
        public byte ExtId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IconID { get; set; }
        public byte Kind { get; set; }
        public byte Slot { get; set; }
        public byte Race { get; set; }
        public byte Class { get; set; }
        public short Damage { get; set; }
        public short Delay { get; set; }
        public short Range { get; set; }
        public short Weight { get; set; }
        public short Duration { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
       
        public short Ac { get; set; }
        public byte Countable { get; set; }
        public int Effect1 { get; set; }
        public int Effect2 { get; set; }
        public byte ReqLevel { get; set; }
        public byte ReqLevelMax { get; set; }
        public byte ReqRank { get; set; }
        public byte ReqTitle { get; set; }
        public byte ReqStr { get; set; }
        public byte ReqSta { get; set; }
        public byte ReqDex { get; set; }
        public byte ReqIntel { get; set; }
        public byte ReqCha { get; set; }
        public byte SellingGroup { get; set; }
        public byte ItemType { get; set; }
        public short Hitrate { get; set; }
        public short EvasionRate { get; set; }

        public short DaggerAc { get; set; }
        public short SwordAc { get; set; }
        public short MaceAc { get; set; }
        public short AxeAc { get; set; }
        public short SpearAc { get; set; }
        public short BowAc { get; set; }
        public byte FireDamage { get; set; }
        public byte IceDamage { get; set; }
        public byte LightningDamage { get; set; }
        public byte PoisonDamage { get; set; }
        public byte HpDrain { get; set; }
        public byte MpDamage { get; set; }
        public byte MpDrain { get; set; }

        public void CopyTo(ItemInformation target)
        {
            target.Num = Num;
            target.ExtId = ExtId;
            target.Name = Name;
            target.Description = Description;
            target.IconID = IconID;
            target.Kind = Kind;
            target.Slot = Slot;
            target.Race = Race;
            target.Class = Class;
            target.Damage = Damage;
            target.Delay = Delay;
            target.Range = Range;
            target.Weight = Weight;
            target.Duration = Duration;
            target.BuyPrice = BuyPrice;
            target.SellPrice = SellPrice;
            target.Ac = Ac;
            target.Countable = Countable;
            target.Effect1 = Effect1;
            target.Effect2 = Effect2;
            target.ReqLevel = ReqLevel;
            target.ReqLevelMax = ReqLevelMax;
            target.ReqRank = ReqRank;
            target.ReqTitle = ReqTitle;
            target.ReqStr = ReqStr;
            target.ReqSta = ReqSta;
            target.ReqDex = ReqDex;
            target.ReqIntel = ReqIntel;
            target.ReqCha = ReqCha;
            target.SellingGroup = SellingGroup;
            target.ItemType = ItemType;
            target.Hitrate = Hitrate;
            target.EvasionRate = EvasionRate;
            target.DaggerAc = DaggerAc;
            target.SwordAc = SwordAc;
            target.MaceAc = MaceAc;
            target.AxeAc = AxeAc;
            target.SpearAc = SpearAc;
            target.BowAc = BowAc;
            target.FireDamage = FireDamage;
            target.IceDamage = IceDamage;
            target.LightningDamage = LightningDamage;
            target.PoisonDamage = PoisonDamage;
            target.HpDrain = HpDrain;
            target.MpDamage = MpDamage;
            target.MpDrain = MpDrain;
            target.MirrorDamage = MirrorDamage;
            target.DropRate = DropRate;
            target.StrB = StrB;
            target.StaB = StaB;
            target.DexB = DexB;
            target.IntelB = IntelB;
            target.ChaB = ChaB;
            target.MaxHpB = MaxHpB;
            target.MaxMpB = MaxMpB;
            target.FireR = FireR;
            target.ColdR = ColdR;
            target.LightningR = LightningR;
            target.MagicR = MagicR;
            target.PoisonR = PoisonR;
            target.CurseR = CurseR;
            target.ItemGrade = ItemGrade;
        }
        public byte MirrorDamage { get; set; }
        public byte DropRate { get; set; }
        public short StrB { get; set; }
        public short StaB { get; set; }
        public short DexB { get; set; }
        public short IntelB { get; set; }
        public short ChaB { get; set; }
        public short MaxHpB { get; set; }
        public short MaxMpB { get; set; }
        public short FireR { get; set; }
        public short ColdR { get; set; }
        public short LightningR { get; set; }
        public short MagicR { get; set; }
        public short PoisonR { get; set; }
        public short CurseR { get; set; }
        public byte ItemGrade { get; set; }
    }

    /// <summary>
    /// Data holder for an item slot.
    /// </summary>
    [Serializable]
    public class InventorySlot
    {
        public void CopyTo(InventorySlot target)
        {
           // target.Pos = Pos;
            target.ItemID = ItemID;
            target.Durability = Durability;
            target.Count = Count;
            target.Flag = Flag;
            target.RemainingRentalTime = RemainingRentalTime;
            target.ExpirationTime = ExpirationTime;
            target.Serial = Serial;
            if (Info == null)
                return;
            if(target.Info == null)
                target.Info = new ItemInformation();
            Info.CopyTo(target.Info);
        }
        public byte Pos { get; set; }
        public uint ItemID { get; set; }
        public int Durability { get; set; }
        public ushort Count { get; set; }
        public byte Flag { get; set; }
        public short RemainingRentalTime { get; set; }
        public uint ExpirationTime { get; set; }
        public ulong Serial { get; set; }

        public bool isEmpty()
        {
            return ItemID == 0;
        }

        public bool Hovered { get; set; }
        public ItemInformation Info { get; set; }

        public void Reset()
        {
           
            ItemID = 0;
            Durability = 0;
            Count = 0;
            Flag = 0;
            RemainingRentalTime = 0;
            ExpirationTime = 0;
            Serial = 0;
            Info = null;
        }
    }

    public static class CharacterClass
    {
        public const int K_NOVICE_WARRIOR = 101;
        public const int K_NORMAL_WARRIOR = 105;
        public const int K_MASTER_WARRIOR = 106;
        public const int K_NOVICE_MAGICIAN = 103;
        public const int K_NORMAL_MAGICIAN = 109;
        public const int K_MASTER_MAGICIAN = 110;
        public const int K_NOVICE_ROGUE = 102;
        public const int K_NORMAL_ROGUE = 107;
        public const int K_MASTER_ROGUE = 108;
        public const int K_NOVICE_PRIEST = 104;
        public const int K_NORMAL_PRIEST = 111;
        public const int K_MASTER_PRIEST = 112;
        public const int E_NOVICE_WARRIOR = 201;
        public const int E_NORMAL_WARRIOR = 205;
        public const int E_MASTER_WARRIOR = 206;
        public const int E_NOVICE_MAGICIAN = 203;
        public const int E_NORMAL_MAGICIAN = 209;
        public const int E_MASTER_MAGICIAN = 210;
        public const int E_NOVICE_ROGUE = 202;
        public const int E_NORMAL_ROGUE = 207;
        public const int E_MASTER_ROGUE = 208;
        public const int E_NOVICE_PRIEST = 204;
        public const int E_NORMAL_PRIEST = 211;
        public const int E_MASTER_PRIEST = 212;

        public static string GetDescription(int classID)
        {
            switch (classID)
            {
                case E_MASTER_MAGICIAN:
                    return "Arch Mage";

                case E_MASTER_PRIEST:
                    return "Paladin";

                case E_MASTER_ROGUE:
                    return "Kasar Hood";

                case E_MASTER_WARRIOR:
                    return "Blade Master";

                case E_NORMAL_MAGICIAN:
                    return "Mage";

                case E_NORMAL_PRIEST:
                    return "Cleric";

                case E_NORMAL_ROGUE:
                    return "Ranger";

                case E_NORMAL_WARRIOR:
                    return "Blade";


                case K_MASTER_MAGICIAN:
                    return "Elemental Lord";

                case K_MASTER_PRIEST:
                    return "Shadow Knight";

                case K_MASTER_WARRIOR:
                    return "Berserker Hero";

                case K_MASTER_ROGUE:
                    return "Shadow Vain";

                case K_NORMAL_MAGICIAN:
                    return "Sorcerer";

                case K_NORMAL_PRIEST:
                    return "Shaman";

                case K_NORMAL_ROGUE:
                    return "Hunter";

                case K_NORMAL_WARRIOR:
                    return "Berserker";

                case K_NOVICE_MAGICIAN:
                case E_NOVICE_MAGICIAN:
                    return "Magician";

                case K_NOVICE_PRIEST:
                case E_NOVICE_PRIEST:
                    return "Priest";

                case K_NOVICE_ROGUE:
                case E_NOVICE_ROGUE:
                    return "Rogue";

                case K_NOVICE_WARRIOR:
                case E_NOVICE_WARRIOR:
                    return "Warrior";

                default:
                    return "Unknown Class (" + classID + ")";

            }
        }
    }

    public static class Authority
    {
        /// <summary>
        /// Administrator [0x00]
        /// </summary>
        public const int Admin = 0;

        /// <summary>
        /// Default Authority [0x01]
        /// </summary>
        public const int NormalUser = 1;

        /// <summary>
        /// User, Muted [0x02]
        /// </summary>
        public const int MutedUser = 2;

        /// <summary>
        /// User, Banned [0xFF]
        /// </summary>
        public const int BannedUser = 255;

        /// <summary>
        /// User, Muted [0x11]
        /// </summary>
        public const int MuteType2 = 11;

        /// <summary>
        /// User, Attack Disabled [0x12]
        /// </summary>
        public const int AttackDisabledUser = 12;

        /// <summary>
        /// Gamemaster [0x250]
        /// </summary>
        public const int GameMaster = 250;

        public static string GetDescription(int auth)
        {
            switch (auth)
            {
                case Admin:
                    return "Admin";
                case AttackDisabledUser:
                    return "User (Attack Disabled)";
                case BannedUser:
                    return "User (Banned)";
                case GameMaster:
                    return "Gamemaster";
                case MuteType2:
                    return "User (Muted, type 2)";
                case MutedUser:
                    return "User (Muted, type 1)";
                case NormalUser:
                    return "User";
                default:
                    return "Unknown Authority (" + auth + ")";
            }
        }
    }

    public static class Nation
    {
        public const int Karus = 1;
        public const int ElMorad = 2;

        public static string GetDescription(int nationID)
        {
            switch (nationID)
            {
                case Karus:
                    return "Karus";
                case ElMorad:
                    return "El Morad";
                default:
                    return "Unknown Nation (" + nationID + ")";
            }
        }
    }

   

    public static class Race
    {
        /// <summary>
        /// Karus, Arch Tuarek [0x01]
        /// </summary>
        public const int ArchTuarek = 1;

        /// <summary>
        /// Karus, Tuarek [0x02]
        /// </summary>
        public const int Tuarek = 2;

        /// <summary>
        /// Karus, Wrinkle Tuarek [0x03]
        /// </summary>
        public const int WrinkleTuarek = 3;

        /// <summary>
        /// Karus, Puri Tuarek [0x04]
        /// </summary>
        public const int PuriTuarek = 4;

        /// <summary>
        /// El Morad, Barbarian [0x11]
        /// </summary>
        public const int Barbarian = 11;

        /// <summary>
        /// El Morad, Male El Moradian [0x12]
        /// </summary>
        public const int MaleElMoradian = 12;

        /// <summary>
        /// El Morad, Female El Moradian [0x13]
        /// </summary>
        public const int FemaleElMoradian = 13;

        public static string GetDescription(int raceID)
        {
            switch (raceID)
            {
                case ArchTuarek:
                    return "Arch Tuarek";

                case Barbarian:
                    return "Barbarian";

                case FemaleElMoradian:
                    return "Female El Moradian";

                case MaleElMoradian:
                    return "Male El Moradian";

                case PuriTuarek:
                    return "Puri Tuarek";

                case Tuarek:
                    return "Tuarek";

                case WrinkleTuarek:
                    return "Wrinkle Tuarek";

                default:
                    return "Unknown Race (" + raceID + ")";
            }
        }
    }

    [Flags]
    public enum StatusFlags
    {
        ALIVE = 1,
        SITTING = 2,
        MERCHANT = 4,
        INVIS = 8,
        MUTED = 16,
        TRANSFORM = 32,
        MOVE = 64,
        BLINK = 128,
        ATTACK = 256,
        TRADE = 512,
        REPORT = 1024,
        TELEPORT = 2048,
        KING = 4096,
        IN_GAME = 8192,
        USING_STORE = 16384,
        GAMEMASTER = 32768,
        STEALTH_DETECTION = 65536,
        PM_BLOCK = 131072,
        CLAN = 262144,
        CLAN_LEADER = 524288,
        CLAN_ASSIST = 1048576,
        PARTY = 2097152,
        PARTY_LEADER = 4194304,
        PARTY_SEEKING = 8388608,
        UNUSED_1 = 16777216,
        UNUSED_2 = 33554432,
        UNUSED_3 = 67108864,
        UNUSED_4 = 134217728,
        UNUSED_5 = 268435456,
        UNUSED_6 = 536870912,
        UNUSED_7 = 1073741824,
        /*   UNUSED_8 = 2147483648*/
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _PERFORMANCE_INFORMATION
    {

        [MarshalAs(UnmanagedType.U4)]
        public DWORD cb;
        //The number of pages currently committed by the system. Note that committing pages (using VirtualAlloc with MEM_COMMIT) changes this value immediately; however, the physical memory is not charged until the pages are accessed.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr CommitTotal;
        //The current maximum number of pages that can be committed by the system without extending the paging file(s). This number can change if memory is added or deleted, or if pagefiles have grown, shrunk, or been added. If the paging file can be extended, this is a soft limit.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr CommitLimit;
        //The maximum number of pages that were simultaneously in the committed state since the last system reboot.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr CommitPeak;
        //The amount of actual physical memory, in pages.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr PhysicalTotal;
        //The amount of physical memory currently available, in pages. This is the amount of physical memory that can be immediately reused without having to write its contents to disk first. It is the sum of the size of the standby, free, and zero lists.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr PhysicalAvailable;
        //The amount of system cache memory, in pages. This is the size of the standby list plus the system working set.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr SystemCache;
        //The sum of the memory currently in the paged and nonpaged kernel pools, in pages.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr KernelTotal;
        //The memory currently in the paged kernel pool, in pages.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr KernelPaged;
        //The memory currently in the nonpaged kernel pool, in pages.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr KernelNonpaged;
        //The size of a page, in bytes.
        [MarshalAs(UnmanagedType.SysUInt)]
        public UIntPtr PageSize;
        //The current number of open handles.
        [MarshalAs(UnmanagedType.U4)]
        public DWORD HandleCount;
        //The current number of processes.
        [MarshalAs(UnmanagedType.U4)]
        public DWORD ProcessCount;
        //The current number of threads.
        [MarshalAs(UnmanagedType.U4)]
        public DWORD ThreadCount;

        public UInt64 GetPhysicalTotal()
        {
            return (UInt64)PageSize * (UInt64)PhysicalTotal;
        }

        public UInt64 GetPhysicalAvailable()
        {
            return (UInt64)PageSize * (UInt64)PhysicalAvailable;
        }

        public UInt64 GetPhysicalUsed()
        {
            return (UInt64)GetPhysicalTotal() - (UInt64)GetPhysicalAvailable();
        }

        public UInt64 GetCommitTotal()
        {
            return (UInt64)PageSize * (UInt64)CommitTotal;
        }

        public UInt64 GetCommitLimit()
        {
            return (UInt64)PageSize * (UInt64)CommitLimit;
        }

        public UInt64 GetCommitPeak()
        {
            return (UInt64)PageSize * (UInt64)CommitPeak;
        }

        public UInt64 GetSystemCached()
        {
            return (UInt64)PageSize * (UInt64)SystemCache;
        }

        public UInt64 GetKernelTotal()
        {
            return (UInt64)PageSize * (UInt64)KernelTotal;
        }

        public UInt64 GetKernelPaged()
        {
            return (UInt64)PageSize * (UInt64)KernelPaged;
        }

        public UInt64 GetKernelNonPaged()
        {
            return (UInt64)PageSize * (UInt64)KernelNonpaged;
        }

        public UInt64 GetPageSize()
        {
            return (UInt64)PageSize;
        }

        public UInt64 GetHandleCount()
        {
            return (UInt64)HandleCount;
        }

        public UInt64 GetProcessCount()
        {
            return (UInt64)ProcessCount;
        }

        public UInt64 GetThreadCount()
        {
            return (UInt64)ThreadCount;
        }

    };




    [StructLayout(LayoutKind.Sequential, Size = 60, Pack = 1)]
    public struct MIB_TCPSTATS
    {
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwRtoAlgorithm;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwRtoMin;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwRtoMax;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwMaxConn;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwActiveOpens;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwPassiveOpens;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwAttemptFails;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwEstabResets;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwCurrEstab;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwInSegs;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwOutSegs;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwRetransSegs;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwInErrs;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwOutRsts;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwNumConns;
    }
    [StructLayout(LayoutKind.Sequential, Size = 20, Pack = 1)]
    public struct MIB_UDPSTATS
    {
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwInDatagrams;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwNoPorts;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwInErrors;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwOutDatagrams;
        [MarshalAs(UnmanagedType.U4)]
        public DWORD dwNumAddrs;
    }

    public enum KAILoginSubopcodes
    {
        KAI_LOGIN_SUCCESS = 0x01,
        KAI_LOGIN_INVALID_CREDENTIALS = 0x02,
        KAI_LOGIN_HANDSHAKE_FAILURE = 0x03,
        KAI_LOGIN_UNAUTHORIZED_IP = 0x04,
        KAI_LOGIN_INSUFFICIENT_PRIVILEGE = 0x05,
        KAI_LOGIN_ALREADY = 0x06,
    };

    public enum KAIHandshakeSubopcodes
    {
        KAI_HANDSHAKE_SUCCESS = 0x01,
        KAI_HANDSHAKE_FAILURE = 0x02,
        KAI_HANDSHAKE_ALREADY = 0x03,

    };


    public enum SMISubOpcodes
    {
        REQUEST_SUCCESS = 0x01,
        REQUEST_NO_PRIVILEGE = 0x02
    };

    public sealed class InterfaceInformation
    {
        public InterfaceInformation()
        {
            SentErrors = new uint[2];
            RecvErrors = new uint[3];
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string MACAddress { get; set; }
        public DWORD Speed { get; set; }
        public DWORD MTU { get; set; }
        public ULONGLONG SentBytes { get; set; }
        public ULONGLONG SentDelta { get; set; }
        public ULONGLONG RecvBytes { get; set; }
        public ULONGLONG RecvDelta { get; set; }
        public DWORD[] SentErrors;
        public DWORD[] RecvErrors;
        public string NetworkIP { get; set; }
        public string HostIP { get; set; }
        public string SubnetMask { get; set; }

        public void UpdateSRAmounts(ULONGLONG s, ULONGLONG r)
        {
            SentBytes = s;
            RecvBytes = r;
        }
    }

    public static class AIStatus
    {
        public static int MonsterCount { get; set; }
        public static int NPCCount { get; set; }
        public static int ObjectCount { get; set; }
        public static bool Running { get; set; }
    }

    public static class CurrentManagerInfo
    {

        public static UInt64 SessionID { get; set; }
        public static string UserID { get; set; }
        public static byte Authority { get; set; }
        public static DateTime AuthDate { get; set; }
        public static string AuthorizedBy { get; set; }
        public static string Name { get; set; }
        public static string Surname { get; set; }
    }

    public static class OnlineCounts
    {
        public static int Maximum { get; set; }
        public static int Karus { get; set; }
        public static int Elmorad { get; set; }
        public static int Gamemaster { get; set; }
        public static int Administrator { get; set; }
        public static int InGame { get; set; }
        public static int Total { get { return Elmorad + Karus; } }

    }

    public sealed class CPU
    {
        public float Load { get; set; }
        public int LogicalCores { get; set; }
        public string Model { get; set; }
        public int PhysicalCores { get; set; }
        public float Speed { get; set; }
    }

    public sealed class Credentials
    {
        public string ManagerID { get; set; }
        public string Password { get; set; }
    }

    public sealed class Memory
    {
        public ulong GameserverUsage { get; set; }
        public ulong SQLUsage { get; set; }
    }

    public sealed class OnlineUser
    {
        public string ID { get; set; }
        public short SocketID { get; set; }
        public ushort Zone { get; set; }
    }

    public sealed class PartyMember
    {
        public ushort SocketID { get; set; }
        public string Name { get; set; }
        public ushort MaxHP { get; set; }
        public byte Nation { get; set; }
        public ushort ClanID { get; set; }
        public ushort HP { get; set; }
        public byte Level { get; set; }
        public ushort Class { get; set; }
        public ushort MaxMP { get; set; }
        public ushort MP { get; set; }
    }

    public sealed class Progress
    {
        public UInt64 Current { get; private set; }
        public UInt64 Maximum { get; private set; }

        public void Set(UInt64 c, UInt64 m)
        {
            Current = c;
            Maximum = m;
        }
    }

    public sealed class Resistances
    {
        public int Curse { get; private set; }
        public int Flame { get; private set; }
        public int Glacier { get; private set; }
        public int Lightning { get; private set; }
        public int Magic { get; private set; }
        public int Poison { get; private set; }

        public void UpdateResistances(int f, int g, int l, int m, int c, int p)
        {
            Flame = f;
            Glacier = g;
            Lightning = l;
            Magic = m;
            Curse = c;
            Poison = p;
        }
    }

    [Serializable]
    public sealed class ServerEntry
    {
        public string Host { get; set; }
        public string Name { get; set; }
        public ushort Port { get; set; }
    }

    public sealed class Skills
    {
        public UInt16 Profession1 { get;  set; }
        public UInt16 Profession2 { get;  set; }
        public UInt16 Profession3 { get;  set; }
        public UInt16 Master { get; set; }
        public UInt16 Unused { get;  set; }

        public void UpdateAll(UInt16 pro1, UInt16 pro2, UInt16 pro3, UInt16 master)
        {
            Profession1 = pro1;
            Profession2 = pro2;
            Profession3 = pro3;
            Master = master;
        }

    }

    public sealed class Stats
    {
        public int DEX { get; private set; }
        public int DEXB { get; private set; }
        public int HP { get; private set; }
        public int HPB { get; private set; }
        public int INT { get; private set; }
        public int INTB { get; private set; }
        public int MP { get; private set; }
        public int MPB { get; private set; }
        public int STR { get; private set; }
        public int STRB { get; private set; }

        public void UpdateDEX(int raw, int bonus)
        {
            DEX = raw;
            DEXB = bonus;
        }

        public void UpdateHP(int raw, int bonus)
        {
            HP = raw;
            HPB = bonus;
        }

        public void UpdateINT(int raw, int bonus)
        {
            INT = raw;
            INTB = bonus;
        }

        public void UpdateMP(int raw, int bonus)
        {
            MP = raw;
            MPB = bonus;
        }

        public void UpdateSTR(int raw, int bonus)
        {
            STR = raw;
            STRB = bonus;
        }
    }

    public sealed class TechnicalInformation
    {
        public CPU CPU = new CPU();
        public Memory Memory = new Memory();
    }

    public sealed class UserTargetInformation
    {
        public Int16 TargetID { get; private set; }
        public Progress Health = new Progress();

        public void Update(Int16 tid, UInt16 hp, UInt16 maxhp)
        {
            TargetID = tid;
            Health.Set(hp, maxhp);
        }
    }

    public sealed class PlayerListEntry
    {
        public long SocketID { get; set; }
        public string PlayerID { get; set; }
        public string AccountID { get; set; }
        public byte Level { get; set; }
        public string Nation { get; set; }
        public string Job { get; set; }
        public short ClanID { get; set; }
        public string ClanName { get; set; }
        public string ClanFame { get; set; }
        public string Authority { get; set; }
        public string PremiumType { get; set; }
        public string Zone { get; set; }
        public long InstanceID { get; set; }
        public string IP { get; set; }
        public int Ping { get; set; }

    }


    public sealed class User
    {
        public delegate void SlotItemChangeDelegate(byte pos);

        public event SlotItemChangeDelegate OnSlotChanged;

        public Int16 SocketID { get; private set; }
        public String AccountID { get; private set; }
        public String CharacterID { get; private set; }
        public UInt16 Zone { get; private set; }
        public UInt16 Attack { get; private set; }
        public UInt16 Defense { get; private set; }
        public float PosX { get; private set; }
        public float PosY { get; private set; }
        public float PosZ { get; private set; }
        public byte Nation { get; private set; }
        public byte Race { get; private set; }
        public UInt16 Class { get; private set; }
        public Int16 ClanID { get; private set; }
        public String ClanName { get; private set; }
        public byte Level { get; private set; }
        public UInt32 MaxWeight { get; private set; }
        public UInt64 NationalPoints { get; private set; }
        public UInt64 LeaderPoints { get; private set; }
        public UInt64 MannerPoints { get; private set; }
        public UInt64 Gold { get; private set; }
        public byte Authority { get; private set; }

        public byte PremiumType { get; private set; }
        public UInt64 PremiumStart { get; private set; }
        public UInt64 PremiumEnd { get; private set; }
        public StatusFlags Flags { get; private set; }


        public InventorySlot[] Inventory = new InventorySlot[72];


        UserTargetInformation Target = new UserTargetInformation();


        public Progress Experience = new Progress();
        public Progress Health = new Progress();
        public Progress Mana = new Progress();
        public Resistances Resistances = new Resistances();
        public Stats Stats = new Stats();
        public Skills Skills = new Skills();


        public bool CheckFlagSet(StatusFlags flagToCheck)
        {
            return (Flags & flagToCheck) == flagToCheck;
        }

        public void PremiumChange(Packet pkt)
        {
            /*	Packet result(WIZ_PREMIUM, m_bAccountStatus);
            result << static_cast<uint8>(Premium.Type()) << uint32(Premium.GetRemainingHours());*/
            byte account_status = pkt.Read<byte>();
            PremiumType = pkt.Read<byte>();
            PremiumStart = (ulong)CommonReference.GetUnixTimestamp();
            PremiumEnd = (ulong)((pkt.Read<UInt32>() * 60 * 60) + Common.Declarations.CommonReference.GetUnixTimestamp());
        }

        public void ZoneChange(ushort newZone)
        {
            Zone = newZone;
        }

        public void LevelChange(Packet pkt)
        {
            Int16 sid = pkt.Read<Int16>();
            Level = pkt.Read<byte>();
            Int16 freeStatPoints = pkt.Read<Int16>();
            byte fskill = pkt.Read<byte>();
            Experience.Set((ulong)pkt.Read<Int32>(), (ulong)pkt.Read<Int32>());
            MaxWeight = pkt.Read<UInt32>();
            Int32 itemWeight = pkt.Read<Int32>();
        }

        public void MpChange(Packet pkt)
        {
            UInt16 max = pkt.Read<UInt16>();
            Mana.Set(pkt.Read<UInt16>(), max);
        }
        public void HpChange(Packet pkt)
        {
            UInt16 max = pkt.Read<UInt16>();
            Health.Set(pkt.Read<UInt16>(), max);

            Int16 tid = pkt.Read<Int16>(); // could be useful later..
        }

        public void Warp(Packet pkt)
        {
            PosX = pkt.Read<ushort>() / 10;
            PosZ = pkt.Read<ushort>() / 10;
        }
        public void GoldChange(Packet pkt)
        {
            var subcommand = pkt.Read<byte>();
            UInt32 changeamount = pkt.Read<UInt32>();
            Gold = pkt.Read<UInt32>();
            // we can use it later if we need to.

        }

     

        public void ItemMove(Packet pkt)
        {
            var subcommand = pkt.Read<byte>(); // skip subopcode
            if (subcommand != 0)
            {
                Attack = pkt.Read<UInt16>();
                Defense = pkt.Read<UInt16>();
                MaxWeight = pkt.Read<UInt32>();
                Health.Set(Health.Current, pkt.Read<ushort>());
                Mana.Set(Mana.Current, pkt.Read<ushort>());
                Stats.UpdateSTR(Stats.STR, pkt.Read<Int16>());
                Stats.UpdateHP(Stats.HP, pkt.Read<Int16>());
                Stats.UpdateDEX(Stats.DEX, pkt.Read<Int16>());
                Stats.UpdateINT(Stats.INT, pkt.Read<Int16>());
                Stats.UpdateMP(Stats.MP, pkt.Read<Int16>());
                Resistances.UpdateResistances(pkt.Read<UInt16>(), pkt.Read<UInt16>(), pkt.Read<UInt16>(),
                    pkt.Read<UInt16>(), pkt.Read<UInt16>(), pkt.Read<UInt16>());
            }
        }

        public void LoyaltyChange(Packet pkt)
        {
            if (pkt.Read<byte>() == 0x01)
            {
                NationalPoints = pkt.Read<UInt32>();
                LeaderPoints = pkt.Read<UInt32>();
            }
            else
            {
                // Manner change
                MannerPoints = pkt.Read<UInt32>();
            }
        }

        public void ExpChange(Packet pkt)
        {
            byte flag = pkt.Read<byte>();
            Experience.Set((ulong)pkt.Read<Int64>(), Experience.Maximum);
        }

        public void TargetHP(Packet pkt)
        {

            Int16 tid = pkt.Read<Int16>();
            byte echo = pkt.Read<byte>();
            int maxhp = pkt.Read<int>(), hp = pkt.Read<int>();
            Target.Update(tid, (ushort)hp, (ushort)maxhp);
        }

        public void Move(Packet pkt)
        {

            // uint16 will_x, will_z, will_y;
            // int16 speed = 0;
            // float real_x, real_z, real_y;
            // uint8 echo;
            // 
            //  pkt >> will_x >> will_z >> will_y >> speed >> echo;
            pkt.Read<UInt16>();UInt16 will_x, will_z, will_y;
            will_x = pkt.Read<UInt16>();
            will_z = pkt.Read<UInt16>();
            will_y = pkt.Read<UInt16>();
            PosX = will_x / 10;
            PosY = will_y / 10;
            PosZ = will_z / 10;
            return;
        }



        public void AllInfoUpdate(short sid, Packet pkt)
        {
            SocketID = sid;
            AccountID = pkt.Read<string>(false);
            CharacterID = pkt.Read<string>(false);
            Zone = pkt.Read<UInt16>();
            PosX = pkt.Read<float>();
            PosZ = pkt.Read<float>();
            Nation = pkt.Read<byte>();
            Race = pkt.Read<byte>();
            Class = pkt.Read<UInt16>();
            ClanID = pkt.Read<Int16>();
            if (ClanID != -1)
                ClanName = pkt.Read<string>(false);
            Level = pkt.Read<byte>();
            Experience.Set(pkt.Read<UInt64>(), pkt.Read<UInt64>());
            NationalPoints = pkt.Read<UInt64>();
            LeaderPoints = pkt.Read<UInt64>();
            Gold = pkt.Read<UInt64>();
            Authority = pkt.Read<byte>();
            Stats.UpdateSTR(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Stats.UpdateHP(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Stats.UpdateDEX(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Stats.UpdateINT(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Stats.UpdateMP(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Resistances.UpdateResistances
            (
                pkt.Read<UInt16>(), pkt.Read<UInt16>(),
                pkt.Read<UInt16>(), pkt.Read<UInt16>(),
                pkt.Read<UInt16>(), pkt.Read<UInt16>()
            );
            Health.Set(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Mana.Set(pkt.Read<UInt16>(), pkt.Read<UInt16>());
            Skills.UpdateAll(pkt.Read<UInt16>(), pkt.Read<UInt16>(), pkt.Read<UInt16>(), pkt.Read<UInt16>());

            PremiumType = pkt.Read<byte>();
            PremiumStart = pkt.Read<UInt64>();
            PremiumEnd = pkt.Read<UInt64>();
 
            Flags = (StatusFlags)pkt.Read<UInt32>();

            /* Read inventory */
            for (int i = 0; i < 72; i++)
            {
                InventorySlot slot = new InventorySlot()
                {
                    Pos = (byte) i,
                    ItemID = pkt.Read<uint>(),
                    Durability = pkt.Read<short>(),
                    Count = pkt.Read<ushort>(),
                    Flag = pkt.Read<byte>(),
                    RemainingRentalTime = pkt.Read<short>(),
                    ExpirationTime = pkt.Read<uint>(),
                    Serial = pkt.Read<ulong>()
                };
                slot.Info = CommonReference.GetItemDetails((int) slot.ItemID);
                Inventory[i] = slot;
            }
            return;

        }


        public void ItemCountChange(Packet pkt)
        {

            var unknown = pkt.Read<ushort>();
            byte bagtype = pkt.Read<byte>();
            byte bPos = pkt.Read<byte>();
            int itemId = pkt.Read<int>();
            short count = pkt.Read<short>(), durability = pkt.Read<short>();
            bool isNew = pkt.Read<byte>() == 100;

            Inventory[14 + bPos].ItemID = (uint)itemId;
            Inventory[14 + bPos].Count = (ushort)count;
            Inventory[14 + bPos].Durability = durability;
            Inventory[14 + bPos].Info = CommonReference.GetItemDetails((int)itemId);
            OnSlotChanged?.Invoke((byte)(bPos + 14));



            /*    Packet result(WIZ_ITEM_COUNT_CHANGE);

            result << uint16(1);
            result << uint8(1);
            result << uint8(bPos);
            result << nItemID << nCount;
            result << uint8(bNewItem ? 100 : 0);
            result << sDurability;
            result << uint32(0);
            result << ulExipration;*/
        }

        enum LootErrorCodes
        {
            LootError = 0,
            LootSolo = 1,
            LootPartyCoinDistribution = 2,
            LootPartyNotification = 3,
            LootPartyItemGivenAway = 4,
            LootPartyItemGivenToUs = 5,
            LootNoRoom = 6
        };


        public void ItemGet(Packet receivedPacket)
        {
        
            byte type = receivedPacket.Read<byte>();
            switch ((LootErrorCodes) type)
            {
                case LootErrorCodes.LootError:
                case LootErrorCodes.LootNoRoom:
                case LootErrorCodes.LootPartyItemGivenAway:
                case LootErrorCodes.LootPartyNotification:
                    return;
            }

            uint nBundleID = receivedPacket.Read<uint>();
            byte bPos = receivedPacket.Read<byte>();
            uint itemId = receivedPacket.Read<uint>();
            short count = receivedPacket.Read<short>();
            uint gold = receivedPacket.Read<uint>();

            if (itemId == 900000000)
            {
                return;
            }

            Inventory[14 + bPos].ItemID = (uint)itemId;
            Inventory[14 + bPos].Count = (ushort)count;
            Inventory[14 + bPos].Info = CommonReference.GetItemDetails((int)itemId);
            Inventory[14 + bPos].Durability = Inventory[14 + bPos].Info.Duration;
         
            OnSlotChanged?.Invoke((byte)(bPos + 14));


        }

        public void ItemBuySell(Packet receivedPacket)
        {
           /*pkt >> sellingGroup >> npcid >> reqItemID >> itemPos >> itemCount;*/
            if (receivedPacket.Size < 14)
                return;
            byte operationType = receivedPacket.Read<byte>();
            int sellingGroup = receivedPacket.Read<int>();
            short npcID = receivedPacket.Read<short>();
            uint itemID = receivedPacket.Read<uint>();
            byte bPos = receivedPacket.Read<byte>();
            short itemCount = receivedPacket.Read<short>();
            switch (operationType)
            {
                /* buy */
                case 1:
                    if (Inventory[14 + bPos].ItemID == itemID)
                        Inventory[14 + bPos].Count += (ushort) itemCount;
                    else
                    {
                        Inventory[14 + bPos].ItemID = itemID;
                        Inventory[14 + bPos].Count = (ushort) itemCount;
                        Inventory[14 + bPos].Info = CommonReference.GetItemDetails((int)itemID);
                    }
                    break;
                /* sell */
                case 2:
                    if (Inventory[14 + bPos].ItemID == itemID)
                    {
                        Inventory[14 + bPos].Count -= (ushort) itemCount;
                        if (Inventory[14 + bPos].Count <= 0)
                            Inventory[14 + bPos].Reset();

                    }
                    else
                    {
                        Inventory[14 + bPos].Reset();
                    }
                    break;
            }

            OnSlotChanged?.Invoke((byte)(bPos + 14));
        }

        public void DoSlotExchange(byte bSrcPos, byte bDstPos)
        {
            var copy = new InventorySlot();
            copy.Info = new ItemInformation();
            Inventory[bSrcPos].CopyTo(copy);
            Inventory[bDstPos].CopyTo(Inventory[bSrcPos]);
            copy.CopyTo(Inventory[bDstPos]);
            OnSlotChanged?.Invoke(bSrcPos);
            OnSlotChanged?.Invoke(bDstPos);
        }
    }

  
}