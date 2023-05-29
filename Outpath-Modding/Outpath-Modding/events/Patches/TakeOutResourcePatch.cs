using HarmonyLib;
using Outpath_Modding.Events.EventArguments;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Outpath_Modding.Events.Patches
{
    [HarmonyPatch(typeof(TakeOutResource), nameof(TakeOutResource.TryTakeOut_General))]
    public class TakeOutResourcePatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            LocalBuilder eventArgs = il.DeclareLocal(typeof(TakeOutResourceEventArgs));

            var code = new List<CodeInstruction>(instructions);
            var instructionsToInsert = new List<CodeInstruction>();

            instructionsToInsert.Add(new(OpCodes.Ldarg_0));
            instructionsToInsert.Add(new(OpCodes.Ldarg_1));
            instructionsToInsert.Add(new(OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(TakeOutResourceEventArgs))[0]));
            instructionsToInsert.Add(new(OpCodes.Stloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Call, AccessTools.Method(typeof(EventsManager), nameof(EventsManager.OnTakeOutResource))));

            instructionsToInsert.Add(new(OpCodes.Ldloc_S, eventArgs.LocalIndex));
            instructionsToInsert.Add(new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(TakeOutResourceEventArgs), nameof(TakeOutResourceEventArgs.Damage))));
            instructionsToInsert.Add(new(OpCodes.Starg_S, 1));

            code.InsertRange(0, instructionsToInsert);

            return code;
        }
    }
}
