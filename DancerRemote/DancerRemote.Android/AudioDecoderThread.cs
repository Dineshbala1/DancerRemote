using Android.Media;
using Android.Util;
using Java.IO;
using Java.Lang;
using Java.Nio;

namespace DancerRemote.Droid
{
    public class AudioDecoderThread
    {
        private static int TIMEOUT_US = 1000;
        private MediaExtractor mExtractor;
        private MediaCodec mDecoder;

        private bool eosReceived;
        private int mSampleRate = 0;

        /**
         * 
         * @param filePath
         */
        public void startPlay(string path)
        {
            eosReceived = false;
            mExtractor = new MediaExtractor();
            try
            {
                mExtractor.SetDataSource(path);
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }

            int channel = 0;
            for (int i = 0; i < mExtractor.TrackCount; i++)
            {
                MediaFormat format = mExtractor.GetTrackFormat(i);
                string mime = format.GetString(MediaFormat.KeyMime);
                if (mime.StartsWith("audio/"))
                {
                    mExtractor.SelectTrack(i);
                    Log.Debug("TAG", "format : " + format);
                    ByteBuffer csd = format.GetByteBuffer("csd-0");

                    for (int k = 0; k < csd.Capacity(); ++k)
                    {
                        Log.Error("TAG", "csd : " + csd.ToArray<Byte>()[k]);
                    }
                    mSampleRate = format.GetInteger(MediaFormat.KeySampleRate);
                    channel = format.GetInteger(MediaFormat.KeyChannelCount);
                    break;
                }
            }
            MediaFormat format2 = makeAACCodecSpecificData(MediaCodecInfo.CodecProfileLevel.AACObjectLC, mSampleRate, channel);
            if (format2 == null)
                return;

            mDecoder = MediaCodec.createDecoderByType("audio/mp4a-latm");
            mDecoder.configure(format, null, null, 0);

            if (mDecoder == null)
            {
                Log.e("DecodeActivity", "Can't find video info!");
                return;
            }

            mDecoder.start();

            new Thread(AACDecoderAndPlayRunnable).start();
        }

        /**
         * The code profile, Sample rate, channel Count is used to
         * produce the AAC Codec SpecificData.
         * Android 4.4.2/frameworks/av/media/libstagefright/avc_utils.cpp refer
         * to the portion of the code written.
         * 
         * MPEG-4 Audio refer : http://wiki.multimedia.cx/index.php?title=MPEG-4_Audio#Audio_Specific_Config
         * 
         * @param audioProfile is MPEG-4 Audio Object Types
         * @param sampleRate
         * @param channelConfig
         * @return MediaFormat
         */
        private MediaFormat makeAACCodecSpecificData(int audioProfile, int sampleRate, int channelConfig)
        {
            MediaFormat format = new MediaFormat();
            format.setString(MediaFormat.KEY_MIME, "audio/mp4a-latm");
            format.setInteger(MediaFormat.KEY_SAMPLE_RATE, sampleRate);
            format.setInteger(MediaFormat.KEY_CHANNEL_COUNT, channelConfig);

            int samplingFreq[] = {
                96000, 88200, 64000, 48000, 44100, 32000, 24000, 22050,
                16000, 12000, 11025, 8000
            };

            // Search the Sampling Frequencies
            int sampleIndex = -1;
            for (int i = 0; i < samplingFreq.length; ++i)
            {
                if (samplingFreq[i] == sampleRate)
                {
                    Log.d("TAG", "kSamplingFreq " + samplingFreq[i] + " i : " + i);
                    sampleIndex = i;
                }
            }

            if (sampleIndex == -1)
            {
                return null;
            }

            ByteBuffer csd = ByteBuffer.allocate(2);
            csd.put((byte)((audioProfile << 3) | (sampleIndex >> 1)));

            csd.position(1);
            csd.put((byte)((byte)((sampleIndex << 7) & 0x80) | (channelConfig << 3)));
            csd.flip();
            format.setByteBuffer("csd-0", csd); // add csd-0

            for (int k = 0; k < csd.capacity(); ++k)
            {
                Log.e("TAG", "csd : " + csd.array()[k]);
            }

            return format;
        }

        Runnable AACDecoderAndPlayRunnable = new Runnable() {

            @Override

            public void run()
            {
            AACDecoderAndPlay();
        }
    };
}