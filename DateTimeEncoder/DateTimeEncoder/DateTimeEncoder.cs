﻿using System;
using System.Collections.Generic;
using System.Globalization;
using NeoCortexApi;
using NeoCortexApi.Encoders;

namespace DateTimeEncoderLib
{
    /// <summary>
    /// For the implemention of encoder for the datetime encoder, date encoder only and time encoder only by using DateTimeEncoder class by using abstract class of EncoderBase.
    /// </summary>
    public class DateTimeEncoder : EncoderBase 
    { 
        /// <summary>
        /// For the setting of radius and the width.
        /// </summary>
        /// <param name="settings"></param>
        public DateTimeEncoder(Dictionary<string, object> settings)
        {
            if (settings.TryGetValue("Radius", out object radius) && (double)radius > 0)
            {
                this.Radius = (double)radius;
            } 
            else 
            {
                this.Radius = 1;
            }

            if (settings.TryGetValue("W", out object width) && (int)width > 0)
            {
                this.W = (int)width;
            }
            else
            {
                this.W = 1;
            }
        }

        /// <summary>
        /// Implementation of only Date encoder.  
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public int[] EncodeDateOnly(object inputData)
        {
            DateTime inputDate = DateTime.Parse((String)inputData); 
            int[] monthArray = Encoding(inputDate.Month - 1, (int)(W * 12 / Radius));
            int[] dayArray = Encoding(inputDate.Day - 1, (int)(W * 31 / Radius));
            int[] yearArray = Encoding(inputDate.Year, (int)(W * 10 / Radius));
            
            int[] dateArray = new int[monthArray.Length + dayArray.Length + yearArray.Length];
            foreach (var item in dateArray)
            {
                dateArray[item] = 0;
            }
            monthArray.CopyTo(dateArray, 0);
            dayArray.CopyTo(dateArray, monthArray.Length);
            yearArray.CopyTo(dateArray, monthArray.Length + dayArray.Length);
            return dateArray;
        }

        /// <summary>
        ///  Implementation of only Time encoder.  
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public int[] EncodeTimeOnly(object inputData)
        {
            DateTime inputTime = DateTime.Parse((String)inputData);
            int[] hourArray = Encoding(inputTime.Hour, (int)(W * 24 / Radius));
            int[] minuteArray = Encoding(inputTime.Minute, (int)(W * 60 / Radius));
            int[] secondArray = Encoding(inputTime.Second, (int)(W * 60 / Radius));
            
            int[] timeArray = new int[hourArray.Length + minuteArray.Length + secondArray.Length];
            foreach (var item in timeArray)
            {
                timeArray[item] = 0;
            }
            hourArray.CopyTo(timeArray, 0);
            minuteArray.CopyTo(timeArray, hourArray.Length);
            secondArray.CopyTo(timeArray, hourArray.Length + minuteArray.Length);
            return timeArray;
        }

        /// <summary>
        /// Implementation of n = width * (radius / range) forumla. 
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public override int[] Encode(object inputData)
        {
            DateTime inputDateTime = DateTime.Parse((String)inputData, CultureInfo.InvariantCulture);
            int[] monthArray = Encoding(inputDateTime.Month - 1, (int)(W * 12 / Radius));
            int[] dayArray = Encoding(inputDateTime.Day - 1, (int)(W * 31 / Radius));
            int[] yearArray = Encoding(inputDateTime.Year, (int)(W * 10 / Radius));
            int[] hourArray = Encoding(inputDateTime.Hour, (int)(W * 24 / Radius));
            int[] minuteArray = Encoding(inputDateTime.Minute, (int)(W * 60 / Radius));
            int[] secondArray = Encoding(inputDateTime.Second, (int)(W * 60 / Radius));

            int[] outArray = new int[monthArray.Length + dayArray.Length + yearArray.Length + hourArray.Length + minuteArray.Length + secondArray.Length];
            foreach (var item in outArray)
            {
                outArray[item] = 0;
            }
            monthArray.CopyTo(outArray, 0);
            dayArray.CopyTo(outArray, monthArray.Length);
            yearArray.CopyTo(outArray, monthArray.Length + dayArray.Length);
            hourArray.CopyTo(outArray, monthArray.Length + dayArray.Length + yearArray.Length);
            minuteArray.CopyTo(outArray, monthArray.Length + dayArray.Length + yearArray.Length + hourArray.Length);
            secondArray.CopyTo(outArray, monthArray.Length + dayArray.Length + yearArray.Length + hourArray.Length + minuteArray.Length);
            return outArray;
        }

        /// <summary>
        /// Implemented logical condition for encoding bits of date and time. 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="numberOfBits"></param>
        /// <returns></returns>
        private int[] Encoding(int element, int numberOfBits)
        {
            int[] encoderArray = new int[numberOfBits];

            foreach (var item in encoderArray)
            {
                encoderArray[item] = 0;
            }

            for (int i = element * (W - (int)Radius + 1); i < element * (W - (int)Radius + 1) + W; i++)
            {
                int j = i % encoderArray.Length;
                encoderArray[j] = 1;
            }

            return encoderArray;
        }

        /// <summary>
        /// From InitTest method for the setting of radius and width.
        /// </summary>
        public DateTimeEncoder()
        {

        }
        
        /// <summary>
        /// Overriding the width of encoder.
        /// </summary>
        public override int Width => W;
        
        /// <summary>
        /// Overriding the IsDelta for the encoder. 
        /// </summary>
        public override bool IsDelta => throw new NotImplementedException();

        /// <summary>
        /// To get the bucket values in encoder. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override List<T> getBucketValues<T>()
        {
            throw new NotImplementedException();
        }
    }
}