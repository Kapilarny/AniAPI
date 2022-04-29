import mongoose from 'mongoose';
const { Schema } = mongoose

require('dotenv').config();

let db = null;

/**
 * Returns the database instance
 * @returns { mongoose }
 */
export function get_database() {
    if (db == null)
        db = mongoose.connect(process.env.MONGODB_URI);
    
    return db;
}

/**
 * Settings Schema
 */
export const settingsSchema = new Schema({
    jwt_secret: String,
    recaptcha_secret: String,
    proxy_host: String,
    proxy_port: String,
    proxy_username: String,
    proxy_password: String,
    proxy_count: Number,
    resources_version: String,
    api_endpoint: String,
    smtp: {
        host: String,
        port: Number,
        username: String,
        password: String,
        address: String
    },
    mal: {
        client_id: String,
        client_secret: String
    }
});

/**
 * Website Schema
 */
export const websiteSchema = new Schema({
    name: String,
    active: Boolean,
    official: Boolean,
    site_url: String,
    can_block_requests: Boolean,
    localization: String,
    creation_date: Date
});