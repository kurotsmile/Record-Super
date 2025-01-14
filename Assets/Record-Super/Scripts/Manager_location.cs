using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class Manager_location : MonoBehaviour
{
    public Text txt_location_map;
    public float latA, lonA;

    public Sprite sp_icon_location_allow;
    public Sprite sp_icon_location_block;
    public Image img_status_location;

    float lonB, latB, overallDistance, lastDistance, timer, lastTime, speed, speed0, acceleration;
    bool firstTime;
    private bool is_location_record = false;

    void Awake()
    {
        overallDistance = 0;
        lastDistance = 0;
        timer = 0;
        lastTime = 0;
        speed = 0;
        speed0 = 0;

        firstTime = true;
        this.check_status_location_record();
    }

    public void re_start()
    {
        StartCoroutine(Start());
    }

    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            this.GetComponent<App>().carrot.delay_function(5f,this.re_start);
            yield break;
        }
            
        Input.location.Start(1, 1);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            lonA = Input.location.lastData.longitude;
            latA = Input.location.lastData.latitude;
        }

        //Input.location.Stop();
    }

    void Update()
    {
        if (this.is_location_record)
        {
            timer += Time.deltaTime;

            if (lonA != Input.location.lastData.longitude || latA != Input.location.lastData.latitude)
            {
                CalculateDistances(lonA, latA, Input.location.lastData.longitude, Input.location.lastData.latitude);
                lonA = Input.location.lastData.longitude;
                latA = Input.location.lastData.latitude;

                this.txt_location_map.text = "Lon:" + lonA + " , Lat:" + latA;

                lastTime = timer;
                timer = 0;


                speed0 = speed;

                CalculateSpeed();
                CalculateAcceleration();
            }
        }

    }

    public static float Radians(float x)
    {
        return x * Mathf.PI / 180;
    }

    public void CalculateDistances(float firstLon, float firstLat, float secondLon, float secondLat)
    {

        float dlon = Radians(secondLon - firstLon);
        float dlat = Radians(secondLat - firstLat);

        float distance = Mathf.Pow(Mathf.Sin(dlat / 2), 2) + Mathf.Cos(Radians(firstLat)) * Mathf.Cos(Radians(secondLat)) * Mathf.Pow(Mathf.Sin(dlon / 2), 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(distance), Mathf.Sqrt(1 - distance));

        lastDistance = 6371 * c * 1000;

        overallDistance += lastDistance; 

        StartCoroutine(Overall());
    }

    IEnumerator Overall()
    {
        if (firstTime)
        {
            firstTime = false;

            yield return new WaitForSeconds(2);

            if (overallDistance > 6000000)
            {
                overallDistance = 0;
                lastDistance = 0;
            }
        }

        overallDistance += lastDistance;
    }

    void CalculateSpeed()
    {
        speed = lastDistance / lastTime * 3.6f;
    }

    void CalculateAcceleration()
    {
        acceleration = (speed - speed0) / lastTime;
    }

    public void reset_location()
    {
        overallDistance = 0;
        lastDistance = 0;
        timer = 0;
        lastTime = 0;
        speed = 0;
        speed0 = 0;
    }

    public void show_map_locations()
    {
        Application.OpenURL("https://maps.google.com?q="+this.latA.ToString().Replace(",",".")+","+this.lonA.ToString().Replace(",", ".") + "");
    }

    public void btn_change_status_location()
    {
        if (this.is_location_record) {
            this.lonA = 0;
            this.latA = 0;
            this.is_location_record = false;
        }
        else
            this.is_location_record = true;

        this.check_status_location_record();
    }

    private void check_status_location_record()
    {
        if (this.is_location_record)
        {
            this.img_status_location.sprite = this.sp_icon_location_block;
            this.txt_location_map.text = "Do not record the location";
        }
        else
        {
            this.img_status_location.sprite = this.sp_icon_location_allow;
            this.txt_location_map.text = "Determining the location...";
        }   
    }
}
