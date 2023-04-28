using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Assets.Plugins.ButtonSoundsEditor;
using Audio;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LeaderboardPanel : MonoCache
{ 
    private LeaderboardView _leaderboardView;
    private GameObject _leaderboardPanel;
    [SerializeField] private Button _button;
    [SerializeField] private Button _buttonBack;
    private AudioSource _sourceSound;

    public void Initiallize( )
    {
        _leaderboardView=GetComponentInChildren<LeaderboardView>();
        _leaderboardPanel = _leaderboardView.gameObject;
        _sourceSound = FindObjectOfType<AudioController>().GetComponentInChildren<AudioSource>();
    }

    public void OpenLeaderboard()
    {
        _leaderboardPanel.SetActive(true);
        _leaderboardView.LeaderboardButton();
    }
    
    public void SetActiveButton()
     {
         if (_button!=null)
         {
             _button.gameObject.SetActive(true);
         }
     }

    private void InitiallizeButtons()
    {
        _button.GetComponentInChildren<ButtonClickSound>().Initiallize(_sourceSound);
        _buttonBack.GetComponentInChildren<ButtonClickSound>().Initiallize(_sourceSound);
    }

    public LeaderboardView GetLeaderboardView()
    {
        return _leaderboardView;
    }
    
//     [SerializeField] private LeaderboardRecord _template;
//     [SerializeField] private Transform _container;
//     [SerializeField] private int _amountRecords;
//     [SerializeField] private Button _button;
//     [SerializeField] private GameObject _panel;
//     [SerializeField] private SaveLoad _saveLoad;
//         
//     private Canvas _canvas ;
//
//         
//     private void Awake()
//     {
//         _canvas = GetComponent<Canvas>();
//         _canvas.sortingOrder = 1;
//         _button.gameObject.SetActive(false);
//     }
//         
//     protected override void OnEnabled()
//     {
//         if (PlayerAccount.IsAuthorized)
//         {
//             Leaderboard.SetScore("LeaderboardDamage",_saveLoad.ReadPointsDamage);
//         }
//     }
//
//     public int AmountRecords => _amountRecords;
//
//     private List<LeaderboardEntryResponse> _entries = new List<LeaderboardEntryResponse>();
//     private readonly List<LeaderboardRecord> _records = new List<LeaderboardRecord>();
//     private LeaderboardEntryResponse _player;
//
//     public void Init(LeaderboardGetEntriesResponse leaderboardGetEntriesResponse)
//     {
//         for (int i = 0; i < _amountRecords; i++)
//         {
//             if (leaderboardGetEntriesResponse.entries.Length <= i)
//                 break;
//
//             LeaderboardRecord record = Instantiate(_template, _container);
//             LeaderboardEntryResponse entity = leaderboardGetEntriesResponse.entries[i];
//             record.UpdateData(entity);
//             _records.Add(record);
//             _entries.Add(entity);
//
//             if (leaderboardGetEntriesResponse.userRank == entity.rank)
//                 _player = entity;
//         }
//     }
//
//     private void UpdateData(int money)
//     {
// #if !UNITY_WEBGL || UNITY_EDITOR
//         return;
// #endif
//         _player.score += Convert.ToInt32(money);
//
//         Agava.YandexGames.Leaderboard.SetScore(LeaderboardConstants.Name, _player.score);
//
//         UpdateViews();
//     }
//
//     private void UpdateViews()
//     {
// #if !UNITY_WEBGL || UNITY_EDITOR
//         return;
// #endif
//         _entries = _entries.OrderByDescending(entry => entry.score).ToList();
//
//         for (int i = 0; i < _records.Count; i++)
//             _records[i].UpdateData(_entries[i], i + 1);
//     }
//
//     public void SetActiveButton()
//     {
//         if (_button!=null)
//         {
//             _button.gameObject.SetActive(true);
//         }
//     }
//
//     public void OnClikedButton()
//     {
//         if (_panel.gameObject.activeInHierarchy==false)
//         {
//             _panel.gameObject.SetActive(true);
//             _canvas.sortingOrder = 5;
//         }
//         else
//         {
//             _panel.gameObject.SetActive(false);
//             _canvas.sortingOrder = 1;
//         }
//             
//     }
}