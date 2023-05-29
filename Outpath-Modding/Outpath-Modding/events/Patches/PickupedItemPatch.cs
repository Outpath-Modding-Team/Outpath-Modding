using HarmonyLib;
using Outpath_Modding.Events.EventArguments;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Outpath_Modding.Events.Patches
{
    [HarmonyPatch(typeof(ItemPrefab), nameof(ItemPrefab.PickupItemFromPlayer))]
    public class PickupedItemPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder eventArgs = il.DeclareLocal(typeof(PickupedItemEventArgs));

            var code = new List<CodeInstruction>(instructions);

            int insertionIndex = -1;
            Label return566Label = il.DefineLabel();
            for (int i = 0; i < code.Count - 1; i++)
            {
                if (code[i].opcode == OpCodes.Ldsfld && code[i - 1].opcode == OpCodes.Stfld && code[i + 1].opcode == OpCodes.Ldarg_0)
                {
                    insertionIndex = i;
                    code[i].labels.Add(return566Label);
                    break;
                }
            }

            var instructionsToInsert = new List<CodeInstruction>();

            instructionsToInsert.Add(new(OpCodes.Ldarg_0));
            instructionsToInsert.Add(new(OpCodes.Ldfld, AccessTools.Field(typeof(ItemPrefab), nameof(ItemPrefab.itemInfo))));
            instructionsToInsert.Add(new(OpCodes.Ldarg_0));
            instructionsToInsert.Add(new(OpCodes.Ldfld, AccessTools.Field(typeof(ItemPrefab), nameof(ItemPrefab.quantity))));
            instructionsToInsert.Add(new(OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(PickupedItemEventArgs))[0]));
            instructionsToInsert.Add(new(OpCodes.Call, AccessTools.Method(typeof(EventsManager), nameof(EventsManager.OnPickupedItem))));

            if (insertionIndex != -1)
            {
                code.InsertRange(insertionIndex, instructionsToInsert);
            }
            return code;
        }
    }
}