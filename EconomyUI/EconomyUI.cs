using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;

[assembly: PluginMetadata("Feli.EconomyUI", DisplayName = "EconomyUI", Website = "fplugins.com", Author = "Feli")]
namespace EconomyUI
{
    public class EconomyUI : OpenModUnturnedPlugin
    {
        public EconomyUI(
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
