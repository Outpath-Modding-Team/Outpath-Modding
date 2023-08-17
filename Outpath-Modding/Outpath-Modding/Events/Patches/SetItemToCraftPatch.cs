using HarmonyLib;
using Outpath_Modding.Events.EventArguments;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Outpath_Modding.Events.Patches
{
    [HarmonyPatch(typeof(Build_Craft), nameof(Build_Craft.SetItemToCraft))]
    public class SetItemToCraftPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder eventArgs = il.DeclareLocal(typeof(SetItemToCraftEventArgs));
            Label ret = il.DefineLabel();

            var code = new List<CodeInstruction>(instructions);
            var instructionsToInsert = new List<CodeInstruction>();

            code[0].labels.Add(ret);

            instructionsToInsert.Add(new(OpCodes.Ldarg_1));
            instructionsToInsert.Add(new(OpCodes.Ldarg_2));
            instructionsToInsert.Add(new(OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(SetItemToCraftEventArgs))[0]));
            instructionsToInsert.Add(new(OpCodes.Stloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Call, AccessTools.Method(typeof(EventsManager), nameof(EventsManager.OnSetItemToCraft))));

            //instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            //instructionsToInsert.Add(new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(SetItemToCraftEventArgs), nameof(SetItemToCraftEventArgs.Quantity))));
            //instructionsToInsert.Add(new(OpCodes.Starg_S, 2));

            instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(SetItemToCraftEventArgs), nameof(SetItemToCraftEventArgs.IsAllowed))));
            instructionsToInsert.Add(new(OpCodes.Brfalse, ret));

            code.InsertRange(0, instructionsToInsert);

            return code;
        }
    }
}
