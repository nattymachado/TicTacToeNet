using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButterflyBehaviourScript : MonoBehaviour {

    private Camera _camera = null;
    private Vector3 _limitOfWorld;
    private SpriteRenderer _sprite = null;
    private float _totalTime = 0;
    private float _initScaleX = 0;
    private float _baseLocation;

    public float WaveLength = 10f;
    public float WaveAmplitude = 5f;
    public float Speed = 5f;

    public void OnClick()
    {
        changeColor();
    }

    private void Start()
    {
        _camera = Camera.main;
        _sprite = GetComponent<SpriteRenderer>();
        float butterflySize = (float) transform.localScale.x / Random.Range(1, 3);
        transform.localScale = new Vector3((float) butterflySize, (float) butterflySize, 1f);
        _initScaleX = transform.localScale.x;
        _totalTime = 0;
        Speed = Random.Range(1f, 5f);
        WaveAmplitude = Random.Range(1f, 5f);
        WaveLength = Random.Range(10f, 20f);
        _limitOfWorld = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        
        initializePositionAndColor();
    }

    private void changeColor()
    {
        _sprite.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void initializePositionAndColor()
    {
        changeColor();
        _baseLocation = Random.Range(-_limitOfWorld.x, _limitOfWorld.x);
        transform.position = new Vector3(_baseLocation, -_limitOfWorld.y, 0f);
    }

    private void updatePosition()
    {
        _totalTime += Time.deltaTime;
        Vector3 position = transform.position;
        position.y += Speed * Time.deltaTime;
        position.x = _baseLocation + Mathf.Sin(position.y * 2 * Mathf.PI / WaveLength) * WaveAmplitude / 2;

        if (position.y > _limitOfWorld.y)
        {
            initializePositionAndColor();
        } else
        {
            transform.position = position;
        }

        if ( (_totalTime % 1) < 0.6)
        {
            transform.localScale = new Vector3(_initScaleX / 2f, transform.localScale.y, 1f);
        } else{
            transform.localScale = new Vector3(_initScaleX, transform.localScale.y, 1f);
        }
            
    }

    void Update () {
        updatePosition();
    }
}
