// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

const cacheNamePrefix = 'offline-cache-';

export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}

export function cacheDataFromPath(path, forceUpdate) {
    /*const cacheKeys = await caches.keys();
    const keys = cacheKeys.filter(key => key.startsWith(cacheNamePrefix));
    if (keys.length == 0) {
        // No cache
    }
    else {
        const key = keys[0];
        const cache = await caches.open(key);
        var req = new Request(path);
        if (forceUpdate || (!cache.match(req))) {
            await cache.add(req);
        }
    }*/
}

export function loadScript(src, tagAttr) {
    var scs = document.getElementsByTagName("script");
    for (var i = 0; i < scs.length; i++) {
        if (scs[i].hasAttribute(tagAttr)) {
            if (scs[i].getAttribute(tagAttr) == src)
                return;
        }
    }

    return new Promise((resolve, reject) => {
        var script = document.createElement('script');
        script.src = src;
        script.type = "text/javascript";

        script.onload = function () {
            resolve()
        }
        script.onerror = function (error) {
            reject(error)
        }
        script.setAttribute(tagAttr, src);

        document.body.appendChild(script);
    });
}

export function loadStyleSheet(href, tagAttr) {
    var lks = document.getElementsByTagName("link");
    for (var i = 0; i < lks.length; i++) {
        if (lks[i].hasAttribute(tagAttr)) {
            if (lks[i].getAttribute(tagAttr) == href)
                return;
        }
    }

    var link = document.createElement('link');
    link.rel = "stylesheet";
    link.type = "text/css";
    link.href = href;
    link.setAttribute(tagAttr, href);
    document.head.appendChild(link);
}