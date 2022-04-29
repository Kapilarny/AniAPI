import { JSDOM, ResourceLoader, DOMWindow } from "jsdom";

import { get_lowest_load_proxy } from "./proxy"

/**
 * Sends the request to given url and returns a JSDOM element
 * @param { string } resName
 * @param { string } url 
 * @param { { referrer: string, runScripts: string } } options 
 * @returns { DOMWindow }
 */
export async function execute_proxied_request(resName, url, options = {}) {
    try {
        const sessionProxy = get_lowest_load_proxy(resName);
        const resourceLoader = new ResourceLoader({
            proxy: `http://${sessionProxy.host}:${sessionProxy.password}@${sessionProxy.host}}`,
            strictSSL: false
        });

        return (await JSDOM.fromURL(url, {
            resources: resourceLoader,
            referrer: options.referrer,
            runScripts: options.runScripts
        })).window;
    } catch (e) {
        console.error(e);
    }
}