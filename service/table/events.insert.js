var _r = require;
var require = function(path) {
	try {  return _r(path);	}
	catch (e) { return _r('../../app_data/config/scripts/table/' + path); }
}

var blobs = require('../shared/blobs.js');
var cryto = require('crypto');

function insert(item, user, request) {
	var sasUrl = blobs.generateUrl(item.id);
    item.imageUrl = sasUrl.substring(0, sasUrl.indexOf('?'));
    // todo - create an image URL for blob storage
    request.execute({
        success : function(){
            item.imageUrl = sasUrl;
            request.respond();
        }
    });
}