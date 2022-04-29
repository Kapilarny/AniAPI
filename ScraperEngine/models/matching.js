export class Matching {

    /**
     * Matching Constructor
     * @param { string } title 
     * @param { string } path 
     */
    Matching(title, path) {
        this.title = title;
        this.path = path;
    }

    /**
     * Returns all Matching Data
     * @returns { { title: string, path: string } }
     */
    dump() {
        return {
            "title": this.title,
            "path": this.path
        }
    }
}