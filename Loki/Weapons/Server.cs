﻿using System.Linq;
using System.Net;
using Loki.Configuration;

namespace Loki.Weapons {
    //https://gist.github.com/joeandaverde/3994603
    static class Server {
        internal static void Go() {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost/");
            listener.Start();

            while (true) {
                try {
                    var ctx = listener.GetContext();
                    var orig = ctx.Request.Url.LocalPath;

                    var index = orig.IndexOf('/', 1);
                    orig = orig.Substring(index + 1, orig.Length - index - 1);
                    
                    var res = ConfigManager.Settings.Responses.SingleOrDefault(r => r.Url == orig);
                    if (res == null) {
                        ctx.Response.StatusCode = 404;
                        ctx.Response.Close();
                        continue;
                    }
                    
                    res.ProcessResponse(ctx.Response);
                    
                    ctx.Response.StatusCode = 200;
                    ctx.Response.Close();
                }
                catch { /* some error happened that we dont care about */ }
            }
        }
    }
}