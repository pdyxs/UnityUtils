using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Networking
{
    public static class JSONRequest
    {
        public static UnityWebRequest Post(string url, object data) {
            var www = UnityWebRequest.Put(url, JsonUtility.ToJson(data));
            www.chunkedTransfer = false;
            www.useHttpContinue = false;
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");
            return www;
        }
    }
}