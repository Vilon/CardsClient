using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    public class SoundManager : Manager
    {
        private AudioSource bgAudio;
        private AudioSource voiceAudio;

        private Hashtable sounds = new Hashtable();

        void Start()
        {
            bgAudio = gameObject.AddComponent<AudioSource>();
            voiceAudio = gameObject.AddComponent<AudioSource>();
        }

        /// <summary>
        /// 添加一个声音
        /// </summary>
        void Add(string key, AudioClip value)
        {
            if (sounds[key] != null || value == null) return;
            sounds.Add(key, value);
        }

        /// <summary>
        /// 获取一个声音
        /// </summary>
        AudioClip Get(string key)
        {
            if (sounds[key] == null) return null;
            return sounds[key] as AudioClip;
        }

        /// <summary>
        /// 载入一个音频
        /// </summary>
        public void LoadAudioClip(string path, System.Action<AudioClip> callback)
        {
            AudioClip ac = Get(path);
            if (ac == null)
            {
                ResManager.LoadSound(path, (args) =>
                 {
                     ac = args[0] as AudioClip;
                     Add(path, ac);
                     callback(ac);
                 });
            }
            else
                callback(ac);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="canPlay"></param>
        public void PlayBacksound(string path, bool canPlay)
        {
            LoadAudioClip(path, (args) =>
             {
                 if (args != null)
                 {
                     bgAudio.loop = true;
                     bgAudio.clip = args;
                     bgAudio.Play();
                 }
             });
        }

        /// <summary>
        /// 播放音频剪辑
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="position"></param>
        public void Play(string path, Vector3 position)
        {
            LoadAudioClip(path, (args) =>
            {
                if (args != null)
                    AudioSource.PlayClipAtPoint(args, position,1);
            });
        }
    }
}