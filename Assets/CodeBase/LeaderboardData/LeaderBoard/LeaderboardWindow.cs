using System.Collections;
using Agava.YandexGames;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Service.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LeaderBoard
{
    public class LeaderboardWindow:MonoCache
    {
        protected IGameStateMachine GameStateMachine;
        protected IAdsService AdsService;
        protected ILeaderboardService LeaderBoardService;
        protected AudioSource AudioSource;
        protected GameObject Hero;
        protected float Volume;

        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private RawImage _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject[] _players;
        [SerializeField] private GameObject _playerDataContainer;
        
        
        private bool _isCurrentScene = true;
        
        protected override void OnEnabled()
        {
            ClearLeaderBoard();
            ClearPlayerData();
            _closeButton.onClick.AddListener(Close);

            if (Application.isEditor || LeaderBoardService == null )
            {
                AddTestData();
                return;
            }

            LeaderBoardService.OnInitializeSuccess += RequestLeaderBoard;
            InitializeLeaderBoard();
        }

        protected override void OnDisabled()
        {
            _closeButton.onClick.RemoveListener(Close);

            if (AdsService != null)
                AdsService.OnInitializeSuccess -= RequestLeaderBoard;
        }

        private void ClearLeaderBoard()
        {
            foreach (GameObject player in _players)
                player.SetActive(false);
        }
        
        private void ClearPlayerData()
        {
            _rankText.text = "";
            _iconImage.texture = null;
            _nameText.text = "";
            _scoreText.text = "";
            _playerDataContainer.SetActive(false);
        }
        
        private void Close() =>
            Hide();


        private void AddTestData()
        {
            LeaderboardEntryResponse leaderboardEntryResponse1 = new LeaderboardEntryResponse();
            leaderboardEntryResponse1.rank = 1;
            leaderboardEntryResponse1.score = 0;
            PlayerAccountProfileDataResponse playerAccountProfileDataResponse1 = new PlayerAccountProfileDataResponse();
            playerAccountProfileDataResponse1.publicName = "Master";
            leaderboardEntryResponse1.player = playerAccountProfileDataResponse1;
            FillPlayerInfo(leaderboardEntryResponse1);

            LeaderboardGetEntriesResponse leaderboardGetEntriesResponse = new LeaderboardGetEntriesResponse();
            leaderboardGetEntriesResponse.entries = new[] { leaderboardEntryResponse1};
            FillLeaderBoard(leaderboardGetEntriesResponse);
        }


        private void FillPlayerInfo(LeaderboardEntryResponse response)
        {
            // Debug.Log("FillPlayerInfo");
            if (!Application.isEditor)
                StartCoroutine(LoadAvatar(response.player.scopePermissions.avatar, _iconImage));

            _rankText.text = $"#{response.rank}";
            _nameText.text = response.player.publicName;
            _scoreText.text = response.score.ToString();

            if (LeaderBoardService != null)
                LeaderBoardService.OnSuccessGetEntry -= FillPlayerInfo;

            if (!string.IsNullOrEmpty(response.player.publicName))
                _playerDataContainer.SetActive(true);
        }
        
        private IEnumerator LoadAvatar(string avatarUrl, RawImage image)
        {
            image.gameObject.SetActive(false);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(avatarUrl);
            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"LoadAvatar {request.error}");
            }
            else
            {
                image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                image.gameObject.SetActive(true);
            }
        }

        private void FillLeaderBoard(LeaderboardGetEntriesResponse leaderboardGetEntriesResponse)
        {
             Debug.Log("FillLeaderBoard");
            LeaderboardEntryResponse[] leaderboardEntryResponses = leaderboardGetEntriesResponse.entries;
             Debug.Log($"leaderboardEntryResponses {leaderboardEntryResponses.Length}");
            LeaderboardEntryResponse response;
            PlayerItem playerItem;

            for (int i = 0; i < leaderboardEntryResponses.Length; i++)
            {
                if (i >= _players.Length)
                    return;

                response = leaderboardEntryResponses[i];
                playerItem = _players[i].GetComponent<PlayerItem>();
                playerItem.Rank.text = response.rank.ToString();

                if (!Application.isEditor)
                    StartCoroutine(LoadAvatar(response.player.scopePermissions.avatar, playerItem.Icon));

                playerItem.Name.text = response.player.publicName;
                playerItem.Score.text = response.score.ToString();
                playerItem.gameObject.SetActive(true);
                 Debug.Log($"i {i}");
                 Debug.Log($"publicName {response.player.publicName}");
                 Debug.Log($"score {response.score}");
                 Debug.Log($"rank {response.rank}");
            }

            if (LeaderBoardService != null)
                LeaderBoardService.OnSuccessGetEntries -= FillLeaderBoard;
        }
        
        public void Show(bool showCursor = true)
        {
            gameObject.SetActive(true);
            Time.timeScale = ConstantsData.TimeScaleStop;
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
            Time.timeScale = ConstantsData.TimeScaleResume;
        }
        
        private void InitializeAdsSDK()
        {
            Debug.Log("InitializeAdsSDK");
            
            if (AdsService.IsInitialized())
                AdsServiceInitializedSuccess();
            else
                StartCoroutine(AdsService.Initialize());
        }
        
        private void AdsServiceInitializedSuccess() =>
            AdsService.OnInitializeSuccess -= AdsServiceInitializedSuccess;
        
        private void InitializeLeaderBoard()
        {
            if (LeaderBoardService.IsInitialized())
                RequestLeaderBoard();
            else
                StartCoroutine(CoroutineInitializeLeaderBoard());
        }
        
        private  void RequestLeaderBoard() =>
            LeaderBoardService.OnInitializeSuccess -= RequestLeaderBoard;

        private IEnumerator CoroutineInitializeLeaderBoard()
        {
            yield return LeaderBoardService.Initialize();
        }
        
        protected void ShowSetValueError(string error)
        {
            Debug.Log($"ShowSetValueError {error}");
            LeaderBoardService.OnSetValueError -= ShowSetValueError;
        }

        private void SuccessSetValue()
        {
            Debug.Log("SuccessSetValue");
            LeaderBoardService.OnSetValueSuccess -= SuccessSetValue;
        }

        private  void SubscribeSetValueSuccess() =>
            LeaderBoardService.OnSetValueSuccess += SuccessSetValue;
    }
}