import mongoose from 'mongoose';

const { get_database, settingsSchema } = require('./mongo')

let proxies_capacitor = {};
let proxy_parameters = null;

/**
 * Returns the proxy with the lowest load
 * @param { string } name 
 */
export function get_lowest_load_proxy(name) {
    if (proxy_parameters == null) {
        const settingsModel = get_database().model('AppSettings', settingsSchema, 'app_settings');
        var settings = settingsModel.findById(0);

        proxy_parameters = {
            "host": `http://${settings.proxy_host}:${settings.proxy_port}`,
            "user": settings.proxy_username,
            "password": settings.proxy_password
        }
    }

    if (!name in proxies_capacitor)
        proxies_capacitor[name] = 0;

    proxies_capacitor[name] += 1;

    if (proxies_capacitor[name] > 100)
        proxies_capacitor[name] = 1;
    
    return {
        "user": `${proxy_parameters['user']}${proxies_capacitor[name]}`,
        "password": proxy_parameters['password'],
        "host": proxy_parameters['host']
    }
}