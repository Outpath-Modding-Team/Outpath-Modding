using HarmonyLib;
using Outpath_Modding.Events.EventArguments;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Outpath_Modding.Events.Patches
{
    public class SetItemToInfiniteCraftPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder eventArgs = il.DeclareLocal(typeof(SetItemToInfiniteCraftEventArgs));
            Label ret = il.DefineLabel();

            var code = new List<CodeInstruction>(instructions);
            var instructionsToInsert = new List<CodeInstruction>();

            code[0].labels.Add(ret);

            instructionsToInsert.Add(new(OpCodes.Ldarg_0));
            instructionsToInsert.Add(new(OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(SetItemToInfiniteCraftEventArgs))[0]));
            instructionsToInsert.Add(new(OpCodes.Stloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Call, AccessTools.Method(typeof(EventsManager), nameof(EventsManager.OnSetItemToInfiniteCraft))));

            instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(SetItemToInfiniteCraftEventArgs), nameof(SetItemToInfiniteCraftEventArgs.IsAllowed))));
            instructionsToInsert.Add(new(OpCodes.Brfalse, ret));

            code.InsertRange(0, instructionsToInsert);

            return code;
        }
    }
}
