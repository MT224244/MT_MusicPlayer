using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT_MusicPlayer.Common
{
    public static class MessagingCenter
    {
        private static Dictionary<string, MessageAction> Messages = Messages ?? new Dictionary<string, MessageAction>();

        /// <summary>
        /// メッセージを送信します
        /// </summary>
        /// <typeparam name="TSender">送信クラス</typeparam>
        /// <param name="sender">送信クラス</param>
        /// <param name="message">メッセージ</param>
        public static void Send<TSender>(TSender sender, string message)
        {
            if (!Messages.Keys.Contains(message)) return;

            if (!(Messages[message].CallBack is Action<TSender>)) return;

            (Messages[message].CallBack as Action<TSender>)(sender);
        }

        /// <summary>
        /// メッセージを送信します
        /// </summary>
        /// <typeparam name="TSender">送信クラス</typeparam>
        /// <typeparam name="TArgs">引数</typeparam>
        /// <param name="sender">送信クラス</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">引数</param>
        public static void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
        {
            if (!Messages.Keys.Contains(message)) return;

            if (!(Messages[message].CallBack is Action<TSender, TArgs>)) return;

            (Messages[message].CallBack as Action<TSender, TArgs>)(sender, args);
        }

        /// <summary>
        /// メッセージを登録します
        /// </summary>
        /// <typeparam name="TSender">送信クラス</typeparam>
        /// <param name="subscriber">登録クラス</param>
        /// <param name="message">メッセージ</param>
        /// <param name="callback">コールバック関数</param>
        public static void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback)
        {
            if (!Messages.Keys.Contains(message)) Messages.Add(message, new MessageAction(subscriber, callback));
        }

        /// <summary>
        /// メッセージを登録します
        /// </summary>
        /// <typeparam name="TSender">送信クラス</typeparam>
        /// <typeparam name="TArgs">引数</typeparam>
        /// <param name="subscriber">登録クラス</param>
        /// <param name="message">メッセージ</param>
        /// <param name="callback">コールバック関数</param>
        public static void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback)
        {
            if (!Messages.Keys.Contains(message)) Messages.Add(message, new MessageAction(subscriber, callback));
        }

        /// <summary>
        /// メッセージを削除します
        /// </summary>
        /// <typeparam name="TSender">送信クラス</typeparam>
        /// <param name="subscriber">登録クラス</param>
        /// <param name="message">メッセージ</param>
        public static void Unsubscribe<TSender>(object subscriber, string message)
        {
            if (!Messages.Keys.Contains(message)) return;

            Messages.Remove(message);
        }

        /// <summary>
        /// メッセージを削除します
        /// </summary>
        /// <typeparam name="TSender">送信クラス</typeparam>
        /// <typeparam name="TArgs">引数</typeparam>
        /// <param name="subscriber">登録クラス</param>
        /// <param name="message">メッセージ</param>
        public static void Unsubscribe<TSender, TArgs>(object subscriber, string message)
        {
            if (!Messages.Keys.Contains(message)) return;

            Messages.Remove(message);
        }

        /// <summary>
        /// 登録クラスとコールバック関数を格納するクラス
        /// </summary>
        private class MessageAction
        {
            public object Subscriber { get; set; }
            public object CallBack { get; set; }

            public MessageAction(object subscriber, object callback)
            {
                Subscriber = subscriber;
                CallBack = callback;
            }
        }
    }
}
