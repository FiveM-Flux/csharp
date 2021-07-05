using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FLUX

{
    public class FLUX : BaseScript

    {
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("Input is not a valid string.");
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        string framework = "[FLUX]";

        public FLUX()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                var model = "Adder";
                if (args.Count > 0)
                {
                    model = FirstCharToUpper(args[0].ToString());
                }

                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 148, 0, 211 },
                        args = new[] { framework, $"Your vehicle input of: ^*{model} ^rdoes not exist!" }
                    });
                    return;
                }

                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 148, 0, 211 },
                    args = new[] { framework, $"Spawned vehicle: ^*{model}" }
                });
            }), false);
        }
    }
}
