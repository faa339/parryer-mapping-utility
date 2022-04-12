using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class InputManager : MonoBehaviour
{
    public KeyCode[] track1;
    public KeyCode[] track2;
    //public List<Vector2> notes; //y is beat, x is position
    string mapStr;
    int lastPos;
    int escCount;
    // Update is called once per frame
    private void Start()
    {
        escCount = 0;
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
            if (escCount == 0)
            {
                var savedText = GameObject.Find("SavedText");
                savedText.GetComponent<TextMeshProUGUI>().enabled = true;
                GameObject c = GameObject.Find("Manager");
                if (c == null) {
                    savedText.GetComponent<TextMeshProUGUI>().text = "You pressed escape too early--" +
                        "nothing was generated. Press esc again to restart process\n";
                    escCount++;
                    return;
                }
                Conductor cond = c.GetComponent<Conductor>();
                string preInput = "{" +cond.songBpm + " " 
                    +cond.firstBeatOffset + " " 
                    +cond.musicSource.clip.length + " " 
                    +cond.musicSource.clip.name+" "
                    + "}\n";
                mapStr = preInput + mapStr;
                Regex r = new Regex("[\\s\"\'*<>\\|\\/:?]|($|\\s\\.)"); //attempt to replace illegal characters lol
                string sanitizedSongName = r.Replace(cond.musicSource.clip.name, "_");
                //sanitizedSongName = sanitizedSongName.Replace(" ", "_");
                sanitizedSongName += "_map.txt";
                System.IO.File.WriteAllText(sanitizedSongName, mapStr);
                savedText.GetComponent<TextMeshProUGUI>().text += sanitizedSongName+"\nPress escape again to reset\n";
                cond.musicSource.Stop();
                escCount++;
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();
        }
    }
}
