using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.Extensions.Economy.Abstractions;
using SDG.Unturned;
using Steamworks;

namespace EconomyUI.Events
{
    public class BalanceUpdatedEventListener : IEventListener<BalanceUpdatedEvent>
    {
        private readonly IStringLocalizer _stringLocalizer;
        
        public BalanceUpdatedEventListener(IStringLocalizer stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        
        public async Task HandleEventAsync(object? sender, BalanceUpdatedEvent @event)
        {
            await UniTask.SwitchToMainThread();
            var transport = Provider.findTransportConnection(new CSteamID(Convert.ToUInt64(@event.OwnerId)));
            EffectManager.sendUIEffectText(431, transport, true, "MoneyText", _stringLocalizer["UI:BalanceFormat", new { Money = Convert.ToInt32(@event.NewBalance) }]);
            await UniTask.SwitchToThreadPool();
        }
    }
}