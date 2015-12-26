using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JuggleServerCore;

namespace DecoServer2
{
    public class Item
    {
        public enum ItemError
        {
            None = 1,
            UnableToFindItemInformation = 2,
            UnableToFindItem = 3,
            InventoryIsFull = 4,
            UnableToEquipHere = 5,
            InsufficientELD = 6,
            CheckTheNumber = 7,
            ItemIsNotForSale = 8,
            UnableToEquipItem = 9,
            UnableToUseItem = 10,

            Denied = 12,
            DisarmLeftHandEquipment = 13,
            DisarmRightHandEquipment = 14,
            TheItemDoesntBelongtoThePlayersCountry=15,

            NoNecessarySkillExisted=18,
            NotATarget=20,
            AwayFromTheRange=21,
            NoAuthority=22,
            ThisSlotIsNotEmpty = 0x17,
            YouCanUseItAfterTheCooldownTimeRunsOut =0x18,
            InsufficientPlace=0x1A,

            SkillLevelIsNotEnough=0x1D,
            TheItemIsNotAvailableToRemodel=0x1E,
            YouAreNotAbleToUse=0x20,

            YouNeedAScrollLog=0x22,
            ItIsAlreadyAnOpenedScroll=0x23,
            UnableToUseScroll=0x24,
            TheSubJobDoesntFitTheScroll=0x25,

            TheOreIsUnableToEffectTheShield=0x28,
            TheOreIsUnableToEffectTheWeapons=0x29,
            CashItemDoesntSupportTheFunctionYet=0x2A,
            MaintenanceInCashItemMall=0x2B,
            CannotUseItemWithThisRoomOption=0x2C,
            NotEnoughFamePoints=0x2D,
            ExcessOverMaximumPetNumber=0x2E,
            PetItemOnlyBeOnPetInventory=0x2F,
            CannotBeUsedForThisTypeOfPet=0x30,
            RidingCannotExceed3Numbers=0x31,
            CannotEquipThisItemOnRidingAnimal=0x32,
            UndressAllItemsFromRidingAnimal=0x33,
            ToMoveYouShouldRepairTheRiding=0x34,
            After30SecondsFromLastAttackYouCanRideOn=0x35,
            RemoveAllTheOtherRidingItems=0x36,
            ThisUserHasntLoggedOnYet=0x37,
            TargetIsInInvalidPositionToMoveOrRecall=0x38,
            FirstJobTransferIsNeeded=0x39,
            SecondJobTransferIsNeeded=0x3A,
            EnchantingWasFailed=0x3B,
            WingSlotHasToBeEmpty=0x3C,
            ThePositionFlagIsOnlyPossibleToThrowDown=0x3D,
            PotionMixingIsWrong=0x3E,

            NotEnoughELDToPayFee=0x43,
            ForChangeItemOfPetYouMustCancelPet=0x44,
            YouCannotChangeTheDistinctionOfSexDurringWearing=0x45,
            CannotSummonRentalSoldierInYourLevel=0x46,
            RentalSoldierWasAlreadyExistedInRentalSlot=0x47,
            ThereIsNoScript=0x48,
            InsufficientScript=0x49,
            ThereIsNoAccessay=0x4A,
            MaximumLevelIs20=0x4B,
            ProcessingAccessoryCannotDropTradeSaleAndKeep=0x4C,
            AccessaryTypeIsDifferent=0x4D,
            UnableToSummonRentalSoldier=0x4E,
            UnableToRecallorGoDirectToGM=0x4F,
            ThisQuestIsOnlyOneChance=0x50,
            ThisQuestIsOnlyOneChancePerDay=0x51,
            ThisScrollIsNotForYourClass=0x52,
            ItemOfSameEffectCannotUseAtTheSameTime=0x53,
            HolyCreatureDoesNotExist=0x54,
            YouAreNotTheMemberOfFrontier=0x55,
            YourHolyCreatureWasDead=0x56,
            TheRightForTheMajorGradeOver=0x57,
            YouMustSummonHolyCreature=0x58,
            NeedSameGradeEldaStone=0x59,
            ConfirmEldastoneSuitibilityForHyperstone=0x5A,
            FailMakingHyperstone=0x5B,
            HyperstoneMakeInSequence=0x5C,
            QuestAlreadyActThatRequestSameQuestItem=0x5D,
            NeedSameGradeInsuranceItem=0x5E,
            ThisPetIsNotAbleToCashThisItem=0x5F,
            ConfirmGradeOfCashItemForAChange=0x60,

