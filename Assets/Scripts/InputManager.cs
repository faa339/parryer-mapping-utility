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
    enum NoteType{SINGLE, HELD, DRUMROLL, HAZARD}; //doing a bit 
    //public List<Vector2> notes; //y is beat, x is position
    List<string> mapLst;
    int lastPos;
    int escCount;
    float held1start;
    float held2start;
    float holdCheck = 0.5f;
    // Update is called once per frame
    private void Start()
    {
        escCount = 0;
        lastPos = 0;
        held1start = -1;
        held2start = -1;
        mapLst = new List<string>();
    }
    void Update()
    {
        // log the time for mapping analysis use
        int pos = (int)Conductor.songPos;
        if (pos != lastPos && pos % 10 == 0)
        {
            print("Here " + pos + " " + lastPos);
            lastPos = pos;
            mapLst.Add("/*" + Conductor.songPos + " current pos*/\n");
        }

    
        for (int i=0; i<track1.Length; i++){
            if (Input.GetKeyDown(track1[i]))
            {
                if (held1start == -1)
                {
                    held1start = Conductor.songPosInBeats;

                }
            }
            else if (Input.GetKeyUp(track1[i]))
            {
                if (Mathf.Abs(Conductor.songPosInBeats - held1start) > holdCheck)
                {
                    //threshhold for held notes
                    mapLst.Add("0 " + NoteType.HELD + " " + held1start + " " + Conductor.songPosInBeats + "\n");
                }
                else
                { //add a kraft singles to the map list
                    mapLst.Add("0 " + NoteType.SINGLE + " " + held1start + "\n");
                }
                held1start = -1;
            }
            else if (Input.GetKeyDown(KeyCode.G)) {
                // add a quick hazard 
                mapLst.Add("0 " + NoteType.HAZARD + " " + Conductor.songPosInBeats + "\n");
            }
        }

        for(int i=0; i<track2.Length; i++){
            if(Input.GetKeyDown(track2[i])){
                //same as above
                if (held2start == -1) {
                    held2start = Conductor.songPosInBeats;
                }
                //mapStr += "1 " + NoteType.SINGLE + " " + Conductor.songPosInBeats + "\n";
            }
            else if (Input.GetKeyUp(track2[i])){
                if (Mathf.Abs(Conductor.songPosInBeats - held2start) > holdCheck){
                    //threshhold for held notes
                    mapLst.Add( "1 " + NoteType.HELD + " " + held2start + " " + Conductor.songPosInBeats + "\n");
                }
                else {
                    mapLst.Add("1 " + NoteType.SINGLE + " " + held2start + "\n");
                }
                held2start = -1;
            }
            else if (Input.GetKeyDown(KeyCode.H)){
                mapLst.Add( "1 " + NoteType.HAZARD + " " + Conductor.songPosInBeats + "\n");
            }
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
                string mapStr = preInput + string.Join("", mapLst);
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
        }
    }

    /* deprecated idea of cleaning the map for double presses
     * before current scheme was reached
    string CleanMap(string map) {
        //TODO: clean the map!
        string cleanmap = map;
        return cleanmap;
    }*/

}
