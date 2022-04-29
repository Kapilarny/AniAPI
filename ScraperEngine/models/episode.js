export class Episode {

    /**
     * Episode Constructor
     * @param { string } title 
     * @param { string } path 
     * @param { string } url 
     * @param { number } quality 
     * @param { string } format 
     */
    Episode(title, path, url, quality, format) {
        this.title = title;
        this.path = path;
        this.url = url;
        this.quality = quality;
        this.format = format
    }

    /**
     * Returns all Episode Data
     * @returns { { title: string, path: string, url: string, quality: number, format: string } }
     */
    dump() {
        return {
            "title": this.title,
            "path": this.path,
            "url": this.url,
            "quality": this.quality,
            "format": this.format
        }
    }
}