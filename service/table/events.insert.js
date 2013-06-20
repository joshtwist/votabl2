var _r = require;
var require = function(path) {
	try {  return _r(path);	}
	catch (e) { return _r('../../app_data/config/scripts/table/' + path); }
}

var blobs = require('../shared/blobs.js');
var uuid = require('../shared/node_modules/uuid');

function insert(item, user, request) {
    // todo - create an image URL for blob storage
    request.execute({
        success : function(){
            item.imageUrl = blobs.generateUrl(item.id);
            request.respond();
        }
    });
}