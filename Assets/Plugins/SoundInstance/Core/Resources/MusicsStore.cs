using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
 public class MusicsStore : ScriptableObject
    {
        public List<Music> gameplayMusics;
        public List<Music> menuMusics;
        public List<Music> loadingMusics;
        public List<S_Effect> soundEffects;
        private HashSet<int> _passedIndices = new HashSet<int>();
        private int _pastIndex = -1;

        public Music GetGameplayMusic(string name)
        {
            foreach (Music music in gameplayMusics)
            {
                if (music.name == name)
                {
                    return music;
                }
            }

            return new Music();
        }

        public Music GetMenuMusic(string name)
        {
            foreach (Music music in menuMusics)
            {
                if (music.name == name)
                {
                    return music;
                }
            }

            return new Music();
        }

        public Music GetLoadingMusic(string name)
        {
            foreach (Music music in loadingMusics)
            {
                if (music.name == name)
                {
                    return music;
                }
            }

            return new Music();
        }


        public Music GetRandomMusic()
        {
            if (_passedIndices.Count == gameplayMusics.Count)
                _passedIndices.Clear();

            int i = NextIndexFrom(_passedIndices);
            return gameplayMusics[i];
        }

        public int NextIndexFrom(HashSet<int> set)
        {
            int i;
            do
            {
                i = Random.Range(0, gameplayMusics.Count);
            } while (set.Contains(i) || i == _pastIndex);

            set.Add(i);
            _pastIndex = i;
            return i;
        }

        public int GetMusicIndex(Music music)
        {
            for (int i = 0; i < gameplayMusics.Count; i++)
            {
                if (gameplayMusics[i].name == music.name)
                {
                    return i;
                }
            }

            return 0;
        }

        public Music GetMusicByIndex(int index)
        {
            if (index < gameplayMusics.Count)
            {
                return gameplayMusics[index];
            }
            else
            {
                return null;
            }
        }


        public S_Effect GetSoundEffect(string name)
        {
            foreach (S_Effect se in soundEffects)
            {
                if (se.name == name)
                {
                    return se;
                }
            }

            return new S_Effect();
        }
    }


    [Serializable]
    public class Music
    {
        public string name;
        public AudioClip Song;
    }


    [Serializable]
    public class S_Effect
    {
        public string name;
        public AudioClip Sound;
    }
    