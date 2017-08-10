using NAudio.FileFormats.Mp3;
using NAudio.Wave;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Models
{
    public class SoundFile : ProjectResource
    {
        public enum Type
        {
            WAV,
            OGG,
            MP3,
            UNKNOWN,
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int Version { get; set; }

        public string SourcePath { get; set; }

        public byte[] Content { get; set; }

        public Type FileType { get; set; }

        public static Type GetFileType(string FileName)
        {
            string type = Path.GetExtension(FileName);

            Type FileType = Type.UNKNOWN;
            if (type.Equals(".wav", StringComparison.CurrentCultureIgnoreCase)) FileType = Type.WAV;
            else if (type.Equals(".ogg", StringComparison.CurrentCultureIgnoreCase)) FileType = Type.OGG;
            else if (type.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase)) FileType = Type.MP3;

            return FileType;
        }

        public void Reload()
        {
            if (File.Exists(SourcePath))
            {
                FileType = GetFileType(SourcePath);
                if(FileType == Type.UNKNOWN)
                {
                    Global.Log.AddEntry("Unknown File Type.");
                } else
                {
                    Content = File.ReadAllBytes(SourcePath);
                    Global.Log.AddEntry("Source " + SourcePath + " reloaded.");
                }
            }
            else
            {
                Global.Log.AddEntry("Source " + SourcePath + " cannot be found on this system.");
            }
        }

        public void Replace(string FileName)
        {
            FileType = GetFileType(SourcePath);
            if(FileType == Type.UNKNOWN)
            {
                Global.Log.AddEntry("Unknown File Type");
            } else
            {
                SourcePath = FileName;
                Content = File.ReadAllBytes(SourcePath);
                Global.Log.AddEntry("Source replaced with " + SourcePath);
            }
        }

        public String Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void SendToClient(string clientID)
        {
            Global.Sender.SendSoundFile(Global.ProjectManager.Current.ProjectID(), clientID, ID, Serialize());
        }

        public BlockAlignReductionStream GetStream()
        {
            MemoryStream memStream = new MemoryStream(Content);

            WaveStream wStream = null;

            switch(FileType)
            {
                case Type.MP3:
                    {
                        Mp3FileReader reader = new Mp3FileReader(memStream, wave=> new DmoMp3FrameDecompressor(wave));
                        wStream = WaveFormatConversionStream.CreatePcmStream(reader);
                        break;
                    }
                case Type.OGG:
                    {
                        NAudio.Vorbis.VorbisWaveReader reader = new NAudio.Vorbis.VorbisWaveReader(memStream);
                        wStream = WaveFormatConversionStream.CreatePcmStream(reader);
                        break;
                    }
                case Type.WAV:
                    {
                        WaveFileReader reader = new WaveFileReader(memStream);
                        wStream = WaveFormatConversionStream.CreatePcmStream(reader);
                        break;
                    }
                case Type.UNKNOWN:
                    {
                        return null;
                    }
            }

            return new BlockAlignReductionStream(wStream);
        }
    }
}
