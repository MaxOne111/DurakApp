using System.Collections.Generic;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Game.Durak.Main
{
    public class AttackResponseLogic : IAttackResponse
    {
        private TestSlot _slotPrefab;

        private Transform _slotContainer;

        private List<TestPlayer> _playersOnScene;

        private List<TestSlot> _slots;

        private TestPlayer _player;

        private DurakGameUI _durakGameUI;

        private DurakGameSounds _gameSounds;

        private List<TestCard> _cardsOnScene;

        private GameLogicMethods _gameLogic;


        [Inject]
        private AttackResponseLogic(TestSlot slotPrefab,
            [Inject(Id = SceneInstallerIdentifiers.SlotContainer)]
            Transform slotContainer,
            [Inject(Id = SceneInstallerIdentifiers.PlayersOnScene)]
            List<TestPlayer> playersOnScene,
            List<TestSlot> slots,
            DurakGameUI durakGameUI,
            DurakGameSounds gameSounds,
            List<TestCard> cardsOnScene,
            GameLogicMethods logicMethods)
        {
            _slotPrefab = slotPrefab;
            _slotContainer = slotContainer;
            _playersOnScene = playersOnScene;
            _slots = slots;
            _durakGameUI = durakGameUI;
            _gameSounds = gameSounds;
            _cardsOnScene = cardsOnScene;
            _gameLogic = logicMethods;
        }


        public void Invoke(string response)
        {
            AttackResponse attackResponse = JsonConvert.DeserializeObject<AttackResponse>(response);
        
            TestCard card;
        
            TestSlot slot = Object.Instantiate(_slotPrefab, _slotContainer.transform.position, Quaternion.identity, _slotContainer);
        
            int currentInit = attackResponse.Slots[^1].init;
            int currentEnemy = attackResponse.Slots[^1].enemy;
            
            slot.Initialize(attackResponse.Slots[^1]);
            slot.SlotInfo.index = attackResponse.Slots.Length - 1;
            
            TestPlayer init = DurakHelper.GetPlayer(_playersOnScene, currentInit);
        
            if (_player.PlayerInfo.user_id == currentInit)
            {
                card = _player.GetCard(attackResponse.Slots[^1].init_card);
                
                _player.RemoveCard(card);
                
                _durakGameUI.DisableButton(_durakGameUI.Beat);
                
            }
            else if (_player.PlayerInfo.user_id == currentEnemy)
            {
                card = _gameLogic.SpawnCard(attackResponse.Slots[^1].init_card, init.transform);
        
                card.transform.localScale = new Vector3(0.5f, 0.5f);
        
                if (attackResponse.Status != EAttackStatus.Pending_Take)
                    _durakGameUI.SwitchButton(_durakGameUI.Take);
            }
            else
            {
                if (_player.Role == EPlayerRole.Enemy)//first slot without enemy
                {
                    if (attackResponse.Status != EAttackStatus.Pending_Take)
                        _gameLogic.CheckTake(attackResponse.Slots);
                }
                
                card = _gameLogic.SpawnCard(attackResponse.Slots[^1].init_card, init.transform);
                card.transform.localScale = new Vector3(0.5f, 0.5f);
            }
        
            _gameLogic.ShowPlayersCardCount(attackResponse.Players);
            
            Debug.Log($"Refreshing cards amount");
        
            card.transform.SetParent(slot.transform);
        
            _gameLogic.MoveCardTo(card, Vector3.zero);
            _gameLogic.SetCardScale(card, Vector3.one);
            slot.ApplySize();
        
            _gameSounds.PlayCardMove();
            
            _slots.Add(slot);
        
            _cardsOnScene.Add(card);
        }
    
    }
}