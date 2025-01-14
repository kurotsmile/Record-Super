using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Pitch : MonoBehaviour
{
    public GameObject panel_audio_pitch;
    public GameObject obj_img_change_status;
    private bool r_is_change = false;

    public Slider Slider_Pitch;
    public Slider Slider_Stereo_Pan;
    public Slider Slider_Spatial_Blend;
    public Slider Slider_Reverb_Zone_Mix;

    public Text val_Pitch;
    public Text val_Stereo_Pan;
    public Text val_Blend;
    public Text val_Reverb_Zone_Mix;

    private AudioSource sound;

    public void load()
    {
        this.panel_audio_pitch.SetActive(false);
        this.sound = this.GetComponent<App>().myAudioPlay;
        if (PlayerPrefs.GetInt("r_is_change", 0) == 1)
            this.r_is_change = true;
        else
            this.r_is_change = false;
        this.check_status_is_change();
        this.load_data();
    }

    private void load_data()
    {
        this.sound.pitch = PlayerPrefs.GetFloat("r_pitch", 0.5f);
        this.sound.panStereo = PlayerPrefs.GetFloat("r_stereo_pan",0);
        this.sound.spatialBlend = PlayerPrefs.GetFloat("r_spatial_blend",0);
        this.sound.reverbZoneMix = PlayerPrefs.GetFloat("r_reverb_zone_mix",1);
    }

    public void btn_show_audio_pitch()
    {
        this.GetComponent<App>().ads.Show_ads_Interstitial();
        this.panel_audio_pitch.SetActive(true);
        this.update_val_slider();
        this.update_val();
    }

    private void update_val_slider()
    {
        this.Slider_Pitch.value = this.sound.pitch;
        this.Slider_Stereo_Pan.value = this.sound.panStereo;
        this.Slider_Spatial_Blend.value = this.sound.spatialBlend;
        this.Slider_Reverb_Zone_Mix.value = this.sound.reverbZoneMix;
    }

    public void close()
    {
        this.panel_audio_pitch.SetActive(false);
    }

    public void change_pitch()
    {
        this.sound.pitch = this.Slider_Pitch.value;
        PlayerPrefs.SetFloat("r_pitch", this.sound.pitch);
        PlayerPrefs.SetInt("r_is_change", 1);
        this.r_is_change = true;
        this.update_val();
    }

    public void change_Stereo_Pan()
    {
        this.sound.panStereo = this.Slider_Stereo_Pan.value;
        PlayerPrefs.SetFloat("r_stereo_pan", this.sound.panStereo);
        PlayerPrefs.SetInt("r_is_change", 1);
        this.r_is_change = true;
        this.update_val();
    }

    public void change_Spatial_Blend()
    {
        this.sound.spatialBlend = this.Slider_Spatial_Blend.value;
        PlayerPrefs.SetFloat("r_spatial_blend", this.sound.spatialBlend);
        PlayerPrefs.SetInt("r_is_change", 1);
        this.r_is_change = true;
        this.update_val();
    }

    public void change_Reverb_Zone_Mix()
    {
        this.sound.reverbZoneMix = this.Slider_Reverb_Zone_Mix.value;
        PlayerPrefs.SetFloat("r_reverb_zone_mix", this.sound.reverbZoneMix);
        PlayerPrefs.SetInt("r_is_change", 1);
        this.r_is_change = true;
        this.update_val();
    }

    private void update_val()
    {
        this.val_Pitch.text= this.sound.pitch.ToString();
        this.val_Stereo_Pan.text= this.sound.panStereo.ToString();
        this.val_Blend.text = this.sound.spatialBlend.ToString();
        this.val_Reverb_Zone_Mix.text = this.sound.reverbZoneMix.ToString();
        this.check_status_is_change();
    }

    public void reset()
    {
        PlayerPrefs.SetFloat("r_pitch", 0.5f);
        PlayerPrefs.SetFloat("r_stereo_pan", 0);
        PlayerPrefs.SetFloat("r_spatial_blend",0);
        PlayerPrefs.SetFloat("r_reverb_zone_mix",1);
        PlayerPrefs.SetInt("r_is_change", 0);
        this.r_is_change = false;
        this.load_data();
        this.update_val();
        this.update_val_slider();
        this.check_status_is_change();
    }

    private void check_status_is_change()
    {
        if (this.r_is_change)
            this.obj_img_change_status.SetActive(true);
        else

            this.obj_img_change_status.SetActive(false);
    }
}
