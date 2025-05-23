using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Game.Durak;
using Game.Durak.Core;
using Game.Durak.Enums;
using Game.Durak.Main;
using Game.Durak.Network.Messages;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using Zenject;

namespace Game.Max
{
    public class WebSocketConnection : MonoBehaviour
    {
        private GameObject _deck;
        
        private Transform _playerSleeve;

        private Transform _slotContainer;
        
        private TestSlot _slotPrefab;

        private CardsConfig _cardsConfig;

        private DurakGameUI _durakGameUI;

        private DurakGameSounds _gameSounds;

        private GameLogicMethods _gameLogicMethods;
        
        private ResponseTextMessageRepository _responseTextMessageRepository;

        private CheckConnection _checkConnectionClone;

        private WebSocket _socket;

        private CancellationTokenSource _token;

        private const float TimeForReconnect = 2f;

        private PingReply _reply;

        private JoinResponseLogic _joinResponse;
        private ReadyResponseLogic _readyResponse;
        private GameStartResponseLogic _gameStartResponseLogic;
        private RoleResponseLogic _roleResponseLogic;
        private BeatResponseLogic _beatResponseLogic;
        private TakeResponseLogic _takeResponseLogic;
        private IAttackResponse _attackResponse;
        private DefenceResponseLogic _defenceResponse;

        private static event Action<CardInfo, GameObject> _playerAttackMove; 
        private static event Action<CardInfo, TestSlot> _playerDefenceMove; 
        private static event Action<CardInfo, TestSlot> _playerTransferMove;

        private List<TestPlayer> _playersOnScene;
        
        private readonly List<TestSlot> _slots = new List<TestSlot>();

        private readonly List<TestCard> _cardsOnScene = new List<TestCard>();

        private Dictionary<ETurnMode, Action<string>> _responses;
        
        [SerializeField] private CheckConnection checkConnectionOriginal;

        [Header("BezierCurve")]
        [SerializeField] private Transform p0; //
        [SerializeField] private Transform p1; //
        [SerializeField] private Transform p2; //
        [SerializeField] private Transform p3; // not used, need to bind
        
        private TestPlayer _activePlayer;
        private TestPlayer _player;

        private TestSlot _transferSlot;
        
        private PlayerInfo _playerInfo;

        private TestCard _trump;
        private ECardSuit _trumpSuit;
        
        private bool _isReady;
        private bool _inGame;

        private int _connectionAttempt = 0;
        

        [Inject]
        private void SceneConstruct(
            GameObject deck,
            TestSlot slotPrefab)
        {
            _deck = deck;
            _slotPrefab = slotPrefab;
        }

        [Inject]
        private void PlayerConstruct(
            [Inject(Id = SceneInstallerIdentifiers.PlayerSleevePosition)]
            Transform playerSleevePosition)
        {
            _playerSleeve = playerSleevePosition;
        }
        
        [Inject]
        private void PlayerListConstruct(List<TestPlayer> players)
        {
            _playersOnScene = players;
        }
        
        [Inject]
        private void ServicesConstruct(DurakGameUI durakGameUI,
            DurakGameSounds durakGameSounds,
            ResponseTextMessageRepository messageRepository,
            CardsConfig cardsConfig,
            GameLogicMethods gameLogicMethods)
        {
            _durakGameUI = durakGameUI;
            _gameSounds = durakGameSounds;
            _responseTextMessageRepository = messageRepository;
            _cardsConfig = cardsConfig;
            _gameLogicMethods = gameLogicMethods;
        }

        [Inject]
        private void ResponsesConstruct(
            JoinResponseLogic joinResponse,
            ReadyResponseLogic readyResponse,
            GameStartResponseLogic gameStartResponseLogic,
            RoleResponseLogic roleResponseLogic,
            BeatResponseLogic beatResponseLogic,
            TakeResponseLogic takeResponseLogic,
            IAttackResponse attackResponse,
            DefenceResponseLogic defenceResponse)
        {
            _joinResponse = joinResponse;
            _readyResponse = readyResponse;
            _gameStartResponseLogic = gameStartResponseLogic;
            _beatResponseLogic = beatResponseLogic;
            _takeResponseLogic = takeResponseLogic;
            _attackResponse = attackResponse;
            _defenceResponse = defenceResponse;
        }
        
