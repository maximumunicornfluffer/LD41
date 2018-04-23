using System.Collections.Generic;
using Assets.Scripts.PlayerManagement;
using States;
using UnityEngine;

namespace DefaultNamespace.UI
{

    public class TutoUI : MonoBehaviour {

        private Animator _animator;
	
        // Use this for initialization
        void Start () {

            _animator = GetComponent<Animator>();
        }
	
        // Update is called once per frame
        void Update () {

            bool gotoMatch = false;

            var validInputIndexes = PlayerInputUtils.GetValidInputIndexes();
            foreach (var index in validInputIndexes)
            {
                if (PlayerInputUtils.GetButtonDown(PlayerInput._A, index)) {
                    gotoMatch = true;
                }
            }

            if (gotoMatch)
            {
                _animator.Play("Close");
            }
        }

        private void GotoMatch()
        {
            FSM.Instance.GotoState<GameState>(new List<string>() {"Level1"});
        }

        public void OnAnimExitEnd()
        {
            GotoMatch();		
        }
	
    }

}