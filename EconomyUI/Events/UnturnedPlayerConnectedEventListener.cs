using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        
        public UnturnedPlayerConnectedEventListener(IStringLocalizer stringLocalizer, IEconomyProvider economyProvider, IConfiguration configuration)
        {
            _stringLocalizer = stringLocalizer;
            _economyProvider = economyProvider;
            _configuration = configuration;
        }
        
        public async Task HandleEventAsync(object? sender, UnturnedPlayerConnectedEvent @event)
        {
            var balance = await _economyProvider.GetBalanceAsync(@event.Player.SteamId.ToString(), KnownActorTypes.Player);

            await UniTask.SwitchToMainThread();
            EffectManager.sendUIEffect(_configuration.GetSection("ui_configuration:id").Get<ushort>(), 431, @event.Player.Player.channel.GetOwnerTransportConnection(), true);
            EffectManager.sendUIEffectText(431, @event.Player.Player.channel.GetOwnerTransportConnection(), true, "MoneyText", _stringLocalizer["UI:BalanceFormat", new { Money = Convert.ToInt32(balance) }]);
            await UniTask.SwitchToThreadPool();
        }
    }
}