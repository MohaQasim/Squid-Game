using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool running = false;
    bool game_started = false;
    bool game_over = false;

    public GameObject player;
    public Animator player_animator;

    public GameObject toy;
    public Animator toy_animator;

    public GameObject leser;
    public GameObject came;

    public ParticleSystem blood_splash;
    public GameObject blood;

    AudioSource source;

    public AudioClip step;
    public AudioClip shooting;
    public AudioClip hit;
    public AudioClip fall;

    float steps_counter;

    public GameObject ui_start;
    public GameObject ui_gameover;
    public GameObject ui_win;
    public Text ui_guide;

    float speed = 1;

    KeyCode key1 = 0, key2 = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
        ui_start.SetActive(true); // Show UI_start  when the game starta3q
        }

   
    void Update()
    {
        if (running) 
            {
            player.transform.position -= new Vector3(0, 0, 0.5f * Time.deltaTime); // MOVE THE PLAYER
            came.transform.position -= new Vector3(0, 0, 0.5f * Time.deltaTime); // Move The Camera

            steps_counter += Time.deltaTime;

            if(steps_counter > .25f) // player pedometer & Repeat the sing every .25s
                {
                steps_counter = 0;
                source.PlayOneShot(step);
                }

            }
        if (Input.GetKeyDown(KeyCode.Space) && !game_started) // IF PRESS (SPACE ) THE PLAYER MOVEING
            {
            running = true;
            game_started = true;  //  move Player
            ui_start.SetActive(false); // hid UI_start when press SPACE
            player_animator.SetTrigger("run"); // CALL Animator Player
            StartCoroutine(Sing());      // Call starting sing

            }

        if (Input.GetKeyDown(KeyCode.Space) && game_over) // when show Gameover && press SPACE 
            {
            SceneManager.LoadScene("Game"); // load scenes GAME

            }
         
        // PRESS AND HOLD BUTTONS TO AVOID HUNTING AND CONTINUE TO WIN!

        if (Input.GetKey(key1) && Input.GetKey(key2) && !game_over) // IF THE BUTTONS ARE NOT PRESSED
            {

            running = false; // THE PLAYER STOPPED RUNNING
            player_animator.speed = 0; // THE PLAYER STOPPED ANIMATOR

            }
        else if ((Input.GetKeyUp(key1) || Input.GetKeyUp(key2)) && !game_over) // IF PRESS THE BUTTONS
            {
            running = true; // RUN CONTINUES
            player_animator.speed = 1; // ANIMATOR CONTINUES
            }

        }
    IEnumerator Sing() // singing stage
        {
        toy.GetComponent<AudioSource>().Play(); // start singing

        yield return new WaitForSeconds(4.5f / speed); // Here Toy sing  For 4,5 seconds

        key1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));
        key2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));
       
        // SHOW THE BUTTONS THAT MUST BE PRESSED TO STOP THE PLAYER
        ui_guide.text = "PRESS " + key1 + " + " + key2 + " TO STOP";

        toy_animator.SetTrigger("look"); // Call Animator Toy to "look"

        yield return new WaitForSeconds(2 / speed); // Toy look & waiting 2 seconds ,If the player moving Toy shoot the laser
        
        if (running) //CHECK IF THE PLAYER IS STILL MOVING , Toy Shoots laser
            {
            Debug.Log("SHOOT THE PLAYER");
            GameObject new_leser = Instantiate(leser);
            new_leser.transform.position = toy.transform.GetChild(0).transform.position;  // call the laser form the head(first child) of the Toy
            game_over = true;
            source.PlayOneShot(shooting); // Call shooting audio
            }
        ui_guide.text = ""; // WHEN THE PLAYER IS KILLED AND SHOWED A TEXT (GAME OVER!) , HIDE TEXT (PRESS + KEY1+ KEY2 + STOP)

        yield return new WaitForSeconds(2 / speed);  // Here Toy waiting 2 S.
        toy_animator.SetTrigger("idle");     // Call Animator Toy to "idle"

        yield return new WaitForSeconds(1 / speed); // Here Toy repeats what she dose , If Toy dont see the player move
        toy.GetComponent<AudioSource>().Stop(); // stop first song

        speed = speed * 1.15f;

        toy.GetComponent<AudioSource>().pitch = speed; // INCREASE THE SPEED OF SOUND

        if (!game_over) StartCoroutine(Sing()); // if not Game over , Song is counted
        }
    public void HitPlayer()//
        {
        running = false; // Stop run
        player_animator.SetTrigger("idle");  // Change animation from run to idle
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 2);   // When a player is injured , Player dash up & back
        player.GetComponent<Rigidbody>().angularVelocity = new Vector3(3, 0, 0); // Player push downfall
        came.GetComponent<Animator>().Play("camera_lose");   // change camera animation to player ,when fall
        blood_splash.Play();
        StartCoroutine(ShowBlood()); // Call  showblood
        source.PlayOneShot(hit);  // Call hit audio
        }
    IEnumerator ShowBlood()
        {
       
        yield return new WaitForSeconds(.3f);
        ui_gameover.SetActive(true); // Show UI_gameover when player fall
        source.PlayOneShot(fall);  // Call fall audio
        blood.SetActive(true);
        blood.transform.position = new Vector3(player.transform.position.x, 0.001f, player.transform.position.z + 0.4f);
        
        }
   public IEnumerator PlayerWin()
        {
        game_over = true;
        yield return new WaitForSeconds(1f);

        running = false; 
        player_animator.SetTrigger("idle");

        ui_win.SetActive(true);
        }
 }
