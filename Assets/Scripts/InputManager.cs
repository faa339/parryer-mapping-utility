using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode[] track1;
    public KeyCode[] track2;
    //public List<Vector2> notes; //y is beat, x is position
    string mapStr;
    int lastPos;
    // Update is called once per frame
    private void Start()
    {
        lastPos = 0;
    }
    void Update()
    {   
        // Get current gamestate (in-game or paused)
        //GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        for(int i=0; i<track1.Length; i++){
            if(Input.GetKeyDown(track1[i])){
                //notes.Add(new Vector2(0, Conductor.songPosInBeats));
                mapStr += "0 " + Conductor.songPosInBeats + "\n";
            }
        }

        for(int i=0; i<track2.Length; i++){
            if(Input.GetKeyDown(track2[i])){
                mapStr += "1 " + Conductor.songPosInBeats + "\n";
            }
        }
        int pos = (int)Conductor.songPos;

        if (pos != lastPos && pos%10==0) {
            print("Here " + pos +" "+ lastPos);
            lastPos = pos;
            mapStr += "/*" + Conductor.songPos + " current pos*/\n";
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            print("Exiting");
            System.IO.File.WriteAllText("newMap.txt", mapStr);
            
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
