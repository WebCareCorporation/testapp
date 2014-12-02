/*
 * Service settings
 */
var warehousedb_settings = {
    "database_url": "https://api.appery.io/rest/1/db",
    "database_id": "547dd76ae4b05e4a44a6c805"
}

/*
 * Services
 */

var warehousedb_Merchandise_list_service = new Apperyio.RestService({
    'url': '{database_url}/collections/Merchandise',
    'dataType': 'json',
    'type': 'get',

    'serviceSettings': warehousedb_settings
});