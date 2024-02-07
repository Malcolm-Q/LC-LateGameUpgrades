using UnityEngine;
using Unity.Netcode;
using GameNetcodeStuff;
using System.Linq;
using System.Collections.Generic;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.UpgradeComponents.Items.Contracts.BombDefusal
{
    internal class BombDefusalScript : NetworkBehaviour
    {
        static LGULogger logger = new LGULogger(nameof(BombDefusalScript));
        AudioSource audio;
        Dictionary<string, GameObject> wires = new Dictionary<string, GameObject>
        {
            {"red",null },
            {"green",null },
            {"blue",null }
        };
        Dictionary<string, GameObject> cutWires = new Dictionary<string, GameObject>
        {
            {"red",null },
            {"green",null },
            {"blue",null }
        };
        Dictionary<string, InteractTrigger> trigs = new Dictionary<string, InteractTrigger>
        {
            {"red",null },
            {"green",null },
            {"blue",null }
        };
        public AudioClip tick, snip;

        BoxCollider grabBox;

        TextMesh serialNumberMesh, countdown;

        float BombTimer = 300f;
        bool armed = true;

        const string allowedLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string allowedNums = "0123456789";

        void Start()
        {
            GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.instance.cfg.CONTRACT_DEFUSE_REWARD);
            grabBox = GetComponent<BoxCollider>();

            Transform intactWires = transform.GetChild(0).GetChild(0);
            wires["blue"] = intactWires.GetChild(0).gameObject;
            wires["green"] = intactWires.GetChild(1).gameObject;
            wires["red"] = intactWires.GetChild(2).gameObject;

            Transform destroyedWires = transform.GetChild(0).GetChild(1);
            cutWires["blue"] = destroyedWires.GetChild(0).gameObject;
            cutWires["green"] = destroyedWires.GetChild(1).gameObject;
            cutWires["red"] = destroyedWires.GetChild(2).gameObject;

            audio = GetComponent<AudioSource>();
            audio.clip = tick;
            audio.Play();

            trigs["red"] = transform.GetChild(0).GetChild(4).GetComponent<InteractTrigger>();
            trigs["blue"] = transform.GetChild(0).GetChild(5).GetComponent<InteractTrigger>();
            trigs["green"] = transform.GetChild(0).GetChild(6).GetComponent<InteractTrigger>();

            trigs["red"].onInteract.AddListener(CutRed);
            trigs["blue"].onInteract.AddListener(CutBlue);
            trigs["green"].onInteract.AddListener(CutGreen);

            serialNumberMesh = transform.GetChild(0).GetChild(2).GetComponent<TextMesh>();
            if (serialNumberMesh == null)
            {
                logger.LogError("FAILED TO GET SERIAL NUMBER MESH");
            }

            countdown = transform.GetChild(0).GetChild(3).GetComponent<TextMesh>();

            if (IsHost || IsServer)
            {
                char[] lets = new char[6];
                lets[0] = allowedLetters[Random.Range(0, allowedLetters.Length)];
                lets[1] = allowedLetters[Random.Range(0, allowedLetters.Length)];
                lets[2] = '-';
                lets[3] = allowedNums[Random.Range(0, allowedNums.Length)];
                lets[4] = allowedNums[Random.Range(0, allowedNums.Length)];
                lets[5] = allowedNums[Random.Range(0, allowedNums.Length)];
                UpgradeBus.instance.bombOrder = new List<string> { "red", "green", "blue" };
                Tools.ShuffleList(UpgradeBus.instance.bombOrder);
                string orderString = string.Join(",", UpgradeBus.instance.bombOrder);
                SyncBombDetailsClientRpc(new string(lets), orderString);
            }
        }

        void Update()
        {
            if (!armed) return;
            BombTimer -= Time.deltaTime;
            int minutes = (int)(BombTimer / 60);
            int seconds = (int)(BombTimer % 60);
            string secString = seconds > 9 ? seconds.ToString() : "0" + seconds.ToString();
            countdown.text = $"0{minutes} {secString}";
            if (BombTimer <= 0f)
            {
                armed = false;
                Landmine.SpawnExplosion(transform.position, true, 20f, 10f);
                gameObject.SetActive(false);
                enabled = false;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void ReqBombStateChangeServerRpc(string wire)
        {
            BombStateChangeClientRpc(wire);
        }

        [ClientRpc]
        void BombStateChangeClientRpc(string wire)
        {
            audio.PlayOneShot(snip);
            if (UpgradeBus.instance.bombOrder[0] == wire)
            {
                UpgradeBus.instance.bombOrder.Remove(wire);
                audio.PlayOneShot(snip);
                wires[wire].SetActive(false);
                cutWires[wire].SetActive(true);
                trigs[wire].enabled = false;
                trigs[wire].GetComponent<BoxCollider>().enabled = false;
                if (UpgradeBus.instance.bombOrder.Count == 0)
                {
                    // win
                    armed = false;
                    grabBox.enabled = true;
                    audio.Stop();
                    enabled = false;
                    GetComponent<PhysicsProp>().enabled = true;
                }
            }
            else
            {
                armed = false;
                Landmine.SpawnExplosion(transform.position, true, 20f, 10f);
                gameObject.SetActive(false);
                enabled = false;
            }

        }

        void CutRed(PlayerControllerB player)
        {
            if (IsHost || IsServer)
            {
                BombStateChangeClientRpc("red");
            }
            else
            {
                ReqBombStateChangeServerRpc("red");
            }
        }
        void CutBlue(PlayerControllerB player)
        {
            if (IsHost || IsServer)
            {
                BombStateChangeClientRpc("blue");
            }
            else
            {
                ReqBombStateChangeServerRpc("blue");
            }
        }
        void CutGreen(PlayerControllerB player)
        {
            if (IsHost || IsServer)
            {
                BombStateChangeClientRpc("green");
            }
            else
            {
                ReqBombStateChangeServerRpc("green");
            }
        }

        [ClientRpc]
        void SyncBombDetailsClientRpc(string serial, string newOrder)
        {
            if (serialNumberMesh == null)
            {
                // I have no clue why or how this can happen but it can and does for remote clients.
                serialNumberMesh = transform.GetChild(0).GetChild(2).GetComponent<TextMesh>();
            }
            UpgradeBus.instance.SerialNumber = serial;
            UpgradeBus.instance.bombOrder = newOrder.Split(",").ToList();
            serialNumberMesh.text = serial;
        }

    }
}
