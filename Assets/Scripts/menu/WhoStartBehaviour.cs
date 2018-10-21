using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoStartBehaviour : MonoBehaviour {

    private SpriteRenderer _coin = null;
    private float _totalTime = 0;
    private float _initScaleX = 0;
    private AudioSource _audioSource = null;
    public Sprite coinPlayer1=null;
    public Sprite coinPlayer2 = null;
    public Sprite coinComputer = null;
    public Sprite coinPlayer = null;
    private float _luckyNumber = 0;
    private BoardConfiguration _configuration = null;
    private string _nextSceneName = "BoardScene";
    private string _actualSceneName = "WhoStartScene";

    private void Start () {
        _coin = GetComponent<SpriteRenderer>();
        _initScaleX = transform.localScale.x;
        _luckyNumber = Random.Range(3f, 6f);
        _configuration = BoardConfigurationGetter.getConfigurationObject();
        _configuration.Starter = 0;
        _audioSource = GetComponent<AudioSource>();
        if (_configuration.GameModeOption.Label == "Player X Computer")
        {
            coinPlayer1 = coinComputer;
            coinPlayer2 = coinPlayer;
        }

    }
	
	private void Update () {
        _totalTime += Time.deltaTime;
        if (_luckyNumber > 0)
        {
            _audioSource.enabled = true;
            float modTime = (_totalTime % 1);
            if (modTime < 0.25)
            {
                if (_coin.sprite == coinPlayer2)
                {
                    _luckyNumber -= 1;
                }

                _coin.sprite = coinPlayer1;
                transform.localScale = new Vector3(_initScaleX, transform.localScale.y, 1f);

            }
            else if (modTime >= 0.25 && modTime < 0.50)
            {
                _coin.sprite = coinPlayer1;
                transform.localScale = new Vector3(_initScaleX / 2f, transform.localScale.y, 1f);
                
            }
            else if (modTime >= 0.50 && modTime < 0.75)
            {
                if (_coin.sprite == coinPlayer1)
                {
                    _luckyNumber -= 1;
                }
                _coin.sprite = coinPlayer2;
                transform.localScale = new Vector3(_initScaleX, transform.localScale.y, 1f);
                
            }
            else
            {
                _coin.sprite = coinPlayer2;
                transform.localScale = new Vector3(_initScaleX / 2f, transform.localScale.y, 1f);

            }
        } else if (_configuration.Starter == 0)
        {
            _audioSource.enabled = false;
            if (_coin.sprite == coinPlayer1)
            {
                _configuration.Starter = 1;
            } else
            {
                _configuration.Starter = 2;
            }
            StartCoroutine(Timer.WaitATime(1));
            StartCoroutine(SceneLoader.LoadScene(_nextSceneName));
            StartCoroutine(SceneLoader.UnloadScene(_actualSceneName));

        }
        

        

    }
}
