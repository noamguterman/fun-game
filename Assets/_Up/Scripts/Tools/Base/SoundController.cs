using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets._Scripts.Tools.Base
{
    /// <summary>
    /// Abstract Sound Controller
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SoundController<T> : IDisposable where T : struct, IConvertible
    {
        
        private static readonly AudioSource AudioSource;
        private static Dictionary<T, AudioClipRelevance> _audioClips;
        private static AssetBundle _asset;
        private static readonly MonoBehaviour SoundHelper;
        private static StateSoundLoad _stateSoundLoad;
        private static float _time;//used in Timer
        private static bool _block;//uset to break _PlaySet

        protected static string Prefix;
        protected static bool IsLocalSound = true;

        public static float Volume
        {
            get { return AudioSource.volume; }
            set { AudioSource.volume = Mathf.Clamp(value, 0, 1); }
        }

        protected static AssetBundle Asset
        {
            get { return _asset; }
            set
            {
                if(!value)return;
                _asset = value;
                _stateSoundLoad = StateSoundLoad.AssetBundle;
            }
        }

        protected static AudioSource Source
        {
            get { return AudioSource; }
        }

        public static Vector3 Position
        {
            get { return SoundHelper.transform.position; }
            set { SoundHelper.transform.position = value; }
        }

        static SoundController()
        {
            var go = new GameObject("SoundObject");
            Object.DontDestroyOnLoad(go);
            SoundHelper = go.AddComponent<MonoBehaviour>();
            AudioSource = go.AddComponent<AudioSource>();
            _audioClips = new Dictionary<T, AudioClipRelevance>();
            //Localization._Scripts.Localization.Instance.LanguageChanged+= local =>
            //{
            //    if (!IsLocalSound) return;
            //    foreach (var clip in _audioClips.Values)
            //    {
            //        clip.Relevance = false;
            //    }
            //};
        }

        public static void Stop()
        {
            AudioSource.Stop();
        }

        public virtual void Dispose()
        {
            _audioClips = null;
            Object.Destroy(AudioSource.gameObject);
        }


        protected static float Play(T sound)
        {
            return Play(sound, sound.ToString());
        }

        protected static float Play(T sound, string path)
        {
            _block = true;
            if (_audioClips.ContainsKey(sound) && _audioClips[sound].Relevance)
            {
                Play(_audioClips[sound].AudioClip);
                Timer(_audioClips[sound].AudioClip.length);
                return _audioClips[sound].AudioClip.length;
            }
            path = MakePath(path);
            LoadAudioClip(sound, path);
            return Play(sound, path);
        }

        protected static float PlaySet(params T[] sounds)
        {
            return
                PlaySet(
                    sounds.Select(
                        sound => new SoundPathPair<T> {Sound = sound, Path = sound.ToString()})
                        .ToArray());
        }

        protected static float PlaySet(params SoundPathPair<T>[] sounds)
        {
            var clipSetLength =
                sounds.Sum(
                    sound =>
                    {
                        if (!_audioClips.ContainsKey(sound.Sound))
                        {
                            return LoadAudioClip(sound.Sound, MakePath(sound.Path));
                        }
                        return _audioClips[sound.Sound].Relevance
                            ? _audioClips[sound.Sound].AudioClip.length
                            : LoadAudioClip(sound.Sound, MakePath(sound.Path));
                    });
            SoundHelper.StopCoroutine("_PlaySet");
            SoundHelper.StartCoroutine(_PlaySet(sounds.Select(pair => pair.Sound).ToArray()));
            Timer(clipSetLength);
            return clipSetLength;
        }

        protected static float PlaySomeOf(params T[] sounds)
        {
            return Play(sounds[Random.Range(0, sounds.Length)]);
        }

        protected static float PlaySomeOf(params SoundPathPair<T>[] sounds)
        {
            var spp = sounds[Random.Range(0, sounds.Length)];
            return Play(spp.Sound, spp.Path);
        }

        protected static void Play(AudioClip clip)
        {
            if (OnStartPlay != null) OnStartPlay();
            AudioSource.Stop();
            AudioSource.clip = clip;
            AudioSource.Play();
        }

        private static float LoadAudioClip(T sound, string path)
        {
            AudioClip ac = null;
            switch (_stateSoundLoad)
            {
                case StateSoundLoad.Resources:
                    ac = Resources.Load<AudioClip>(path);
                    break;
                case StateSoundLoad.AssetBundle:
#if UNITY_5
                    ac = Asset.LoadAsset<AudioClip>(path);//unity 5
#elif UNITY_4
                    ac = Asset.Load(path, typeof (AudioClip)) as AudioClip; //Unity 4
#endif
                    break;
            }
            _audioClips[sound] = new AudioClipRelevance {AudioClip = ac, Relevance = true};
            return ac.length;
        }

        private static string MakePath(string path)
        {
            return null;//todo fix sounds localization
            if (string.IsNullOrEmpty(Prefix))
            {
                //return IsLocalSound ? string.Format("{0}/{1}", _local, path) : path;
            }
            else
            {
                //return IsLocalSound
                //    ? string.Format("{0}/{1}/{2}", Prefix, _local, path)
                //    : string.Format("{0}/{1}", Prefix, path);
            }

        }

        private static IEnumerator _PlaySet(params T[] sounds)
        {
            _block = false;
            var delta = 0f;
            if (OnStartPlay != null) OnStartPlay();
            foreach (var ac in sounds.TakeWhile(sound => !_block).Select(sound => _audioClips[sound].AudioClip))
            {
                Play(ac);
                yield return new WaitForSeconds(ac.length + delta);
            }
            //if (OnStopPlaySet != null) OnStopPlaySet();
        }

        private static void Timer(float time)
        {
            if (_time > 0)
            {
                _time = time;
            }
            else
            {
                _time = time;
                SoundHelper.StartCoroutine(_Timer());
            }
        }

        private static IEnumerator _Timer()
        {
            while (_time > 0)
            {
                _time -= Time.deltaTime;
                yield return null;
            }
            if (OnStopPlay != null) OnStopPlay();
        }

        protected static void PlaySetAtOneTime(params SoundPathPair<T>[] sounds)
        {
            foreach (var sound in sounds)
            {
                if (!(_audioClips.ContainsKey(sound.Sound) && _audioClips[sound.Sound].Relevance))
                {
                    var path = MakePath(sound.Path);
                    LoadAudioClip(sound.Sound, path);
                }
                AudioSource.PlayClipAtPoint(_audioClips[sound.Sound].AudioClip, SoundHelper.transform.position, 1);
            }
        }

        protected static void PlaySetAtOneTime(params T[] sounds)
        {
            PlaySetAtOneTime(sounds.Select(s => new SoundPathPair<T>
            {
                Sound = s,
                Path = s.ToString()
            }).ToArray());
        }

        public static event Action OnStartPlay;
        public static event Action OnStopPlay;
        /// <summary>
        /// AudioClip Relevance
        /// </summary>
        private class AudioClipRelevance
        {
            public AudioClip AudioClip { get; set; }
            public bool Relevance { get; set; }
        }

    }
    /// <summary>
    /// Defines a Sound Path Pair
    /// </summary>
    /// <typeparam name="T">Sound enum type </typeparam>
    public class SoundPathPair<T> where T : struct, IConvertible
    {
        public T Sound { get; set; }
        public string Path { get; set; }
    }
    /// <summary>
    /// Load from Resources or AssetBundle
    /// </summary>
    public enum StateSoundLoad : byte
    {
        Resources,
        AssetBundle
    }
}