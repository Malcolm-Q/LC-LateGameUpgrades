using MoreShipUpgrades.Managers;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class coilHeadItem : GrabbableObject
    {
        private bool Active, audioInit;
        private Animator anim;
        private AudioSource audio;
        public AudioClip robot, mineBeep;

        public override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            UpgradeBus.instance.coilHeadItems.Add(this);
            audio = GetComponent<AudioSource>();
            audio.maxDistance = 6;
        }
        public override void Update()
        {
            base.Update();
            if (isHeld || isHeldByEnemy) 
            {
                Active = false;
                anim.SetBool("Grounded", false);
                audio.Stop();
                audioInit = false;
            }
            else 
            {
                Active = true; 
                anim.SetBool("Grounded", true);
                if(!audioInit)
                {
                    audio.PlayOneShot(mineBeep);
                    audio.Play();
                    audioInit = true;
                }
            }
        }

        public bool HasLineOfSightToPosition(Vector3 pos, int range = 60)
        {
            if(!Active) return false;
            float num = Vector3.Distance(base.transform.position, pos);
            bool result = num < (float)range && !Physics.Linecast(transform.position, pos, StartOfRound.Instance.collidersRoomDefaultAndFoliage, QueryTriggerInteraction.Ignore);
            return result;
        }
    }
}
