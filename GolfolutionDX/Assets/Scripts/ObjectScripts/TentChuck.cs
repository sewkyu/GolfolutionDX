using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentChuck : MonoBehaviour
{
    private ChuckSubInstance myChuck;
    string myGlobalVariableName;
    // Start is called before the first frame update
    void Start()
    {
        myChuck = GetComponent<ChuckSubInstance>();
        myGlobalVariableName = myChuck.GetUniqueVariableName("event");
        GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
            SawOsc sqr => LPF lpf => dac;

            0.4 => lpf.Q;
            440 => lpf.freq;
            -1 => int octave;

            // our notes
            [ 48, 51, 53, 55, 58, 60, 63, 65, 67, 70, 72, 75, 77, 79, 82 ] @=> int notes[];

            // basic play function (add more arguments as needed)
            fun void play( float note )
            {{
                // start the note
                //<<<note>>>;
                Std.mtof( note + (12 * octave) )=> sqr.freq;
                250::ms * Math.random2(0, 3) => now;
            }}
            fun void playing() {{
                while( true ) {{
                    play(notes[Math.random2(0, notes.size() - 1)]);
                }}
            }}
            // infinite time-loop
            spork ~ playing();
            
            global Event {0};

            while (true) {{
                {0} => now;
                lpf.freq() * 2 => lpf.freq;
                octave + 1 => octave;
                10::second => now;
            }}
	    ", myGlobalVariableName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // print the object that the ball collided with
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Ball")
        {
            GetComponent<ChuckSubInstance>().BroadcastEvent(myGlobalVariableName);
            print("Broadcasted event: " + myGlobalVariableName);
        }

    }
}
