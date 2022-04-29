const { Express } = require('express')
const { get_database, websiteSchema } = require('../utils/mongo')

class ScraperResource {
    
    /**
     * Resource Constructor
     * @param { Express } app 
     * @param { string } name 
     */
    ScraperResource(app, name) {
        self.name = name;
        self.size = 0;
        app.get(`/${name}/matchings`, (req, res));
        app.get
    } 

    /**
     * Checks if base url (defined in database) is valid
     */
    check_base_url() {
        if (self.db_data == null)
            self.db_data = get_database().model('Website', websiteSchema, 'website')
        
        self.base_url = self.db_data.get
    }

    /**
     * Function that gets called on GET request
     * @param { Request } req 
     * @param { Response } res 
     */
    on_get(req, res) {
        self.size = 0;
        var time = `${req.url} completed in`
        console.time(time);

        // DO STUFF HERE

        console.timeEnd(time);
    }

}