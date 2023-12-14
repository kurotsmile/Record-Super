using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
	[Header("Obj App")]
	public Carrot.Carrot carrot;
	public GameObject obj_btn_play;
	public Data_Records data_record;
	public int timer_second_record = 60;

	[Header("Emp Scene Roation")]
	public Transform tr_record_info;
	public Transform tr_record_control;

	[Header("Area Scene Roation")]
	public Transform tr_area_portrait_top;
	public Transform tr_area_portrait_bottom;
	public Transform tr_area_landspace_left;
	public Transform tr_area_landspace_right;

	[Header("Panel Main")]
	public Sprite sp_icon_recording;
	public Sprite sp_icon_pause;
	public Sprite sp_icon_play_audio;
	public Sprite sp_icon_pause_audio;
	public Sprite sp_icon_list_record;
	public Sprite sp_icon_list_device;
	public Sprite sp_icon_maximum_duration;
	public Sprite sp_icon_audio_channels;
    public Sprite sp_icon_audio_stereo;

    public GameObject panel_control_record;
	public GameObject panel_control_play;

	[Header("Ui Recording")]
	public RawImage rawImg_wave;
	public Color wavebackgroundColor = Color.black;
	public AudioSource myAudioRecord;
	public AudioSource myAudioPlay;
	public Image img_status_record;
	public Image img_status_play;
	public Text txt_time_record;
	public Text txt_name_device;
	public Text txt_name_record;
	public Text txt_time_length_record;
	public Slider slider_timer_audio;

	[Header("Setting")]
	public AudioSource audio_setting_test;
	private Carrot.Carrot_Box box_audio_channels;
	private Carrot.Carrot_Box_Item item_channels_left_right;

    private bool is_record=false;
	private bool is_playing_record = false;

	private int width = 500;
	private int height = 100;
	private Color waveformColor = Color.red;
	private int size = 2048;
	private Carrot.Carrot_Box box_devices;
	private Carrot.Carrot_Box box_timer_record;
	private string s_cur_device_name;

    private void Start()
    {
		this.carrot.Load_Carrot(this.check_exit_app);
		this.obj_btn_play.SetActive(false);
		this.check_record_device();
		this.panel_control_play.SetActive(false);
		this.panel_control_record.SetActive(true);
		this.slider_timer_audio.gameObject.SetActive(false);

		this.data_record.load(this.carrot);
		this.txt_name_record.text = "New Recording";

		this.timer_second_record = PlayerPrefs.GetInt("timer_second_record",300);
		this.txt_time_length_record.text = FormatTime(PlayerPrefs.GetInt("timer_second_record"));

		this.GetComponent<Audio_Mix>().load();
		this.GetComponent<Audio_Pitch>().load();
		this.change_scene_rotation();
    }

	private void check_record_device()
    {
        if (Microphone.devices.Length > 0)
        {
			this.s_cur_device_name = Microphone.devices[0];
			this.txt_name_device.text= Microphone.devices[0];
		}
        else
        {
			this.carrot.show_msg("No recording device found!");
        }
    }

	public void change_scene_rotation()
    {
		this.carrot.delay_function(1.5f, check_scene_rotation);
    }

	private void check_scene_rotation()
    {
		bool is_rotaion = this.GetComponent<Carrot.Carrot_DeviceOrientationChange>().get_status_portrait();

		if (is_rotaion)
        {
			Debug.Log("Man hinh doc");
			this.tr_record_info.SetParent(this.tr_area_portrait_top);
			this.tr_record_control.SetParent(this.tr_area_portrait_bottom);
			this.resize_rectangre(this.tr_record_info);
			this.resize_rectangre(this.tr_record_control);
		}
        else
        {
			Debug.Log("Man hinh ngan");
			this.tr_record_info.SetParent(this.tr_area_landspace_left);
			this.tr_record_control.SetParent(this.tr_area_landspace_right);
			this.resize_rectangre(this.tr_record_info);
			this.resize_rectangre(this.tr_record_control);
		}
	}

	private void resize_rectangre(Transform tr_area)
    {
		RectTransform r_tr = tr_area.GetComponent<RectTransform>();
		r_tr.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
		r_tr.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
		r_tr.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
		r_tr.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
		r_tr.anchorMin = new Vector2(0f, 0f);
		r_tr.anchorMax = new Vector2(1f, 1f);
	}

	private void check_exit_app()
    {
        if (this.GetComponent<Audio_Mix>().panel_audio_mix.activeInHierarchy)
        {
			this.GetComponent<Audio_Mix>().close();
			this.carrot.set_no_check_exit_app();
		}else if (this.GetComponent<Audio_Pitch>().panel_audio_pitch.activeInHierarchy)
        {
			this.GetComponent<Audio_Pitch>().close();
			this.carrot.set_no_check_exit_app();
		}
    }

    public void voice_record()
	{
		if (this.is_record==false)
			this.act_statrt_record();
		else
			this.act_stop_record();
	}

	private void act_statrt_record()
    {
		this.txt_name_record.text = "Recording " + (this.data_record.get_length() + 1);
		this.txt_time_length_record.text =FormatTime(PlayerPrefs.GetInt("timer_second_record"));
		this.rawImg_wave.color = Color.white;
		myAudioRecord.clip = Microphone.Start(this.s_cur_device_name, false, timer_second_record, 44100);
		while (!(Microphone.GetPosition(Microphone.devices[0]) > 0)) { };
		this.myAudioRecord.Play();
		this.myAudioRecord.mute = true;
		this.Start_img_ware();
		this.waveformColor = Color.red;
		this.is_record = true;
		this.is_playing_record = false;
		this.myAudioPlay.Stop();
		this.img_status_record.sprite = this.sp_icon_pause;
		this.obj_btn_play.SetActive(false);
		this.slider_timer_audio.gameObject.SetActive(false);
	}

	private void act_stop_record()
    {
		string path_audio_wab = SavWav.Save_in_path_app(this.txt_name_record.text + ".wav", this.myAudioRecord.clip);
		this.data_record.add_record(this.txt_name_record.text, path_audio_wab, this.myAudioRecord.clip.length);
		this.myAudioRecord.Pause();
		this.waveformColor = Color.red;
		this.myAudioRecord.mute = false;
		this.myAudioPlay.clip = this.myAudioRecord.clip;
		this.myAudioRecord.clip = null;
		this.is_record = false;
		this.img_status_record.sprite = this.sp_icon_recording;
		this.obj_btn_play.SetActive(true);
		Microphone.End(this.s_cur_device_name);
		this.carrot.play_vibrate();
	}

	public static AudioClip Combine(List<AudioClip> clips)
	{
		if (clips == null || clips.Count == 0)
			return null;

		int length = 0;
		for (int i = 0; i < clips.Count; i++)
		{
			if (clips[i] == null)
				continue;

			length += clips[i].samples * clips[i].channels;
		}

		float[] data = new float[length];
		length = 0;
		for (int i = 0; i < clips.Count; i++)
		{
			if (clips[i] == null)
				continue;

			float[] buffer = new float[clips[i].samples * clips[i].channels];
			clips[i].GetData(buffer, 0);
			buffer.CopyTo(data, length);
			length += buffer.Length;
		}

		if (length == 0)
			return null;

		AudioClip result = AudioClip.Create("Combine", length / 2, 2, 44100, false, false);
		result.SetData(data, 0);

		return result;
	}

	void Update()
    {
		if (this.is_record)
		{
			this.txt_time_record.text = string.Format("{0}:{1:00}", (int)this.myAudioRecord.time / 60, (int)this.myAudioRecord.time % 60);
			texture.SetPixels(blank, 0);
			this.myAudioRecord.GetOutputData(samples, 0);

			for (int i = 0; i < size; i++)
			{
				if (this.myAudioRecord.mute)
					texture.SetPixel((int)(width * i / size), (int)(height * (UnityEngine.Random.Range(0.0f, 10f) + 1f) / 2f), waveformColor);
				else
					texture.SetPixel((int)(width * i / size), (int)(height * (samples[i] + 1f) / 2f), waveformColor);
			}

			texture.Apply();

            if (!this.myAudioRecord.isPlaying)
            {
				this.act_stop_record();
				this.act_statrt_record();
			}
		}

        if (this.is_playing_record)
        {
			this.txt_time_record.text =this.FormatTime(this.myAudioPlay.time);
			this.slider_timer_audio.value = this.myAudioPlay.time;
			texture.SetPixels(blank, 0);
			this.myAudioPlay.GetOutputData(samples, 0);
			for (int i = 0; i < size; i++) texture.SetPixel((int)(width * i / size), (int)(height * (samples[i] + 1f) / 2f), waveformColor);
			texture.Apply();
			                            
            if (!this.myAudioPlay.isPlaying)
            {
				this.myAudioPlay.Stop();
				this.is_playing_record = false;
				this.img_status_play.sprite = this.sp_icon_play_audio;
            }
		}
	}

	private Color[] blank;
	private Texture2D texture;
	private float[] samples;

	public void Start_img_ware()
	{
		samples = new float[size];
		texture = new Texture2D(width, height);

		this.rawImg_wave.texture = texture;
		blank = new Color[width * height];

		for (int i = 0; i < blank.Length; i++) blank[i] = this.wavebackgroundColor;
	}

	public void btn_audio_play_record()
    {
        if (this.is_playing_record == false)
        {
			this.img_status_play.sprite = this.sp_icon_pause_audio;
			this.is_playing_record = true;
			this.myAudioPlay.Play();
        }
        else
        {
			this.img_status_play.sprite = this.sp_icon_play_audio;
			this.is_playing_record = false;
			this.myAudioPlay.Stop();
		}
    }

	public void btn_show_list_record()
    {
		this.carrot.ads.show_ads_Interstitial();
		this.data_record.show_list_record();
	}

	public void play_player_audio(AudioClip clip_audio)
    {
		this.carrot.ads.show_ads_Interstitial();
		this.Start_img_ware();
		this.panel_control_play.SetActive(true);
		this.panel_control_record.SetActive(false);

		this.slider_timer_audio.gameObject.SetActive(true);
		this.is_playing_record = true;
		this.myAudioPlay.clip= clip_audio;
		this.slider_timer_audio.value = 0;
		this.slider_timer_audio.maxValue = clip_audio.length;

		this.txt_time_length_record.text = FormatTime(clip_audio.length);
		this.myAudioPlay.Play();
	}

	public void btn_show_list_device()
    {
		this.carrot.ads.show_ads_Interstitial();
		this.box_devices= this.carrot.Create_Box("List Devices", this.sp_icon_list_device);
		string[] s_name_devices=Microphone.devices;

		for(int i = 0; i < s_name_devices.Length; i++)
        {
			var s_name_device = s_name_devices[i];
			Carrot.Carrot_Box_Item device_item = box_devices.create_item();
			device_item.set_title(s_name_devices[i]);
			device_item.set_tip("Recording Device");
			device_item.set_icon(this.sp_icon_recording);
			device_item.set_act(()=>this.select_device_record(s_name_device));
		}
	}

	private void select_device_record(string s_name_device_record)
    {
		this.s_cur_device_name = s_name_device_record;
		this.txt_name_device.text = s_name_device_record;
		this.box_devices.close();
    }

	public void btn_show_setting()
    {
		this.carrot.ads.show_ads_Interstitial();
		Carrot.Carrot_Box box_setting = this.carrot.Create_Setting();

        Carrot.Carrot_Box_Item test_audio_channels = box_setting.create_item_of_top();
        test_audio_channels.set_title("Check audio channels");
        test_audio_channels.set_tip("Check the audio channels on your device");
        test_audio_channels.set_icon(this.sp_icon_audio_channels);
        test_audio_channels.set_act(check_audio_channels);

        Carrot.Carrot_Box_Item setting_time_record = box_setting.create_item_of_top();
		setting_time_record.set_title("Recorder maximum duration");
		setting_time_record.set_tip("Change recorder duration");
		setting_time_record.set_icon(this.sp_icon_maximum_duration);
		setting_time_record.set_act(change_timer_record);

		box_setting.set_act_before_closing(this.close_setting);
    }

	private void close_setting()
	{
		if (this.box_audio_channels != null) this.box_audio_channels.close();
		this.audio_setting_test.Stop();
	}

	private void check_audio_channels()
	{
		if (this.box_audio_channels != null) this.box_audio_channels.close();

		this.box_audio_channels = this.carrot.Create_Box("check_audio_channels");
		box_audio_channels.set_title("Check audio channels");
        box_audio_channels.set_icon(this.sp_icon_audio_channels);

        this.item_channels_left_right=box_audio_channels.create_item("left_right");
        this.item_channels_left_right.set_title("Adjust left and right");
		this.item_channels_left_right.set_icon(this.sp_icon_audio_stereo);
        this.item_channels_left_right.set_type(Carrot.Box_Item_Type.box_value_slider);
        this.item_channels_left_right.set_val("0");
        this.item_channels_left_right.check_type();
        this.item_channels_left_right.set_tip("Check that the speakers play the left and right audio channels correctly");

        this.item_channels_left_right.slider_val.maxValue = 1f;
        this.item_channels_left_right.slider_val.minValue = -1f;
        this.item_channels_left_right.slider_val.value = 0f;


        this.item_channels_left_right.slider_val.onValueChanged.RemoveAllListeners();
		this.item_channels_left_right.slider_val.onValueChanged.AddListener(change_slider_audio_channels);

        this.audio_setting_test.Play();
		box_audio_channels.set_act_before_closing(this.close_check_audio_channels);
	}

    private void close_check_audio_channels()
	{
		this.audio_setting_test.Stop();
    }
	private void change_slider_audio_channels(float f_val)
	{
		this.audio_setting_test.panStereo = this.item_channels_left_right.slider_val.value;
        this.item_channels_left_right.txt_tip.text = this.item_channels_left_right.slider_val.value.ToString();
    }

    private void change_timer_record()
    {
		this.carrot.ads.show_ads_Interstitial();
		this.box_timer_record = this.carrot.Create_Box();
		box_timer_record.set_icon(this.sp_icon_maximum_duration);
		box_timer_record.set_title("Recorder maximum duration");

        for(int i = 10; i <500; i += 10)
        {
			var length_timer = i;
			Carrot.Carrot_Box_Item btn_limit_timer_record_10s = box_timer_record.create_item();
			btn_limit_timer_record_10s.set_icon(this.sp_icon_maximum_duration);
			btn_limit_timer_record_10s.set_title("Maximum duration "+i+" seconds");
			btn_limit_timer_record_10s.set_tip("Change the recording time limit");
			btn_limit_timer_record_10s.set_act(() => this.set_time_maximum(length_timer));
			if (this.timer_second_record == i) btn_limit_timer_record_10s.GetComponent<Image>().color = Color.yellow;
		}
	}

	public void set_time_maximum(int timer)
    {
		this.timer_second_record = timer;
		this.box_timer_record.close();
		PlayerPrefs.SetInt("timer_second_record",timer);
	}

	public void btn_show_control_record()
	{
		this.carrot.play_sound_click();
		this.panel_control_record.SetActive(true);
		this.panel_control_play.SetActive(false);
		this.slider_timer_audio.gameObject.SetActive(false);
	}

	public void btn_show_control_play()
    {
		this.carrot.play_sound_click();
		this.panel_control_record.SetActive(false);
		this.panel_control_play.SetActive(true);
    }

	public string FormatTime(float time)
	{
		int minutes = (int)time / 60;
		int seconds = (int)time - 60 * minutes;
		int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
		return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
	}

	public void play_audio_record_curent()
    {
		this.btn_show_control_play();
		this.play_player_audio(this.myAudioPlay.clip);
    }
}
