export class Song {
    
    /**
     * Song Constructor
     * @param { string } id 
     * @param { string } type 
     */
    Song(id, type) {
        this.id = id;
        this.type = type;
    }

    /**
     * Returns all Song data
     * @returns { { id: string, type: string } }
     */
    dump() {
        return {
            "id": this.id,
            "type": this.type
        }
    }
}