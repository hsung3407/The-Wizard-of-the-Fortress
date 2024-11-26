﻿namespace Utility.SingleTon
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                return _instance = new T();
            }
        }
    }
}