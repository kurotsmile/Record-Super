using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Mix : MonoBehaviour
{
    public GameObject panel_audio_mix;
    public AudioReverbFilter audio_filter;
    public GameObject obj_img_change_status;
    private bool m_is_change = false;

    public Dropdown Dropdown_Reverb_Preset;
    private System.Collections.Generic.List<AudioReverbPreset> reverbPresets = new System.Collections.Generic.List<AudioReverbPreset>();

    [Header("Slider Contronl")]
    public Slider slider_Dry_Level;
    public Slider slider_Room;
    public Slider slider_Room_HF;
    public Slider slider_Room_LF;
    public Slider slider_Decay_Time;
    public Slider slider_Decay_HF_Ratio;
    public Slider slider_Reflections_Level;
    public Slider slider_Reflections_Delay;
    public Slider slider_Reverb_Level;
    public Slider slider_Reverb_Delay;
    public Slider slider_HF_Reference;
    public Slider slider_LF_Reference;
    public Slider slider_Diffusion;
    public Slider slider_Density;

    [Header("Value Contronl")]
    public Text val_Dry_Level;
    public Text val_Room;
    public Text val_Room_HF;
    public Text val_Room_LF;
    public Text val_Decay_Time;
    public Text val_Decay_HF_Ratio;
    public Text val_Reflections_Level;
    public Text val_Reflections_Delay;
    public Text val_Reverb_Level;
    public Text val_Reverb_Delay;
    public Text val_HF_Reference;
    public Text val_LF_Reference;
    public Text val_Diffusion;

    public void load()
    {
		System.Collections.Generic.List<Dropdown.OptionData> options = new System.Collections.Generic.List<Dropdown.OptionData>();

        foreach (AudioReverbPreset arp in System.Enum.GetValues(typeof(AudioReverbPreset)))
        {
            options.Add(new Dropdown.OptionData(arp.ToString()));
            reverbPresets.Add(arp);
        }

        this.Dropdown_Reverb_Preset.ClearOptions();
        this.Dropdown_Reverb_Preset.AddOptions(options);

        this.panel_audio_mix.SetActive(false);
        if (PlayerPrefs.GetInt("m_is_change", 0) == 1)
            this.m_is_change = true;
        else
            this.m_is_change = false;
        this.check_status_change();
        this.load_data_control();
    }

    public void btn_show_audio_mix()
    {
        this.GetComponent<App>().ads.Show_ads_Interstitial();
        this.panel_audio_mix.SetActive(true);
        this.update_value_control();
    }

    public void close()
    {
        this.panel_audio_mix.SetActive(false);
    }

    public void change_Dry_Leve()
    {
        this.audio_filter.dryLevel = this.slider_Dry_Level.value;
        PlayerPrefs.SetFloat("m_dryLevel",this.audio_filter.dryLevel);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_Room()
    {
        this.audio_filter.room = this.slider_Room.value;
        PlayerPrefs.SetFloat("m_room", this.audio_filter.room);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_roomHF()
    {
        this.audio_filter.roomHF = this.slider_Room_HF.value;
        PlayerPrefs.SetFloat("m_roomHF", this.audio_filter.roomHF);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_room_LF()
    {
        this.audio_filter.roomLF = this.slider_Room_LF.value;
        PlayerPrefs.SetFloat("m_roomLF", this.audio_filter.roomLF);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_decayTime()
    {
        this.audio_filter.decayTime = this.slider_Decay_Time.value;
        PlayerPrefs.SetFloat("m_decayTime", this.audio_filter.decayTime);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_decayHFRatio()
    {
        this.audio_filter.decayHFRatio = this.slider_Decay_HF_Ratio.value;
        PlayerPrefs.SetFloat("m_decayHFRatio", this.audio_filter.decayHFRatio);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_reflectionsLevel()
    {
        this.audio_filter.reflectionsLevel = this.slider_Reflections_Level.value;
        PlayerPrefs.SetFloat("m_reflectionsLevel", this.audio_filter.reflectionsLevel);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_reflectionsDelay()
    {
        this.audio_filter.reflectionsDelay = this.slider_Reflections_Delay.value;
        PlayerPrefs.SetFloat("m_reflectionsDelay", this.audio_filter.reflectionsDelay);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_reverbLevel()
    {
        this.audio_filter.reverbLevel = this.slider_Reverb_Level.value;
        PlayerPrefs.SetFloat("m_reverbLevel", this.audio_filter.reverbLevel);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_reverbDelay()
    {
        this.audio_filter.reverbDelay = this.slider_Reverb_Delay.value;
        PlayerPrefs.SetFloat("m_reverbDelay", this.audio_filter.reverbDelay);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_hfReference()
    {
        this.audio_filter.hfReference = this.slider_HF_Reference.value;
        PlayerPrefs.SetFloat("m_hfReference", this.audio_filter.hfReference);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_lfReference()
    {
        this.audio_filter.lfReference = this.slider_LF_Reference.value;
        PlayerPrefs.SetFloat("m_lfReference", this.audio_filter.lfReference);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_diffusion()
    {
        this.audio_filter.diffusion = this.slider_Diffusion.value;
        PlayerPrefs.SetFloat("m_diffusion", this.audio_filter.diffusion);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    public void change_density()
    {
        this.audio_filter.density = this.slider_Density.value;
        PlayerPrefs.SetFloat("m_density", this.audio_filter.density);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.update_value_control();
    }

    private void update_value_control()
    {
        this.val_Dry_Level.text = this.audio_filter.dryLevel.ToString();
        this.val_Room.text = this.audio_filter.room.ToString();
        this.val_Room_HF.text = this.audio_filter.roomHF.ToString();
        this.val_Room_LF.text = this.audio_filter.roomLF.ToString();
        this.val_Decay_Time.text = this.audio_filter.decayTime.ToString();
        this.val_Decay_HF_Ratio.text = this.audio_filter.decayHFRatio.ToString();
        this.val_Reflections_Level.text = this.audio_filter.reflectionsLevel.ToString();
        this.val_Reverb_Level.text = this.audio_filter.reverbLevel.ToString();
        this.val_Reverb_Delay.text = this.audio_filter.reverbDelay.ToString();
        this.val_HF_Reference.text = this.audio_filter.hfReference.ToString();
        this.val_LF_Reference.text  = this.audio_filter.lfReference.ToString();
        this.val_Diffusion.text = this.audio_filter.diffusion.ToString();
        this.Dropdown_Reverb_Preset.value = PlayerPrefs.GetInt("m_reverbPreset",0);
        this.Dropdown_Reverb_Preset.RefreshShownValue();
        this.check_status_change();
    }

    private void load_data_control()
    {
        this.audio_filter.reverbPreset = this.reverbPresets[PlayerPrefs.GetInt("m_reverbPreset",0)];
        this.audio_filter.dryLevel = PlayerPrefs.GetFloat("m_dryLevel",0f);
        this.audio_filter.room = PlayerPrefs.GetFloat("m_room", -1000f);
        this.audio_filter.roomHF = PlayerPrefs.GetFloat("m_roomHF", -100f);
        this.audio_filter.roomLF = PlayerPrefs.GetFloat("m_roomLF", 0f);
        this.audio_filter.decayTime = PlayerPrefs.GetFloat("m_decayTime", 1.49f);
        this.audio_filter.decayHFRatio = PlayerPrefs.GetFloat("m_decayHFRatio", 0.83f);
        this.audio_filter.reflectionsLevel = PlayerPrefs.GetFloat("m_reflectionsLevel", -2602f);
        this.audio_filter.reverbLevel = PlayerPrefs.GetFloat("m_reverbLevel", 200f);
        this.audio_filter.reverbDelay = PlayerPrefs.GetFloat("m_reverbDelay", 0.011f);
        this.audio_filter.hfReference = PlayerPrefs.GetFloat("m_hfReference", 5000f);
        this.audio_filter.lfReference = PlayerPrefs.GetFloat("m_lfReference", 250f);
        this.audio_filter.diffusion = PlayerPrefs.GetFloat("m_diffusion", 100f);
        this.audio_filter.density = PlayerPrefs.GetFloat("m_density", 100f);
    }

    public void reset()
    {
        PlayerPrefs.SetFloat("m_dryLevel", 0f);
        PlayerPrefs.SetFloat("m_room", -1000f);
        PlayerPrefs.SetFloat("m_roomHF", -100f);
        PlayerPrefs.SetFloat("m_roomLF", 0f);
        PlayerPrefs.SetFloat("m_decayTime", 1.49f);
        PlayerPrefs.SetFloat("m_decayHFRatio", 0.83f);
        PlayerPrefs.SetFloat("m_reflectionsLevel", -2602f);
        PlayerPrefs.SetFloat("m_reverbLevel", 200f);
        PlayerPrefs.SetFloat("m_reverbDelay", 0.011f);
        PlayerPrefs.SetFloat("m_hfReference", 5000f);
        PlayerPrefs.SetFloat("m_lfReference", 250f);
        PlayerPrefs.SetFloat("m_diffusion", 100f);
        PlayerPrefs.SetFloat("m_density", 100f);
        PlayerPrefs.SetInt("m_is_change",0);
        PlayerPrefs.SetInt("m_reverbPreset", 0);
        this.m_is_change = false;
        this.load_data_control();
        this.update_value_control();
    }

    private void check_status_change()
    {
        if (this.m_is_change)
            this.obj_img_change_status.SetActive(true);
        else
            this.obj_img_change_status.SetActive(false);
    }

    public void change_AudioReverbPreset()
    {
        this.audio_filter.reverbPreset = reverbPresets[this.Dropdown_Reverb_Preset.value];
        PlayerPrefs.SetInt("m_reverbPreset", this.Dropdown_Reverb_Preset.value);
        PlayerPrefs.SetInt("m_is_change", 1);
        this.m_is_change = true;
        this.check_status_change();
    }
}
