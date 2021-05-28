using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Core.Users;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;

namespace EconomyUI.Commands
{
    [Command("economyui")]
    [CommandDescription("A command to manage the EconomyUI")]
    public class EconomyUICommand : UnturnedCommand
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IEconomyProvider _economyProvider;
        
        public EconomyUICommand(IEconomyProvider economyProvider, IStringLocalizer stringLocalizer, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _economyProvider = economyProvider;
            _stringLocalizer = stringLocalizer;
        }

        protected async override UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Length < 1)
            {
                throw new CommandWrongUsageException(_stringLocalizer["EconomyUICommand:CorrectUsage"]);
            }
            
            var parameter = await Context.Parameters.GetAsync<string>(0);
            var player = (UnturnedUser) Context.Actor;
            
            switch (parameter)
            {
                case "on":
                    var balance = await _economyProvider.GetBalanceAsync(player.SteamId.ToString(), KnownActorTypes.Player);
                    await UniTask.SwitchToMainThread();
                    EffectManager.sendUIEffect(4425, 431, player.Player.Player.channel.GetOwnerTransportConnection(), true);
                    EffectManager.sendUIEffectText(431, player.Player.Player.channel.GetOwnerTransportConnection(), true, "MoneyText", _stringLocalizer["UI:BalanceFormat", new { Money = Convert.ToInt32(balance) }]);
                    await UniTask.SwitchToThreadPool();
                    await player.PrintMessageAsync(_stringLocalizer["EconomyUICommand:OnSuccessfully"]);
                    break;
                case "off":
                    await UniTask.SwitchToMainThread();
                    EffectManager.askEffectClearByID(4425, player.Player.Player.channel.GetOwnerTransportConnection());
                    await UniTask.SwitchToThreadPool();
                    await player.PrintMessageAsync(_stringLocalizer["EconomyUICommand:OffSuccessfully"]);
                    break;
                case "move":
                    await UniTask.SwitchToMainThread();
                    player.Player.Player.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                    EffectManager.sendUIEffectVisibility(431, player.Player.Player.channel.GetOwnerTransportConnection(), true, "Controllers", true);
                    await UniTask.SwitchToThreadPool();    
                    break;
                default:
                    throw new CommandWrongUsageException(_stringLocalizer["EconomyUICommand:CorrectUsage"]);
            }
        }
    }
}