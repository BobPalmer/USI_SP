using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

namespace AirbagTools
{
    public class ModuleAirbag : PartModule
    {
        [KSPField] 
        public float dampenFactor = .75f;
        [KSPField] 
        public float dampenSpeed = 15f;
        [KSPField]
        public String deployAnimationName = "Deploy";

        private bool _isDeployed;

        [KSPAction("Toggle")]
        public void ToggleAction(KSPActionParam param)
        {
            ToggleAirbags();
        }


        [KSPEvent(guiActive = true, guiName = "Toggle Airbags", active = true, externalToEVAOnly = true, guiActiveEditor = true)]
        public void ToggleAirbags()
        {
            if (_isDeployed)
            {
                _isDeployed = false;
                DeployAnimation[deployAnimationName].speed = -1;
                DeployAnimation[deployAnimationName].time = DeployAnimation[deployAnimationName].length;
                DeployAnimation.Play(deployAnimationName);
            }
            else
            {
                _isDeployed = true;
                DeployAnimation[deployAnimationName].speed = 1;
                DeployAnimation.Play(deployAnimationName);
            }
        }

        public Animation DeployAnimation
        {
            get
            {
                return part.FindModelAnimators(deployAnimationName)[0];
            }
        }


        public override void OnFixedUpdate()
        {
            if (part.checkLanded())
            {
                Dampen();   
            }
            if (part.Landed)
            {
                Dampen();
            }
        }

        private void Dampen()
        {
            if (vessel.srfSpeed > dampenSpeed
                || vessel.horizontalSrfSpeed > dampenSpeed)
            {
                print("Dampening...");
                foreach (var p in vessel.parts)
                {
                    p.Rigidbody.angularVelocity *= dampenFactor;
                    p.Rigidbody.velocity *= dampenFactor;
                }
            }
        }

        public override void OnLoad(ConfigNode node)
        {
            part.force_activate();
        }

        public override void OnStart(StartState state)
        {
            part.force_activate();
        }

        public override void OnAwake()
        {
            part.force_activate();
        }

        public override void OnInitialize()
        {
            part.force_activate();
        }
    }
}
