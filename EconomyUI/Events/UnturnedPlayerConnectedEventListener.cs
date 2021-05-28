using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.Core.Users;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Players.Connections.Events;
using SDG.Unturned;

namespace EconomyUI.Events
{
    public class UnturnedPlayerConnectedEventListener : IEventListener<UnturnedPlayerConnectedEvent>
    {
        private readonly IStringLocalizer _stringLocalizer;
        private readonly IEconomyProvider _economyProvider;
        
        public UnturnedPlayerConnectedEventListener(IStringLocalizer stringLocalizer, IEconomyProvider economyProvider)
        {
            _stringLocalizer = stringLocalizer;
            _economyProvider = economyProvider;
        }
        
        public async Task HandleEventAsync(object? sender, UnturnedPlayerConnectedEvent @event)
        {
            var balance = await _economyProvider.GetBalanceAsync(@event.Player.SteamId.ToString(), KnownActorTypes.Player);

            await UniTask.SwitchToMainThread();
            EffectManager.sendUIEffect(4425, 431, @event.Player.Player.channel.GetOwnerTransportConnection(), true);
            EffectManager.sendUIEffectText(431, @event.Player.Player.channel.GetOwnerTransportConnection(), true, "MoneyText", _stringLocalizer["UI:BalanceFormat", new { Money = Convert.ToInt32(balance) }]);
            await UniTask.SwitchToThreadPool();
        }
    }
}