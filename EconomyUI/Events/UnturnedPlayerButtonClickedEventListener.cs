using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.Core.Users;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Players.UI.Events;
using SDG.Unturned;

namespace EconomyUI.Events
{
    public class UnturnedPlayerButtonClickedEventListener : IEventListener<UnturnedPlayerButtonClickedEvent>
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IEconomyProvider _economyProvider;
        
        public UnturnedPlayerButtonClickedEventListener(IStringLocalizer stringLocalizer, IEconomyProvider economyProvider)
        {
            _stringLocalizer = stringLocalizer;
            _economyProvider = economyProvider;
        }
        
        public async Task HandleEventAsync(object? sender, UnturnedPlayerButtonClickedEvent @event)
        {
            if (@event.ButtonName == "DefaultPosition")
            {
                var balance = await _economyProvider.GetBalanceAsync(@event.Player.SteamId.ToString(), KnownActorTypes.Player);
                
                await UniTask.SwitchToMainThread();
                @event.Player.Player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                EffectManager.sendUIEffect(4425, 431, @event.Player.Player.channel.GetOwnerTransportConnection(), true);
                EffectManager.sendUIEffectText(431, @event.Player.Player.channel.GetOwnerTransportConnection(), true, "MoneyText", _stringLocalizer["UI:BalanceFormat", new { Money = Convert.ToInt32(balance) }]);
                await UniTask.SwitchToThreadPool();
            }
            else if (@event.ButtonName == "Close")
            {
                await UniTask.SwitchToMainThread();
                @event.Player.Player.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                EffectManager.sendUIEffectVisibility(431, @event.Player.Player.channel.GetOwnerTransportConnection(), true, "Controllers", false);
                await UniTask.SwitchToThreadPool();
            }
        }
    }
}