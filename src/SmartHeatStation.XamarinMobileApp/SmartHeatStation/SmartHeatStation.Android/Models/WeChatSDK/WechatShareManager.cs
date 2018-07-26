
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Text;

using Com.Tencent.MM.Sdk.Modelmsg;
using Com.Tencent.MM.Sdk.Openapi;

namespace WeChatSDK.Droid
{
    public class WechatShareManager
    {
        private const int THUMB_SIZE = 150;
        public const int WECHAT_SHARE_TYPE_TALK = SendMessageToWX.Req.WXSceneSession;  //�Ự  
        public const int WECHAT_SHARE_TYPE_FRENDS = SendMessageToWX.Req.WXSceneTimeline; //����Ȧ  
        //public const string WECHAT_APP_ID = "Your WeChat APP_ID";

        IWXAPI mWXApi;
        Context mContext;

        public WechatShareManager(Context context,string yourWeChatAppId)
        {
            this.mContext = context;
            //��ʼ������  
            //��ʼ��΢�ŷ������  
            initWechatShare(context,yourWeChatAppId);
        }

        void initWechatShare(Context context, string yourWeChatAppId)
        {
            if (mWXApi == null)
            {
                mWXApi = WXAPIFactory.CreateWXAPI(context, yourWeChatAppId, true);
            }
            mWXApi.RegisterApp(yourWeChatAppId);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="shareContent">����arrayע��</param>
        /// <param name="isWXSceneTimeline">true:��������Ȧ;false:���������</param>
        /// <returns>��</returns>
        public void ShareText(string shareContent,bool isWXSceneTimeline)
        {
            if (!TextUtils.IsEmpty(shareContent))
            {
                WXTextObject textObj = new WXTextObject()
                {
                    Text = shareContent,
                };
                WXMediaMessage msg = new WXMediaMessage()
                {
                    MyMediaObject = textObj,
                    Description = shareContent,
                };
                SendMessageToWX.Req req = new SendMessageToWX.Req()
                {
                    Transaction = buildTransaction("text"),
                    Message = msg,
                    Scene = isWXSceneTimeline ? WECHAT_SHARE_TYPE_FRENDS
                        : WECHAT_SHARE_TYPE_TALK,
                };               
                mWXApi.SendReq(req);
            }
        }

        /// <summary>
        /// ����һ����ҳ
        /// </summary>
        /// <param name="httpUrl">��ҳ����</param>
        /// <param name="isWXSceneTimeline">true:��������Ȧ;false:���������</param>
        /// <param name="icon">������ʾ��ͼ��</param>
        /// <param name="title">���˿����ı���</param>
        /// <param name="description">���˿���������</param>
        /// <returns>��</returns>
        public void ShareWebPage(string httpUrl, bool isWXSceneTimeline, Bitmap icon, string title, string description)
        {
            WXWebpageObject webpage = new WXWebpageObject()
            {
                WebpageUrl = httpUrl,
            };
            WXMediaMessage msg = new WXMediaMessage(webpage)
            {
                Title = title,
                Description = description,
            };
            using (MemoryStream stream = new MemoryStream())
            {
                icon.Compress(Bitmap.CompressFormat.Png, 100, stream);
                msg.ThumbData = stream.ToArray();  //��������ͼ  
            }
            SendMessageToWX.Req req = new SendMessageToWX.Req()
            {
                Transaction = buildTransaction("webpage"),
                Message = msg,
                Scene = isWXSceneTimeline ? WECHAT_SHARE_TYPE_FRENDS
                               : WECHAT_SHARE_TYPE_TALK,
            };
            mWXApi.SendReq(req);
        }

        /// <summary>
        /// ����һ��ͼƬ
        /// </summary>
        /// <param name="shareBitmap">��ҳ����</param>
        /// <param name="isWXSceneTimeline">true:��������Ȧ;false:���������</param>
        /// <returns>��</returns>
        public void ShareImage(Bitmap shareBitmap, bool isWXSceneTimeline)
        {
            WXImageObject imgObj = new WXImageObject(shareBitmap);
            WXMediaMessage msg = new WXMediaMessage();
            msg.MyMediaObject = imgObj;

            Bitmap thumbBmp = Bitmap.CreateScaledBitmap(shareBitmap, THUMB_SIZE, THUMB_SIZE, true);
            using (MemoryStream stream = new MemoryStream())
            {
                shareBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                msg.ThumbData = stream.ToArray();  //��������ͼ  
            }
            SendMessageToWX.Req req = new SendMessageToWX.Req()
            {
                Transaction = buildTransaction("img"),
                Message = msg,
                Scene = isWXSceneTimeline ? WECHAT_SHARE_TYPE_FRENDS
                         : WECHAT_SHARE_TYPE_TALK,
            };
            mWXApi.SendReq(req);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="musicUrl">��������</param>
        /// <param name="isWXSceneTimeline">true:��������Ȧ;false:���������</param>
        /// <param name="showImgResId">������ʾ��ͼ��</param>
        /// <param name="title">���˿����ı���</param>
        /// <param name="description">���˿���������</param>
        /// <returns>��</returns>
        public void ShareMusic(string musicUrl, bool isWXSceneTimeline, 
            int showImgResId,string title, string description)
        {
            WXMusicObject musciObj = new WXMusicObject();
            musciObj.MusicUrl = musicUrl;
            WXMediaMessage msg = new WXMediaMessage();
            msg.MyMediaObject = musciObj;
            msg.Title = title;
            msg.Description = description;

            Bitmap thumbBmp = BitmapFactory.DecodeResource(mContext.Resources, showImgResId);
            using (MemoryStream stream = new MemoryStream())
            {
                thumbBmp.Compress(Bitmap.CompressFormat.Png, 100, stream);
                msg.ThumbData = stream.ToArray();  //��������ͼ  
            }

            SendMessageToWX.Req req = new SendMessageToWX.Req()
            {
                Transaction = buildTransaction("music"),
                Message = msg,
                Scene = isWXSceneTimeline ? WECHAT_SHARE_TYPE_FRENDS
                         : WECHAT_SHARE_TYPE_TALK,
            };
            mWXApi.SendReq(req);
        }

        /// <summary>
        /// ����һ����Ƶ
        /// </summary>
        /// <param name="videoUrl">��������</param>
        /// <param name="isWXSceneTimeline">true:��������Ȧ;false:���������</param>
        /// <param name="showImgResId">������ʾ��ͼ��</param>
        /// <param name="title">���˿����ı���</param>
        /// <param name="description">���˿���������</param>
        /// <returns>��</returns>
        public void ShareVideo(string videoUrl, bool isWXSceneTimeline,
            int showImgResId,string title, string description)
        {
            WXVideoObject musciObj = new WXVideoObject();
            musciObj.VideoUrl = videoUrl;
            WXMediaMessage msg = new WXMediaMessage();
            msg.MyMediaObject = musciObj;
            msg.Title = title;
            msg.Description = description;

            Bitmap thumbBmp = BitmapFactory.DecodeResource(mContext.Resources, showImgResId);
            using (MemoryStream stream = new MemoryStream())
            {
                thumbBmp.Compress(Bitmap.CompressFormat.Png, 100, stream);
                msg.ThumbData = stream.ToArray();  //��������ͼ  
            }

            SendMessageToWX.Req req = new SendMessageToWX.Req()
            {
                Transaction = buildTransaction("video"),
                Message = msg,
                Scene = isWXSceneTimeline ? WECHAT_SHARE_TYPE_FRENDS
                         : WECHAT_SHARE_TYPE_TALK,
            };
            mWXApi.SendReq(req);
        }

        private string buildTransaction(string type)
        {
            return (type == null) ? Java.Lang.String.ValueOf(Java.Lang.JavaSystem.CurrentTimeMillis()) : type + Java.Lang.JavaSystem.CurrentTimeMillis();
        }

    }
}