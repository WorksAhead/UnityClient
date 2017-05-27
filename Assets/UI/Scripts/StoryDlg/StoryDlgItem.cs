using System;

namespace StoryDlg
{
    public class StoryDlgItem
    {
        public int Number = 0;
        public float IntervalTime = 1.0f;
        public int UnitId = -1;
        public string SpeakerName;
        public string ImageLeftAtlas;
        public string ImageLeft;
        public string ImageLeftBig;
        public string ImageLeftSmall;
        public string ImageRightAtlas;
        public string ImageRight;
        public string ImageRightBig;
        public string ImageRightSmall;
        public string Words;
        //以下用于动漫效果的UI（定义类型为BigDlgPanel）
        public string TextureAnimationPath;
        public string AnimationWords;
        //Pos
        public float FromOffsetLeft;
        public float FromOffsetBottom;
        public float ToOffsetLeft;
        public float ToOffsetBottom;
        public float TweenPosDelay;
        public float TweenPosDuration;
        //Scale
        public float FromScale;
        public float ToScale;
        public float TweenScaleDelay;
        public float TweenScaleDuration;
        //Alpha
        public float FromAlpha;
        public float ToAlpha;
        public float TweenAlphaDelay;
        public float TweenAlphaDuration;
        //WordDuration
        public float WordDuration;
    }
}
