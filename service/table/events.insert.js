var _r = require;
var require = function(path) {
	try {  return _r(path);	}
	catch (e) { return _r('../../app_data/config/scripts/table/' + path); }
}

var blobs = require('../shared/blobs.js');
var uuid = require('../shared/node_modules/uuid');

function insert(item, user, request) {
    // todo - create an image URL for blob storage
    item.eventShareId = uuid.v4();
    var imgId = uuid.v4();
    var imgSasUrl = blobs.generateUrl(imgId);
    item.imageUrl = imgSasUrl.substring(0, imgSasUrl.indexOf('?'));
    request.execute({
        success : function(){
            item.imageUrl = imgSasUrl;
            request.respond();
        }
    });
}