        private void OnEnable()
        {
            _durakGameUI.OnSelectEmoji += SendEmoji;
            _durakGameUI.OnSelectTextMessage += SendTextMessage;

            GameEvents.OnConnectionReconnected += Reconnect;
        }

        private void OnDisable()
        {
            _durakGameUI.OnSelectEmoji -= SendEmoji;
            _durakGameUI.OnSelectTextMessage -= SendTextMessage;

            GameEvents.OnConnectionReconnected -= Reconnect;
        }
        
        private void Start()
        {
            ConnectToLobby(GetLobbyID());

            _durakGameUI.InitializeQuitBtn(Quit);
            _durakGameUI.InitializeTryConnectBtn(ReconnectAfterDisconnection);
        }
        
        public PlayerInfo GetPlayerInfo()
        {
            return _player.PlayerInfo;
        }
        
        private void Reconnect()
        {
            StartCoroutine(Reconnecting());
        }

        private void ReconnectAfterDisconnection()
        {
            _connectionAttempt--;
            
            GameEvents.ConnectionReconnected();
        }

        private IEnumerator Reconnecting()
        {
            Debug.Log("Reconnecting...");
            
            yield return new WaitForSeconds(TimeForReconnect);
            
            ConnectToLobby(GetLobbyID());

            _connectionAttempt++;
        }
        
        //----------Buttons----------
        public void Join() => JoinMessage();
        public void Watch() => WatchMessage();

        public void Ready() => ReadyMessage();
        
        public void Beat() => BeatMessage();
        
        public void Take() => TakeMessage();
        
        public void Pass() => PassMessage();
        
        public void Quit()
        {
            StartCoroutine(DurakHelper.Loading(SceneManager.LoadSceneAsync("Menu")));
        }
        
        //--------------------------    

