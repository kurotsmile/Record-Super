using System;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Data_Records : MonoBehaviour
{
    [Header("Main Obj")]
    public App app;

    [Header("Data Obj")]
    public Sprite sp_icon_list;
    public Sprite sp_icon_trim;

    public Sprite sp_icon_save;
    public Sprite sp_icon_cassette;

    private int length_record;
    private int index_record_del = -1;
    private int index_record_trim = -1;
    private int index_audio_play = -1;
    private Carrot.Carrot_Box box_list;
    private Carrot.Carrot carrot;
    private Carrot.Carrot_Window_Msg msg_question_del;

    public void load(Carrot.Carrot carrot)
    {
        this.carrot = carrot;
        this.length_record = PlayerPrefs.GetInt("length_record",0);
    }

    public void show_list_record()
    {
        if (this.box_list != null) this.box_list.close();
        this.box_list = this.carrot.Create_Box("List Recording", this.sp_icon_list);

        for (int i = this.length_record-1; i >=0; i--)
        {
            if (PlayerPrefs.GetString("record_name_" + i) == "") continue;

            var index_data_audio = i;
            string s_tip = this.GetComponent<App>().FormatTime(PlayerPrefs.GetFloat("record_length_time_" + i));
            s_tip = s_tip + " - " + PlayerPrefs.GetString("record_date_"+i);

            string s_path=PlayerPrefs.GetString("record_path_"+i);

            Carrot.Carrot_Box_Item recording = this.box_list.create_item();
            recording.set_title(PlayerPrefs.GetString("record_name_"+i));
            recording.set_tip(s_tip);
            recording.set_icon(this.sp_icon_cassette);
            recording.set_act(() => play_audio_select(index_data_audio));

            Carrot.Carrot_Box_Btn_Item btn_save = recording.create_item();
            btn_save.set_icon(this.sp_icon_save);
            btn_save.set_color(this.carrot.color_highlight);
            btn_save.set_act(()=>{
                this.app.file.Set_filter(Carrot.Carrot_File_Data.AudioData);
                this.app.file.Save_file((spaths)=>{
                    string s_path_new=spaths[0];
                    File.Copy(s_path,s_path_new, true);
                    this.app.carrot.Show_msg("Save","File saved to "+s_path_new+" path successfully!",Carrot.Msg_Icon.Success);
                });
            });

            Carrot.Carrot_Box_Btn_Item btn_audio_del = recording.create_item();
            btn_audio_del.set_icon(this.carrot.sp_icon_del_data);
            btn_audio_del.set_color(this.carrot.color_highlight);
            btn_audio_del.set_act(()=>this.delete_record(index_data_audio));
        }
        this.box_list.update_color_table_row();
    }

    private void play_audio_select(int index_audio)
    {
        this.index_audio_play = index_audio;
        this.GetComponent<App>().txt_name_record.text = PlayerPrefs.GetString("record_name_"+ index_audio);
        StartCoroutine(load_file_audio(PlayerPrefs.GetString("record_path_" + index_audio)));
        this.box_list.close();
    }

    public void btn_audio_next()
    {
        this.index_audio_play++;
        if (this.index_audio_play >= this.length_record) this.index_audio_play = 0;
        this.GetComponent<App>().txt_name_record.text = PlayerPrefs.GetString("record_name_" + this.index_audio_play);
        StartCoroutine(load_file_audio(PlayerPrefs.GetString("record_path_" + this.index_audio_play)));
    }

    public void btn_audio_prev()
    {
        this.index_audio_play--;
        if (this.index_audio_play <0) this.index_audio_play = this.length_record-1;
        this.GetComponent<App>().txt_name_record.text = PlayerPrefs.GetString("record_name_" + this.index_audio_play);
        StartCoroutine(load_file_audio(PlayerPrefs.GetString("record_path_" + this.index_audio_play)));
    }

    IEnumerator load_file_audio(string s_url_audio)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(s_url_audio, AudioType.WAV))
        {
            www.SendWebRequest();
            while (!www.isDone) yield return null;

            if (www.result== UnityWebRequest.Result.Success)
            {
                AudioClip data_clip = DownloadHandlerAudioClip.GetContent(www);
                this.GetComponent<App>().play_player_audio(data_clip);
            }
        }
    }

    IEnumerator load_file_audio_to_trim(string s_url_audio)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(s_url_audio, AudioType.WAV))
        {
            www.SendWebRequest();
            while (!www.isDone) yield return null;

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip data_clip = DownloadHandlerAudioClip.GetContent(www);
                data_clip=SavWav.TrimSilence(data_clip, 0);
                this.GetComponent<App>().play_player_audio(data_clip);
                this.box_list.close();
            }
        }
    }

    public void add_record(string s_name,string s_path_audio,float length_timer)
    {
        PlayerPrefs.SetString("record_name_" + this.length_record, s_name);
        PlayerPrefs.SetString("record_path_" + this.length_record, s_path_audio);
        PlayerPrefs.SetFloat("record_length_time_" + this.length_record, length_timer);
        PlayerPrefs.SetString("record_date_" + this.length_record, DateTime.Now.Date.ToString("f"));

        this.length_record++;
        PlayerPrefs.SetInt("length_record", this.length_record);
    }

    public void delete_record(int index_record)
    {
        this.index_record_del = index_record;
        this.msg_question_del=this.carrot.Show_msg("List Recording",PlayerPrefs.GetString("del_tip", "Are you sure delete this selected item?"), del_yes, del_no);
    }

    public void trim_record(int index_record)
    {
        this.index_record_trim = index_record;
        StartCoroutine(load_file_audio_to_trim(PlayerPrefs.GetString("record_path_" + index_record)));
    }

    private void del_yes()
    {
        PlayerPrefs.DeleteKey("record_name_" + this.index_record_del);
        PlayerPrefs.DeleteKey("record_path_" + this.index_record_del);
        PlayerPrefs.DeleteKey("record_length_time_" + this.index_record_del);
        PlayerPrefs.DeleteKey("record_date_" + this.index_record_del);
        this.msg_question_del.close();
        this.show_list_record();
    }

    private void del_no()
    {
        this.msg_question_del.close();
    }

    public int get_length()
    {
        return this.length_record;
    }

    public void Btn_open_file(){
        this.app.carrot.play_sound_click();
        this.app.file.Set_filter(Carrot.Carrot_File_Data.AudioData);
        this.app.file.Open_file((s_paths)=>{
            string s_p=s_paths[0];
            string file_name = System.IO.Path.GetFileNameWithoutExtension(s_p);
            byte[] audioData=FileBrowserHelpers.ReadBytesFromFile(s_p);
            AudioClip audioClip = AudioClip.Create("TempAudio", audioData.Length / 2, 2, 44100, false);
            float[] audioFloatData = ConvertToFloatArray(audioData);
            audioClip.SetData(audioFloatData, 0);
            this.add_record(file_name,s_p,audioClip.length);
            this.app.carrot.delay_function(1f,()=>{
                this.show_list_record();
            });
        });
    }

    private  float[] ConvertToFloatArray(byte[] byteData)
    {
        int sampleCount = byteData.Length / 2;
        float[] floatData = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            short sample = System.BitConverter.ToInt16(byteData, i * 2);
            floatData[i] = sample / 32768.0f; 
        }
        return floatData;
    }
}
