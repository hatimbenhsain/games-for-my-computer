/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System.Collections;
using UnityEngine;
using Yarn.Unity;

namespace Yarn.Unity.Example
{
    /// <summary>
    /// Attached to the non-player characters, and stores the name of the Yarn
    /// node that should be run when you talk to them.
    /// </summary>
    public class NPC : MonoBehaviour
    {
        public string characterName = "";

        public string talkToNode = "";

        private Animator animator;

        private DialogueAssets dialogueAssets;

        private void Start() {
            animator=GetComponent<Animator>();
            dialogueAssets=FindObjectOfType<DialogueAssets>();
        }

        [YarnCommand]
        public void ChangeStartNode(string newNode){
            talkToNode=newNode;
        }

        [YarnCommand]
        public void AnimatorSetBool(string parameter, bool b){
            animator.SetBool(parameter,b);
        }

        [YarnCommand]
        public void TriggerVFX(string parameter){
            if(parameter.ToLower()=="compliment"){
                CharacterVFX characterVFX=GetComponentInChildren<CharacterVFX>();
                if(characterVFX==null){
                    GameObject prefab=dialogueAssets.vfxPrefab;
                    characterVFX=Instantiate(prefab,transform).GetComponent<CharacterVFX>();
                    Debug.Log("instantiate new");
                }
                characterVFX.TriggerComplimentVFX();
            }
        }

        [YarnCommand]
        public void Complimented(){
            TriggerVFX("compliment");
            StartCoroutine("ComplimentAnimation");
        }

        private IEnumerator ComplimentAnimation(){
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("complimented",true);

        }
    }


}
