using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Yoyo.AzureAi.AzureCognitive
{
    public class ReadPCMStream : PullAudioInputStreamCallback
    {
        private Stream stream;
        private long streamLength = 0;
        private long nextStartPostion = 0;
        // 是否自动释放流
        private bool autoDispose;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">音频流</param>
        /// <param name="autoDispose">自动释放音频流，默认为true</param>
        public ReadPCMStream(Stream stream, bool autoDispose = true)
        {
            this.stream = stream;
            this.streamLength = stream.Length;
            this.autoDispose = autoDispose;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">音频路径</param>
        /// <param name="autoDispose">自动释放音频流，默认为true</param>
        public ReadPCMStream(string filePath, bool autoDispose = true)
            :this(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), autoDispose)
        {
           
        }


        public override int Read(byte[] dataBuffer, uint size)
        {
            // 已经读完了，返回0
            if (nextStartPostion >= streamLength)
            {
                this.Close();
                return 0;
            }

            // 已读的数据减去未读的数据，表示剩余的数据长度
            var remaining = streamLength - nextStartPostion;
            // 本次可读的数据量,默认等于剩余的数据长度
            var readLength = (int)remaining;
            // 如果剩余的量大于字节数组的长度，那么本次的读取量为数组长度
            if (remaining > size)
            {
                readLength = (int)size;
            }


            // 设置流开始的位置
            stream.Seek(nextStartPostion, SeekOrigin.Begin);
            // 将流数据填充到字节数组
            stream.Read(dataBuffer, 0, readLength);
            // 累计总共读取的量
            nextStartPostion += readLength;



            return readLength;
        }


        public override void Close()
        {
            // 释放
            if (this.autoDispose)
            {
                stream?.Dispose();
            }

            base.Close();
        }
    }
}