        private void InitializeResponses()
        {
            
            _responses = new Dictionary<ETurnMode, Action<string>>()
            {
                { ETurnMode.Join, _joinResponse.Invoke },
                { ETurnMode.Ready, _readyResponse.Invoke },
                { ETurnMode.StartDistribution, _gameStartResponseLogic.Invoke },
                { ETurnMode.Role, _roleResponseLogic.Invoke },
                { ETurnMode.Attack, _attackResponse.Invoke },
                { ETurnMode.Defence, _defenceResponse.Invoke },
                { ETurnMode.Take, _takeResponseLogic.Invoke },
                { ETurnMode.Beat, _beatResponseLogic.Invoke },
                { ETurnMode.Info, InfoResponse },
                { ETurnMode.Error, ErrorResponse },
                { ETurnMode.Status, StatusResponse },
                { ETurnMode.Text, ChatResponse },
                { ETurnMode.TimerGame, TimerDataResponse },
                { ETurnMode.TimerReady, TimerDataResponse },
                { ETurnMode.Cancelled, TimerFinishResponse },
                { ETurnMode.Expired, TimerFinishResponse },
                { ETurnMode.Finish, FinishResponse },
                { ETurnMode.Reconnect, ReconnectResponse },
                { ETurnMode.Watch, WatchResponse },
                { ETurnMode.Tournament, TournamentRoomResponse },
            };
        }
        
        
        private void OnOpen(object obj, EventArgs args)
        {
            Debug.Log("Connected to server");
            
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                InitializeResponses();
            
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
                PlayerActionsInitialize();

                _playerInfo = SceneMediator.GetPlayerInfo();

                CleanLobby();

                if (SceneMediator.PlayerStatus is SceneMediator.EPlayerStatus.Player)
                    Join();
                else
                    Watch();
                
                _inGame = true;

                _connectionAttempt = 0;
                
                _checkConnectionClone = new CheckConnection(this, _socket).Clone(checkConnectionOriginal);

                GameEvents.ConnectionRestored();
                
                _checkConnectionClone.PingHostAsync(CloseSocket);
                
            });
        }
    
        private void OnClose(object obj, CloseEventArgs args)
        {
            Debug.Log("Disconnected from server");
            
            SocketDispose(_socket);
            
            CloseToken();
            
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                PlayerActionsDispose();

                Screen.sleepTimeout = SleepTimeout.SystemSetting;

                _checkConnectionClone.CanselPinging();
                
                if (_inGame)
                {
                    if(_connectionAttempt <= 3)
                        GameEvents.ConnectionReconnected();
                    else
                        GameEvents.ConnectionLost();
                }
                
            });
            
        }
    
        private void OnMessage(object obj, MessageEventArgs args)
        {
            var response = args.Data;
            
            Debug.Log("Message:" + response);
            
            PlayAction(response);
        }
    
        private void OnError(object obj, ErrorEventArgs args)
        {
            Debug.LogError($"Error: {args.Message}");
        }
        
        
        private void PlayAction(string response)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                ETurnMode mode = GetResponseMode(response);

                InvokeResponseMethod(response, mode);
            });
            
        }

        private void InvokeResponseMethod(string response, ETurnMode mode)
        {
            if (!_responses.ContainsKey(mode))
                return;
            
            DebugMethod(_responses[mode].Method, response);
            _responses[mode].Invoke(response);
        }

        private ETurnMode GetResponseMode(string response)
        {
            JObject jObject = JObject.Parse(response);

            string modeStr = (string)jObject["mode"];

            if (!Enum.TryParse<ETurnMode>(modeStr, true, out var mode))
                return ETurnMode.Unknown;

            return mode;
        }
        
        private void ConnectToLobby(int id)
        {
            DebugMethod("ConnectToLobby");

            CloseSocket();

            var url = DurakHelper.WSS + id;

            _socket = new WebSocket(url);

            _token = new CancellationTokenSource();

            SocketInitialize(_socket);
            
            _socket.Connect();
        }
        
        private void SocketInitialize(WebSocket socket)
        {
            socket.OnOpen += OnOpen;
            socket.OnMessage += OnMessage;
            socket.OnError += OnError;
            socket.OnClose += OnClose;
        }
        
        //----------Responses and messages----------
        
        private void ChatResponse(string response)
        {
            var chatResponse = JsonConvert.DeserializeObject<ChatMessage>(response);

            var prefix = "ActionMessage:";
            if (chatResponse.Text.StartsWith(prefix))
            {
                var mode = Enum.Parse<ETurnMode>(new string(chatResponse.Text.Skip(prefix.Length).ToArray()));
                ShowActionMessage(chatResponse.Sender, mode, false);
                return;
            }

            var sender = DurakHelper.GetPlayer(_playersOnScene, chatResponse.Sender);

            switch (chatResponse.Type)
            {
                case EChatMessageType.Message:
                    var backgroundColor = Color.white;
                    var textColor = Color.black;
                    
                    var message = new TextMessage(backgroundColor, textColor, chatResponse.Text, 0);
                    sender.ShowTextMessage(message);
                    break;
                
                case EChatMessageType.Emoji:
                    sender.ShowEmoji(chatResponse.Text);
                    break;
            }
        }
        
        private void ErrorResponse(string response)
        {
            ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);

            switch (errorResponse.Error)
            {
                case ELogicError.WeakCard:
                    break;
                case ELogicError.MissingTableCard:
                    //TODO
                    break;
                case ELogicError.MissingPlayerCard:
                    //TODO
                    break;
                case ELogicError.OpenSlot:
                    //TODO
                    break;
                case ELogicError.NotInit:
                    //TODO
                    break;
                case ELogicError.NotEnemy:
                    //TODO
                    break;
                case ELogicError.WaitInit:
                    //TODO
                    break;
                case ELogicError.SlotClosed:
                    //TODO
                    break;
                    
                case ELogicError.TournamentRoomNotFound:
                    SceneMediator.Room = errorResponse.next_game;
                    break;
            }

            if (_player)
                _player.ReturnSleeveToPlayer();
        }
        
        private void InfoResponse(string response)
        {
            InfoResponse infoResponse = JsonConvert.DeserializeObject<InfoResponse>(response);

            TestPlayer player;
            
            switch (infoResponse?.Info)
            {
                case ELogicInfo.DeckSize:
                    _gameLogicMethods.CheckDeck(int.Parse(infoResponse.value));
                    
                    break;

                case ELogicInfo.CanPass:

                    break;
                
                case ELogicInfo.DefenderTake:
                    ShowActionMessage(infoResponse.user, infoResponse.action);

                    break;
                
                case ELogicInfo.DefenderTook:
                    Debug.Log($"Defender took the cards: {infoResponse.user.username}");
                    
                    if (!DurakHelper.IsPlayer(_player, infoResponse.user))
                        _gameLogicMethods.MoveCardsToEnemy(infoResponse.user);
                    
                    break;
                
                case ELogicInfo.AttackerSkipped:
                    ShowActionMessage(infoResponse.user, infoResponse.action);
                    Debug.Log($"AttackerSkipped: user: {infoResponse.user}, mode: {infoResponse.action}");
                    break;
                
                case ELogicInfo.InitSkipped:
                    ShowActionMessage(infoResponse.user, infoResponse.action);
                    Debug.Log($"InitSkipped: user: {infoResponse.user}, mode: {infoResponse.action}");
                    break;
                
                case ELogicInfo.YourTurn:
                    if (infoResponse.action == ETurnMode.Beat)
                        _durakGameUI.SwitchButton(_durakGameUI.Beat);
                    
                    else if (infoResponse.action == ETurnMode.Pass)
                        _durakGameUI.SwitchButton(_durakGameUI.Pass);
                    break;
                
                case ELogicInfo.PlayerWon:
                    player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);
                    player.DarkenAvatar();
                    player.DisableFrames();

                    if (DurakHelper.IsPlayer(_player, player))
                    {
                        _gameSounds.PlayVictory();
                        _durakGameUI.DisableButtons();
                    }
                    
                    break;
                
                case ELogicInfo.PlayerLose:
                    player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);
                    player.DisableFrames();

                    if (DurakHelper.IsPlayer(_player, player))
                    {
                        _gameSounds.PlayDefeat();
                        _durakGameUI.DisableButtons();
                    }

                    break;
                
                case ELogicInfo.NewRound:

                    if (infoResponse.action is ETurnMode.Beat)
                        _gameLogicMethods.MoveCardsToBeat();

                    for (int i = 0; i < _playersOnScene.Count; i++)
                        _playersOnScene[i].HideActionMessage();
                    
                    _gameLogicMethods.CleanDeck();
                    break;
                
                case ELogicInfo.UserLogOut:
                    _isReady = false;

                    _durakGameUI.DisableButton(_durakGameUI.Ready);
                    
                    player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);

                    if (!_inGame)
                    {
                        for (int i = 0; i < _playersOnScene.Count; i++)
                            _playersOnScene[i].StopTimer();
                    }
                    
                    if (player.PlayerInfo.user_id == _playerInfo.user_id)
                    {
                        Quit();
                        break;
                    }
                    
                    player.DarkenAvatar();
                    player.ShowDisconnectMessage();

                    break;
                
                case ELogicInfo.UserReconnect:
                    player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);

                    if (DurakHelper.IsPlayer(_player, player))
                        break;
                    
                    player.LightenAvatar();
                    player.HideDisconnectMessage();

                    break;
                
                case ELogicInfo.ActivePlayer:
                    _activePlayer = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);
                    _activePlayer.EnableAttackFrame();

                    _activePlayer.HideActionMessage();

                    if (DurakHelper.IsPlayer(_player, _activePlayer))
                        Vibrate();

                    break;
                
                case ELogicInfo.MinTrumpCard:

                    player = DurakHelper.GetPlayer(_playersOnScene, infoResponse.user);

                    TestCard card = _cardsConfig.GetCard(infoResponse.user.min_trump_card);
                    
                    _durakGameUI.SetTrumpSuit(card.CardInfo.suit);
                    
                    if (DurakHelper.IsPlayer(_player, player))
                        break;
                    
                    if(player)
                        player.ShowMinTrump(card);
                    
                    break;
                
                case ELogicInfo.ActiveReady:
                    
                    Debug.Log(_playersOnScene.Count);
                    
                    for (int i = 0; i < _playersOnScene.Count; i++)
                        _playersOnScene[i].EnableAttackFrame();
                    
                    break;
            }
        }

        private void StatusResponse(string response)
        {
            StatusResponse statusResponse = JsonConvert.DeserializeObject<StatusResponse>(response);

            switch (statusResponse.Status)
            {
                case EGameStatus.In_Game:
                    break;
                
                case EGameStatus.GameInReady:

                    if (!_isReady)
                        _durakGameUI.SwitchButton(_durakGameUI.Ready);
                    
                    break;
            }
        }

        private void FinishResponse(string response)
        {
            FinishResponse finishResponse = JsonConvert.DeserializeObject<FinishResponse>(response);

            CheckWinners();
            CheckLosers();
            
            FinishGame(5000);
            
            void CheckWinners()
            {
                for (int i = 0; i < finishResponse.winners.Length; i++)
                {
                    TestPlayer winner = DurakHelper.GetPlayer(_playersOnScene, finishResponse.winners[i].user);
                
                    winner.ShowWinnerBetHolder(finishResponse.winners[i].amount.ToString());

                    if (DurakHelper.IsPlayer(_player, winner))
                        GameEvents.PlayerWonCash(decimal.Parse(finishResponse.winners[i].amount.ToString()));

                }
            }

            void CheckLosers()
            {
                for (int i = 0; i < finishResponse.losers.Length; i++)
                {
                    TestPlayer loser = DurakHelper.GetPlayer(_playersOnScene, finishResponse.losers[i].user);
                
                    loser.ShowLoserBetHolder(finishResponse.losers[i].amount.ToString());
                
                    if (DurakHelper.IsPlayer(_player, loser))
                        GameEvents.PlayerLostCash(decimal.Parse(finishResponse.losers[i].amount.ToString()));
                }
            }

        }
        
        private void TournamentRoomResponse(string response)
        {
            TournamentRoomResponse tournamentResponse = JsonConvert.DeserializeObject<TournamentRoomResponse>(response);

            SceneMediator.Room = tournamentResponse.next_game;

            _inGame = false;
            
            ConnectToLobby(GetLobbyID());

        }

        private void ReconnectResponse(string response)
        {
            ReconnectResponse reconnectResponse = JsonConvert.DeserializeObject<ReconnectResponse>(response);

            _inGame = true;
            
            if (_player != null)
                CleanLobby();
            
            _gameLogicMethods.CreatePlayers(reconnectResponse.players.Length);
            
            _gameLogicMethods.InitializePlayers(reconnectResponse.players);

            ShowPlayersCardCount(reconnectResponse.players);
            
            _player.SetRole(reconnectResponse.role.Turn);
            _player.ShowRoleFrame();
            
            SetRoleFrames(reconnectResponse.role.Users);

            _durakGameUI.ChangeRoleImage(_player.Role);
            
            _gameLogicMethods.SpawnSleeve(reconnectResponse.hand, 0, 0);
            
            _durakGameUI.ShowLobbyID(SceneMediator.Room.ID);
            _durakGameUI.ShowLobbyBet(SceneMediator.Room.Bank);
            
            _gameLogicMethods.CheckDeck(reconnectResponse.deck);
            
            for (int i = 0; i < reconnectResponse.round.slots.Length; i++)
            {
                if (reconnectResponse.round.slots.Length == 0)
                    break;
                
                TestSlot slot = Instantiate(_slotPrefab, _slotContainer);

                slot.Initialize(reconnectResponse.round.slots[i]);
                slot.SlotInfo.index = i;
                
                _slots.Add(slot);
                
                TestCard intiCard = _gameLogicMethods.SpawnCard(reconnectResponse.round.slots[i].init_card, slot.transform);

                _cardsOnScene.Add(intiCard);
                
                if (reconnectResponse.round.slots[i].enemy_card == null)
                    continue;
                
                TestCard enemyCard = _gameLogicMethods.SpawnCard(reconnectResponse.round.slots[i].enemy_card, slot.transform);
                
                _cardsOnScene.Add(enemyCard);
                
                enemyCard.RotateCard();
            }
            
            CheckPlayerStatusAfterReconnect(reconnectResponse);
        }
        
        private void WatchResponse(string response)
        {
            WatchResponse watchResponse = JsonConvert.DeserializeObject<WatchResponse>(response);

            _gameLogicMethods.CreatePlayers(watchResponse.players.Length, false);

            _gameLogicMethods.InitializePlayers(watchResponse.players);
            
            ShowPlayersCardCount(watchResponse.players);

            SetRoleFrames(watchResponse.role.Users);

            _durakGameUI.ShowLobbyID(SceneMediator.Room.ID);
            _durakGameUI.ShowLobbyBet(SceneMediator.Room.Bank);

            for (int i = 0; i < watchResponse.round.slots.Length; i++)
            {
                TestSlot slot = Instantiate(_slotPrefab, _slotContainer);

                slot.Initialize(watchResponse.round.slots[i]);
                slot.SlotInfo.index = i;
                
                _slots.Add(slot);
                
                TestCard initCard = _gameLogicMethods.SpawnCard(watchResponse.round.slots[i].init_card, slot.transform);
                var initSize = ((RectTransform)slot.transform).sizeDelta / ((RectTransform)initCard.transform).sizeDelta;
                SetCardScale(initCard, initSize);

                _cardsOnScene.Add(initCard);
                
                if (watchResponse.round.slots[i].enemy_card == null)
                    continue;
                
                TestCard enemyCard = _gameLogicMethods.SpawnCard(watchResponse.round.slots[i].enemy_card, slot.transform);
                var enemySize = ((RectTransform)slot.transform).sizeDelta / ((RectTransform)enemyCard.transform).sizeDelta;
                SetCardScale(enemyCard, enemySize);
                
                _cardsOnScene.Add(enemyCard);
                
                enemyCard.RotateCard();
            }
            
            _gameLogicMethods.CheckDeck(watchResponse.deck);
            
        }
        
        
        private void TimerDataResponse(string response)
        {
            TimerDataResponse timerDataResponse = JsonConvert.DeserializeObject<TimerDataResponse>(response);
            
            DateTime currentTime = DateTime.Parse(timerDataResponse.timer.now);
            DateTime finishTime = DateTime.Parse(timerDataResponse.timer.expiration_time);
            
            int seconds = finishTime.Subtract(currentTime).Seconds;
            int minutes = finishTime.Subtract(currentTime).Minutes;

            int timerDuration = seconds + minutes * 60;

            switch (timerDataResponse.mode)
            {
                case ETurnMode.TimerGame:
                    _activePlayer.StopTimer();
                    _activePlayer.StartTimer(timerDuration);
                    
                    break;
                
                case ETurnMode.TimerReady:
                    Debug.Log(_playersOnScene.Count);
                    for (int i = 0; i < _playersOnScene.Count; i++)
                        _playersOnScene[i].StartTimer(timerDuration);

                    break;
            }
        }
        
        private void TimerFinishResponse(string response)
        {
            TimerFinishResponse timerFinishResponse = JsonConvert.DeserializeObject<TimerFinishResponse>(response);

            if (_activePlayer)
                _activePlayer.StopTimer();
        }

        private void JoinMessage()
        {
            JoinMessage joinMessage = new JoinMessage(_playerInfo);
        
            SendToServer(joinMessage);
        }

        private void WatchMessage()
        {
            WatchMessage watchMessage = new WatchMessage(_playerInfo);
            
            SendToServer(watchMessage);
        }

        private void ReadyMessage()
        {
            ReadyMessage readyMessage = new ReadyMessage(_playerInfo);
            
            SendToServer(readyMessage);
            ShowActionMessage(_player.PlayerInfo, ETurnMode.Ready, true);

            _isReady = true;
            
            _durakGameUI.DisableButtons();
        }
        
        private void AttackMessage(CardInfo cardInfo, GameObject table)
        {
            if (!table)
            {
                _player.ReturnSleeveToPlayer();
                return;
            }

            AttackMessage attackMessage = new AttackMessage(_playerInfo, cardInfo);

            SendToServer(attackMessage);
        }
        
        private void TransferMessage(CardInfo cardInfo, TestSlot slot)
        {
            if (!slot)
            {
                _player.ReturnSleeveToPlayer();
                return;
            }
            
            if (slot.SlotInfo.init != 0)
                return;

            TransferMessage transferMessage = new TransferMessage(_playerInfo, cardInfo);

            SendToServer(transferMessage);
        }
    
        private void DefenceMessage(CardInfo cardInfo, TestSlot slot)
        {
            if (!slot)
            {
                _player.ReturnSleeveToPlayer();
                return;
            }

            if (slot.SlotInfo.init == 0)
                return;
            
            DefenseMessage defenceMessage = new DefenseMessage(_playerInfo, cardInfo, slot.SlotInfo.index);

            SceneDefenceSlot.SetDefenceSlot(slot.transform);

            ScenePlayerSlotNumber.SetPlayerSlot(slot.SlotInfo.index);
            
            SendToServer(defenceMessage);
        }

        private void BeatMessage()
        {
            BeatMessage beatMessage = new BeatMessage(_playerInfo);
            
            SendToServer(beatMessage);

            _durakGameUI.DisableButtons();
        }
        
        private void TakeMessage()
        {
            TakeMessage takeMessage = new TakeMessage(_playerInfo);
            
            SendToServer(takeMessage);
            
            _durakGameUI.DisableButtons();
        }

        private void PassMessage()
        {
            PassMessage passMessage = new PassMessage(_playerInfo);

            SendToServer(passMessage);

            _durakGameUI.DisableButtons();
        }
        
        public void SendEmoji(string value)
        {
            Debug.Log($"Sending emoji: {value}");
            
            var player = _player.PlayerInfo;
            var message = new ChatMessage(player, value, EChatMessageType.Emoji);
            SendToServer(message);
        }

        public void SendTextMessage(string value)
        {
            Debug.Log($"Sending text message: {value}");
            
            var player = _player.PlayerInfo;
            var message = new ChatMessage(player, value, EChatMessageType.Message);
            SendToServer(message);
        }
        
        //------------------------------------------

        private void SendToServer<TMessage>(TMessage message)
        {
            string _json_String = JsonConvert.SerializeObject(message);
            Debug.Log($"Sending to server: {_json_String}");

            _socket?.Send(_json_String);
        }

        public static void AttackMove(CardInfo info, GameObject table)
        {
            _playerAttackMove?.Invoke(info, table);
        }
        
        public static void TransferMove(CardInfo info, TestSlot slot)
        {
            _playerTransferMove?.Invoke(info, slot);
        }

        public static void DefenceMove(CardInfo info, TestSlot slot)
        {
            _playerDefenceMove?.Invoke(info, slot);
        }


        //----------Game methods----------
        
        private void SetCardScale(TestCard card, Vector3 scale)
        {
            card.transform.DOScale(scale, 0.5f)
                .SetLink(card.gameObject);
        }

        private void CheckPlayerStatusAfterReconnect(ReconnectResponse reconnectResponse)
        {
            switch (_player.Role)
            {
                case EPlayerRole.Attacker:

                    if (reconnectResponse.turn == ETurn.Attack)
                    {
                        if (reconnectResponse.round.status == EAttackStatus.Pending_Take)
                        {
                            _durakGameUI.SwitchButton(_durakGameUI.Pass);
                            return;
                        }
                            
                        _gameLogicMethods.CheckBeat(reconnectResponse.round.slots);
                    }
                    
                    break;
                    
                case EPlayerRole.Enemy:
                    _gameLogicMethods.CheckTake(reconnectResponse.round.slots);
                            
                    break;
                
                case EPlayerRole.Waiting:
                    if (reconnectResponse.turn == ETurn.Attack)
                    {
                        if (reconnectResponse.round.status == EAttackStatus.Pending_Take)
                            _durakGameUI.SwitchButton(_durakGameUI.Pass);
                        else
                            _durakGameUI.SwitchButton(_durakGameUI.Beat);
                        
                    }
                    break;
                    
            }
        }

        private void SetRoleFrames(PlayerInfo[] users)
        {
            for (int i = 0; i < _playersOnScene.Count; i++)
            {
                if (_playersOnScene[i].PlayerInfo.user_id == _playerInfo.user_id)
                    continue;
                
                _playersOnScene[i].SetRole(EPlayerRole.Waiting);
                _playersOnScene[i].ShowRoleFrame();
            }

            for (int i = 0; i < users.Length; i++)
            {
                var player = DurakHelper.GetPlayer(_playersOnScene, users[i]);
                
                player.SetRole(users[i].Turn);
                player.ShowRoleFrame();
            }
                
        }

        private void ShowActionMessage(PlayerInfo user, ETurnMode mode, bool sendToServer = false)
        {
            Debug.Log($"ShowActionMessage: username {user.username}, mode: {mode}");
            
            var player = DurakHelper.GetPlayer(_playersOnScene, user);

            switch (mode)
            {
                case ETurnMode.Beat:
                    if (player.PlayerInfo.user_id == _player.PlayerInfo.user_id)
                    {
                        _durakGameUI.DisableButton(_durakGameUI.Beat);
                    }
                    break;
                
                case ETurnMode.Pass:
                    if (player.PlayerInfo.user_id == _player.PlayerInfo.user_id)
                    {
                        _durakGameUI.DisableButton(_durakGameUI.Pass);
                    }
                    break;
                
                case ETurnMode.Take:
                    if (player.PlayerInfo.user_id == _player.PlayerInfo.user_id)
                    {
                        _durakGameUI.DisableButton(_durakGameUI.Take);
                    }
                    break;
            }

            var hasMessage = _responseTextMessageRepository.TryGetMessage(mode, out var message);
            if (hasMessage)
            {
                player.ShowActionMessage(message);

                if (sendToServer)
                {
                    SendTextMessage($"ActionMessage:{mode}");
                }
            }
        }

        private async void FinishGame(int delay)
        {
            try
            {
                GameEvents.GameFinished();

                _durakGameUI.DisableButtons();

                if (_trump)
                    Destroy(_trump.gameObject);

                for (int i = 0; i < _playersOnScene.Count; i++)
                {
                    _playersOnScene[i].LightenAvatar();
                    _playersOnScene[i].DisableFrames();
                    _playersOnScene[i].StopTimer();
                }

                _isReady = false;
                
                _player.DeleteCards();
                
                _gameLogicMethods.CleanDeck();
                _gameLogicMethods.CleanBeat();
                
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                await Task.Delay(delay);
                
                GameEvents.GameStarted();

                PlayerGameData gameData = new PlayerGameData();
                
                DurakHelper.CreateLoadBar();
                
                var data = await gameData.UpdatePlayerDataAsync();

                DurakHelper.DestroyLoadBar();

                int value = DurakHelper.GetWallet(data.wallets, WalletType.BonusBalance).balance;

                _player.ShowDefaultBetHolder(value.ToString());
                
                GameEvents.CashChanged(value);
                
                _durakGameUI.SwitchButton(_durakGameUI.Ready);

                for (int i = 0; i < _playersOnScene.Count; i++)
                {

                    if (DurakHelper.IsPlayer(_player, _playersOnScene[i]))
                        continue;
                    
                    _playersOnScene[i]?.HideBetHolder();
                }
            }
        }

        private void CleanLobby()
        {
            if(_player)
                _player.DeleteCards();
            
            _gameLogicMethods.CleanDeck();
            _gameLogicMethods.CleanBeat();
            ClearPlayers();
            GameEvents.LobbyCleared();
            if (_trump)
                Destroy(_trump.gameObject);
        }
        
        private void ClearPlayers()
        {        
            for (int i = 0; i < _playersOnScene.Count; i++)
                Destroy(_playersOnScene[i].gameObject);
                
            _playersOnScene.Clear();
        }

        private void ShowPlayersCardCount(PlayerCardInfo[] players)
        {

            foreach (var playerToRefresh in players)
            {
                if (playerToRefresh.cards == 0)
                {
                    continue;
                }
                
                if (playerToRefresh.user.user_id == _playerInfo.user_id)
                {
                    continue;
                }

                Debug.Log($"Player: {playerToRefresh.user.username}, cards amount: {playerToRefresh.cards}");
                
                DurakHelper
                    .GetPlayer(_playersOnScene, playerToRefresh.user)
                    .SetCardAmount(playerToRefresh.cards);
            }
        }

        private void Vibrate()
        {
#if !UNITY_WEBGL
            Handheld.Vibrate();
#endif        
        }


        private int GetLobbyID() => SceneMediator.Room.ID;

        private void CloseSocket()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }

            _isReady = false;
        }

        private void CloseToken()
        {
            if (_token != null)
            {
                _token.Cancel();
                _token.Dispose();
                _token = null;
            }
        }

        
        private void DebugMethod(string methodName)
        {
            Debug.Log($"----------Now: {methodName}----------[{DateTime.Now.TimeOfDay}]");
        }
        
        private void DebugMethod(MethodInfo method, string response)
        {
            Debug.Log($"----------Now: {method.Name}----------[{DateTime.Now.TimeOfDay}]");
            Debug.Log($"Message received: {JObject.Parse(response)}----------[{DateTime.Now.TimeOfDay}]");
        }
        
        private void SocketDispose(WebSocket socket)
        {
            socket.OnOpen -= OnOpen;
            socket.OnMessage -= OnMessage;
            socket.OnError -= OnError;
            socket.OnClose -= OnClose;
            
            Debug.Log($"Socket Execute");
        }

        private void PlayerActionsInitialize()
        {
            _playerAttackMove += AttackMessage;
            _playerTransferMove += TransferMessage;
            _playerDefenceMove += DefenceMessage;
        }

        private void PlayerActionsDispose()
        {
            _playerAttackMove -= AttackMessage;
            _playerTransferMove -= TransferMessage;
            _playerDefenceMove -= DefenceMessage;
        }

        private void OnDestroy()
        {
            Debug.Log("Destroy");
            
            PlayerActionsDispose();

            SceneMediator.FromGame = true;

            _inGame = false;
            
            CloseSocket();
            CloseToken();
        }
        
    }
}