            CannotRideHorseInInstantDungeon=0x62,
            ExceedBundleOfCreateItem=0x63,
            DoesNotEquipTheItemWhichIsNecessary=0x64,

            ArgoreHeights_CannotEnterInYourLevel=0x66,
            GordoCanyon_CannotEnterInYourLevel=0x67,
            ThePlayerOfTheDifferentNationIsNotAPossibilityOfWhichWillRecallOrWillAppear=0x68,
            CannotAddMoreInventory=0x69,

        }


        public enum Type
        {
            Equipped,
            General,
            Stackable,
            Quest,
            Riding
        }

        uint _id;
        ushort _model;
        byte _slot;
        ushort _durability;
        ushort _remainingTime;
        Type _type;
        uint _templateID;
        ItemTemplate _template;
        
        public void Write(BinaryWriter bw)
        {
            bw.Write(_id);              // 0xF  - DWORD - Item ID
            bw.Write(_model);           // 0x13 - WORD - Model ID
            bw.Write((byte)0);          // 0x15 - BYTE - Unused by client
            bw.Write(_slot);            // 0x16 - BYTE - Slot
            bw.Write((uint)0);          // 0x17 - DWORD - Unused by client
            bw.Write(_durability);      // 0x1B - WORD - Durability/Quantity
            bw.Write(_remainingTime);   // 0x1D - WORD - Not used by client?
        }

        public string WriteDBString(uint itemTemplate, int characterID)
        {
            string sql = string.Format("INSERT INTO item_instances SET template_id={0},durability={1},character_id={2},inventory_type={3},slot={4}; SELECT LAST_INSERT_ID();", itemTemplate, _durability, characterID, (byte)_type, _slot);
            return sql;
        }

        public string UpdateDBString()
        {
            string sql = string.Format("UPDATE item_instances SET durability={0},inventory_type={1},slot={2} WHERE instance_id={3};", _durability, (byte)_type, _slot, _id);
            return sql;
        }

        public void AddQuantity(int quantity)
        {
            _durability = (ushort)(_durability + quantity);
        }

        public ItemError Use(Connection c)
        {
            ItemError err = ItemError.None;
            switch (_template.ItemFunction)
            {
                case ItemTemplate.ItemUseFunction.GainHealth:
                    if (c.Character.CurHP < c.Character.MaxHP)
                    {
                        _durability--;

                        // Adjust character health
                        c.Character.TakeDamage(-_template.ItemFunctionParam);

                        // Send health change to client
                        c.SendPacket(new HealthChangePacket(c.Character.CurHP));
                    }
                    else
                        err = ItemError.UnableToUseItem;
                    break;
                default:
                case ItemTemplate.ItemUseFunction.None:
                    break;
            }
            return err;
        }

        public static Item ReadFromDB(object[] row)
        {
            // 0: instance_id int(10) unsigned
            // 1: template_id int(10) unsigned
            // 2: durability  tinyint(3) unsigned
            // 3: character_id    int(10) unsigned
            // 4: inventory_type  tinyint(3) unsigned
            // 5: slot    tinyint(3) unsigned


            Item i = new Item();
            i._id = (uint)row[0];
            i._templateID = (uint)row[1];
            i._durability = (byte)row[2];
            i._type = (Type)((byte)row[4]);
            i._slot = (byte)(row[5]);

            i._template = Program.Server.GetItemTemplate(i._templateID);
            i._model = i._template.Model;

            return i;
        }

        public static Item Instantiate(ItemTemplate it)
        {
            // Setup the item
            Item item = new Item();
            item._id = 0;
            item._model = it.Model;
            item._durability = it.GenerateDurability();
            item._remainingTime = 0;
            item._type = (Type)it.Type;
            item._slot = 0xFF;
            item._templateID = it.ID;
            item._template = it;

            return item;
        }


        #region Accessors
        public Type ItemType
        {
            get { return _type; }
            set { _type = value; }
        }

        public byte Slot
        {
            get { return _slot; }
            set { _slot = value; }
        }

        public uint ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public ushort Model
        {
            get { return _model; }
        }

        public ushort Durability
        {
            get { return _durability; }
        }

        public ushort Quantity
        {
            get { return _durability; }
        }

        public int StackSpace
        {
            get { return _template.DQMax - _durability; }
        }

        public ushort RemainingTime
        {
            get { return _remainingTime; }
        }

        public uint TemplateID
        {
            get { return _templateID; }
        }
        #endregion
    }
}
