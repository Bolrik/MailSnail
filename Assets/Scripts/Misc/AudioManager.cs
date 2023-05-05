using FreschGames.Core.Managers;
using UnityEngine;

namespace MailSnail.Misc
{
    [CreateAssetMenu(fileName = "Audio Manager", menuName = "Managers/Audio/new Audio Manager")]
    public class AudioManager : Manager
    {
        private AudioSource source = null;
        private AudioSource Source
        {
            get
            {
                if (this.source == null)
                {
                    this.CreateSource();
                }

                return source;
            }
            set { source = value; }
        }


        protected override void OnEnable()
        {
            base.OnEnable();

            this.Source = null;
        }

        protected override void OnSceneHookCreated()
        {
            base.OnSceneHookCreated();
        }

        public void Play(AudioClip clip)
        {
            this.Source.clip = clip;
            this.Source.Play();
        }

        private void CreateSource()
        {
            // Needs to be the field, else StackOverflow!
            if (this.source)
                return;

            GameObject obj = new GameObject("AudioSource");
            DontDestroyOnLoad(obj);
            obj.AddComponent<ManagerHook>().Set(this);
            this.Source = obj.AddComponent<AudioSource>();
        }
    }
}