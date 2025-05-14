using System.Collections.Generic;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Game.Durak.Main
{
    public class DefenceResponseLogic : IResponse
    {
        private List<TestPlayer> _playersOnScene;

        private List<TestSlot> _slots;

        private TestPlayer _player;

        private DurakGameSounds _gameSounds;

        private List<TestCard> _cardsOnScene;

        private GameLogicMethods _gameLogic;


        [Inject]
        private DefenceResponseLogic(
            [Inject(Id = SceneInstallerIdentifiers.PlayersOnScene)]
            List<TestPlayer> playersOnScene,
            List<TestSlot> slots,
            DurakGameSounds gameSounds,
            List<TestCard> cardsOnScene,
            GameLogicMethods logicMethods)
        {
            _playersOnScene = playersOnScene;
            _slots = slots;
            _gameSounds = gameSounds;
            _cardsOnScene = cardsOnScene;
            _gameLogic = logicMethods;
        }


        public void Invoke(string response)
        {
            DefenseResponse defenseResponse = JsonConvert.DeserializeObject<DefenseResponse>(response);

            TestCard card;

            int slotNumber = defenseResponse.SlotNumber;

            int currentInit = defenseResponse.Slots[slotNumber].init;
            int currentEnemy = defenseResponse.Slots[slotNumber].enemy;
            
            TestPlayer enemy = DurakHelper.GetPlayer(_playersOnScene, currentEnemy);

            if (_player.PlayerInfo.user_id == currentInit)//You are init
            {

                card = _gameLogic.SpawnCard(defenseResponse.Slots[slotNumber].enemy_card, enemy.transform);

                var slot = _slots[slotNumber];
                card.transform.SetParent(slot.transform);
                _gameLogic.SetCardScale(card, Vector3.one);
                slot.ApplySize();
                
                _gameLogic.CheckBeat(defenseResponse.Slots);

            }
            else if (_player.PlayerInfo.user_id == currentEnemy)//You are enemy
            {
                int playerSlot = ScenePlayerSlotNumber.PlayerSlot;
                
                card = _player.GetCard(defenseResponse.Slots[playerSlot].enemy_card);

                SceneDefenceSlot.FillDefenseSlot(card.transform);

                _player.RemoveCard(card);
                
                _gameLogic.CheckTake(defenseResponse.Slots);
            
                SceneDefenceSlot.ReleaseDefenseSlot();
                SceneDefenceSlot.ResetDefenceSlot();
                
                ScenePlayerSlotNumber.ResetPlayerSlot();
            }
            else
            {
                card = _gameLogic.SpawnCard(defenseResponse.Slots[slotNumber].enemy_card, enemy.transform);
                
                card.transform.localScale = new Vector3(0.5f, 0.5f);
                
                card.transform.SetParent(_slots[slotNumber].transform);
            }

            _gameLogic.ShowPlayersCardCount(defenseResponse.Players);

            _gameLogic.MoveCardTo(card, Vector3.zero);
            
            _gameLogic.SetCardScale(card, Vector3.one);

            _gameSounds.PlayCardMove();
            
            card.RotateCard();

            _cardsOnScene.Add(card);
        
        }
    
    }
}