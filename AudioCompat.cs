using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Microsoft.Xna.Framework.Audio
{
    // Minimal shim reproducing the tiny slice of the old XACT API (AudioEngine /
    // WaveBank / SoundBank / AudioCategory / Cue / AudioStopOptions) that the
    // original XNA game code calls, backed by plain MonoGame SoundEffects.
    // The original project shipped raw .wav files alongside the compiled XACT
    // project (LGF_Audio.xap); those .wav files are what actually get built by
    // the content pipeline now, and this shim loads them by cue name.

    public enum AudioStopOptions
    {
        Immediate,
        AsAuthored
    }

    public class AudioEngine
    {
        public static AudioEngine Current { get; private set; }

        private readonly Dictionary<string, AudioCategory> categories = new Dictionary<string, AudioCategory>();

        public AudioEngine(string settingsFile)
        {
            Current = this;
        }

        public void Update()
        {
        }

        public AudioCategory GetCategory(string name)
        {
            if (!categories.TryGetValue(name, out var category))
            {
                category = new AudioCategory(name);
                categories[name] = category;
            }

            return category;
        }
    }

    public class WaveBank
    {
        public WaveBank(AudioEngine engine, string path)
        {
        }
    }

    public class SoundBank
    {
        private readonly ContentManager content;

        public SoundBank(AudioEngine engine, string path)
        {
            content = SoundEffectRegistry.Content;
        }

        public Cue GetCue(string name)
        {
            return new Cue(name, content);
        }

        public void PlayCue(string name)
        {
            new Cue(name, content).Play();
        }
    }

    public class AudioCategory
    {
        public string Name { get; }

        private readonly List<Cue> activeCues = new List<Cue>();

        internal AudioCategory(string name)
        {
            Name = name;
        }

        internal void Register(Cue cue)
        {
            activeCues.RemoveAll(c => c.IsStopped);
            activeCues.Add(cue);
        }

        public void SetVolume(float volume)
        {
            activeCues.RemoveAll(c => c.IsStopped);

            foreach (var cue in activeCues)
                cue.Volume = volume;
        }

        public void Stop(AudioStopOptions options)
        {
            foreach (var cue in activeCues)
                cue.Stop(options);

            activeCues.Clear();
        }

        public void Pause()
        {
            activeCues.RemoveAll(c => c.IsStopped);

            foreach (var cue in activeCues)
                cue.Pause();
        }

        public void Resume()
        {
            foreach (var cue in activeCues)
                cue.Resume();
        }
    }

    public class Cue
    {
        private static readonly HashSet<string> MusicCueNames = new HashSet<string>
        {
            "menu_music", "main_club_music", "vip_music", "boss_music", "credits_song"
        };

        private readonly SoundEffectInstance instance;

        public string Name { get; }

        public bool IsPlaying => instance != null && instance.State == SoundState.Playing;
        internal bool IsStopped => instance == null || instance.State == SoundState.Stopped;

        public float Volume
        {
            get => instance?.Volume ?? 0f;
            set { if (instance != null) instance.Volume = value; }
        }

        internal Cue(string name, ContentManager content)
        {
            Name = name;

            var effect = SoundEffectRegistry.Find(content, name);

            if (effect != null)
            {
                instance = effect.CreateInstance();

                if (MusicCueNames.Contains(name))
                    instance.IsLooped = true;

                var categoryName = MusicCueNames.Contains(name) ? "Music" : "Default";
                AudioEngine.Current?.GetCategory(categoryName).Register(this);
            }
        }

        public void Play()
        {
            instance?.Play();
        }

        public void Pause()
        {
            instance?.Pause();
        }

        public void Resume()
        {
            instance?.Resume();
        }

        public void Stop(AudioStopOptions options)
        {
            instance?.Stop(options == AudioStopOptions.Immediate);
        }
    }

    // Loads and caches every SoundEffect referenced by the game's XACT cue
    // names. Populated once at startup from a manifest generated from the
    // original raw .wav assets.
    public static class SoundEffectRegistry
    {
        public static ContentManager Content;

        private static readonly Dictionary<string, SoundEffect> cache = new Dictionary<string, SoundEffect>();

        public static SoundEffect Find(ContentManager content, string cueName)
        {
            if (cache.TryGetValue(cueName, out var cached))
                return cached;

            if (!AudioManifest.CuePaths.TryGetValue(cueName, out var assetPath))
                return null;

            var effect = content.Load<SoundEffect>(assetPath);
            cache[cueName] = effect;
            return effect;
        }
    }
}
